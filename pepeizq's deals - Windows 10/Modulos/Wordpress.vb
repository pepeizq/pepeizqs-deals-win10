Imports WordPressPCL

Module Wordpress

    Public Async Function Cargar(tipo As String, paginas As Integer, categoria As Integer) As Task(Of List(Of Entrada))

        Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
            .AuthMethod = Models.AuthMethod.JWT
        }

        Dim categoriaString As String = String.Empty

        If Not categoria = Nothing Then
            categoriaString = "&categories=" + categoria.ToString
        End If

        Dim entradas As List(Of Entrada) = Await cliente.CustomRequest.Get(Of List(Of Entrada))("wp/v2/" + tipo + "?per_page=" + paginas.ToString + categoriaString.ToString)

        Return entradas
    End Function

End Module
