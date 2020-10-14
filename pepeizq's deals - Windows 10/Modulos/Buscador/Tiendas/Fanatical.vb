Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Fanatical
        Public Async Function Buscar(titulo As String, id As String) As Task(Of Tienda)

            Dim html As String = Await Decompiladores.HttpClient(New Uri("https://feed.fanatical.com/feed"))

            If Not html = Nothing Then
                html = "[" + html + "]"
                html = html.Replace("{" + ChrW(34) + "features", ",{" + ChrW(34) + "features")

                Dim int3 As Integer = html.IndexOf(",")
                html = html.Remove(int3, 1)

                Dim listaJuegos As List(Of FanaticalJuego) = JsonConvert.DeserializeObject(Of List(Of FanaticalJuego))(html)

                If Not listaJuegos Is Nothing Then
                    If listaJuegos.Count > 0 Then
                        For Each juego In listaJuegos
                            If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Then

                                Dim enlace As String = juego.Enlace

                                Dim pais As New Windows.Globalization.GeographicRegion

                                Dim precio As String = String.Empty

                                If Not juego.PrecioRebajado Is Nothing Then
                                    If pais.CodeTwoLetter.ToLower = "uk" Then
                                        precio = juego.PrecioRebajado.GBP
                                    ElseIf pais.CodeTwoLetter.ToLower = "es" Or pais.CodeTwoLetter.ToLower = "fr" Or pais.CodeTwoLetter.ToLower = "de" Or pais.CodeTwoLetter.ToLower = "it" Then
                                        precio = juego.PrecioRebajado.EUR
                                    Else
                                        precio = juego.PrecioRebajado.USD
                                    End If
                                End If

                                If precio = String.Empty Then
                                    If pais.CodeTwoLetter.ToLower = "uk" Then
                                        precio = juego.PrecioBase.GBP
                                    ElseIf pais.CodeTwoLetter.ToLower = "es" Or pais.CodeTwoLetter.ToLower = "fr" Or pais.CodeTwoLetter.ToLower = "de" Or pais.CodeTwoLetter.ToLower = "it" Then
                                        precio = juego.PrecioBase.EUR
                                    Else
                                        precio = juego.PrecioBase.USD
                                    End If
                                End If

                                If Not precio = String.Empty Then
                                    Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                    Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                    Dim formateador As New CurrencyFormatter(moneda) With {
                                        .Mode = CurrencyFormatterMode.UseSymbol
                                    }

                                    precio = formateador.Format(tempDouble)

                                    Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/fanatical3.png")
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

    Public Class FanaticalJuego

        <JsonProperty("title")>
        Public Titulo As String

        <JsonProperty("url")>
        Public Enlace As String

        <JsonProperty("steam_app_id")>
        Public SteamID As String

        <JsonProperty("current_price")>
        Public PrecioRebajado As FanaticalJuegoPrecio

        <JsonProperty("regular_price")>
        Public PrecioBase As FanaticalJuegoPrecio

        <JsonProperty("regions")>
        Public Regiones As List(Of String)

    End Class

    Public Class FanaticalJuegoPrecio

        <JsonProperty("USD")>
        Public USD As String

        <JsonProperty("GBP")>
        Public GBP As String

        <JsonProperty("EUR")>
        Public EUR As String

    End Class
End Namespace

