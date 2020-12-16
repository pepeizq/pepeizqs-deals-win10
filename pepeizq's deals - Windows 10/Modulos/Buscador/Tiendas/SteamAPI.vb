Imports Newtonsoft.Json

Namespace Buscador.Tiendas
    Module SteamAPI

        Public dominioImagenes As String = "https://cdn.cloudflare.steamstatic.com"

        Dim tiendas As List(Of Tienda)
        Dim WithEvents bw As New BackgroundWorker
        Dim id As String
        Dim nuevaTienda As Tienda

        Public Sub Buscar(tiendas_ As List(Of Tienda), id_ As String)

            tiendas = tiendas_
            id = id_

            nuevaTienda = Nothing

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Try
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://store.steampowered.com/api/appdetails/?appids=" + id + "&l=english"))
                Dim html As String = html_.Result

                If Not html = Nothing Then
                    Dim temp As String
                    Dim int As Integer

                    int = html.IndexOf(":")
                    temp = html.Remove(0, int + 1)
                    temp = temp.Remove(temp.Length - 1, 1)

                    Dim datos As SteamAPIJson = JsonConvert.DeserializeObject(Of SteamAPIJson)(temp)

                    If Not datos.Datos.Precio Is Nothing Then
                        Dim precio As String = datos.Datos.Precio.Formateado

                        If Pais.DetectarEuro = True Then
                            precio = precio.Replace("€", " €")
                        End If

                        nuevaTienda = New Tienda(Referidos.Generar("https://store.steampowered.com/app/" + id + "/"), precio, "Assets/Tiendas/steam3.png", Nothing, Nothing)
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

