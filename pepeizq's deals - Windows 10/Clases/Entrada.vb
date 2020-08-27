Imports Newtonsoft.Json

'https://pepeizqdeals.com/wp-json/wp/v2/posts

Public Class Entrada

    <JsonProperty("title")>
    Public Titulo As Titulo

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

    <JsonProperty("imagen_v2")>
    Public Imagen2 As String

    <JsonProperty("imagen_v2_anuncios")>
    Public ImagenAnuncio As String

    <JsonProperty("title2")>
    Public SubTitulo As String

    <JsonProperty("date")>
    Public Fecha As String

    <JsonProperty("link")>
    Public Enlace As String

End Class

Public Class Titulo

    <JsonProperty("rendered")>
    Public Texto As String

End Class
