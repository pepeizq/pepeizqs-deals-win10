Imports Newtonsoft.Json

'https://pepeizqdeals.com/wp-json/wp/v2/posts

Public Class Entrada

    <JsonProperty("title")>
    Public Titulo As EntradaTitulo

    <JsonProperty("content")>
    Public Contenido As EntradaContenido

    <JsonProperty("status")>
    Public Estado As String

    <JsonProperty("id")>
    Public ID As Integer

    <JsonProperty("categories")>
    Public Categorias As List(Of Integer)

    <JsonProperty("tags")>
    Public Etiquetas As List(Of Integer)

    <JsonProperty("fifu_image_url")>
    Public Imagen As String

    <JsonProperty("redirect")>
    Public Redireccion As String

    <JsonProperty("image_v2")>
    Public Imagen2 As String

    <JsonProperty("image_v2_announcements")>
    Public ImagenAnuncio As String

    <JsonProperty("title2")>
    Public SubTitulo As String

    <JsonProperty("date")>
    Public Fecha As String

    <JsonProperty("link")>
    Public Enlace As String

    <JsonProperty("game_price_lowest")>
    Public JuegoPrecioMinimo As String

End Class

Public Class EntradaTitulo

    <JsonProperty("rendered")>
    Public Texto As String

End Class

Public Class EntradaContenido

    <JsonProperty("rendered")>
    Public Texto As String

End Class
