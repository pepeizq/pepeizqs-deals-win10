Public Class FiltroDeseado

    Public Property Titulo As String
    Public Property Enlaces As List(Of String)
    Public Property Imagen As String

    Public Sub New(ByVal titulo As String, ByVal enlaces As List(Of String), ByVal imagen As String)
        Me.Titulo = titulo
        Me.Enlaces = enlaces
        Me.Imagen = imagen
    End Sub

End Class
