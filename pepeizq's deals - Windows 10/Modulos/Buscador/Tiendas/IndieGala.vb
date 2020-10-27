Imports System.Globalization
Imports System.Net
Imports System.Xml.Serialization
Imports Windows.Globalization.NumberFormatting

Namespace Buscador.Tiendas
    Module IndieGala

        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim tienda As Tienda
        Dim salirBucles As Boolean

        Public Sub Buscar(titulo_ As String)

            salirBucles = False

            titulo = titulo_

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Try
                Dim numPaginas_ As Task(Of Integer) = GenerarNumPaginas(New Uri("https://www.indiegala.com/store_games_rss?page=1"))
                Dim numPaginas As Integer = numPaginas_.Result

                Dim i As Integer = 1
                While i < numPaginas
                    Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.indiegala.com/store_games_rss?page=" + i.ToString))
                    Dim html As String = html_.Result

                    If Not html = Nothing Then
                        Dim stream As New StringReader(html)
                        Dim xml As New XmlSerializer(GetType(IndieGalaRSS))
                        Dim rss As IndieGalaRSS = xml.Deserialize(stream)
                        Dim listaJuegosIG As IndieGalaJuegos = rss.Canal.Juegos

                        If Not listaJuegosIG Is Nothing Then
                            If listaJuegosIG.Juegos.Count > 0 Then
                                For Each juego In listaJuegosIG.Juegos
                                    Dim tituloJuego As String = WebUtility.HtmlDecode(juego.Titulo)
                                    tituloJuego = tituloJuego.Replace("(Steam)", Nothing)
                                    tituloJuego = tituloJuego.Replace("(Epic)", Nothing)
                                    tituloJuego = tituloJuego.Replace("Â", Nothing)
                                    tituloJuego = tituloJuego.Replace("¢", Nothing)
                                    tituloJuego = tituloJuego.Replace("â", "'")
                                    tituloJuego = tituloJuego.Trim

                                    If Limpieza.Limpiar(titulo) = Limpieza.Limpiar(tituloJuego) Then
                                        Dim enlace As String = juego.Enlace

                                        Dim pais2 As New Windows.Globalization.GeographicRegion

                                        Dim precio As String = String.Empty

                                        If pais2.CodeTwoLetter.ToLower = "uk" Then
                                            precio = juego.PrecioDescontadoUK
                                        ElseIf Pais.DetectarEuro = True Then
                                            precio = juego.PrecioDescontadoEU
                                        Else
                                            precio = juego.PrecioDescontadoUS
                                        End If

                                        If precio = String.Empty Then
                                            If pais2.CodeTwoLetter.ToLower = "uk" Then
                                                precio = juego.PrecioBaseUK
                                            ElseIf Pais.DetectarEuro = True Then
                                                precio = juego.PrecioBaseEU
                                            Else
                                                precio = juego.PrecioBaseUS
                                            End If
                                        End If

                                        If Not precio = String.Empty Then
                                            Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                            Dim moneda As String = String.Empty

                                            If pais2.CodeTwoLetter.ToLower = "uk" Then
                                                moneda = "GBP"
                                            ElseIf Pais.DetectarEuro = True Then
                                                moneda = "EUR"
                                            Else
                                                moneda = "USD"
                                            End If

                                            Dim formateador As New CurrencyFormatter(moneda) With {
                                                .Mode = CurrencyFormatterMode.UseSymbol
                                            }

                                            precio = formateador.Format(tempDouble)

                                            tienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/indiegala3.png", Nothing, Nothing)
                                        End If

                                        salirBucles = True
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                    i += 1

                    If salirBucles = True Then
                        Exit While
                    End If
                End While
            Catch ex As Exception

            End Try

        End Sub

        Private Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            If Not tienda Is Nothing Then
                AñadirTienda(tienda)
            End If

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Value = pb.Value + 1

            If pb.Value = pb.Maximum Then
                pb.Visibility = Visibility.Collapsed
            End If

        End Sub

        Private Async Function GenerarNumPaginas(url As Uri) As Task(Of Integer)

            Dim numPaginas As Integer = 0

            Dim html As String = Await HttpClient(url)

            If Not html = Nothing Then
                Dim stream As New StringReader(html)
                Dim xml As New XmlSerializer(GetType(IndieGalaRSS))
                Dim rss As IndieGalaRSS = xml.Deserialize(stream)

                numPaginas = rss.Canal.Paginas
            End If

            numPaginas = numPaginas + 1

            Return numPaginas
        End Function

    End Module

    <XmlRoot("rss")>
    Public Class IndieGalaRSS

        <XmlElement("channel")>
        Public Canal As IndieGalaCanal

    End Class

    Public Class IndieGalaCanal

        <XmlElement("totalPages")>
        Public Paginas As Integer

        <XmlElement("totalGames")>
        Public TotalJuegos As Integer

        <XmlElement("browse")>
        Public Juegos As IndieGalaJuegos

    End Class

    Public Class IndieGalaJuegos

        <XmlElement("item")>
        Public Juegos As List(Of IndieGalaJuego)

    End Class

    Public Class IndieGalaJuego

        <XmlElement("title")>
        Public Titulo As String

        <XmlElement("link")>
        Public Enlace As String

        <XmlElement("priceEUR")>
        Public PrecioBaseEU As String

        <XmlElement("priceGBP")>
        Public PrecioBaseUK As String

        <XmlElement("priceUSD")>
        Public PrecioBaseUS As String

        <XmlElement("discountPriceEUR")>
        Public PrecioDescontadoEU As String

        <XmlElement("discountPriceGBP")>
        Public PrecioDescontadoUK As String

        <XmlElement("discountPriceUSD")>
        Public PrecioDescontadoUS As String

    End Class
End Namespace

