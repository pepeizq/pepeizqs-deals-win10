Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.Storage

Namespace Buscador.Tiendas
    Module WinGameStore

        Dim tiendas As List(Of Tienda)
        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim dolar As String
        Dim nuevaTienda As Tienda

        Public Sub Buscar(tiendas_ As List(Of Tienda), titulo_ As String)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            If Pais.DetectarEuro = True Then
                If config.Values("Estado_App") = 1 Then
                    dolar = config.Values("dolar")
                End If
            End If

            tiendas = tiendas_
            titulo = titulo_

            nuevaTienda = Nothing

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Try
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.macgamestore.com/affiliate/feeds/p_C1B2A3.json"))
                Dim html As String = html_.Result

                If Not html = Nothing Then
                    Dim listaJuegos As List(Of WinGameStoreJuego) = JsonConvert.DeserializeObject(Of List(Of WinGameStoreJuego))(html)

                    If Not listaJuegos Is Nothing Then
                        If listaJuegos.Count > 0 Then
                            For Each juego In listaJuegos
                                If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Then
                                    Dim enlace As String = juego.Enlace

                                    Dim precio As String = String.Empty

                                    If Not juego.PrecioRebajado = Nothing Then
                                        precio = juego.PrecioRebajado
                                    End If

                                    If precio = String.Empty Then
                                        precio = juego.PrecioBase
                                    End If

                                    If Not precio = String.Empty Then
                                        Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                        Dim moneda As String = "USD"

                                        Dim formateador As New CurrencyFormatter(moneda) With {
                                            .Mode = CurrencyFormatterMode.UseSymbol
                                        }

                                        precio = formateador.Format(tempDouble)

                                        If Not dolar = Nothing Then
                                            precio = Divisas.CambioMoneda(precio, dolar)
                                        End If

                                        nuevaTienda = New Tienda(Referidos.Generar(enlace), precio, "Assets/Tiendas/wingamestore3.png", Nothing, Nothing)
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

        End Sub

        Private Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            If Not nuevaTienda Is Nothing Then
                Interfaz.Buscador.AñadirTienda(tiendas, nuevaTienda)
            End If

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Value = pb.Value + 1

            If pb.Value = pb.Maximum Then
                pb.Visibility = Visibility.Collapsed
            End If

        End Sub

    End Module

    Public Class WinGameStoreJuego

        <JsonProperty("title")>
        Public Titulo As String

        <JsonProperty("url")>
        Public Enlace As String

        <JsonProperty("current_price")>
        Public PrecioRebajado As String

        <JsonProperty("retail_price")>
        Public PrecioBase As String

        <JsonProperty("drmid")>
        Public SteamID As String

    End Class
End Namespace

