Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Humble

        Public Async Function Buscar(titulo As String) As Task(Of Tienda)

            Dim html As String = Await HttpClient(New Uri("https://www.humblebundle.com/store/api/search?filter=all&search=" + titulo + "&request=1"))

            If Not html = Nothing Then
                Dim resultados As HumbleResultados = JsonConvert.DeserializeObject(Of HumbleResultados)(html)

                If Not resultados Is Nothing Then
                    If resultados.Juegos.Count > 0 Then
                        If Limpieza.Limpiar(titulo) = Limpieza.Limpiar(resultados.Juegos(0).Titulo) Then

                            Dim enlace As String = "https://www.humblebundle.com/store/" + resultados.Juegos(0).Enlace

                            Dim precio As String = String.Empty

                            If Not resultados.Juegos(0).PrecioDescontado Is Nothing Then
                                If resultados.Juegos(0).PrecioDescontado.Cantidad.Trim.Length > 0 Then
                                    Dim tempDouble As Double = Double.Parse(resultados.Juegos(0).PrecioDescontado.Cantidad, CultureInfo.InvariantCulture).ToString

                                    Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                    Dim formateador As New CurrencyFormatter(moneda) With {
                                        .Mode = CurrencyFormatterMode.UseSymbol
                                    }

                                    precio = formateador.Format(tempDouble)
                                End If
                            End If

                            If precio = String.Empty Then
                                If Not resultados.Juegos(0).PrecioBase Is Nothing Then
                                    If resultados.Juegos(0).PrecioBase.Cantidad.Trim.Length > 0 Then
                                        Dim tempDouble As Double = Double.Parse(resultados.Juegos(0).PrecioBase.Cantidad, CultureInfo.InvariantCulture).ToString

                                        Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                        Dim formateador As New CurrencyFormatter(moneda) With {
                                            .Mode = CurrencyFormatterMode.UseSymbol
                                        }

                                        precio = formateador.Format(tempDouble)
                                    End If
                                End If
                            End If

                            If Not precio = String.Empty Then
                                Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/humble3.png")
                                Return tienda
                            End If
                        End If
                    End If
                End If
            End If

            Return Nothing

        End Function

    End Module

    Public Class HumbleResultados

        <JsonProperty("results")>
        Public Juegos As List(Of HumbleJuego)

    End Class

    Public Class HumbleJuego

        <JsonProperty("human_name")>
        Public Titulo As String

        <JsonProperty("current_price")>
        Public PrecioDescontado As HumbleJuegoPrecio

        <JsonProperty("full_price")>
        Public PrecioBase As HumbleJuegoPrecio

        <JsonProperty("human_url")>
        Public Enlace As String

    End Class

    Public Class HumbleJuegoPrecio

        <JsonProperty("currency")>
        Public Moneda As String

        <JsonProperty("amount")>
        Public Cantidad As String

    End Class
End Namespace

