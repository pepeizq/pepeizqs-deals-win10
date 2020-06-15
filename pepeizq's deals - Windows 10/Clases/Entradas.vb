Imports Newtonsoft.Json

Public Class Entrada

    <JsonProperty("title")>
    Public Titulo As Titulo

End Class

Public Class Titulo

    <JsonProperty("rendered")>
    Public Texto As String

End Class
