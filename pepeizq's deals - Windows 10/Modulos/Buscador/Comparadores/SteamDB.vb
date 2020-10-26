Namespace Buscador.Comparadores
    Module SteamDB

        Public Sub Buscar(id As String)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridComparadorSteamDB")
            grid.Visibility = Visibility.Collapsed

            Dim wv As WebView = pagina.FindName("wvSteamDB")
            AddHandler wv.NavigationCompleted, AddressOf Comprobar

            wv.Navigate(New Uri("https://steamdb.info/app/" + id + "/"))

        End Sub

        Private Async Sub Comprobar(sender As Object, e As WebViewNavigationCompletedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim wv As WebView = sender

            Dim html As String = Await wv.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If Not html = Nothing Then
                Dim paisBuscar As String = String.Empty

                If Pais.DetectarEuro = True Then
                    paisBuscar = "eu"
                End If

                If paisBuscar = String.Empty Then
                    Dim pais2 As New Windows.Globalization.GeographicRegion
                    paisBuscar = pais2.CodeTwoLetter.ToLower
                End If

                Try
                    If html.Contains(ChrW(34) + "flag flag-" + paisBuscar + ChrW(34)) Then
                        Dim temp, temp2, temp3, temp4 As String
                        Dim int, int2, int3, int4 As Integer

                        int = html.LastIndexOf(ChrW(34) + "flag flag-" + paisBuscar + ChrW(34))
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf("data-sort=")
                        temp2 = temp.Remove(0, int2 + 1)

                        int2 = temp2.IndexOf("data-sort=")
                        temp2 = temp2.Remove(0, int2 + 1)

                        int3 = temp2.IndexOf(">")
                        temp3 = temp2.Remove(0, int3 + 1)

                        int4 = temp3.IndexOf("<")
                        temp4 = temp3.Remove(int4, temp3.Length - int4)

                        If Not temp4 = String.Empty Then
                            Dim grid As Grid = pagina.FindName("gridComparadorSteamDB")
                            grid.Visibility = Visibility.Visible

                            Dim tbPrecio As TextBlock = pagina.FindName("tbComparadorSteamDBPrecio")
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

