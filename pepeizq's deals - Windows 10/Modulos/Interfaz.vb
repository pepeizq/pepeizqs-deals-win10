Imports System.Net
Imports FontAwesome.UWP
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

        Dim recursos As New Resources.ResourceLoader()

        Dim item As New ListViewItem With {
            .Margin = New Thickness(0, 0, 0, 20),
            .Padding = New Thickness(10, 10, 10, 10)
        }

        Dim spMaestro As New StackPanel With {
            .Orientation = Orientation.Horizontal
        }

        '------------------------------

        Dim panelCompartir As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = App.Current.Resources("ColorCuarto"),
            .Margin = New Thickness(0, 0, 20, 0),
            .Tag = entrada
        }

        Dim tbCompartir As New TextBlock With {
            .Text = recursos.GetString("Share"),
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(12, 10, 12, 10)
        }

        Dim spCompartir As New StackPanel With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorTercero"))
        }

        spCompartir.Children.Add(tbCompartir)

        Dim compartir As New Button With {
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Tag = entrada,
            .Content = spCompartir,
            .Padding = New Thickness(0, 0, 0, 0),
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        }

        AddHandler compartir.Click, AddressOf CompartirClick
        AddHandler compartir.PointerEntered, AddressOf UsuarioEntraBotonCompartir
        AddHandler compartir.PointerExited, AddressOf UsuarioSaleBotonCompartir

        panelCompartir.Content = compartir

        spMaestro.Children.Add(panelCompartir)

        '------------------------------

        Dim panelTwitter As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = App.Current.Resources("ColorCuarto"),
            .Margin = New Thickness(0, 0, 20, 0),
            .Tag = entrada
        }

        Dim iconoTwitter As New FontAwesome.UWP.FontAwesome With {
            .Icon = FontAwesomeIcon.Twitter,
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(12, 10, 12, 10)
        }

        Dim spTwitter As New StackPanel With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorTercero"))
        }

        spTwitter.Children.Add(iconoTwitter)

        Dim twitter As New Button With {
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Tag = entrada,
            .Content = spTwitter,
            .Padding = New Thickness(0, 0, 0, 0),
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        }

        AddHandler twitter.Click, AddressOf TwitterClick
        AddHandler twitter.PointerEntered, AddressOf UsuarioEntraBotonCompartir
        AddHandler twitter.PointerExited, AddressOf UsuarioSaleBotonCompartir

        panelTwitter.Content = twitter

        spMaestro.Children.Add(panelTwitter)

        '------------------------------

        Dim panelReddit As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = App.Current.Resources("ColorCuarto"),
            .Margin = New Thickness(0, 0, 20, 0),
            .Tag = entrada
        }

        Dim iconoReddit As New FontAwesome.UWP.FontAwesome With {
            .Icon = FontAwesomeIcon.Reddit,
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(12, 10, 12, 10)
        }

        Dim spReddit As New StackPanel With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorTercero"))
        }

        spReddit.Children.Add(iconoReddit)

        Dim reddit As New Button With {
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Tag = entrada,
            .Content = spReddit,
            .Padding = New Thickness(0, 0, 0, 0),
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        }

        AddHandler reddit.Click, AddressOf RedditClick
        AddHandler reddit.PointerEntered, AddressOf UsuarioEntraBotonCompartir
        AddHandler reddit.PointerExited, AddressOf UsuarioSaleBotonCompartir

        panelReddit.Content = reddit

        spMaestro.Children.Add(panelReddit)

        '------------------------------

        item.Content = spMaestro

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

    Private Async Sub TwitterClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim entrada As Entrada = boton.Tag

        Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
        Dim enlace As String = "https://twitter.com/intent/tweet?url=" + entrada.Enlace + "&title=" + WebUtility.UrlEncode(tituloTexto)

        Await Launcher.LaunchUriAsync(New Uri(enlace))

    End Sub

    Private Async Sub RedditClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim entrada As Entrada = boton.Tag

        Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
        Dim enlace As String = "https://www.reddit.com/submit?url=" + entrada.Enlace + "&title=" + WebUtility.UrlEncode(tituloTexto)

        Await Launcher.LaunchUriAsync(New Uri(enlace))

    End Sub

    Public Sub UsuarioEntraBotonCompartir(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBotonCompartir(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    '------------------------------------------------------------------------------------------------

    Public Function GenerarAnuncio(entrada As Entrada)

        Dim gridMaestro As New Grid

        Dim panelImagen As New DropShadowPanel With {
            .BlurRadius = 4,
            .ShadowOpacity = 0.9,
            .Color = App.Current.Resources("ColorSegundo"),
            .Margin = New Thickness(0, 10, 0, 10)
        }

        Dim gridImagen As New Grid With {
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimero")),
            .BorderThickness = New Thickness(2, 2, 2, 2)
        }

        Dim imagen As New ImageEx With {
            .Source = entrada.Imagen,
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Top
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

End Module
