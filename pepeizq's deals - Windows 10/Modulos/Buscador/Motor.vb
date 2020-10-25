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

                Dim spCarga As StackPanel = pagina.FindName("spBusquedaCarga")
                Dim sv As ScrollViewer = pagina.FindName("svBusquedaEntradas")

                If tb.Text.Trim.Length > 0 Then
                    GridVisibilidad.Mostrar("gridBusqueda")
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
                End If
            End If

        End Sub

        Public Sub BuscarJuego(juego As SteamWeb)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            GridVisibilidad.Mostrar("gridBusquedaJuego")

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
                Tiendas.SteamAPI.Buscar(juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 1"

            Try
                Tiendas.Humble.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 2"

            Try
                Tiendas.GamersGate.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Dim pais As New Windows.Globalization.GeographicRegion

            If Not pais.CodeTwoLetter.ToLower = "uk" Then
                Try
                    Tiendas.GamersGateUK.Buscar(juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            tbMensaje.Text = recursos.GetString("Scanning") + " 3"

            Try
                Tiendas.Fanatical.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 4"

            Try
                Tiendas.GamesplanetUK.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 5"

            Try
                Tiendas.GamesplanetFR.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 6"

            Try
                Tiendas.GamesplanetDE.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 7"

            Try
                Tiendas.GamesplanetUS.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 8"

            Try
                Tiendas.GreenManGaming.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            tbMensaje.Text = recursos.GetString("Scanning") + " 9"

            Try
                Tiendas.WinGameStore.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            '---------------------------------

            pb.Visibility = Visibility.Collapsed
            tbMensaje.Text = String.Empty
        End Sub

    End Module
End Namespace

