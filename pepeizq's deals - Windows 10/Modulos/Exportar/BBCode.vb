Imports Windows.ApplicationModel.DataTransfer

Namespace Exportar
    Module BBCode

        Public Sub Generar(json As EntradaOfertas)

            Dim recursos As New Resources.ResourceLoader()

            Dim contenido As String = String.Empty

            For Each juego In json.Juegos
                contenido = contenido + "[url=" + juego.Enlace + "][img]" + juego.Imagen + "[/img][/url]" + Environment.NewLine + Environment.NewLine + "[url=" + juego.Enlace + "]" + juego.Titulo + " • " + juego.Descuento + " • " + juego.Precio + "[/url]" + Environment.NewLine + Environment.NewLine
            Next

            Dim datos As New DataPackage With {
                .RequestedOperation = DataPackageOperation.Copy
            }
            datos.SetText(contenido)
            Clipboard.SetContent(datos)

            Notificaciones.Toast(recursos.GetString("ExportBBCodeMessage"), Nothing)

        End Sub

    End Module
End Namespace