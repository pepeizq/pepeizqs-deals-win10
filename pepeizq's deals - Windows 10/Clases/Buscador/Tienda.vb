Namespace Buscador
    Public Class Tienda

        Public Property Enlace As String
        Public Property Precio As String
        Public Property Imagen As String

        Public Sub New(ByVal enlace As String, ByVal precio As String, ByVal imagen As String)
            Me.Enlace = enlace
            Me.Precio = precio
            Me.Imagen = imagen
        End Sub

    End Class
End Namespace

