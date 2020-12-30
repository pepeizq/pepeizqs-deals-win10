Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports pepeizq_s_deals___Windows_10.Buscador.Tiendas
Imports Windows.Storage

Namespace Buscador
    Module Motor

        Dim busqueda As String

        Public Async Sub Buscar(sender As Object, e As TextChangedEventArgs)

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
                Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
                Dim spCarga As StackPanel = pagina.FindName("spBusquedaCarga")
                Dim spBusquedaWeb As StackPanel = pagina.FindName("spBusquedaEntradas")

                If tb.Text.Trim.Length > 0 Then
                    Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
                    Interfaz.Pestañas.Visibilidad(gridBusqueda, Nothing, Nothing)

                    spCarga.Visibility = Visibility.Visible
                    spBusquedaWeb.Visibility = Visibility.Collapsed

                    Dim listadoEncontrados As New List(Of FiltroEntradaDeseado)
                    Dim entradas As New List(Of Entrada)

                    For Each grid In spEntradas.Children
                        If TypeOf grid Is Grid Then
                            Dim grid2 As Grid = grid
                            Dim entrada As Entrada = grid2.Tag

                            If Not entrada Is Nothing Then
                                entradas.Add(entrada)
                            End If
                        End If
                    Next

                    Dim spResultados As StackPanel = pagina.FindName("spBusquedaEntradasResultados")
                    spResultados.Children.Clear()

                    For Each entrada In entradas
                        If entrada.Categorias(0) = 3 Then
                            If Not entrada.JsonExpandido = Nothing Then
                                Dim json As EntradaOfertas = JsonConvert.DeserializeObject(Of EntradaOfertas)(entrada.JsonExpandido)

                                If Not json Is Nothing Then
                                    If Not json.Juegos Is Nothing Then
                                        For Each juego2 In json.Juegos
                                            If config.Values("Busqueda") = 1 Then
                                                If Limpieza.Limpiar(juego2.Titulo) = Limpieza.Limpiar(WebUtility.HtmlDecode(busqueda)) Then
                                                    listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                                End If
                                            ElseIf config.Values("Busqueda") = 0 Then
                                                If Limpieza.Limpiar(juego2.Titulo).Contains(Limpieza.Limpiar(WebUtility.HtmlDecode(busqueda))) Then
                                                    listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            Else
                                Dim añadir As Boolean = False

                                If Not entrada.Titulo Is Nothing Then
                                    If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(busqueda)) Then
                                        añadir = True
                                    End If
                                End If

                                If Not entrada.SubTitulo = Nothing Then
                                    If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(busqueda)) Then
                                        añadir = True
                                    End If
                                End If

                                If añadir = True Then
                                    listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                                End If
                            End If
                        Else
                            Dim añadir As Boolean = False

                            If Not entrada.Titulo Is Nothing Then
                                If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(busqueda)) Then
                                    añadir = True
                                End If
                            End If

                            If Not entrada.SubTitulo = Nothing Then
                                If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(busqueda)) Then
                                    añadir = True
                                End If
                            End If

                            If añadir = True Then
                                listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                            End If
                        End If
                    Next

                    spCarga.Visibility = Visibility.Collapsed

                    If listadoEncontrados.Count > 0 Then
                        spResultados.Children.Clear()

                        For Each resultado In listadoEncontrados
                            Dim listaJuegos As New List(Of EntradaOfertasJuego) From {
                                resultado.Juego
                            }

                            Dim json As String = GenerarJsonOfertas(listaJuegos, Nothing)

                            If resultado.Juego Is Nothing Then
                                If resultado.Entrada.Categorias(0) = 3 Then
                                    json = resultado.Entrada.Json
                                End If
                            End If

                            Dim entrada As New Entrada With {
                                .TiendaLogo = resultado.Entrada.TiendaLogo,
                                .Json = json
                            }
                            entrada.Categorias = resultado.Entrada.Categorias

                            spResultados.Children.Add(Interfaz.Entradas.GenerarEntrada(entrada))
                        Next

                        spBusquedaWeb.Visibility = Visibility.Visible
                    Else
                        spBusquedaWeb.Visibility = Visibility.Collapsed
                    End If

                    Dim spBusquedaSteam As StackPanel = pagina.FindName("spBusquedaSteam")

                    If config.Values("BusquedaSteam") = 1 Then
                        spBusquedaSteam.Visibility = Visibility.Visible

                        Dim gvResultados As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")
                        gvResultados.Items.Clear()

                        Steam.Buscar(tb.Text.Trim)
                    Else
                        spBusquedaSteam.Visibility = Visibility.Collapsed
                    End If
                End If
            End If

        End Sub

        Public Async Sub BuscarJuego(juego As SteamWeb)

            Dim tiendas As New List(Of Tienda)
            tiendas.Clear()

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridBusquedaJuego As Grid = pagina.FindName("gridBusquedaJuego")
            Interfaz.Pestañas.Visibilidad(gridBusquedaJuego, juego.Titulo, Nothing)

            Dim fondo As String = SteamAPI.dominioImagenes + "/steam/apps/" + juego.ID + "/page_bg_generated_v6b.jpg"
            Dim fondoBrush As New ImageBrush With {
                .ImageSource = New BitmapImage(New Uri(fondo)),
                .Stretch = Stretch.UniformToFill
            }
            gridBusquedaJuego.Background = fondoBrush

            Dim imagen As ImageEx = pagina.FindName("imagenBusquedaJuego")
            imagen.Source = juego.Imagen

            Dim lvTiendas As ListView = pagina.FindName("lvBusquedaJuegoTiendas")
            lvTiendas.Items.Clear()

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Visibility = Visibility.Visible
            pb.Value = 0

            Dim i As Integer = 0

            '---------------------------------

            Try
                Await Task.Delay(100)
                i += 1
                SteamAPI.Buscar(tiendas, juego.ID)
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
                Humble.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GamersGate.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            Dim pais As New Windows.Globalization.GeographicRegion

            If Not pais.CodeTwoLetter.ToLower = "uk" Then
                Try
                    Await Task.Delay(100)
                    i += 1
                    GamersGateUK.Buscar(tiendas, juego.Titulo)
                Catch ex As Exception

                End Try
            End If

            Try
                Await Task.Delay(100)
                i += 1
                Fanatical.Buscar(tiendas, juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GamesplanetUK.Buscar(tiendas, juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GamesplanetFR.Buscar(tiendas, juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GamesplanetDE.Buscar(tiendas, juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GamesplanetUS.Buscar(tiendas, juego.Titulo, juego.ID)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                GreenManGaming.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                WinGameStore.Buscar(tiendas, juego.Titulo)
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
                GOG.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Allyouplay.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            Try
                Await Task.Delay(100)
                i += 1
                Voidu.Buscar(tiendas, juego.Titulo)
            Catch ex As Exception

            End Try

            '---------------------------------

            pb.Maximum = i

        End Sub

    End Module
End Namespace

