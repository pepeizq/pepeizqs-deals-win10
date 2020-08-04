Public Class Anuncio

    Public Property Mensaje As String
    Public Property Enlace As String
    Public Property Imagen As String

    Public Sub New(ByVal mensaje As String, ByVal enlace As String, ByVal imagen As String)
        Me.Mensaje = mensaje
        Me.Enlace = enlace
        Me.Imagen = imagen
    End Sub

End Class
