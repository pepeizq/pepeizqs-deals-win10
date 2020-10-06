Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.UI.Core

Module SteamDeseados

    Public Async Sub Cargar(usuario As String, entradas As List(Of Entrada))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim expanderCuenta As Expander = pagina.FindName("expanderCuentaSteam")
        Dim expanderFiltro As Expander = pagina.FindName("expanderFiltroJuegosDeseados")
        Dim gvFiltro As AdaptiveGridView = pagina.FindName("gvJuegosDeseados")
        gvFiltro.Items.Clear()

        If Not usuario = Nothing Then
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

                If juegosDeseados.Count > 0 Then
                    expanderCuenta.IsExpanded = False
                    expanderFiltro.IsExpanded = True
                    expanderFiltro.IsEnabled = True

                    Dim filtros As New List(Of FiltroDeseado)

                    For Each juego In juegosDeseados
                        For Each entrada In entradas
                            If entrada.Contenido.Texto.Contains(">" + juego.Value.Titulo + "<") Then
                                Dim añadir As Boolean = True

                                Dim k As Integer = 0
                                While k < filtros.Count
                                    If filtros(k).Titulo = juego.Value.Titulo Then
                                        añadir = False
                                        filtros(k).Enlaces.Add(entrada.Enlace)
                                    End If
                                    k += 1
                                End While

                                If añadir = True Then
                                    Dim enlaces As New List(Of String) From {
                                        entrada.Enlace
                                    }

                                    filtros.Add(New FiltroDeseado(juego.Value.Titulo, enlaces, juego.Value.Imagen))
                                End If
                            End If
                        Next
                    Next

                    If filtros.Count > 0 Then
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
                    End If
                Else
                    expanderCuenta.IsExpanded = True
                    expanderFiltro.IsExpanded = False
                    expanderFiltro.IsEnabled = False
                End If
            Else
                expanderCuenta.IsExpanded = True
                expanderFiltro.IsExpanded = False
                expanderFiltro.IsEnabled = False
            End If
        Else
            expanderCuenta.IsExpanded = True
            expanderFiltro.IsExpanded = False
            expanderFiltro.IsEnabled = False
        End If

    End Sub

    Private Sub AbrirFiltroClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim filtro As FiltroDeseado = boton.Tag

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spEntradas As StackPanel = pagina.FindName("spEntradas")

        For Each hijo In spEntradas.Children
            If TypeOf hijo Is DropShadowPanel Then
                Dim panel As DropShadowPanel = hijo
                Dim entrada As Entrada = panel.Tag

                Dim visible As Boolean = False

                For Each enlace In filtro.Enlaces
                    If entrada.Enlace = enlace Then
                        visible = True
                    End If
                Next

                If visible = True Then
                    panel.Visibility = Visibility.Visible
                Else
                    panel.Visibility = Visibility.Collapsed
                End If
            ElseIf TypeOf hijo Is StackPanel Then
                hijo.Visibility = Visibility.Collapsed
            End If
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
