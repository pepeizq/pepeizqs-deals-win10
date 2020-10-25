Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
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

        Public Function ResultadoTienda(resultado As Tienda, pais As String, mensaje As String)

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Vertical
            }

            Dim gridImagen As New Grid

            If Not pais = Nothing Then
                Dim imagenPais As New ImageEx With {
                    .Source = "Assets/Paises/" + pais + ".png",
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
                .Source = resultado.Imagen,
                .IsCacheEnabled = True,
                .Tag = resultado,
                .Width = 200
            }

            gridImagen.Children.Add(imagenTienda)

            sp.Children.Add(gridImagen)

            Dim tbPrecio As New TextBlock With {
                .Text = resultado.Precio,
                .Margin = New Thickness(0, 20, 0, 0),
                .Foreground = New SolidColorBrush(Colors.White),
                .FontSize = 22,
                .HorizontalAlignment = HorizontalAlignment.Center
            }

            sp.Children.Add(tbPrecio)

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(10, 10, 10, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(15, 10, 15, 10),
                .Content = sp
            }

            AddHandler boton.Click, AddressOf AbrirTiendaClick
            AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaTienda
            AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaTienda

            If Not mensaje = Nothing Then
                Dim tbMensaje As New TextBlock With {
                    .Text = mensaje,
                    .TextWrapping = TextWrapping.Wrap
                }

                ToolTipService.SetToolTip(boton, mensaje)
                ToolTipService.SetPlacement(boton, PlacementMode.Bottom)
            End If

            Return boton

        End Function

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

