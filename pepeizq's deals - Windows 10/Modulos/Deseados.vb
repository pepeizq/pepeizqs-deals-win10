Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.Storage
Imports Windows.UI.Core

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
            For Each entrada In entradas
                Dim añadir As Boolean = False

                If Not entrada.Contenido Is Nothing Then
                    If entrada.Contenido.Texto.Contains(">" + juego.Value.Titulo.Trim + "<") Then
                        añadir = True

                        Dim k As Integer = 0
                        While k < filtros.Count
                            If filtros(k).Titulo = juego.Value.Titulo Then
                                añadir = False
                                filtros(k).Enlaces.Add(entrada.Enlace)
                            End If
                            k += 1
                        End While
                    End If
                ElseIf Not entrada.Titulo Is Nothing Then
                    If entrada.Titulo.Texto.Contains(juego.Value.Titulo.Trim + " •") Then
                        añadir = True
                    End If
                ElseIf Not entrada.subTitulo = Nothing Then
                    If entrada.SubTitulo.Contains(juego.Value.Titulo.Trim + " ,") Or entrada.SubTitulo.Contains(", " + juego.Value.Titulo.Trim) Or entrada.SubTitulo.Contains("and " + juego.Value.Titulo.Trim) Then
                        añadir = True
                    End If
                End If

                If añadir = True Then
                    Dim enlaces As New List(Of String) From {
                        entrada.Enlace
                    }

                    filtros.Add(New FiltroDeseado(juego.Value.Titulo, enlaces, juego.Value.Imagen))
                End If
            Next
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
                AddHandler botonFiltro.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler botonFiltro.PointerExited, AddressOf UsuarioSaleBoton

                gvFiltro.Items.Add(botonFiltro)
            Next
        Else
            tbDeseadosNoHay.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub AbrirFiltroClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim filtro As FiltroDeseado = boton.Tag

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spEntradas As StackPanel = pagina.FindName("spEntradas")

        For Each grid In spEntradas.Children
            If TypeOf grid Is Grid Then
                Dim grid2 As Grid = grid
                Dim entrada As Entrada = grid2.Tag

                If Not entrada Is Nothing Then
                    Dim visible As Boolean = False

                    For Each enlace In filtro.Enlaces
                        If entrada.Enlace = enlace Then
                            visible = True
                        End If
                    Next

                    If visible = True Then
                        grid2.Visibility = Visibility.Visible
                    Else
                        grid2.Visibility = Visibility.Collapsed
                    End If
                Else
                    grid2.Visibility = Visibility.Collapsed
                End If
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

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content
        imagen.Saturation(0.2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content
        imagen.Saturation(1).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
