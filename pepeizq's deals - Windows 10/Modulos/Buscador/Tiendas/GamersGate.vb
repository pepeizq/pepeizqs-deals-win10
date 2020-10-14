Imports System.Globalization
Imports System.Xml.Serialization
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module GamersGate

        Public Async Function Buscar(titulo As String) As Task(Of Tienda)

            Dim pais As New Windows.Globalization.GeographicRegion
            Dim html As String = Await HttpClient(New Uri("https://www.gamersgate.com/feeds/products?country=" + pais.CodeThreeLetter.ToLower + "&q=" + titulo))

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

                                    Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/gamersgate3.png")
                                    Return tienda
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            Return Nothing

        End Function

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

