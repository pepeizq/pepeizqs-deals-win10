Imports Windows.System

Namespace Interfaz

    Module RedesSociales

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

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