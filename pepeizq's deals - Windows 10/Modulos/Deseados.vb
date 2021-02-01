Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.Storage

Module Deseados

    Public Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbDeseados As TextBox = pagina.FindName("tbDeseadosSteam")
        tbDeseados.Text = String.Empty

        AddHandler tbDeseados.TextChanged, AddressOf CargarUsuario

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If Not config.Values("Cuenta_Steam") = Nothing Then
            tbDeseados.Text = config.Values("Cuenta_Steam")
        End If

    End Sub

    Public Async Sub CargarUsuario()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbCuentaSteam As TextBox = pagina.FindName("tbDeseadosSteam")
        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        config.Values("Cuenta_Steam") = tbCuentaSteam.Text.Trim

        Dim juegosDeseados As List(Of SteamJuegoDeseado2) = Await CargarCuentaJuegosDeseados(tbCuentaSteam.Text)

        If Not juegosDeseados Is Nothing Then
            If juegosDeseados.Count > 0 Then
                If config.Values("Estado_App") = 1 Then
                    Interfaz.Filtros.Cargar(juegosDeseados)
                End If

                CargarInterfazJuegosDeseados(juegosDeseados)
            End If
        End If

    End Sub

    Private Async Function CargarCuentaJuegosDeseados(usuario As String) As Task(Of List(Of SteamJuegoDeseado2))

        If Not usuario = String.Empty Then
            Dim tipo As Integer = 0

            If usuario.Contains("/id/") Then
                tipo = 1
            ElseIf usuario.Contains("/profiles/") Then
                tipo = 2
            End If

            usuario = usuario.Replace("https://steamcommunity.com/id/", Nothing)
            usuario = usuario.Replace("http://steamcommunity.com/id/", Nothing)
            usuario = usuario.Replace("https://steamcommunity.com/profiles/", Nothing)
            usuario = usuario.Replace("http://steamcommunity.com/profiles/", Nothing)
            usuario = usuario.Replace("/", Nothing)
            usuario = usuario.Trim

            Dim juegosDeseadosFinal As New List(Of SteamJuegoDeseado2)
            Dim i As Integer = 0
            While i < 10
                Dim htmlUsuario As String = String.Empty

                If tipo = 1 Then
                    htmlUsuario = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/wishlist/id/" + usuario + "/wishlistdata/?p=" + i.ToString))
                ElseIf tipo = 2 Then
                    htmlUsuario = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/wishlist/profiles/" + usuario + "/wishlistdata/?p=" + i.ToString))
                End If

                If Not htmlUsuario = Nothing Then
                    Dim juegosDeseados As New Dictionary(Of String, SteamJuegoDeseado)

                    Try
                        juegosDeseados = JsonConvert.DeserializeObject(Of Dictionary(Of String, SteamJuegoDeseado))(htmlUsuario)
                    Catch ex As Exception
                        Exit While
                    End Try

                    If Not juegosDeseados Is Nothing Then
                        If juegosDeseados.Count > 0 Then
                            For Each deseado In juegosDeseados
                                Dim añadir As Boolean = True

                                For Each deseadoFinal In juegosDeseadosFinal
                                    If deseadoFinal.ID = deseado.Key Then
                                        añadir = False
                                    End If
                                Next

                                If añadir = True Then
                                    juegosDeseadosFinal.Add(New SteamJuegoDeseado2(deseado.Value.Titulo, deseado.Value.Imagen, deseado.Key))
                                End If
                            Next
                        End If
                    End If
                End If

                i += 1
            End While

            Return juegosDeseadosFinal
        End If

        Return Nothing

    End Function

    Private Sub CargarInterfazJuegosDeseados(juegosDeseados As List(Of SteamJuegoDeseado2))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvJuegos As AdaptiveGridView = pagina.FindName("gvDeseadosJuegos")
        gvJuegos.Items.Clear()

        juegosDeseados.Sort(Function(x As SteamJuegoDeseado2, y As SteamJuegoDeseado2)
                                Dim resultado As Integer = x.Titulo.CompareTo(y.Titulo)
                                If resultado = 0 Then
                                    resultado = x.Titulo.CompareTo(y.Titulo)
                                End If
                                Return resultado
                            End Function)

        For Each juego In juegosDeseados
            Dim imagen As String = juego.Imagen

            Dim resultado As New Buscador.SteamWeb(juego.ID, juego.Titulo, imagen)
            gvJuegos.Items.Add(Interfaz.Buscador.ResultadoSteam(resultado))
        Next

    End Sub

    Public Function GenerarJsonOfertas(listaJuegos As List(Of EntradaOfertasJuego), comentario As String)

        Dim contenido As String = String.Empty

        contenido = "{" + ChrW(34) + "message" + ChrW(34) + ":"

        If Not comentario = Nothing Then
            If comentario.Trim.Length > 0 Then
                contenido = contenido + ChrW(34) + comentario.Trim + ChrW(34)
            Else
                contenido = contenido + "null"
            End If
        Else
            contenido = contenido + "null"
        End If

        contenido = contenido + "," + ChrW(34) + "games" + ChrW(34) + ":["

        For Each juego In listaJuegos
            If Not juego Is Nothing Then
                Dim titulo As String = juego.Titulo
                titulo = titulo.Replace(ChrW(34), Nothing)

                Dim imagen As String = juego.Imagen

                Dim drm As String = juego.DRM

                If drm = String.Empty Then
                    drm = "null"
                Else
                    drm = drm.Trim
                End If

                Dim analisisPorcentaje As String = "null"
                Dim analisisCantidad As String = "null"
                Dim analisisEnlace As String = "null"

                If Not juego.AnalisisPorcentaje Is Nothing Then
                    analisisPorcentaje = juego.AnalisisPorcentaje
                    analisisCantidad = juego.AnalisisCantidad
                    analisisEnlace = juego.AnalisisEnlace
                End If

                contenido = contenido + "{" + ChrW(34) + "title" + ChrW(34) + ":" + ChrW(34) + titulo + ChrW(34) + "," +
                                              ChrW(34) + "image" + ChrW(34) + ":" + ChrW(34) + imagen + ChrW(34) + "," +
                                              ChrW(34) + "dscnt" + ChrW(34) + ":" + ChrW(34) + juego.Descuento + ChrW(34) + "," +
                                              ChrW(34) + "price" + ChrW(34) + ":" + ChrW(34) + juego.Precio + ChrW(34) + "," +
                                              ChrW(34) + "link" + ChrW(34) + ":" + ChrW(34) + juego.Enlace + ChrW(34) + "," +
                                              ChrW(34) + "drm" + ChrW(34) + ":" + ChrW(34) + drm + ChrW(34) + "," +
                                              ChrW(34) + "revw1" + ChrW(34) + ":" + ChrW(34) + analisisPorcentaje + ChrW(34) + "," +
                                              ChrW(34) + "revw2" + ChrW(34) + ":" + ChrW(34) + analisisCantidad + ChrW(34) + "," +
                                              ChrW(34) + "revw3" + ChrW(34) + ":" + ChrW(34) + analisisEnlace + ChrW(34) +
                                        "},"
            End If
        Next

        contenido = contenido.Remove(contenido.Length - 1, 1)
        contenido = contenido + "]}"

        Return contenido

    End Function

End Module
