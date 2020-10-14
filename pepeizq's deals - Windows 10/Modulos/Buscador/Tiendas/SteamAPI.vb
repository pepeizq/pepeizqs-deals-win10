Imports Newtonsoft.Json

Namespace Buscador.Tiendas
    Module SteamAPI

        Public Async Function Buscar(id As String) As Task(Of Tienda)

            Dim html As String = Await HttpClient(New Uri("https://store.steampowered.com/api/appdetails/?appids=" + id + "&l=english"))

            If Not html = Nothing Then
                Dim temp As String
                Dim int As Integer

                int = html.IndexOf(":")
                temp = html.Remove(0, int + 1)
                temp = temp.Remove(temp.Length - 1, 1)

                Dim datos As SteamAPIJson = JsonConvert.DeserializeObject(Of SteamAPIJson)(temp)

                If Not datos.Datos.Precio Is Nothing Then
                    Dim tienda As New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar("https://store.steampowered.com/app/" + id + "/"), datos.Datos.Precio.Formateado, "Assets/Tiendas/steam3.png")
                    Return tienda
                End If
            End If

            Return Nothing
        End Function

    End Module

    Public Class SteamAPIJson

        <JsonProperty("data")>
        Public Datos As SteamAPIJsonDatos

    End Class

    Public Class SteamAPIJsonDatos

        <JsonProperty("price_overview")>
        Public Precio As SteamAPIJsonPrecio

    End Class

    Public Class SteamAPIJsonPrecio

        <JsonProperty("final_formatted")>
        Public Formateado As String

    End Class
End Namespace

