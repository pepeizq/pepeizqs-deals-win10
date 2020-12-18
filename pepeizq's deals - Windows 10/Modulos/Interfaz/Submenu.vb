Imports Windows.System

Namespace Interfaz

    Module Submenu

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonOfertas As Button = pagina.FindName("botonOfertas")
            botonOfertas.Tag = recursos.GetString("Deals2")

            AddHandler botonOfertas.Click, AddressOf AbrirSeccionClick
            AddHandler botonOfertas.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonOfertas.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonBundles As Button = pagina.FindName("botonBundles")
            botonBundles.Tag = recursos.GetString("Bundles2")

            AddHandler botonBundles.Click, AddressOf AbrirSeccionClick
            AddHandler botonBundles.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonBundles.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonGratis As Button = pagina.FindName("botonGratis")
            botonGratis.Tag = recursos.GetString("Free2")

            AddHandler botonGratis.Click, AddressOf AbrirSeccionClick
            AddHandler botonGratis.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonGratis.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonSuscripciones As Button = pagina.FindName("botonSuscripciones")
            botonSuscripciones.Tag = recursos.GetString("Subscriptions2")

            AddHandler botonSuscripciones.Click, AddressOf AbrirSeccionClick
            AddHandler botonSuscripciones.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonSuscripciones.PointerExited, AddressOf Sale_Boton_Texto

            '-----------------------------------------------

            Dim botonTwitter As Button = pagina.FindName("botonTwitter")

            AddHandler botonTwitter.Click, AddressOf TwitterClick
            AddHandler botonTwitter.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonTwitter.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonSteam As Button = pagina.FindName("botonSteam")

            AddHandler botonSteam.Click, AddressOf SteamClick
            AddHandler botonSteam.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonSteam.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonReddit As Button = pagina.FindName("botonReddit")

            AddHandler botonReddit.Click, AddressOf RedditClick
            AddHandler botonReddit.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonReddit.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonDiscord As Button = pagina.FindName("botonDiscord")

            AddHandler botonDiscord.Click, AddressOf DiscordClick
            AddHandler botonDiscord.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonDiscord.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonTelegram As Button = pagina.FindName("botonTelegram")

            AddHandler botonTelegram.Click, AddressOf TelegramClick
            AddHandler botonTelegram.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonTelegram.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonRSS As Button = pagina.FindName("botonRSS")

            AddHandler botonRSS.Click, AddressOf RSSClick
            AddHandler botonRSS.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonRSS.PointerExited, AddressOf Sale_Boton_Icono

        End Sub

        Private Sub AbrirSeccionClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Wordpress.CargarEntradas(100, boton.Tag, False, False)

        End Sub

        Private Async Sub TwitterClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/pepeizqdeals"))

        End Sub

        Private Async Sub SteamClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://steamcommunity.com/groups/pepeizqdeals/"))

        End Sub

        Private Async Sub RedditClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://new.reddit.com/r/pepeizqdeals/new/"))

        End Sub

        Private Async Sub DiscordClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://discord.gg/hsTfC9a"))

        End Sub

        Private Async Sub TelegramClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://t.me/pepeizqdeals"))

        End Sub

        Private Async Sub RSSClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/rss-2/"))

        End Sub

    End Module

End Namespace