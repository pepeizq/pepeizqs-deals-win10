Namespace Buscador.Comparadores
    Module GGdeals

        Dim i As Integer

        Public Sub Buscar(titulo As String)

            i = 0

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridComparadorGGdeals")
            grid.Visibility = Visibility.Collapsed

            Dim wv As WebView = pagina.FindName("wvGGdeals")
            AddHandler wv.NavigationCompleted, AddressOf Comprobar

            wv.Navigate(New Uri("https://gg.deals/games/?title=" + titulo))

        End Sub

        Private Async Sub Comprobar(sender As Object, e As WebViewNavigationCompletedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim wv As WebView = sender

            Dim html As String = Await wv.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If Not html = Nothing Then
                Dim temp, temp2, temp3, temp4 As String
                Dim int, int2, int3, int4 As Integer

                Try
                    If html.Contains("id=" + ChrW(34) + "games-list") Then
                        int = html.IndexOf("id=" + ChrW(34) + "games-list")
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf("href=" + ChrW(34))
                        temp2 = temp.Remove(0, int2 + 6)

                        int3 = temp2.IndexOf(ChrW(34))
                        temp3 = temp2.Remove(int3, temp2.Length - int3)

                        Dim enlaceJuego As String = "https://gg.deals" + temp3.Trim
                        wv.Navigate(New Uri(enlaceJuego))

                    ElseIf html.Contains("Historical low<") Then
                        int = html.IndexOf("Historical low<")
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf(ChrW(34) + "numeric" + ChrW(34))
                        temp2 = temp.Remove(0, int2)

                        int3 = temp2.IndexOf(">")
                        temp3 = temp2.Remove(0, int3 + 1)

                        int4 = temp3.IndexOf("<")
                        temp4 = temp3.Remove(int4, temp3.Length - int4)

                        If Not temp4 = String.Empty Then
                            temp4 = temp4.Replace("~", Nothing)

                            Dim grid As Grid = pagina.FindName("gridComparadorGGdeals")
                            grid.Visibility = Visibility.Visible

                            Dim tbPrecio As TextBlock = pagina.FindName("tbComparadorGGdealsPrecio")
                            tbPrecio.Text = temp4.Trim
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If

            i += 1

            If i = 1 Then
                Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
                pb.Value = pb.Value + 1

                If pb.Value = pb.Maximum Then
                    pb.Visibility = Visibility.Collapsed
                End If
            End If

        End Sub

    End Module
End Namespace

