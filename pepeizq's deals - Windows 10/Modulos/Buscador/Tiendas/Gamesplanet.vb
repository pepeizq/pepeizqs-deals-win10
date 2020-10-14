Imports System.Globalization
Imports System.Xml.Serialization
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Gamesplanet

        Public Async Function Buscar(titulo As String, id As String) As Task(Of Tienda)

            Dim pais As New Windows.Globalization.GeographicRegion

            Dim paisLetras As String = String.Empty

            If pais.CodeTwoLetter.ToLower = "uk" Or pais.CodeTwoLetter.ToLower = "fr" Or pais.CodeTwoLetter.ToLower = "de" Or pais.CodeTwoLetter.ToLower = "es" Or pais.CodeTwoLetter.ToLower = "it" Then
                paisLetras = "uk"
            Else
                paisLetras = "us"
            End If

            Dim html As String = Await HttpClient(New Uri("https://" + paisLetras + ".gamesplanet.com/api/v1/products/feed.xml"))

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

                                    Dim moneda As String = "USD"

                                    If paisLetras = "uk" Then
                                        moneda = "GBP"
                                    End If

                                    Dim formateador As New CurrencyFormatter(moneda) With {
                                        .Mode = CurrencyFormatterMode.UseSymbol
                                    }

                                    precio = formateador.Format(tempDouble)

                                    Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/gamesplanet3.png")
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

    <XmlRoot("products")>
    Public Class GamesPlanetJuegos

        <XmlElement("product")>
        Public Juegos As List(Of GamesPlanetJuego)

    End Class

    Public Class GamesPlanetJuego

        <XmlElement("name")>
        Public Titulo As String

        <XmlElement("link")>
        Public Enlace As String

        <XmlElement("price")>
        Public PrecioDescontado As String

        <XmlElement("price_base")>
        Public PrecioBase As String

        <XmlElement("steam_id")>
        Public SteamID As String

    End Class
End Namespace

