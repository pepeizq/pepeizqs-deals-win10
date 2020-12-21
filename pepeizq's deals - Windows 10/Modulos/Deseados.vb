Imports System.Net
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

        Dim juegosDeseados As Dictionary(Of String, SteamJuegoDeseado) = Await CargarJuegosDeseados(tbCuentaSteam.Text)

        If Not juegosDeseados Is Nothing Then
            If juegosDeseados.Count > 0 Then
                If config.Values("Estado_App") = 1 Then
                    CargarFiltro(juegosDeseados)
                End If

                CargarJuegosDeseados2(juegosDeseados)
            End If
        End If

    End Sub

    Private Async Function CargarJuegosDeseados(usuario As String) As Task(Of Dictionary(Of String, SteamJuegoDeseado))

        If Not usuario = String.Empty Then
            usuario = usuario.Replace("https://steamcommunity.com/id/", Nothing)
            usuario = usuario.Replace("http://steamcommunity.com/id/", Nothing)
            usuario = usuario.Replace("https://steamcommunity.com/profiles/", Nothing)
            usuario = usuario.Replace("http://steamcommunity.com/profiles/", Nothing)
            usuario = usuario.Replace("/", Nothing)
            usuario = usuario.Trim

            Dim htmlUsuario As String = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/wishlist/id/" + usuario + "/wishlistdata/"))

            If Not htmlUsuario = Nothing Then
                Dim juegosDeseados As Dictionary(Of String, SteamJuegoDeseado) = JsonConvert.DeserializeObject(Of Dictionary(Of String, SteamJuegoDeseado))(htmlUsuario)

                If juegosDeseados.Count = 0 Then
                    Dim htmlUsuario2 As String = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/wishlist/profiles/" + usuario + "/wishlistdata/"))

                    If Not htmlUsuario2 = Nothing Then
                        juegosDeseados = JsonConvert.DeserializeObject(Of Dictionary(Of String, SteamJuegoDeseado))(htmlUsuario2)
                    End If
                End If

                Return juegosDeseados
            End If
        End If

        Return Nothing

    End Function

    Private Sub CargarFiltro(juegosDeseados As Dictionary(Of String, SteamJuegoDeseado))

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim pr As ProgressRing = pagina.FindName("prDeseados")
        pr.Visibility = Visibility.Visible

        Dim gvFiltro As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")
        gvFiltro.Items.Clear()

        Dim entradas As New List(Of Entrada)
        Dim spEntradas As StackPanel = pagina.FindName("spEntradas")

        For Each grid In spEntradas.Children
            If TypeOf grid Is Grid Then
                Dim grid2 As Grid = grid
                Dim entrada As Entrada = grid2.Tag

                If Not entrada Is Nothing Then
                    entradas.Add(entrada)
                End If
            End If
        Next

        Dim filtros As New List(Of FiltroDeseado)

        For Each juego In juegosDeseados
            Dim listadoEncontrados As New List(Of FiltroEntradaDeseado)

            For Each entrada In entradas
                If entrada.Categorias(0) = 3 Then
                    If Not entrada.JsonExpandido = Nothing Then
                        Dim json As EntradaOfertas = JsonConvert.DeserializeObject(Of EntradaOfertas)(entrada.JsonExpandido)

                        If Not json Is Nothing Then
                            If Not json.Juegos Is Nothing Then
                                For Each juego2 In json.Juegos
                                    If config.Values("Deseados") = 1 Then
                                        If Limpieza.Limpiar(juego2.Titulo) = Limpieza.Limpiar(WebUtility.HtmlDecode(juego.Value.Titulo)) Then
                                            listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                        End If
                                    ElseIf config.Values("Deseados") = 0 Then
                                        If Limpieza.Limpiar(juego2.Titulo).Contains(Limpieza.Limpiar(WebUtility.HtmlDecode(juego.Value.Titulo))) Then
                                            listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Else
                        Dim añadir As Boolean = False

                        If Not entrada.Titulo Is Nothing Then
                            If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(juego.Value.Titulo.Trim)) Then
                                añadir = True
                            End If
                        End If

                        If Not entrada.SubTitulo = Nothing Then
                            If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(juego.Value.Titulo.Trim)) Then
                                añadir = True
                            End If
                        End If

                        If añadir = True Then
                            listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                        End If
                    End If
                Else
                    Dim añadir As Boolean = False

                    If Not entrada.Titulo Is Nothing Then
                        If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(juego.Value.Titulo.Trim)) Then
                            añadir = True
                        End If
                    End If

                    If Not entrada.SubTitulo = Nothing Then
                        If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(juego.Value.Titulo.Trim)) Then
                            añadir = True
                        End If
                    End If

                    If añadir = True Then
                        listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                    End If
                End If
            Next

            If listadoEncontrados.Count > 0 Then
                filtros.Add(New FiltroDeseado(juego.Value.Titulo, listadoEncontrados, juego.Value.Imagen))
            End If
        Next

        Dim tbDeseadosNoHay As TextBlock = pagina.FindName("tbJuegosDeseadosNoHay")
        pr.Visibility = Visibility.Collapsed

        If filtros.Count > 0 Then
            tbDeseadosNoHay.Visibility = Visibility.Collapsed

            filtros.Sort(Function(x As FiltroDeseado, y As FiltroDeseado)
                             Dim resultado As Integer = x.Titulo.CompareTo(y.Titulo)
                             If resultado = 0 Then
                                 resultado = x.Titulo.CompareTo(y.Titulo)
                             End If
                             Return resultado
                         End Function)

            For Each filtro In filtros
                Dim imagenFiltro As New ImageEx With {
                    .Source = filtro.Imagen,
                    .IsCacheEnabled = True
                }

                Dim botonFiltro As New Button With {
                    .Tag = filtro,
                    .Padding = New Thickness(0, 0, 0, 0),
                    .Content = imagenFiltro
                }

                AddHandler botonFiltro.Click, AddressOf AbrirFiltroClick
                AddHandler botonFiltro.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Boton_Imagen
                AddHandler botonFiltro.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Boton_Imagen

                gvFiltro.Items.Add(botonFiltro)
            Next
        Else
            tbDeseadosNoHay.Visibility = Visibility.Visible
        End If

    End Sub

    Private Async Sub AbrirFiltroClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim filtro As FiltroDeseado = boton.Tag

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
        spEntradas.Children.Clear()

        For Each entrada2 In filtro.Entradas
            If Not entrada2.Juego Is Nothing Then
                Dim listaJuegos As New List(Of EntradaOfertasJuego) From {
                    entrada2.Juego
                }

                Dim json As String = GenerarJsonOfertas(listaJuegos, Nothing)

                If entrada2.Juego Is Nothing Then
                    If entrada2.Entrada.Categorias(0) = 3 Then
                        json = entrada2.Entrada.Json
                    End If
                End If

                Dim entrada As New Entrada With {
                    .TiendaLogo = entrada2.Entrada.TiendaLogo,
                    .Json = json
                }
                entrada.Categorias = entrada2.Entrada.Categorias

                spEntradas.Children.Add(Await Interfaz.Entradas.GenerarEntrada(entrada))
            Else
                spEntradas.Children.Add(Await Interfaz.Entradas.GenerarEntrada(entrada2.Entrada))
            End If
        Next

    End Sub

    Private Sub CargarJuegosDeseados2(juegosDeseados As Dictionary(Of String, SteamJuegoDeseado))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvJuegos As AdaptiveGridView = pagina.FindName("gvDeseadosJuegos")
        gvJuegos.Items.Clear()

        For Each juego In juegosDeseados
            Dim imagen As String = juego.Value.Imagen
            imagen = imagen.Replace("header_292x136", "library_600x900")

            Dim resultado As New Buscador.SteamWeb(juego.Key, juego.Value.Titulo, imagen)
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
