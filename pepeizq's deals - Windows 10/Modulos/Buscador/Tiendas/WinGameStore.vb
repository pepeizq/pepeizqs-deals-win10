Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting

Namespace Buscador.Tiendas
    Module WinGameStore

        Public Async Function Buscar(titulo As String) As Task(Of Tienda)

            Dim html As String = Await HttpClient(New Uri("https://www.macgamestore.com/affiliate/feeds/p_C1B2A3.json"))

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

                                    Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/wingamestore3.png")
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

