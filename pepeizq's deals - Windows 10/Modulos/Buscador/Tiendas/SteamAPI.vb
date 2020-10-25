﻿Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json

Namespace Buscador.Tiendas
    Module SteamAPI

        Dim WithEvents bw As New BackgroundWorker
        Dim id As String
        Dim tienda As Tienda

        Public Sub Buscar(id_ As String)

            id = id_

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

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
                    tienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar("https://store.steampowered.com/app/" + id + "/"), datos.Datos.Precio.Formateado, "Assets/Tiendas/steam3.png")
                End If
            End If

        End Sub

        Private Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            If Not tienda Is Nothing Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gvTiendas As AdaptiveGridView = pagina.FindName("gvBusquedaJuegoTiendas")

                gvTiendas.Items.Add(ResultadoTienda(tienda, Nothing, Nothing))
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

