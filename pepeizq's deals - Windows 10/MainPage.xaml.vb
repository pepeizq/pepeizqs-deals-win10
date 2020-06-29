Imports Windows.ApplicationModel.Core
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        '--------------------------------------------------------

        CargaListado(10, Nothing, 0, spSeleccionarTodo)
        CargaListado(100, Nothing, 0, spSeleccionarTodo)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        Dim barra2 As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView().TitleBar
        barra2.ExtendViewIntoTitleBar = True

    End Sub

    Public Async Sub CargaListado(paginas As Integer, categoria As String, tipo As Integer, sp As StackPanel)

        tbTitulo.Text = "pepeizq's deals (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        If Not categoria = Nothing Then
            tbTitulo.Text = tbTitulo.Text + " • " + categoria
        End If

        spBotonesSeleccion.IsHitTestVisible = False

        spSeleccionarTodo.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarBundles.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarOfertas.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarGratis.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarSuscripciones.BorderThickness = New Thickness(0, 0, 0, 0)

        sp.BorderThickness = New Thickness(0, 0, 0, 2)

        botonActualizar.IsHitTestVisible = False

        '--------------------------------------

        lvPrincipal.IsItemClickEnabled = False
        lvAnuncios.IsItemClickEnabled = False

        prPrincipal.Visibility = Visibility.Visible
        prPrincipal.IsActive = True

        lvPrincipal.Items.Clear()

        If categoria = Nothing Then
            lvAnuncios.Items.Clear()
        End If

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

        Dim entradas As List(Of Entrada) = Await Wordpress.CargarEntradas(paginas, categoriaNumero)

        If entradas.Count > 0 Then
            For Each entrada In entradas
                Dim añadirPrincipal As Boolean = False

                If tipo = 0 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Or categoria = 3 Or categoria = 12 Or categoria = 13 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 1 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 2 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 3 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 3 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 12 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 4 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 13 Then
                            añadirPrincipal = True
                        End If
                    Next
                End If

                If lvPrincipal.Items.Count > 0 Then
                    For Each item In lvPrincipal.Items
                        If TypeOf item Is Button Then
                            Dim boton As Button = item
                            Dim botonEntrada As Entrada = boton.Tag

                            If entrada.Enlace = botonEntrada.Enlace Then
                                añadirPrincipal = False
                            End If
                        End If
                    Next
                End If

                If añadirPrincipal = True Then
                    lvPrincipal.Items.Add(Interfaz.GenerarEntrada(entrada))
                    lvPrincipal.Items.Add(Interfaz.GenerarCompartir(entrada))
                End If

                Dim añadirAnuncio As Boolean = False

                For Each categoria In entrada.Categorias
                    If categoria = 1208 Then
                        If lvAnuncios.Items.Count < 2 Then
                            añadirAnuncio = True
                        End If
                    End If
                Next

                If lvAnuncios.Items.Count > 0 Then
                    For Each item In lvAnuncios.Items
                        If TypeOf item Is Button Then
                            Dim boton As Button = item
                            Dim botonEntrada As Entrada = boton.Tag

                            If entrada.Enlace = botonEntrada.Enlace Then
                                añadirAnuncio = False
                            End If
                        End If
                    Next
                End If

                If añadirAnuncio = True Then
                    lvAnuncios.Items.Add(Interfaz.GenerarAnuncio(entrada))
                End If

                Dim añadirWeb As Boolean = False

                For Each categoria In entrada.Categorias
                    If categoria = 1242 Then
                        añadirWeb = True

                        If lvAnuncios.Items.Count > 2 Then
                            añadirWeb = False
                        End If
                    End If
                Next

                If lvAnuncios.Items.Count > 0 Then
                    For Each item In lvAnuncios.Items
                        If TypeOf item Is Button Then
                            Dim boton As Button = item
                            Dim botonEntrada As Entrada = boton.Tag

                            If entrada.Enlace = botonEntrada.Enlace Then
                                añadirWeb = False
                            End If
                        End If
                    Next
                End If

                If añadirWeb = True Then
                    lvAnuncios.Items.Add(Interfaz.GenerarAnuncio(entrada))
                End If
            Next
        End If

        prPrincipal.Visibility = Visibility.Collapsed
        prPrincipal.IsActive = False

        botonActualizar.IsHitTestVisible = True
        spBotonesSeleccion.IsHitTestVisible = True

        lvPrincipal.IsItemClickEnabled = True
        lvAnuncios.IsItemClickEnabled = True

    End Sub

    Private Sub BotonSeleccionarTodo_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarTodo.Click

        CargaListado(100, Nothing, 0, spSeleccionarTodo)

    End Sub

    Private Sub BotonSeleccionarBundles_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarBundles.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(100, recursos.GetString("Bundles2"), 1, spSeleccionarBundles)

    End Sub

    Private Sub BotonSeleccionarOfertas_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarOfertas.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(100, recursos.GetString("Deals2"), 2, spSeleccionarOfertas)

    End Sub

    Private Sub BotonSeleccionarGratis_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarGratis.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(100, recursos.GetString("Free2"), 3, spSeleccionarGratis)

    End Sub

    Private Sub BotonSeleccionarSuscripciones_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarSuscripciones.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(100, recursos.GetString("Subscriptions2"), 4, spSeleccionarSuscripciones)

    End Sub

    Private Sub BotonActualizar_Click(sender As Object, e As RoutedEventArgs) Handles botonActualizar.Click

        Dim recursos As New Resources.ResourceLoader()

        If tbTitulo.Text.Contains(recursos.GetString("Bundles2")) Then
            CargaListado(100, recursos.GetString("Bundles2"), 1, spSeleccionarBundles)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
            CargaListado(100, recursos.GetString("Deals2"), 2, spSeleccionarOfertas)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
            CargaListado(100, recursos.GetString("Free2"), 3, spSeleccionarGratis)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
            CargaListado(100, recursos.GetString("Subscriptions2"), 4, spSeleccionarSuscripciones)
        Else
            CargaListado(100, Nothing, 0, spSeleccionarTodo)
        End If

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

    Private Async Sub BotonGithub_Click(sender As Object, e As RoutedEventArgs) Handles botonGithub.Click

        Await Launcher.LaunchUriAsync(New Uri("https://github.com/pepeizq/pepeizqs-deals-win10"))

    End Sub

    Private Async Sub BotonVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Public Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Class
