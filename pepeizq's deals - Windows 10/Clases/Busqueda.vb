Imports Newtonsoft.Json

'https://pepeizqdeals.com/wp-json/wp/v2/search/?search=fall

Public Class Busqueda

    <JsonProperty("title")>
    Public Titulo As String

    <JsonProperty("id")>
    Public ID As Integer

    <JsonProperty("url")>
    Public Enlace As String

    <JsonProperty("type")>
    Public Tipo As String

End Class
