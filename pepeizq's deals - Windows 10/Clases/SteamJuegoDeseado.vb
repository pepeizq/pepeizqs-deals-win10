Imports Newtonsoft.Json

Public Class SteamJuegoDeseado

    <JsonProperty("name")>
    Public Property Titulo As String

    <JsonProperty("capsule")>
    Public Property Imagen As String

End Class

Public Class SteamJuegoDeseado2

    Public Property Titulo As String
    Public Property Imagen As String
    Public Property ID As String

    Public Sub New(ByVal titulo As String, ByVal imagen As String, ByVal id As String)
        Me.Titulo = titulo
        Me.Imagen = imagen
        Me.ID = id
    End Sub

End Class
