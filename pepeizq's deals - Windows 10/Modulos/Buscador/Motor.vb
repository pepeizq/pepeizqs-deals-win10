Imports Microsoft.Toolkit.Uwp.UI.Controls

Namespace Buscador
    Module Motor

        Public Async Sub BuscarWebSteam(sender As Object, e As TextChangedEventArgs)

            Dim tb As TextBox = sender

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
            Dim spCarga As StackPanel = pagina.FindName("spBusquedaCarga")
            Dim sv As ScrollViewer = pagina.FindName("svBusquedaEntradas")

            Dim tbNoResultados As TextBlock = pagina.FindName("tbBusquedaNoResultados")
            tbNoResultados.Visibility = Visibility.Collapsed

            If tb.Text.Trim.Length > 0 Then
                gridBusqueda.Visibility = Visibility.Visible
                spCarga.Visibility = Visibility.Visible
                sv.Visibility = Visibility.Collapsed
                tbNoResultados.Visibility = Visibility.Collapsed

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
                    tbNoResultados.Visibility = Visibility.Collapsed
                Else
                    sv.Visibility = Visibility.Collapsed
                    tbNoResultados.Visibility = Visibility.Visible
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

        End Sub

        Public Async Sub BuscarJuego(juego As SteamWeb)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

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

            Dim steam As Tienda = Await Tiendas.SteamAPI.Buscar(juego.ID)

            If Not steam Is Nothing Then
                gvTiendas.Items.Add(ResultadoTienda(steam))
            End If

            Dim humble As Tienda = Await Tiendas.Humble.Buscar(juego.Titulo)

            If Not humble Is Nothing Then
                gvTiendas.Items.Add(ResultadoTienda(humble))
            End If

            Dim gamersgate As Tienda = Await Tiendas.GamersGate.Buscar(juego.Titulo)

            If Not gamersgate Is Nothing Then
                gvTiendas.Items.Add(ResultadoTienda(gamersgate))
            End If

            Dim fanatical As Tienda = Await Tiendas.Fanatical.Buscar(juego.Titulo, juego.ID)

            If Not fanatical Is Nothing Then
                gvTiendas.Items.Add(ResultadoTienda(fanatical))
            End If

            Dim gamesplanet As Tienda = Await Tiendas.Gamesplanet.Buscar(juego.Titulo, juego.ID)

            If Not gamesplanet Is Nothing Then
                gvTiendas.Items.Add(ResultadoTienda(gamesplanet))
            End If

            pb.Visibility = Visibility.Collapsed
        End Sub

    End Module
End Namespace

