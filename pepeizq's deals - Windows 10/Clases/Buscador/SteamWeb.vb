Namespace Buscador
    Public Class SteamWeb

        Public Property ID As String
        Public Property Titulo As String
        Public Property Imagen As String

        Public Sub New(id As String, titulo As String, imagen As String)
            Me.ID = id
            Me.Titulo = titulo
            Me.Imagen = imagen
        End Sub

    End Class
End Namespace

