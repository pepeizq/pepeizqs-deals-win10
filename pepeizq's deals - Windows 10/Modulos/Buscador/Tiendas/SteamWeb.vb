Imports System.Net

Namespace Buscador.Tiendas
    Module Steam

        Public Async Function Buscar(tituloBuscar As String) As Task(Of List(Of SteamWeb))

            Dim listaJuegos As New List(Of SteamWeb)

            tituloBuscar = tituloBuscar.Replace(" ", "+")

            Dim html As String = Await HttpClient(New Uri("https://store.steampowered.com/search/?sort_by=revelance&term=" + tituloBuscar + "&category1=998&ignore_preferences=1"))

            If Not html = Nothing Then
                Dim int0 As Integer

                int0 = html.IndexOf("<!-- List Items -->")

                If Not int0 = -1 Then
                    html = html.Remove(0, int0)

                    int0 = html.IndexOf("<!-- End List Items -->")
                    html = html.Remove(int0, html.Length - int0)

                    Dim j As Integer = 0
                    While j < 50
                        If html.Contains("<a href=" + ChrW(34) + "https://store.steampowered.com/") Then
                            Dim temp, temp2 As String
                            Dim int, int2 As Integer

                            int = html.IndexOf("<a href=" + ChrW(34) + "https://store.steampowered.com/")
                            temp = html.Remove(0, int + 5)

                            html = temp

                            int2 = temp.IndexOf("</a>")
                            temp2 = temp.Remove(int2, temp.Length - int2)

                            Dim temp3, temp4 As String
                            Dim int3, int4 As Integer

                            int3 = temp2.IndexOf("<span class=" + ChrW(34) + "title" + ChrW(34) + ">")
                            temp3 = temp2.Remove(0, int3)

                            int4 = temp3.IndexOf("</span>")
                            temp4 = temp3.Remove(int4, temp3.Length - int4)

                            int4 = temp4.IndexOf(">")
                            temp4 = temp4.Remove(0, int4 + 1)

                            temp4 = temp4.Trim
                            temp4 = WebUtility.HtmlDecode(temp4)

                            Dim titulo As String = temp4

                            Dim temp5, temp6 As String
                            Dim int5, int6 As Integer

                            int5 = temp2.IndexOf("https://")
                            temp5 = temp2.Remove(0, int5)

                            temp5 = temp5.Replace("https://store.steampowered.com/app/", Nothing)

                            int6 = temp5.IndexOf("/")
                            temp6 = temp5.Remove(int6, temp5.Length - int6)

                            Dim id As String = temp6.Trim

                            Dim temp7, temp8 As String
                            Dim int7, int8 As Integer

                            int7 = temp2.IndexOf("<img src=")
                            temp7 = temp2.Remove(0, int7 + 10)

                            int8 = temp7.IndexOf("?")
                            temp8 = temp7.Remove(int8, temp7.Length - int8)

                            temp8 = temp8.Trim

                            Dim imagen As String = temp8
                            imagen = imagen.Replace("capsule_sm_120", "library_600x900")

                            Dim juego As New SteamWeb(id, titulo, imagen)

                            Dim añadir As Boolean = True
                            Dim k As Integer = 0
                            While k < listaJuegos.Count
                                If listaJuegos(k).ID = juego.ID Then
                                    añadir = False
                                End If
                                k += 1
                            End While

                            'If Not Limpieza.Limpiar(titulo).Contains(Limpieza.Limpiar(tituloBuscar)) Then
                            '    añadir = False
                            'End If

                            If añadir = True Then
                                listaJuegos.Add(juego)
                            End If
                        End If
                        j += 1
                    End While
                End If
            End If

            Return listaJuegos
        End Function

    End Module
End Namespace

