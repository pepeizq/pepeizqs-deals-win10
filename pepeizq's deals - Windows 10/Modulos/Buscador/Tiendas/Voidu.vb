Imports System.Globalization
Imports System.Net
Imports System.Xml.Serialization
Imports Windows.Globalization.NumberFormatting
Imports Windows.System.UserProfile

Namespace Buscador.Tiendas
    Module Voidu

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
                Dim html_ As Task(Of String) = HttpClient(New Uri("https://daisycon.io/datafeed/?filter_id=80367&settings_id=10133"))
                Dim html As String = html_.Result

                If Not html = Nothing Then
                    Dim xml As New XmlSerializer(GetType(AllyouplayJuegos))
                    Dim stream As New StringReader(html)
                    Dim listaJuegos As AllyouplayJuegos = xml.Deserialize(stream)

                    If Not listaJuegos Is Nothing Then
                        If listaJuegos.Juegos.Count > 0 Then
                            For Each juego In listaJuegos.Juegos
                                If DevolverMoneda() = juego.Moneda Then
                                    Dim tituloJuego As String = WebUtility.HtmlDecode(juego.Titulo)
                                    titulo = WebUtility.HtmlDecode(titulo)
                                    titulo = titulo.Replace("?", Nothing)
                                    titulo = titulo.Replace("(Mac/Pc)", Nothing)
                                    titulo = titulo.Replace("[Mac]", Nothing)
                                    titulo = titulo.Replace("(ROW)", Nothing)
                                    titulo = titulo.Replace("(DLC)", Nothing)
                                    titulo = titulo.Replace("- ASIA+EMEA", Nothing)
                                    titulo = titulo.Replace("- EMEA", Nothing)
                                    titulo = titulo.Replace("- ANZ+EMEA", Nothing)
                                    titulo = titulo.Replace("- PC", Nothing)
                                    titulo = titulo.Replace("- ANZ + EU", Nothing)
                                    titulo = titulo.Replace("- EMEA + ANZ", Nothing)
                                    titulo = titulo.Replace("(STEAM)", Nothing)
                                    titulo = titulo.Replace("(Steam)", Nothing)
                                    titulo = titulo.Replace("(EPIC GAMES)", Nothing)
                                    titulo = titulo.Trim

                                    If Limpieza.Limpiar(titulo) = Limpieza.Limpiar(tituloJuego) Then
                                        Dim enlace As String = juego.Enlace

                                        Dim precio As String = juego.PrecioRebajado

                                        If precio = String.Empty Then
                                            precio = juego.PrecioBase
                                        End If

                                        If Not precio = String.Empty Then
                                            Dim tempDouble As Double = Double.Parse(precio, CultureInfo.InvariantCulture).ToString

                                            Dim moneda As String = GlobalizationPreferences.Currencies(0)

                                            Dim formateador As New CurrencyFormatter(moneda) With {
                                                .Mode = CurrencyFormatterMode.UseSymbol
                                            }

                                            precio = formateador.Format(tempDouble)

                                            nuevaTienda = New Tienda(Referidos.Generar(enlace), precio, "Assets/Tiendas/voidu3.png", Nothing, Nothing)
                                        End If

                                        Exit For
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

    <XmlRoot("datafeed")>
    Public Class VoiduJuegos

        <XmlElement("product_info")>
        Public Juegos As List(Of VoiduJuego)

    End Class

    Public Class VoiduJuego

        <XmlElement("link")>
        Public Enlace As String

        <XmlElement("title")>
        Public Titulo As String

        <XmlElement("price_old")>
        Public PrecioBase As String

        <XmlElement("price")>
        Public PrecioRebajado As String

        <XmlElement("currency")>
        Public Moneda As String

    End Class

End Namespace

