Imports Newtonsoft.Json

'https://pepeizqdeals.com/wp-json/wp/v2/search/?search=fall

Namespace Buscador
    Public Class pepeizqdeals

        <JsonProperty("title")>
        Public Titulo As String

        <JsonProperty("id")>
        Public ID As Integer

        <JsonProperty("url")>
        Public Enlace As String

        <JsonProperty("type")>
        Public Tipo As String

    End Class
End Namespace

