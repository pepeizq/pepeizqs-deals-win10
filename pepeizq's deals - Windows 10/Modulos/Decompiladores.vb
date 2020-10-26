Imports System.Net
Imports Windows.Web.Http

Module Decompiladores

    Public Async Function HttpClient(url As Uri) As Task(Of String)

        Dim html As String = String.Empty

        Dim cliente As New HttpClient()
        cliente.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1")
        'cliente.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36")

        Try
            Dim respuesta As New HttpResponseMessage
            respuesta = Await cliente.GetAsync(url)
            respuesta.EnsureSuccessStatusCode()

            html = TryCast(Await respuesta.Content.ReadAsStringAsync(), String)
        Catch ex As Exception

        End Try

        cliente.Dispose()

        Return html

    End Function

    Public Function HttpClient2(url As Uri) As String

        Dim html As String = String.Empty

        Using cliente As New WebClient
            cliente.Headers.Add("user-agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1")

            Dim datos As Stream = cliente.OpenRead(url.AbsoluteUri)
            Dim lector As New StreamReader(datos)
            html = lector.ReadToEnd

            datos.Close()
            lector.Close()

        End Using

        Return html

    End Function

End Module
