﻿Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module GOG

        Dim tiendas As List(Of Tienda)
        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim nuevaTienda As Tienda

        Public Sub Buscar(tiendas_ As List(Of Tienda), titulo_ As String)

            tiendas = tiendas_
            titulo = titulo_

            nuevaTienda = Nothing

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

            Try
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.gog.com/games/ajax/filtered?mediaType=game&page=1&search=" + titulo + "&sort=title"))
                Dim html As String = html_.Result

                If Not html = Nothing Then
                    Dim listaJuegosGOG As GOGJuegos = JsonConvert.DeserializeObject(Of GOGJuegos)(html)

                    If Not listaJuegosGOG Is Nothing Then
                        If listaJuegosGOG.Juegos.Count > 0 Then
                            For Each juego In listaJuegosGOG.Juegos
                                If Limpieza.Limpiar(juego.Titulo) = Limpieza.Limpiar(titulo) Then
                                    Dim enlace As String = "https://www.gog.com" + juego.Enlace

                                    Dim precio As String = juego.Precio.Formateado

                                    If Not precio = String.Empty Then
                                        Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                        Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                        Dim formateador As New CurrencyFormatter(moneda) With {
                                            .Mode = CurrencyFormatterMode.UseSymbol
                                        }

                                        precio = formateador.Format(tempDouble)

                                        nuevaTienda = New Tienda(Referidos.Generar(enlace), precio, "Assets/Tiendas/gog3.png", Nothing, Nothing)
                                    End If
                                End If
                            Next
                        End If
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

    Public Class GOGJuegos

        <JsonProperty("products")>
        Public Juegos As List(Of GOGJuego)

    End Class

    Public Class GOGJuego

        <JsonProperty("title")>
        Public Titulo As String

        <JsonProperty("price")>
        Public Precio As GOGJuegoPrecio

        <JsonProperty("url")>
        Public Enlace As String

    End Class

    Public Class GOGJuegoPrecio

        <JsonProperty("finalAmount")>
        Public Formateado As String

    End Class
End Namespace

