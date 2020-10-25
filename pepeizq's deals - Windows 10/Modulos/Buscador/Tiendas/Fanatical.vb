Imports System.Globalization
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Fanatical

        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim id As String
        Dim tienda As Tienda

        Public Sub Buscar(titulo_ As String, id_ As String)

            titulo = titulo_
            id = id_

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Dim html_ As Task(Of String) = Decompiladores.HttpClient(New Uri("https://feed.fanatical.com/feed"))
            Dim html As String = html_.Result

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

                                    tienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/fanatical3.png")
                                End If
                            End If
                        Next
                    End If
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

