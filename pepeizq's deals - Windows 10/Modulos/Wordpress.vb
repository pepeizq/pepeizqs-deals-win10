Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.UI.Core
Imports WordPressPCL

Module Wordpress

    Dim entradas As New List(Of Entrada)

    Public Async Sub CargarEntradas(paginas As Integer, categoria As String, actualizar As Boolean, primeraVez As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridCarga As Grid = pagina.FindName("gridCarga")
        Interfaz.Pestañas.Visibilidad(gridCarga, Nothing, Nothing)

        Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                         CargarEntradas2(pagina, paginas, categoria, actualizar, primeraVez)
                                                                                                     End Sub)

    End Sub

    Private Async Sub CargarEntradas2(pagina As Page, paginas As Integer, categoria As String, actualizar As Boolean, primeraVez As Boolean)

        Dim recursos As New Resources.ResourceLoader()
        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        Dim spEntradas As StackPanel = pagina.FindName("spEntradas")

        If spEntradas.Children.Count > 0 Then
            For Each item In spEntradas.Children
                If TypeOf item Is Grid Then
                    Dim grid As Grid = item
                    Dim gridEntrada As Entrada = grid.Tag

                    If Not gridEntrada Is Nothing Then
                        Dim añadir As Boolean = True

                        For Each entrada In entradas
                            If entrada.ID = gridEntrada.ID Then
                                añadir = False
                            End If
                        Next

                        If añadir = True Then
                            entradas.Add(gridEntrada)
                        End If
                    End If
                End If
            Next
        End If

        spEntradas.Children.Clear()

        Dim categoriaN As Integer = 0
        If categoria = recursos.GetString("Bundles2") Then
            categoriaN = 4
        ElseIf categoria = recursos.GetString("Deals2") Then
            categoriaN = 3
        ElseIf categoria = recursos.GetString("Free2") Then
            categoriaN = 12
        ElseIf categoria = recursos.GetString("Subscriptions2") Then
            categoriaN = 13
        End If

        If actualizar = True Then
            Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
                .AuthMethod = Models.AuthMethod.JWT
            }

            Dim categoriaString As String = String.Empty

            If Not categoriaN = Nothing Then
                categoriaString = "&categories=" + categoriaN.ToString
            End If

            entradas = Await cliente.CustomRequest.Get(Of List(Of Entrada))("wp/v2/posts?per_page=" + paginas.ToString + categoriaString.ToString)

            For Each nuevaEntrada In entradas
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

        If Not entradas Is Nothing Then
            If entradas.Count > 0 Then
                Dim fechas As New List(Of String)
                spEntradas.Tag = entradas

                For Each entrada In entradas
                    Dim añadir As Boolean = False

                    If categoria = Nothing Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 4 Or categoriaN = 3 Or categoriaN = 12 Or categoriaN = 13 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoria = recursos.GetString("Bundles2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 4 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoria = recursos.GetString("Deals2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 3 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoria = recursos.GetString("Free2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 12 Then
                                añadir = True
                            End If
                        Next
                    ElseIf categoria = recursos.GetString("Subscriptions2") Then
                        For Each categoriaN In entrada.Categorias
                            If categoriaN = 13 Then
                                añadir = True
                            End If
                        Next
                    End If

                    If spEntradas.Children.Count > 0 Then
                        For Each item In spEntradas.Children
                            If TypeOf item Is Grid Then
                                Dim grid As Grid = item
                                Dim gridEntrada As Entrada = grid.Tag

                                If Not gridEntrada Is Nothing Then
                                    If entrada.ID = gridEntrada.ID Then
                                        añadir = False
                                    End If
                                End If
                            End If
                        Next
                    End If

                    If añadir = True Then
                        Dim añadirFecha As Boolean = True
                        Dim fecha As New Date

                        Try
                            fecha = Date.Parse(entrada.Fecha)
                        Catch ex As Exception

                        End Try

                        If Not fecha = Nothing Then
                            If fechas.Count > 0 Then
                                For Each fecha2 In fechas
                                    If fecha2 = fecha.Date.ToString Then
                                        añadirFecha = False
                                    End If
                                Next
                            End If

                            If añadirFecha = True Then
                                fechas.Add(fecha.Date.ToString)
                            End If

                            If Not entrada.Json = String.Empty Then
                                If añadirFecha = True Then
                                    spEntradas.Children.Add(Interfaz.Entradas.GenerarFecha(fecha.Date))
                                End If

                                spEntradas.Children.Add(Interfaz.Entradas.GenerarEntrada(entrada))
                            End If
                        End If
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
                        Else
                            listaAnuncios = New List(Of Anuncio)
                        End If

                        If mostrarAnuncio2 = True Then
                            Notificaciones.ToastOferta(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen)
                            listaAnuncios.Add(New Anuncio(entrada.Titulo.Texto, entrada.Enlace, entrada.Imagen))

                            Try
                                Await helper.SaveFileAsync(Of List(Of Anuncio))("listaAnuncios", listaAnuncios)
                            Catch ex As Exception

                            End Try
                        End If

                        Dim botonSorteosImagen As Button = pagina.FindName("botonSorteosImagen")

                        If config.Values("Sorteos") = 1 Then
                            Dim esSorteo As Boolean = False

                            For Each categoria In entrada.Categorias
                                If categoria = 1208 Then
                                    If entrada.Titulo.Texto.ToLower.Contains("giveaways") = True And entrada.Titulo.Texto.ToLower.Contains("steamgifts") = True Then
                                        esSorteo = True
                                    End If
                                End If
                            Next

                            If esSorteo = True Then
                                botonSorteosImagen.Visibility = Visibility.Visible
                                botonSorteosImagen.Tag = entrada

                                Dim imagenBotonSorteos As ImageEx = pagina.FindName("imagenBotonSorteos")
                                Dim imagen As String = entrada.Imagen
                                imagen = imagen.Replace(".webp", ".png")
                                imagenBotonSorteos.Source = imagen
                            End If
                        Else
                            botonSorteosImagen.Visibility = Visibility.Collapsed
                        End If
                    End If
                Next

                If spEntradas.Children.Count > 0 Then
                    Dim grid As Grid = spEntradas.Children(spEntradas.Children.Count - 1)
                    grid.Margin = New Thickness(0, 0, 0, 0)
                End If
            End If
        End If

        If config.Values("Estado_App") = 1 Then
            Dim gvFiltroJuegosDeseados As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")
            gvFiltroJuegosDeseados.Tag = entradas
        End If

        If primeraVez = True Then
            Deseados.CargarUsuario()

            If config.Values("Estado_App") = 1 Then
                If config.Values("Notificaciones") = 1 Then
                    Push.Escuchar(entradas)
                End If
            End If
        End If

        Dim gridEntradas As Grid = pagina.FindName("gridEntradas")
        Interfaz.Pestañas.Visibilidad(gridEntradas, categoria, Nothing)

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

    Public Async Function RecuperarPagina(id As String) As Task(Of Entrada)

        Dim cliente As New WordPressClient("https://pepeizqdeals.com/wp-json/") With {
            .AuthMethod = Models.AuthMethod.JWT
        }

        Dim entrada As Entrada = Await cliente.CustomRequest.Get(Of Entrada)("wp/v2/pages/" + id)

        Return entrada

    End Function

End Module
