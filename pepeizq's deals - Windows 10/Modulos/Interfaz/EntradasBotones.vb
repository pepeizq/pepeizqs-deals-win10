Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.System
Imports Windows.UI

Namespace Interfaz

    Module EntradasBotones

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonActualizar As Button = pagina.FindName("botonActualizar")

            AddHandler botonActualizar.Click, AddressOf ActualizarClick
            AddHandler botonActualizar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonActualizar.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonSubir As Button = pagina.FindName("botonSubir")

            AddHandler botonSubir.Click, AddressOf SubirClick
            AddHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonComprar As Button = pagina.FindName("botonComprar")

            AddHandler botonComprar.Click, AddressOf Trial.BotonComprarClick
            AddHandler botonComprar.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonComprar.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonSorteos As Button = pagina.FindName("botonSorteosImagen")

            AddHandler botonSorteos.Click, AddressOf AbrirSorteosClick
            AddHandler botonSorteos.PointerEntered, AddressOf Entra_Boton_1_02
            AddHandler botonSorteos.PointerExited, AddressOf Sale_Boton_1_02

        End Sub

        Private Sub ActualizarClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")

            If tbTitulo.Text.Contains(recursos.GetString("Bundles2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Bundles2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Deals2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Free2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Subscriptions2"), True, False)
            Else
                Wordpress.CargarEntradas(100, Nothing, True, False)
            End If

        End Sub

        Private Sub SubirClick(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim svEntradas As ScrollViewer = pagina.FindName("svEntradas")

            svEntradas.ChangeView(Nothing, 0, Nothing)

        End Sub

        Private Async Sub AbrirSorteosClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/giveaways/"))

        End Sub

    End Module

End Namespace