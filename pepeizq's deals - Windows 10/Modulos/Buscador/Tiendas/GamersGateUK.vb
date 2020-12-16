Imports System.Globalization
Imports System.Xml.Serialization
Imports Windows.Globalization.NumberFormatting
Imports Windows.Storage

Namespace Buscador.Tiendas
    Module GamersGateUK

        Dim tiendas As List(Of Tienda)
        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim libra As String
        Dim nuevaTienda As Tienda

        Public Sub Buscar(tiendas_ As List(Of Tienda), titulo_ As String)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            If Pais.DetectarEuro = True Then
                If config.Values("Estado_App") = 1 Then
                    libra = config.Values("libra")
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
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.gamersgate.com/feeds/products?country=gbr&q=" + titulo))
                Dim html As String = html_.Result

                If Not html = Nothing Then
                    Dim stream As New StringReader(html)
                    Dim xml As New XmlSerializer(GetType(GamersGateJuegos))
                    Dim listaJuegos As GamersGateJuegos = xml.Deserialize(stream)

                    If Not listaJuegos Is Nothing Then
                        If listaJuegos.Juegos.Count > 0 Then
                            For Each juego In listaJuegos.Juegos
                                If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Then
                                    If juego.Estado.ToLower = "available" Or juego.Estado.ToLower = "preorder" Then
                                        Dim enlace As String = juego.Enlace

                                        Dim precio As String = String.Empty

                                        If Not juego.PrecioDescontado = Nothing Then
                                            precio = juego.PrecioDescontado
                                        End If

                                        If precio = String.Empty Then
                                            precio = juego.PrecioBase
                                        End If

                                        If Not precio = String.Empty Then
                                            Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                            Dim moneda As String = "GBP"

                                            Dim formateador As New CurrencyFormatter(moneda) With {
                                                .Mode = CurrencyFormatterMode.UseSymbol
                                            }

                                            precio = formateador.Format(tempDouble)

                                            If Not libra = Nothing Then
                                                precio = Divisas.CambioMoneda(precio, libra)
                                            End If

                                            nuevaTienda = New Tienda(Referidos.Generar(enlace), precio, "Assets/Tiendas/gamersgate3.png", Nothing, "uk")
                                        End If
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
End Namespace