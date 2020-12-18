Public Class FiltroDeseado

    Public Property Titulo As String
    Public Property Entradas As New List(Of FiltroEntradaDeseado)
    Public Property Imagen As String

    Public Sub New(ByVal titulo As String, ByVal entradas As List(Of FiltroEntradaDeseado), ByVal imagen As String)
        Me.Titulo = titulo
        Me.Entradas = entradas
        Me.Imagen = imagen
    End Sub

End Class

Public Class FiltroEntradaDeseado

    Public Property Juego As EntradaOfertasJuego
    Public Property Entrada As Entrada

    Public Sub New(ByVal juego As EntradaOfertasJuego, ByVal entrada As Entrada)
        Me.Juego = juego
        Me.Entrada = entrada
    End Sub

End Class
