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
                Dim spEntradas As StackPanel = pagina.FindName("spBusquedaEntradas")

                If tb.Text.Trim.Length > 0 Then
                    GridVisibilidad.Mostrar("gridBusqueda")
                    spCarga.Visibility = Visibility.Visible
                    spEntradas.Visibility = Visibility.Collapsed

                    Dim spResultados As StackPanel = pagina.FindName("spBusquedaEntradasResultados")
                    spResultados.Children.Clear()

                    Dim resultadosWeb As List(Of pepeizqdeals) = Await Wordpress.Buscar(tb.Text.Trim)

                    spCarga.Visibility = Visibility.Collapsed

                    If resultadosWeb.Count > 0 Then
                        spResultados.Children.Clear()

                        For Each resultado In resultadosWeb
                            spResultados.Children.Add(ResultadoWeb(resultado))
                        Next

                        spEntradas.Visibility = Visibility.Visible
                    Else
                        spEntradas.Visibility = Visibility.Collapsed
                    End If

                    Dim gvResultados As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")
                    gvResultados.Items.Clear()

                    Interfaz.IniciarTiendas()

                    Tiendas.Steam.Buscar(tb.Text.Trim)
                End If
            End If

        End Sub

        Public Async Sub BuscarJuego(juego As SteamWeb)

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
            pb.Value = 0

            Dim i As Integer = 0

            '---------------------------------

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.SteamAPI.Buscar(juego.ID)
            Catch ex As Exception

            End Try

            If config.Values("Estado_App") = 1 Then
                Try
                    Await Task.Delay(100)
                    i += 1
                    Comparadores.SteamDB.Buscar(juego.ID)
                Catch ex As Exception

                End Try

                Try
                    Await Task.Delay(100)
                    i += 1
                    Comparadores.Isthereanydeal.Buscar(juego.ID)
                Catch ex As Exception

                End Try

                Try
                    Await Task.Delay(100)
                    i += 1
                    Comparadores.GGdeals.Buscar(juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.Humble.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GamersGate.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Dim pais As New Windows.Globalization.GeographicRegion

            If Not pais.CodeTwoLetter.ToLower = "uk" Then
                Try
                    Await Task.Delay(100)
                    i += 1
                    Tiendas.GamersGateUK.Buscar(juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.Fanatical.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GamesplanetUK.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GamesplanetFR.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GamesplanetDE.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GamesplanetUS.Buscar(juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GreenManGaming.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.WinGameStore.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            'Try
            '    Await Task.Delay(100)
            '    i += 1
            '    Tiendas.IndieGala.Buscar(juego.Titulo)
            'Catch ex As Exception

            'End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.GOG.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.Allyouplay.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Tiendas.Voidu.Buscar(juego.Titulo)
            Catch ex As Exception

            End Try

            '---------------------------------

            pb.Maximum = i

        End Sub

    End Module
End Namespace

