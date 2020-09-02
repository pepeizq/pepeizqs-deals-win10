Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.Core
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Dim entradas As List(Of Entrada)
    Dim juegos As List(Of Entrada)

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Home"), FontAwesome5.EFontAwesomeIcon.Solid_Home, 0))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Search"), FontAwesome5.EFontAwesomeIcon.Solid_Search, 1))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Deals2"), Nothing, 2))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Bundles2"), Nothing, 3))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Free2"), Nothing, 4))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Subscriptions2"), Nothing, 5))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(MasCosas.Generar("https://github.com/pepeizq/pepeizqs-deals-win10", Nothing, "https://www.youtube.com/watch?v=uF6zm8cTakE"))

    End Sub

    Private Async Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If Not item Is Nothing Then
            If item.Text = recursos.GetString("Home") Then
                CargarEntradas(entradas, 100, Nothing, 0, False)
            ElseIf item.Text = recursos.GetString("Search") Then
                Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/search/"))
            ElseIf item.Text = recursos.GetString("Deals2") Then
                CargarEntradas(entradas, 100, recursos.GetString("Deals2"), 2, False)
            ElseIf item.Text = recursos.GetString("Bundles2") Then
                CargarEntradas(entradas, 100, recursos.GetString("Bundles2"), 1, False)
            ElseIf item.Text = recursos.GetString("Free2") Then
                CargarEntradas(entradas, 100, recursos.GetString("Free2"), 3, False)
            ElseIf item.Text = recursos.GetString("Subscriptions2") Then
                CargarEntradas(entradas, 100, recursos.GetString("Subscriptions2"), 4, False)
            ElseIf item.Text = recursos.GetString("MoreThings") Then
                FlyoutBase.ShowAttachedFlyout(nvPrincipal.MenuItems.Item(nvPrincipal.MenuItems.Count - 1))
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

        entradas = New List(Of Entrada)
        juegos = New List(Of Entrada)

        CargarEntradas(entradas, 100, Nothing, 0, True)
        CargarJuegos(juegos, 20)

    End Sub

    Public Async Sub CargarEntradas(entradas As List(Of Entrada), paginas As Integer, categoria As String, tipo As Integer, actualizar As Boolean)

        tbTitulo.Text = "pepeizq's deals (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        If Not categoria = Nothing Then
            tbTitulo.Text = tbTitulo.Text + " • " + categoria
        End If

        '--------------------------------------

        gridCarga.Visibility = Visibility.Visible
        gridEntradas.Visibility = Visibility.Collapsed

        spEntradas.Children.Clear()

        Dim recursos As New Resources.ResourceLoader()

        Dim categoriaNumero As Integer = Nothing

        If categoria = recursos.GetString("Bundles2") Then
            categoriaNumero = 4
        ElseIf categoria = recursos.GetString("Deals2") Then
            categoriaNumero = 3
        ElseIf categoria = recursos.GetString("Free2") Then
            categoriaNumero = 12
        ElseIf categoria = recursos.GetString("Subscriptions2") Then
            categoriaNumero = 13
        End If

        If actualizar = True Then
            Dim nuevasEntradas As List(Of Entrada) = Await Wordpress.Cargar("posts", paginas, categoriaNumero)

            For Each nuevaEntrada In nuevasEntradas
                Dim añadir As Boolean = True

                For Each viejaEntrada In entradas
                    If viejaEntrada.ID = nuevaEntrada.ID Then
                        añadir = False
                    End If
                Next

                If añadir = True Then
                    entradas.Add(nuevaEntrada)
                End If
            Next
        End If

        If entradas.Count > 0 Then
            For Each entrada In entradas
                Dim añadir As Boolean = False

                If tipo = 0 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Or categoria = 3 Or categoria = 12 Or categoria = 13 Then
                            añadir = True
                        End If
                    Next
                ElseIf tipo = 1 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Then
                            añadir = True
                        End If
                    Next
                ElseIf tipo = 2 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 3 Then
                            añadir = True
                        End If
                    Next
                ElseIf tipo = 3 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 12 Then
                            añadir = True
                        End If
                    Next
                ElseIf tipo = 4 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 13 Then
                            añadir = True
                        End If
                    Next
                End If

                If spEntradas.Children.Count > 0 Then
                    For Each item In spEntradas.Children
                        If TypeOf item Is DropShadowPanel Then
                            Dim panel As DropShadowPanel = item
                            Dim panelEntrada As Entrada = panel.Tag

                            If entrada.Enlace = panelEntrada.Enlace Then
                                añadir = False
                            End If
                        End If
                    Next
                End If

                If añadir = True Then
                    spEntradas.Children.Add(Interfaz.GenerarEntrada(entrada))
                    spEntradas.Children.Add(Interfaz.GenerarCompartir(entrada))
                End If

                '-------------------------------------------------------------

                Dim mostrarAnuncio As Boolean = False

                For Each categoria In entrada.Categorias
                    If categoria = 1208 Then
                        mostrarAnuncio = True
                    End If
                Next

                If mostrarAnuncio = True Then
                    Dim mostrarAnuncio2 As Boolean = True

                    Dim helper As New LocalObjectStorageHelper
                    Dim listaAnuncios As New List(Of Anuncio)

                    If Await helper.FileExistsAsync("listaAnuncios") Then
                        listaAnuncios = Await helper.ReadFileAsync(Of List(Of Anuncio))("listaAnuncios")
                    End If

                    If Not listaAnuncios Is Nothing Then
                        If listaAnuncios.Count > 0 Then
                            For Each item In listaAnuncios
                                If entrada.Enlace = item.Enlace Then
                                    mostrarAnuncio2 = False
                                End If
                            Next
                        End If
                    End If

                    If mostrarAnuncio2 = True Then
                        Notificaciones.ToastAnuncio(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen)
                        listaAnuncios.Add(New Anuncio(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen))

                        Try
                            Await helper.SaveFileAsync(Of List(Of Anuncio))("listaAnuncios", listaAnuncios)
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Next
        End If

        gridCarga.Visibility = Visibility.Collapsed
        gridEntradas.Visibility = Visibility.Visible

    End Sub

    Public Async Sub CargarJuegos(juegos As List(Of Entrada), paginas As Integer)

        Dim nuevosJuegos As List(Of Entrada) = Await Wordpress.Cargar("us_portfolio", paginas, Nothing)

        For Each nuevoJuego In nuevosJuegos
            Dim añadir As Boolean = True

            For Each viejoJuego In juegos
                If viejoJuego.ID = nuevoJuego.ID Then
                    añadir = False
                End If
            Next

            If añadir = True Then
                juegos.Add(nuevoJuego)
            End If
        Next

        If juegos.Count > 0 Then
            gvNuevosJuegos.Items.Clear()

            Dim r As Random = New Random
            Dim exclusive() As Integer = Enumerable.Range(0, juegos.Count).OrderBy(Function(n) r.Next(juegos.Count + 1)).ToArray()
            Dim shuffled As New List(Of Entrada)

            Array.ForEach(exclusive, Sub(e) shuffled.Add(juegos(e)))

            Dim i As Integer = 0
            For Each subjuego In shuffled
                If i < 6 Then
                    If Not subjuego.Imagen2 = Nothing Then
                        subjuego.Imagen2 = subjuego.Imagen2.Replace("<img src=" + ChrW(34), Nothing)
                        subjuego.Imagen2 = subjuego.Imagen2.Replace(ChrW(34) + " class=" + ChrW(34) + "ajustarImagen" + ChrW(34) + "/>", Nothing)
                        subjuego.Imagen = subjuego.Imagen2

                        gvNuevosJuegos.Items.Add(Interfaz.GenerarJuego(subjuego))
                    End If
                End If
                i += 1
            Next
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
            CargarEntradas(entradas, 100, recursos.GetString("Bundles2"), 1, True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
            CargarEntradas(entradas, 100, recursos.GetString("Deals2"), 2, True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
            CargarEntradas(entradas, 100, recursos.GetString("Free2"), 3, True)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
            CargarEntradas(entradas, 100, recursos.GetString("Subscriptions2"), 4, True)
        Else
            CargarEntradas(entradas, 100, Nothing, 0, True)
        End If

    End Sub

    Private Sub BotonSubir_Click(sender As Object, e As RoutedEventArgs) Handles botonSubir.Click

        svEntradas.ChangeView(Nothing, 0, Nothing)

    End Sub

    Private Async Sub BotonAbrirNuevosJuegos_Click(sender As Object, e As RoutedEventArgs) Handles botonAbrirNuevosJuegos.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqdeals.com/incoming/"))

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

End Class
