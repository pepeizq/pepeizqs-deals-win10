Namespace Buscador
    Public Class Tienda

        Public Property Enlace As String
        Public Property Precio As String
        Public Property Imagen As String
        Public Property Mensaje As String
        Public Property Pais As String

        Public Sub New(ByVal enlace As String, ByVal precio As String, ByVal imagen As String, ByVal mensaje As String, ByVal pais As String)
            Me.Enlace = enlace
            Me.Precio = precio
            Me.Imagen = imagen
            Me.Mensaje = mensaje
            Me.Pais = pais
        End Sub

    End Class
End Namespace

