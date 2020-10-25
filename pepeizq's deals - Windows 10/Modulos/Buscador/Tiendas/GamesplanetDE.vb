Imports System.Globalization
Imports System.Xml.Serialization
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Globalization.NumberFormatting

Namespace Buscador.Tiendas
    Module GamesplanetDE

        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim id As String
        Dim tienda As Tienda

        Public Sub Buscar(titulo_ As String, id_ As String)

            titulo = titulo_
            id = id_

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Dim html_ As Task(Of String) = HttpClient(New Uri("https://de.gamesplanet.com/api/v1/products/feed.xml"))
            Dim html As String = html_.Result

            If Not html = Nothing Then
                Dim stream As New StringReader(html)
                Dim xml As New XmlSerializer(GetType(GamesPlanetJuegos))
                Dim listaJuegos As GamesPlanetJuegos = xml.Deserialize(stream)

                If Not listaJuegos Is Nothing Then
                    If listaJuegos.Juegos.Count > 0 Then
                        For Each juego In listaJuegos.Juegos
                            If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Or id = juego.SteamID Then

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

                                    Dim moneda As String = "EUR"

                                    Dim formateador As New CurrencyFormatter(moneda) With {
                                        .Mode = CurrencyFormatterMode.UseSymbol
                                    }

                                    precio = formateador.Format(tempDouble)

                                    tienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/gamesplanet3.png")
                                End If
                            End If
                        Next
                    End If
                End If
            End If

        End Sub

        Private Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            If Not tienda Is Nothing Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")

                gvTiendas.Items.Add(ResultadoTienda(tienda, "de", Nothing))
            End If

        End Sub

    End Module
End Namespace
