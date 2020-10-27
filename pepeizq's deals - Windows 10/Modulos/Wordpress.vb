Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports WordPressPCL

Module Wordpress

    Dim WithEvents bw As New BackgroundWorker
    Dim paginas As Integer
    Dim categoriaS As String
    Dim categoriaN As Integer
    Dim actualizar As Boolean
    Dim entradas As New List(Of Entrada)

    Public Sub CargarEntradas(paginas_ As Integer, categoria_ As String, actualizar_ As Boolean)

        If bw.IsBusy = False Then
            paginas = paginas_
            categoriaS = categoria_
            actualizar = actualizar_

            GridVisibilidad.Mostrar("gridCarga")

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = "pepeizq's deals (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

            If Not categoria_ = Nothing Then
                tbTitulo.Text = tbTitulo.Text + " • " + categoria_
            End If

            Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
            spEntradas.Children.Clear()

            Dim recursos As New Resources.ResourceLoader()

            If categoria_ = recursos.GetString("Bundles2") Then
                categoriaN = 4
            ElseIf categoria_ = recursos.GetString("Deals2") Then
                categoriaN = 3
            ElseIf categoria_ = recursos.GetString("Free2") Then
                categoriaN = 12
            ElseIf categoria_ = recursos.GetString("Subscriptions2") Then
                categoriaN = 13
            End If

            bw.RunWorkerAsync()
        End If

    End Sub

    Private Sub Bw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw.DoWork

        If actualizar = True Then
            Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
                .AuthMethod = Models.AuthMethod.JWT
            }

            Dim categoriaString As String = String.Empty

            If Not categoriaN = Nothing Then
                categoriaString = "&categories=" + categoriaN.ToString
            End If

            Dim entradas_ As Task(Of List(Of Entrada)) = cliente.CustomRequest.Get(Of List(Of Entrada))("wp/v2/posts?per_page=" + paginas.ToString + categoriaString.ToString)
            Dim entradas2 As List(Of Entrada) = entradas_.Result

            For Each nuevaEntrada In entradas2
                Dim añadir As Boolean = True

                For Each viejaEntrada In entradas
                    If viejaEntrada.ID = nuevaEntrada.ID Then
                        añadir = False
                    End If
                Next

                If añadir = True Then
                    entradas.Add(nuevaEntrada)
                End If
            Next
        End If

    End Sub

    Private Async Sub Bw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim recursos As New Resources.ResourceLoader()

        If Not entradas Is Nothing Then
            If entradas.Count > 0 Then
                Dim spEntradas As StackPanel = pagina.FindName("spEntradas")

                For Each entrada In entradas
                    Dim añadir As Boolean = False

                    If categoriaS = Nothing Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 4 Or categoriaN = 3 Or categoriaN = 12 Or categoriaN = 13 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoriaS = recursos.GetString("Bundles2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 4 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoriaS = recursos.GetString("Deals2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 3 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoriaS = recursos.GetString("Free2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 12 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoriaS = recursos.GetString("Subscriptions2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 13 Then
                                añadir = True
                            End If
                        Next
                    End If

                    If spEntradas.Children.Count > 0 Then
                        For Each item In spEntradas.Children
                            If TypeOf item Is DropShadowPanel Then
                                Dim panel As DropShadowPanel = item
                                Dim panelEntrada As Entrada = panel.Tag

                                If entrada.Enlace = panelEntrada.Enlace Then
                                    añadir = False
                                End If
                            End If
                        Next
                    End If

                    If añadir = True Then
                        spEntradas.Children.Add(Interfaz.GenerarEntrada(entrada))
                        spEntradas.Children.Add(Interfaz.GenerarCompartir(entrada))
                    End If

                    '-------------------------------------------------------------

                    Dim mostrarAnuncio As Boolean = False

                    For Each categoriaN In entrada.Categorias
                        If categoriaN = 1208 Then
                            mostrarAnuncio = True
                        End If
                    Next

                    If mostrarAnuncio = True Then
                        Dim mostrarAnuncio2 As Boolean = True

                        Dim helper As New LocalObjectStorageHelper
                        Dim listaAnuncios As New List(Of Anuncio)

                        If Await helper.FileExistsAsync("listaAnuncios") Then
                            listaAnuncios = Await helper.ReadFileAsync(Of List(Of Anuncio))("listaAnuncios")
                        End If

                        If Not listaAnuncios Is Nothing Then
                            If listaAnuncios.Count > 0 Then
                                For Each item In listaAnuncios
                                    If entrada.Enlace = item.Enlace Then
                                        mostrarAnuncio2 = False
                                    End If
                                Next
                            End If
                        End If

                        If mostrarAnuncio2 = True Then
                            Notificaciones.ToastAnuncio(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen)
                            listaAnuncios.Add(New Anuncio(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen))

                            Try
                                Await helper.SaveFileAsync(Of List(Of Anuncio))("listaAnuncios", listaAnuncios)
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                Next
            End If
        End If

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If config.Values("Estado_App") = 1 Then
            Dim gvFiltroJuegosDeseados As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")
            gvFiltroJuegosDeseados.Tag = entradas
        End If

        GridVisibilidad.Mostrar("gridEntradas")

        Deseados.CargarUsuario()

    End Sub

    '------------------------------------------------------

    Public Async Function Buscar(texto As String) As Task(Of List(Of Buscador.pepeizqdeals))

        Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
            .AuthMethod = Models.AuthMethod.JWT
        }

        Dim resultados As List(Of Buscador.pepeizqdeals) = Nothing

        Try
            resultados = Await cliente.CustomRequest.Get(Of List(Of Buscador.pepeizqdeals))("wp/v2/search/?search=" + texto)
        Catch ex As Exception

        End Try

        Return resultados
    End Function

End Module
