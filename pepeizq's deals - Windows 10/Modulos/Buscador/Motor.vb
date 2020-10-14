Imports Microsoft.Toolkit.Uwp.UI.Controls

Namespace Buscador
    Module Motor

        Dim busqueda As String

        Public Async Sub BuscarWebSteam(sender As Object, e As TextChangedEventArgs)

            Dim tb As TextBox = sender
            Dim arrancar As Boolean = True

            Await Task.Delay(1000)

            If tb.Text.Trim.Length > 0 Then
                If busqueda = tb.Text.Trim Then
                    arrancar = False
                End If

                busqueda = tb.Text.Trim
            End If

            If arrancar = True Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gridJuego As Grid = pagina.FindName("gridBusquedaJuego")
                gridJuego.Visibility = Visibility.Collapsed

                Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
                Dim spCarga As StackPanel = pagina.FindName("spBusquedaCarga")
                Dim sv As ScrollViewer = pagina.FindName("svBusquedaEntradas")

                If tb.Text.Trim.Length > 0 Then
                    gridBusqueda.Visibility = Visibility.Visible
                    spCarga.Visibility = Visibility.Visible
                    sv.Visibility = Visibility.Collapsed

                    Dim spResultados As StackPanel = pagina.FindName("spBusquedaEntradas")
                    spResultados.Children.Clear()

                    Dim gvResultados As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")
                    gvResultados.Items.Clear()

                    Dim resultadosWeb As List(Of pepeizqdeals) = Await Wordpress.Buscar(tb.Text.Trim)

                    spCarga.Visibility = Visibility.Collapsed

                    If resultadosWeb.Count > 0 Then
                        spResultados.Children.Clear()

                        For Each resultado In resultadosWeb
                            spResultados.Children.Add(ResultadoWeb(resultado))
                        Next

                        sv.Visibility = Visibility.Visible
                    Else
                        sv.Visibility = Visibility.Collapsed
                    End If

                    Dim resultadosSteam As List(Of SteamWeb) = Await Tiendas.Steam.Buscar(tb.Text.Trim)

                    If resultadosSteam.Count > 0 Then
                        gvResultados.Items.Clear()

                        For Each resultado In resultadosSteam
                            gvResultados.Items.Add(ResultadoSteam(resultado))
                        Next

                    End If
                Else
                    gridBusqueda.Visibility = Visibility.Collapsed
                End If
            End If

        End Sub

        Public Async Sub BuscarJuego(juego As SteamWeb)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim nvPrincipal As NavigationView = pagina.FindName("nvPrincipal")

            For Each item In nvPrincipal.MenuItems
                If TypeOf item Is NavigationViewItem Then
                    Dim item2 As NavigationViewItem = item
                    item2.IsEnabled = False
                    item2.IsHitTestVisible = False
                ElseIf TypeOf item Is StackPanel Then
                    Dim item2 As StackPanel = item
                    item2.IsHitTestVisible = False
                End If
            Next

            Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
            gridBusqueda.Visibility = Visibility.Collapsed

            Dim gridJuego As Grid = pagina.FindName("gridBusquedaJuego")
            gridJuego.Visibility = Visibility.Visible

            Dim imagen As ImageEx = pagina.FindName("imagenBusquedaJuego")
            imagen.Source = juego.Imagen

            Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")
            gvTiendas.Items.Clear()

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Visibility = Visibility.Visible

            Dim tbMensaje As TextBlock = pagina.FindName("tbBusquedaJuego")
            Dim recursos As New Resources.ResourceLoader()

            '---------------------------------

            tbMensaje.Text = recursos.GetString("Scanning") + " 0"

            Try
                Dim steam As Tienda = Await Tiendas.SteamAPI.Buscar(juego.ID)

                If Not steam Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(steam))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 1"

            Try
                Dim humble As Tienda = Await Tiendas.Humble.Buscar(juego.Titulo)

                If Not humble Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(humble))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 2"

            Try
                Dim gamersgate As Tienda = Await Tiendas.GamersGate.Buscar(juego.Titulo)

                If Not gamersgate Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(gamersgate))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 3"

            Try
                Dim fanatical As Tienda = Await Tiendas.Fanatical.Buscar(juego.Titulo, juego.ID)

                If Not fanatical Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(fanatical))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 4"

            Try
                Dim gamesplanet As Tienda = Await Tiendas.Gamesplanet.Buscar(juego.Titulo, juego.ID)

                If Not gamesplanet Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(gamesplanet))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 5"

            Try
                Dim gmg As Tienda = Await Tiendas.GreenManGaming.Buscar(juego.Titulo)

                If Not gmg Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(gmg))
                End If
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 6"

            Try
                Dim wgs As Tienda = Await Tiendas.WinGameStore.Buscar(juego.Titulo)

                If Not wgs Is Nothing Then
                    gvTiendas.Items.Add(ResultadoTienda(wgs))
                End If
            Catch ex As Exception

            End Try

            '---------------------------------

            pb.Visibility = Visibility.Collapsed
            tbMensaje.Text = String.Empty

            For Each item In nvPrincipal.MenuItems
                If TypeOf item Is NavigationViewItem Then
                    Dim item2 As NavigationViewItem = item
                    item2.IsEnabled = True
                ElseIf TypeOf item Is StackPanel Then
                    Dim item2 As StackPanel = item
                    item2.IsHitTestVisible = True
                End If
            Next
        End Sub

    End Module
End Namespace

