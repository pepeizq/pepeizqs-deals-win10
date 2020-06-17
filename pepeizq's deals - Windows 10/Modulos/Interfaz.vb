Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Function GenerarEntrada(entrada As Entrada)

        Dim gridMaestro As New Grid

        Dim panelImagen As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = Colors.Black,
            .Margin = New Thickness(0, 10, 0, 10)
        }

        Dim gridImagen As New Grid With {
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorTercero")),
            .BorderThickness = New Thickness(2, 2, 2, 2)
        }

        Dim imagen As New ImageEx With {
            .Source = entrada.Imagen,
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill
        }

        gridImagen.Children.Add(imagen)

        panelImagen.Content = gridImagen

        gridMaestro.Children.Add(panelImagen)

        Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)

        Dim titulo As New TextBlock With {
            .Text = tituloTexto,
            .TextWrapping = TextWrapping.Wrap,
            .Opacity = 0,
            .Margin = New Thickness(40, 40, 40, 40),
            .FontSize = 30,
            .Foreground = New SolidColorBrush(App.Current.Resources("ColorQuinto")),
            .VerticalAlignment = VerticalAlignment.Center,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .HorizontalTextAlignment = TextAlignment.Center
        }

        gridMaestro.Children.Add(titulo)

        Dim boton As New Button With {
            .Background = New SolidColorBrush(Colors.Transparent),
            .Padding = New Thickness(0, 0, 0, 0),
            .BorderThickness = New Thickness(0, 0, 0, 0),
            .Tag = entrada
        }

        AddHandler boton.Click, AddressOf AbrirEnlaceClick
        AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonEntrada
        AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonEntrada

        boton.Content = gridMaestro

        Return boton
    End Function

    Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim entrada As Entrada = boton.Tag

        Await Launcher.LaunchUriAsync(New Uri(entrada.Enlace))

    End Sub

    Public Sub UsuarioEntraBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        Dim boton As Button = sender
        Dim grid As Grid = boton.Content

        Dim panelImagen As DropShadowPanel = grid.Children(0)
        panelImagen.Opacity = 0.2

        Dim titulo As TextBlock = grid.Children(1)
        titulo.Opacity = 1

    End Sub

    Public Sub UsuarioSaleBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        Dim boton As Button = sender
        Dim grid As Grid = boton.Content

        Dim panelImagen As DropShadowPanel = grid.Children(0)
        panelImagen.Opacity = 1

        Dim titulo As TextBlock = grid.Children(1)
        titulo.Opacity = 0

    End Sub

    '------------------------------------------------------------------------------------------------

    Public Function GenerarCompartir(entrada As Entrada)

        Dim item As New ListViewItem With {
            .Margin = New Thickness(0, 0, 0, 20)
        }

        Dim panelCompartir As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = Colors.Black,
            .Margin = New Thickness(0, 10, 0, 10),
            .Tag = entrada
        }

        Dim recursos As New Resources.ResourceLoader()

        Dim tb As New TextBlock With {
            .Text = recursos.GetString("Share"),
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(12, 10, 12, 10)
        }

        Dim sp As New StackPanel With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorTercero"))
        }

        sp.Children.Add(tb)

        Dim compartir As New Button With {
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Tag = entrada,
            .Content = sp,
            .Padding = New Thickness(0, 0, 0, 0),
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        }

        AddHandler compartir.Click, AddressOf CompartirClick
        AddHandler compartir.PointerEntered, AddressOf UsuarioEntraBotonCompartir
        AddHandler compartir.PointerExited, AddressOf UsuarioSaleBotonCompartir

        panelCompartir.Content = compartir

        item.Content = panelCompartir

        Return item
    End Function

    Private Sub CompartirClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim entrada As Entrada = boton.Tag

        Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
        ApplicationData.Current.LocalSettings.Values("compartir_titulo") = tituloTexto
        ApplicationData.Current.LocalSettings.Values("compartir_enlace") = entrada.Enlace

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf CompartirVentana

        DataTransferManager.ShowShareUI()

    End Sub

    Public Sub CompartirVentana(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("compartir_titulo")
        request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("compartir_enlace")))

    End Sub

    Public Sub UsuarioEntraBotonCompartir(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBotonCompartir(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
