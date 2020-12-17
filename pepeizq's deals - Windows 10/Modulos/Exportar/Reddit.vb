Imports Windows.ApplicationModel.DataTransfer

Namespace Exportar

    Module Reddit

        Public Sub Generar(json As EntradaOfertas)

            Dim recursos As New Resources.ResourceLoader()

            Dim contenido As String = String.Empty

            contenido = contenido + "**Title** | **Discount** | **Price** | **Rating**" + Environment.NewLine
            contenido = contenido + ":--------|:---------:|:---------:|:---------:" + Environment.NewLine

            For Each juego In json.Juegos
                Dim analisis As String = Nothing

                If Not juego.AnalisisPorcentaje = Nothing Then
                    If Not juego.AnalisisPorcentaje = "null" Then
                        If Not juego.AnalisisEnlace = Nothing Then
                            analisis = "[" + juego.AnalisisPorcentaje + "](" + juego.AnalisisEnlace + ")"
                        Else
                            analisis = juego.AnalisisPorcentaje
                        End If
                    Else
                        analisis = "--"
                    End If
                Else
                    analisis = "--"
                End If

                Dim linea As String = "[" + juego.Titulo + "](" + juego.Enlace + ") | " + juego.Descuento + " | " + juego.Precio + " | " + analisis

                If Not linea = Nothing Then
                    contenido = contenido + linea + Environment.NewLine
                End If
            Next

            Dim datos As New DataPackage With {
                .RequestedOperation = DataPackageOperation.Copy
            }
            datos.SetText(contenido)
            Clipboard.SetContent(datos)

            Notificaciones.Toast(recursos.GetString("ExportRedditMessage"), Nothing)

        End Sub

    End Module

End Namespace