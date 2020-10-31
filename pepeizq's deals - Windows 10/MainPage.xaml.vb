Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Windows.ApplicationModel.Core
Imports Windows.Services.Store
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Home"), FontAwesome5.EFontAwesomeIcon.Solid_Home, 0))
        nvPrincipal.MenuItems.Add(Interfaz.Busqueda)
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Deals2"), Nothing, 2))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Bundles2"), Nothing, 3))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Free2"), Nothing, 4))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Subscriptions2"), Nothing, 5))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Wishlist"), Nothing, 6))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Giveaways"), Nothing, 7))
        nvPrincipal.MenuItems.Add(MasCosas.Generar("https://github.com/pepeizq/pepeizqs-deals-win10", Nothing, "https://www.youtube.com/watch?v=uF6zm8cTakE"))

    End Sub

    Private Async Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        If TypeOf args.InvokedItem Is TextBlock Then
            gridBusqueda.Visibility = Visibility.Collapsed
            gridBusquedaJuego.Visibility = Visibility.Collapsed

            Dim item As TextBlock = args.InvokedItem

            If Not item Is Nothing Then
                If item.Text = recursos.GetString("Home") Then
                    CargarEntradas(100, Nothing, False)
                ElseIf item.Text = recursos.GetString("Deals2") Then
                    CargarEntradas(100, recursos.GetString("Deals2"), False)
                ElseIf item.Text = recursos.GetString("Bundles2") Then
                    CargarEntradas(100, recursos.GetString("Bundles2"), False)
                ElseIf item.Text = recursos.GetString("Free2") Then
                    CargarEntradas(100, recursos.GetString("Free2"), False)
                ElseIf item.Text = recursos.GetString("Subscriptions2") Then
                    CargarEntradas(100, recursos.GetString("Subscriptions2"), False)
                ElseIf item.Text = recursos.GetString("Wishlist") Then
                    GridVisibilidad.Mostrar("gridDeseados")
                ElseIf item.Text = recursos.GetString("Giveaways") Then
                    Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/giveaways/"))
                ElseIf item.Text = recursos.GetString("MoreThings") Then
                    FlyoutBase.ShowAttachedFlyout(nvPrincipal.MenuItems.Item(nvPrincipal.MenuItems.Count - 1))
                End If
            End If
        End If

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        tbTitulo.Text = Package.Current.DisplayName

        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveBackgroundColor = Colors.Transparent
        barra.ButtonInactiveForegroundColor = Colors.White

        '--------------------------------------------------------

        CargarEntradas(100, Nothing, True)

        Deseados.Cargar()
        Trial.Detectar(True)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        Dim recursos As New Resources.ResourceLoader()
        Dim mostrarCalificar As Boolean = True

        If Not config.Values("Calificar_App") = Nothing Then
            If config.Values("Calificar_App") = 1 Then
                mostrarCalificar = False
            End If
        End If

        If mostrarCalificar = True Then
            botonCalificar.Visibility = Visibility.Visible
            tbBotonCalificar.Text = recursos.GetString("MoreThings_RateApp")
        End If

    End Sub
    Private Sub GridEntradas_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles gridEntradas.SizeChanged

        Dim grid As Grid = sender

        If grid.ActualWidth > 1000 Then
            spControles.Visibility = Visibility.Visible
        Else
            spControles.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Sub SvEntradas_ViewChanging(sender As Object, e As ScrollViewerViewChangingEventArgs) Handles svEntradas.ViewChanging

        Dim sv As ScrollViewer = sender

        If sv.VerticalOffset > 50 Then
            botonSubir.Visibility = Visibility.Visible
        Else
            botonSubir.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Sub BotonActualizar_Click(sender As Object, e As RoutedEventArgs) Handles botonActualizar.Click

        Dim recursos As New Resources.ResourceLoader()

        If tbTitulo.Text.Contains(recursos.GetString("Bundles2")) Then
            CargarEntradas(100, recursos.GetString("Bundles2"), True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
            CargarEntradas(100, recursos.GetString("Deals2"), True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
            CargarEntradas(100, recursos.GetString("Free2"), True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
            CargarEntradas(100, recursos.GetString("Subscriptions2"), True)
        Else
            CargarEntradas(100, Nothing, True)
        End If

    End Sub

    Private Sub BotonSubir_Click(sender As Object, e As RoutedEventArgs) Handles botonSubir.Click

        svEntradas.ChangeView(Nothing, 0, Nothing)

    End Sub

    Private Sub BotonCalificar_Click(sender As Object, e As RoutedEventArgs) Handles botonCalificar.Click

        MasCosas.CalificarApp(False)

    End Sub

    Private Async Sub BotonTwitter_Click(sender As Object, e As RoutedEventArgs) Handles botonTwitter.Click

        Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/pepeizqdeals"))

    End Sub

    Private Async Sub BotonSteam_Click(sender As Object, e As RoutedEventArgs) Handles botonSteam.Click

        Await Launcher.LaunchUriAsync(New Uri("https://steamcommunity.com/groups/pepeizqdeals/"))

    End Sub

    Private Async Sub BotonReddit_Click(sender As Object, e As RoutedEventArgs) Handles botonReddit.Click

        Await Launcher.LaunchUriAsync(New Uri("https://new.reddit.com/r/pepeizqdeals/new/"))

    End Sub

    Private Async Sub BotonDiscord_Click(sender As Object, e As RoutedEventArgs) Handles botonDiscord.Click

        Await Launcher.LaunchUriAsync(New Uri("https://discord.gg/hsTfC9a"))

    End Sub

    Private Async Sub BotonTelegram_Click(sender As Object, e As RoutedEventArgs) Handles botonTelegram.Click

        Await Launcher.LaunchUriAsync(New Uri("https://t.me/pepeizqdeals"))

    End Sub

    Private Async Sub BotonRSS_Click(sender As Object, e As RoutedEventArgs) Handles botonRSS.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/rss-2/"))

    End Sub

    Public Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    Public Sub UsuarioEntraBoton2(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content
        Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
        icono.Saturation(1).Scale(1.2, 1.2, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBoton2(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content
        Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
        icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    Private Async Sub BotonComprar_Click(sender As Object, e As RoutedEventArgs) Handles botonComprar.Click

        Dim usuarios As IReadOnlyList(Of User) = Await User.FindAllAsync

        If Not usuarios Is Nothing Then
            If usuarios.Count > 0 Then
                Dim usuario As User = usuarios(0)

                Dim contexto As StoreContext = StoreContext.GetForUser(usuario)
                Await contexto.RequestPurchaseAsync("9P7836M1TW15")
            End If
        End If

    End Sub

End Class
