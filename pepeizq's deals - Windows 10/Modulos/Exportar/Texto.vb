Imports Windows.ApplicationModel.DataTransfer

Namespace Exportar
    Module Texto

        Public Sub Generar(json As EntradaOfertas)

            Dim recursos As New Resources.ResourceLoader()

            Dim contenido As String = String.Empty

            For Each juego In json.Juegos
                contenido = contenido + juego.Titulo + " • " + juego.Descuento + " • " + juego.Precio + Environment.NewLine + juego.Enlace + Environment.NewLine + Environment.NewLine
            Next

            Dim datos As New DataPackage With {
                .RequestedOperation = DataPackageOperation.Copy
            }
            datos.SetText(contenido)
            Clipboard.SetContent(datos)

            Notificaciones.Toast(recursos.GetString("ExportTextMessage"), Nothing)

        End Sub

    End Module

End Namespace