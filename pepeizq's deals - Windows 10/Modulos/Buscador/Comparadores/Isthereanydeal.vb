Namespace Buscador.Comparadores
    Module Isthereanydeal

        Public Sub Buscar(id As String)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridComparadorIsthereanydeal")
            grid.Visibility = Visibility.Collapsed

            Dim wv As WebView = pagina.FindName("wvIsthereanydeal")
            AddHandler wv.NavigationCompleted, AddressOf Comprobar

            wv.Navigate(New Uri("https://new.isthereanydeal.com/steam/app/" + id))

        End Sub

        Private Async Sub Comprobar(sender As Object, e As WebViewNavigationCompletedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim wv As WebView = sender

            Dim html As String = Await wv.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If Not html = Nothing Then
                Try
                    If html.Contains(">Historical Low<") Then
                        Dim temp, temp2, temp3, temp4 As String
                        Dim int, int2, int3, int4 As Integer

                        int = html.IndexOf(">Historical Low<")
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf("ptag__price")
                        temp2 = temp.Remove(0, int2)

                        int3 = temp2.IndexOf(">")
                        temp3 = temp2.Remove(0, int3 + 1)

                        int4 = temp3.IndexOf("<")
                        temp4 = temp3.Remove(int4, temp3.Length - int4)

                        If Not temp4 = String.Empty Then
                            Dim grid As Grid = pagina.FindName("gridComparadorIsthereanydeal")
                            grid.Visibility = Visibility.Visible

                            Dim tbPrecio As TextBlock = pagina.FindName("tbComparadorIsthereanydealPrecio")
                            tbPrecio.Text = temp4.Trim
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If

            Dim pb As ProgressBar = pagina.FindName("pbBusquedaJuego")
            pb.Value = pb.Value + 1

            If pb.Value = pb.Maximum Then
                pb.Visibility = Visibility.Collapsed
            End If

        End Sub

    End Module
End Namespace

