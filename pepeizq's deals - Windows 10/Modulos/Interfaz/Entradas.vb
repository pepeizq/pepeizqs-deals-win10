Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace Interfaz

    Module Entradas

        Public Function GenerarEntrada(entrada As Entrada)

            Dim gridMaestro As New Grid

            Dim gridImagen As New Grid With {
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .BorderThickness = New Thickness(2, 2, 2, 2)
            }

            Dim imagen As New ImageEx With {
                .Source = entrada.Imagen,
                .IsCacheEnabled = True,
                .Stretch = Stretch.Uniform
            }

            gridImagen.Children.Add(imagen)

            gridMaestro.Children.Add(gridImagen)

            Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)

            Dim titulo As New TextBlock With {
                .Text = tituloTexto,
                .TextWrapping = TextWrapping.Wrap,
                .Opacity = 0,
                .Margin = New Thickness(40, 40, 40, 40),
                .FontSize = 30,
                .Foreground = New SolidColorBrush(App.Current.Resources("ColorTerciario")),
                .VerticalAlignment = VerticalAlignment.Center,
                .HorizontalAlignment = HorizontalAlignment.Center,
                .HorizontalTextAlignment = TextAlignment.Center
            }

            gridMaestro.Children.Add(titulo)

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = entrada,
                .MaxWidth = 850,
                .Content = gridMaestro
            }

            AddHandler boton.Click, AddressOf AbrirEnlaceClick
            AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonEntrada
            AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonEntrada

            Dim panelSombra As New DropShadowPanel With {
                .BlurRadius = 20,
                .ShadowOpacity = 0.9,
                .Color = Colors.Black,
                .Margin = New Thickness(0, 10, 0, 10),
                .Content = boton,
                .Tag = entrada
            }

            Return panelSombra
        End Function

        Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim entrada As Entrada = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(entrada.Enlace))

        End Sub

        Public Sub UsuarioEntraBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content

            Dim subgrid As Grid = grid.Children(0)
            subgrid.Opacity = 0.2
            subgrid.Saturation(1).Scale(1.01, 1.01, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Dim titulo As TextBlock = grid.Children(1)
            titulo.Opacity = 1

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content

            Dim subgrid As Grid = grid.Children(0)
            subgrid.Opacity = 1
            subgrid.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Dim titulo As TextBlock = grid.Children(1)
            titulo.Opacity = 0

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '------------------------------------------------------------------------------------------------

        Public Function GenerarCompartir(entrada As Entrada)

            Dim recursos As New Resources.ResourceLoader()

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(0, 0, 0, 20),
                .Padding = New Thickness(0, 10, 10, 10)
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
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim compartir As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = tbCompartir,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler compartir.Click, AddressOf CompartirClick
            AddHandler compartir.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto
            AddHandler compartir.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto

            panelCompartir.Content = compartir

            sp.Children.Add(panelCompartir)

            '------------------------------

            Dim panelTwitter As New DropShadowPanel With {
                .BlurRadius = 10,
                .ShadowOpacity = 0.9,
                .Color = App.Current.Resources("ColorCuarto"),
                .Margin = New Thickness(0, 0, 20, 0),
                .Tag = entrada
            }

            Dim iconoTwitter As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Twitter,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim twitter As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = iconoTwitter,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler twitter.Click, AddressOf TwitterClick
            AddHandler twitter.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono
            AddHandler twitter.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono

            panelTwitter.Content = twitter

            sp.Children.Add(panelTwitter)

            '------------------------------

            Dim panelReddit As New DropShadowPanel With {
                .BlurRadius = 10,
                .ShadowOpacity = 0.9,
                .Color = App.Current.Resources("ColorCuarto"),
                .Margin = New Thickness(0, 0, 20, 0),
                .Tag = entrada
            }

            Dim iconoReddit As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Reddit,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim reddit As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = iconoReddit,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler reddit.Click, AddressOf RedditClick
            AddHandler reddit.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono
            AddHandler reddit.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono

            panelReddit.Content = reddit

            sp.Children.Add(panelReddit)

            '------------------------------

            Return sp

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

        '------------------------------------------------------------------------------------------------

        Public Function GenerarJuego(entrada As Entrada)

            Dim spJuego As New StackPanel With {
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .BorderThickness = New Thickness(2, 2, 2, 2),
                .Orientation = Orientation.Vertical,
                .Background = New SolidColorBrush(App.Current.Resources("ColorTerciario"))
            }

            Dim imagen As New ImageEx With {
                .Source = entrada.Imagen,
                .IsCacheEnabled = True,
                .Stretch = Stretch.Uniform
            }

            spJuego.Children.Add(imagen)

            Dim tbPrecio As New TextBlock With {
                .Text = entrada.JuegoPrecioMinimo,
                .Foreground = New SolidColorBrush(Colors.White),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .FontSize = 17,
                .Margin = New Thickness(5, 8, 5, 8)
            }

            spJuego.Children.Add(tbPrecio)

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = entrada,
                .MaxWidth = 150,
                .Content = spJuego
            }

            AddHandler boton.Click, AddressOf AbrirEnlaceClick
            AddHandler boton.PointerEntered, AddressOf Entra_Boton_ImagenTexto
            AddHandler boton.PointerExited, AddressOf Sale_Boton_ImagenTexto

            Dim panelSombra As New DropShadowPanel With {
                .BlurRadius = 20,
                .ShadowOpacity = 0.9,
                .Color = Colors.Black,
                .Margin = New Thickness(5, 10, 5, 10),
                .Content = boton,
                .Tag = entrada
            }

            Return panelSombra

        End Function

        '------------------------------------------------------------------------------------------------

        Public Function Busqueda()

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim icono As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Search,
                .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .Margin = New Thickness(0, 0, 10, 0)
            }

            sp.Children.Add(icono)

            Dim tb As New TextBox With {
                .MinWidth = 150
            }

            RemoveHandler tb.TextChanged, AddressOf pepeizq_s_deals___Windows_10.Buscador.Motor.BuscarWebSteam
            AddHandler tb.TextChanged, AddressOf pepeizq_s_deals___Windows_10.Buscador.Motor.BuscarWebSteam

            sp.Children.Add(tb)

            Return sp

        End Function

    End Module

End Namespace