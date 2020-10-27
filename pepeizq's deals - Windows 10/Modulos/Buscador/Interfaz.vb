Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace Buscador
    Module Interfaz

        Public Function ResultadoWeb(resultado As pepeizqdeals)

            Dim spJuego As New StackPanel With {
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 2),
                .Orientation = Orientation.Vertical,
                .Padding = New Thickness(5, 0, 5, 0)
            }

            Dim tbPrecio As New TextBlock With {
                .Text = WebUtility.HtmlDecode(resultado.Titulo),
                .Foreground = New SolidColorBrush(Colors.White),
                .HorizontalAlignment = HorizontalAlignment.Stretch,
                .FontSize = 16,
                .Margin = New Thickness(5, 8, 5, 8),
                .TextWrapping = TextWrapping.Wrap
            }

            spJuego.Children.Add(tbPrecio)

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(5, 10, 5, 10),
                .Content = spJuego
            }

            AddHandler boton.Click, AddressOf AbrirEnlaceClick
            AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaWeb
            AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaWeb

            Return boton

        End Function

        Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As pepeizqdeals = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(resultado.Enlace))

        End Sub

        Public Sub UsuarioEntraBotonBusquedaWeb(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content
            sp.BorderBrush = New SolidColorBrush(Colors.White)

            Dim tb As TextBlock = sp.Children(0)
            tb.Saturation(1).Scale(1.05, 1.05, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonBusquedaWeb(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content
            sp.BorderBrush = New SolidColorBrush(Colors.Transparent)

            Dim tb As TextBlock = sp.Children(0)
            tb.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '---------------------------------------------------------------

        Public Function ResultadoSteam(resultado As SteamWeb)

            Dim imagen As New ImageEx With {
                .Source = resultado.Imagen,
                .IsCacheEnabled = True,
                .Tag = resultado
            }

            AddHandler imagen.ImageExFailed, AddressOf ImagenFalla

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(5, 5, 5, 5),
                .Content = imagen
            }

            AddHandler boton.Click, AddressOf AbrirJuegoClick
            AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaSteam
            AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaSteam

            Return boton

        End Function

        Private Sub ImagenFalla(sender As Object, e As ImageExFailedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim imagen As ImageEx = sender
            Dim juego As SteamWeb = imagen.Tag

            Dim gv1 As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")

            For Each item In gv1.Items
                Dim sp As Button = item
                Dim juegoItem As SteamWeb = sp.Tag

                If juego.ID = juegoItem.ID Then
                    gv1.Items.Remove(item)
                End If
            Next

            Dim gv2 As AdaptiveGridView = pagina.FindName("gvDeseadosJuegos")

            For Each item In gv2.Items
                Dim sp As Button = item
                Dim juegoItem As SteamWeb = sp.Tag

                If juego.ID = juegoItem.ID Then
                    gv2.Items.Remove(item)
                End If
            Next

        End Sub

        Private Sub AbrirJuegoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As SteamWeb = boton.Tag

            BuscarJuego(resultado)

        End Sub

        Public Sub UsuarioEntraBotonBusquedaSteam(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim imagen As ImageEx = boton.Content
            imagen.Saturation(0).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonBusquedaSteam(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim imagen As ImageEx = boton.Content
            imagen.Saturation(1).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '---------------------------------------------------------------

        Dim tiendas As New List(Of Tienda)

        Public Sub IniciarTiendas()
            tiendas.Clear()
        End Sub

        Public Sub AñadirTienda(nuevaTienda As Tienda)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim añadir As Boolean = True

            For Each tienda In tiendas
                If tienda.Enlace = nuevaTienda.Enlace Then
                    añadir = False
                End If
            Next

            If añadir = True Then
                tiendas.Add(nuevaTienda)

                If config.Values("Estado_App") = 1 Then
                    If Pais.DetectarEuro = True Then
                        tiendas.Sort(Function(x As Tienda, y As Tienda)
                                         Dim precioX As String = x.Precio

                                         If precioX.Length = 6 Then
                                             precioX = "0" + precioX
                                         End If

                                         Dim precioY As String = y.Precio

                                         If precioY.Length = 6 Then
                                             precioY = "0" + precioY
                                         End If

                                         Dim resultado As Integer = precioX.CompareTo(precioY)
                                         Return resultado
                                     End Function)
                    End If
                End If

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")
                gvTiendas.Items.Clear()

                For Each tienda In tiendas
                    Dim sp As New StackPanel With {
                        .Orientation = Orientation.Vertical
                    }

                    Dim gridImagen As New Grid

                    If Not tienda.Pais = Nothing Then
                        Dim imagenPais As New ImageEx With {
                            .Source = "Assets/Paises/" + tienda.Pais + ".png",
                            .IsCacheEnabled = True,
                            .HorizontalAlignment = HorizontalAlignment.Right,
                            .VerticalAlignment = VerticalAlignment.Bottom,
                            .Width = 32,
                            .Height = 24,
                            .Opacity = 0.3
                        }
                        gridImagen.Children.Add(imagenPais)
                    End If

                    Dim imagenTienda As New ImageEx With {
                        .Source = tienda.Imagen,
                        .IsCacheEnabled = True,
                        .Tag = tienda,
                        .Width = 200
                    }

                    gridImagen.Children.Add(imagenTienda)

                    sp.Children.Add(gridImagen)

                    Dim tbPrecio As New TextBlock With {
                        .Text = tienda.Precio,
                        .Margin = New Thickness(0, 20, 0, 0),
                        .Foreground = New SolidColorBrush(Colors.White),
                        .FontSize = 22,
                        .HorizontalAlignment = HorizontalAlignment.Center
                    }

                    sp.Children.Add(tbPrecio)

                    Dim boton As New Button With {
                        .Background = New SolidColorBrush(Colors.Transparent),
                        .Padding = New Thickness(20, 20, 20, 20),
                        .BorderThickness = New Thickness(0, 0, 0, 0),
                        .Tag = tienda,
                        .Margin = New Thickness(15, 15, 15, 15),
                        .Content = sp
                    }

                    AddHandler boton.Click, AddressOf AbrirTiendaClick
                    AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaTienda
                    AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaTienda

                    If Not tienda.Mensaje = Nothing Then
                        Dim tbMensaje As New TextBlock With {
                            .Text = tienda.Mensaje,
                            .TextWrapping = TextWrapping.Wrap
                        }

                        ToolTipService.SetToolTip(boton, tienda.Mensaje)
                        ToolTipService.SetPlacement(boton, PlacementMode.Bottom)
                    End If

                    gvTiendas.Items.Add(boton)
                Next
            End If

        End Sub

        Private Async Sub AbrirTiendaClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As Tienda = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(resultado.Enlace))

        End Sub

        Public Sub UsuarioEntraBotonBusquedaTienda(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim grid As Grid = sp.Children(0)
            Dim imagen As ImageEx = grid.Children(grid.Children.Count - 1)
            imagen.Saturation(1).Scale(1.05, 1.05, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonBusquedaTienda(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim grid As Grid = sp.Children(0)
            Dim imagen As ImageEx = grid.Children(grid.Children.Count - 1)
            imagen.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace

