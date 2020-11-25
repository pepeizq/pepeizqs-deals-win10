Imports Windows.ApplicationModel.Core
Imports Windows.Services.Store
Imports Windows.Storage
Imports Windows.System
Imports Windows.System.Threading
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Home"), FontAwesome5.EFontAwesomeIcon.Solid_Home))
        nvPrincipal.MenuItems.Add(Interfaz.Busqueda)
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Deals2"), Nothing))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Bundles2"), Nothing))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Free2"), Nothing))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Subscriptions2"), Nothing))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Wishlist"), Nothing))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Giveaways"), Nothing))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("MoreThings"), FontAwesome5.EFontAwesomeIcon.Solid_Cube))

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
                    Interfaz.Pestañas.Visibilidad_Pestañas(gridDeseados, Nothing)
                ElseIf item.Text = recursos.GetString("Giveaways") Then
                    Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/giveaways/"))
                ElseIf item.Text = recursos.GetString("MoreThings") Then
                    Interfaz.Pestañas.Visibilidad_Pestañas(gridMasCosas, Nothing)
                End If
            End If
        End If

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        CargarEntradas(100, Nothing, True)

        Deseados.Cargar()
        Trial.Detectar(True)
        Interfaz.Inicio.Cargar()
        Interfaz.RedesSociales.Cargar()
        MasCosas.Cargar()

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If config.Values("Calificar_App") = 0 Then
            Dim periodoCalificar As TimeSpan = TimeSpan.FromSeconds(300)
            Dim contadorCalificar As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                               Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                                                                                MasCosas.CalificarApp(True)
                                                                                                                                                                                            End Sub)
                                                                                           End Sub, periodoCalificar)
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







End Class
