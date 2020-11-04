Imports System.Globalization
Imports Newtonsoft.Json
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Humble

        Dim tiendas As List(Of Tienda)
        Dim WithEvents bw As New BackgroundWorker
        Dim titulo As String
        Dim nuevaTienda As Tienda
        Dim mensaje As String

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
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://www.humblebundle.com/store/api/search?filter=all&search=" + titulo + "&request=1"))
                Dim html As String = html_.Result

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
                                    Dim cuponPorcentaje As String = String.Empty
                                    cuponPorcentaje = DescuentoChoice(resultados.Juegos(0).DescuentoChoice)

                                    If Not resultados.Juegos(0).CosasIncompatibles Is Nothing Then
                                        If resultados.Juegos(0).CosasIncompatibles.Count > 0 Then
                                            If resultados.Juegos(0).CosasIncompatibles(0) = "subscriber-discount-coupons" Then
                                                cuponPorcentaje = String.Empty
                                            End If
                                        End If
                                    End If

                                    If Not cuponPorcentaje = String.Empty Then
                                        Dim recursos As New Resources.ResourceLoader()
                                        mensaje = recursos.GetString("HumbleChoice")

                                        If Not precio = String.Empty Then
                                            precio = precio.Replace(",", ".")
                                            precio = precio.Replace("€", Nothing)
                                            precio = precio.Trim

                                            Dim dcupon As Double = Double.Parse(precio, CultureInfo.InvariantCulture) * cuponPorcentaje
                                            Dim dprecio As Double = Double.Parse(precio, CultureInfo.InvariantCulture) - dcupon
                                            precio = Math.Round(dprecio, 2).ToString + " €"
                                        End If
                                    End If

                                    nuevaTienda = New Tienda(pepeizq.Editor.pepeizqdeals.Referidos.Generar(enlace), precio, "Assets/Tiendas/humble3.png", mensaje, Nothing)
                                End If
                            End If
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
                AñadirTienda(tiendas, nuevaTienda)
            End If

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Value = pb.Value + 1

            If pb.Value = pb.Maximum Then
                pb.Visibility = Visibility.Collapsed
            End If

        End Sub

        Private Function DescuentoChoice(descuento As Double)

            Dim cuponPorcentaje As String = String.Empty

            If descuento = 0.1 Then
                cuponPorcentaje = "0,2"
            ElseIf descuento = 0.05 Then
                cuponPorcentaje = "0,15"
            ElseIf descuento = 0.03 Then
                cuponPorcentaje = "0,13"
            ElseIf descuento = 0.02 Then
                cuponPorcentaje = "0,12"
            ElseIf descuento = 0 Then
                cuponPorcentaje = "0,10"
            End If

            Return cuponPorcentaje

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

        <JsonProperty("rewards_split")>
        Public DescuentoChoice As Double

        <JsonProperty("incompatible_features")>
        Public CosasIncompatibles As List(Of String)

    End Class

    Public Class HumbleJuegoPrecio

        <JsonProperty("currency")>
        Public Moneda As String

        <JsonProperty("amount")>
        Public Cantidad As String

    End Class
End Namespace

