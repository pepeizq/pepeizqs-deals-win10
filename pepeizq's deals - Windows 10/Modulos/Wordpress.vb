Imports WordPressPCL

Module Wordpress

    Public Async Function CargarEntradas() As Task(Of List(Of Entrada))

        Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
            .AuthMethod = Models.AuthMethod.JWT
        }

        Dim entradas As List(Of Entrada) = Await cliente.CustomRequest.Get(Of List(Of Entrada))("wp/v2/posts")

        Return entradas
    End Function

End Module
