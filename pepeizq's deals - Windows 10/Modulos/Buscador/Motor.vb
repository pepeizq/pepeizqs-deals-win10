Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage

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

                    Tiendas.Steam.Buscar(tb.Text.Trim)
                End If
            End If

        End Sub

        Public Sub BuscarJuego(juego As SteamWeb)

            GridVisibilidad.Mostrar("gridBusquedaJuego")

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim imagen As ImageEx = pagina.FindName("imagenBusquedaJuego")
            imagen.Source = juego.Imagen

            Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")
            gvTiendas.Items.Clear()

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Visibility = Visibility.Visible

            Dim i As Integer = 0

            '---------------------------------

            Try
                i += 1
                Tiendas.SteamAPI.Buscar(juego.ID)
            Catch ex As Exception

            End Try

            If config.Values("Estado_App") = 1 Then
                Try
                    i += 1
                    Comparadores.SteamDB.Buscar(juego.ID)
                Catch ex As Exception

                End Try

                Try
                    i += 1
                    Comparadores.Isthereanydeal.Buscar(juego.ID)
                Catch ex As Exception

                End Try

                Try
                    i += 1
                    Comparadores.GGdeals.Buscar(juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            Try
                i += 1
                Tiendas.Humble.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GamersGate.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Dim pais As New Windows.Globalization.GeographicRegion

            If Not pais.CodeTwoLetter.ToLower = "uk" Then
                Try
                    i += 1
                    Tiendas.GamersGateUK.Buscar(juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            Try
                i += 1
                Tiendas.Fanatical.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GamesplanetUK.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GamesplanetFR.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GamesplanetDE.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GamesplanetUS.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.GreenManGaming.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                i += 1
                Tiendas.WinGameStore.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            '---------------------------------

            pb.Maximum = i

        End Sub

    End Module
End Namespace

