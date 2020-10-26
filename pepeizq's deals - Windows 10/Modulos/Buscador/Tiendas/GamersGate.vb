Imports System.Globalization
Imports System.Xml.Serialization
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module GamersGate

        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim tienda As Tienda

        Public Sub Buscar(titulo_ As String)

            titulo = titulo_

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Dim pais As New Windows.Globalization.GeographicRegion
            Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.gamersgate.com/feeds/products?country=" + pais.CodeThreeLetter.ToLower + "&q=" + titulo))
            Dim html As String = html_.Result

            If Not html = Nothing Then
                Dim stream As New StringReader(html)
                Dim xml As New XmlSerializer(GetType(GamersGateJuegos))
                Dim listaJuegos As GamersGateJuegos = xml.Deserialize(stream)

                If Not listaJuegos Is Nothing Then
                    If listaJuegos.Juegos.Count > 0 Then
                        For Each juego In listaJuegos.Juegos
                            If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Then

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

                                    Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                    Dim formateador As New CurrencyFormatter(moneda) With {
                                        .Mode = CurrencyFormatterMode.UseSymbol
                                    }

                                    precio = formateador.Format(tempDouble)

                                    tienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/gamersgate3.png")
                                End If
                            End If
                        Next
                    End If
                End If
            End If

        End Sub

        Private Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            If Not tienda Is Nothing Then
                Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")
                gvTiendas.Items.Add(ResultadoTienda(tienda, Nothing, Nothing))
            End If

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Value = pb.Value + 1

            If pb.Value = pb.Maximum Then
                pb.Visibility = Visibility.Collapsed
            End If

        End Sub

    End Module

    <XmlRoot("xml")>
    Public Class GamersGateJuegos

        <XmlElement("item")>
        Public Juegos As List(Of GamersGateJuego)

    End Class

    Public Class GamersGateJuego

        <XmlElement("title")>
        Public Titulo As String

        <XmlElement("link")>
        Public Enlace As String

        <XmlElement("price")>
        Public PrecioDescontado As String

        <XmlElement("srp")>
        Public PrecioBase As String

    End Class
End Namespace

