Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Home"), FontAwesome5.EFontAwesomeIcon.Solid_Home))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Wishlist"), FontAwesome5.EFontAwesomeIcon.Solid_Star))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesome5.EFontAwesomeIcon.Solid_Cog))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        If gridCarga.Visibility = Visibility.Collapsed Then
            If TypeOf args.InvokedItem Is TextBlock Then
                gridBusqueda.Visibility = Visibility.Collapsed
                gridBusquedaJuego.Visibility = Visibility.Collapsed

                Dim item As TextBlock = args.InvokedItem

                If Not item Is Nothing Then
                    If item.Text = recursos.GetString("Home") Then
                        CargarEntradas(100, Nothing, False, False)
                    ElseIf item.Text = recursos.GetString("Wishlist") Then
                        Interfaz.Pestañas.Visibilidad(gridDeseados, item.Text, item)
                    ElseIf item.Text = recursos.GetString("Config") Then
                        Interfaz.Pestañas.Visibilidad(gridConfig, item.Text, item)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Wordpress.CargarEntradas(100, Nothing, True, True)

        Deseados.Cargar()
        Trial.Detectar(True)
        Interfaz.Inicio.Cargar()
        Interfaz.Buscador.Cargar()
        Interfaz.Submenu.Cargar()
        Configuracion.Cargar()
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
            gridEntradasSubmenu.Visibility = Visibility.Collapsed
            botonSubir.Visibility = Visibility.Visible
        Else
            gridEntradasSubmenu.Visibility = Visibility.Visible
            botonSubir.Visibility = Visibility.Collapsed
        End If

    End Sub

End Class
