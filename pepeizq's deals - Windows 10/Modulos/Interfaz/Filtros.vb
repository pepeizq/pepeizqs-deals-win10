Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.Storage
Imports Windows.UI

Namespace Interfaz
    Module Filtros

        Public Sub Cargar()

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonFiltroDeseados As Button = pagina.FindName("botonFiltroDeseados")

            AddHandler botonFiltroDeseados.Click, AddressOf EnseñarFiltroDeseadosClick
            AddHandler botonFiltroDeseados.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
            AddHandler botonFiltroDeseados.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

            Dim botonFiltroTiendas As Button = pagina.FindName("botonFiltroTiendas")

            AddHandler botonFiltroTiendas.Click, AddressOf EnseñarFiltroTiendasClick
            AddHandler botonFiltroTiendas.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
            AddHandler botonFiltroTiendas.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

        End Sub

        Private Sub EnseñarFiltroDeseadosClick(sender As Object, e As RoutedEventArgs)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim iconoFiltroDeseados As FontAwesome5.FontAwesome = pagina.FindName("iconoFiltroDeseados")
            Dim gvFiltroJuegosDeseados As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")

            If gvFiltroJuegosDeseados.Visibility = Visibility.Visible Then
                config.Values("FiltroDeseados") = 0
                iconoFiltroDeseados.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleUp
                gvFiltroJuegosDeseados.Visibility = Visibility.Collapsed
            Else
                config.Values("FiltroDeseados") = 1
                iconoFiltroDeseados.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleDown
                gvFiltroJuegosDeseados.Visibility = Visibility.Visible
            End If

        End Sub

        Private Sub EnseñarFiltroTiendasClick(sender As Object, e As RoutedEventArgs)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim iconoFiltroTiendas As FontAwesome5.FontAwesome = pagina.FindName("iconoFiltroTiendas")
            Dim gvFiltroTiendas As AdaptiveGridView = pagina.FindName("gvFiltroTiendas")

            If gvFiltroTiendas.Visibility = Visibility.Visible Then
                config.Values("FiltroTiendas") = 0
                iconoFiltroTiendas.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleUp
                gvFiltroTiendas.Visibility = Visibility.Collapsed
            Else
                config.Values("FiltroTiendas") = 1
                iconoFiltroTiendas.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleDown
                gvFiltroTiendas.Visibility = Visibility.Visible
            End If

        End Sub

        '------------------------------------------------------------------

        Dim WithEvents bw As New BackgroundWorker
        Dim usuarioDeseados As String
        Dim entradas As List(Of Entrada)
        Dim juegosDeseados As List(Of SteamJuegoDeseado2)

        Public Sub Cargar(juegosDeseados_ As List(Of SteamJuegoDeseado2))

            juegosDeseados = juegosDeseados_

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
            usuarioDeseados = config.Values("Deseados")

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spFiltroDeseados As StackPanel = pagina.FindName("spFiltroDeseados")
            spFiltroDeseados.Visibility = Visibility.Collapsed

            Dim pr As ProgressRing = pagina.FindName("prFiltros")
            pr.Visibility = Visibility.Visible

            Dim gvFiltroDeseados As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")
            gvFiltroDeseados.Items.Clear()

            Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
            entradas = spEntradas.Tag

            If entradas Is Nothing Then
                entradas = New List(Of Entrada)
            End If

            If bw.IsBusy = False Then
                bw.RunWorkerAsync()
            End If

        End Sub

        Dim filtros As New List(Of FiltroDeseado)

        Private Sub Bw_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles bw.DoWork

            filtros.Clear()

            For Each entrada In entradas
                Dim i As Integer = 0
                While i < juegosDeseados.Count
                    Dim listadoEncontrados As New List(Of FiltroEntradaDeseado)

                    If entrada.Categorias(0) = 3 Then
                        If Not entrada.JsonExpandido = Nothing Then
                            Dim json As EntradaOfertas = JsonConvert.DeserializeObject(Of EntradaOfertas)(entrada.JsonExpandido)

                            If Not json Is Nothing Then
                                If Not json.Juegos Is Nothing Then
                                    For Each juego2 In json.Juegos
                                        If usuarioDeseados = 1 Then
                                            If Limpieza.Limpiar(juego2.Titulo) = Limpieza.Limpiar(WebUtility.HtmlDecode(juegosDeseados(i).Titulo)) Then
                                                listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                                Exit For
                                            End If
                                        ElseIf usuarioDeseados = 0 Then
                                            If Limpieza.Limpiar(juego2.Titulo).Contains(Limpieza.Limpiar(WebUtility.HtmlDecode(juegosDeseados(i).Titulo))) Then
                                                listadoEncontrados.Add(New FiltroEntradaDeseado(juego2, entrada))
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Else
                            Dim añadir As Boolean = False

                            If Not entrada.Titulo Is Nothing Then
                                If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(juegosDeseados(i).Titulo)) Then
                                    añadir = True
                                End If
                            End If

                            If Not entrada.SubTitulo = Nothing Then
                                If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(juegosDeseados(i).Titulo)) Then
                                    añadir = True
                                End If
                            End If

                            If añadir = True Then
                                listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                            End If
                        End If
                    Else
                        Dim añadir As Boolean = False

                        If Not entrada.Titulo Is Nothing Then
                            If Limpieza.Limpiar(entrada.Titulo.Texto).Contains(Limpieza.Limpiar(juegosDeseados(i).Titulo)) Then
                                añadir = True
                            End If
                        End If

                        If Not entrada.SubTitulo = Nothing Then
                            If Limpieza.Limpiar(entrada.SubTitulo).Contains(Limpieza.Limpiar(juegosDeseados(i).Titulo)) Then
                                añadir = True
                            End If
                        End If

                        If añadir = True Then
                            listadoEncontrados.Add(New FiltroEntradaDeseado(Nothing, entrada))
                        End If
                    End If

                    If listadoEncontrados.Count > 0 Then
                        Dim encontrado As Boolean = False

                        For Each filtro In filtros
                            If filtro.Titulo = juegosDeseados(i).Titulo Then
                                filtro.Entradas.Add(New FiltroEntradaDeseado(listadoEncontrados(0).Juego, listadoEncontrados(0).Entrada))
                                encontrado = True
                            End If
                        Next

                        If encontrado = False Then
                            filtros.Add(New FiltroDeseado(juegosDeseados(i).Titulo, listadoEncontrados, juegosDeseados(i).Imagen))
                        End If
                    End If
                    i += 1
                End While
            Next

        End Sub

        Private Sub Bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles bw.RunWorkerCompleted

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pr As ProgressRing = pagina.FindName("prFiltros")

            Dim spFiltroDeseados As StackPanel = pagina.FindName("spFiltroDeseados")
            Dim iconoFiltroDeseados As FontAwesome5.FontAwesome = pagina.FindName("iconoFiltroDeseados")
            Dim gvFiltroDeseados As AdaptiveGridView = pagina.FindName("gvFiltroJuegosDeseados")

            pr.Visibility = Visibility.Collapsed

            If filtros.Count > 0 Then
                spFiltroDeseados.Visibility = Visibility.Visible

                If config.Values("FiltroDeseados") = 1 Then
                    iconoFiltroDeseados.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleDown
                    gvFiltroDeseados.Visibility = Visibility.Visible
                ElseIf config.Values("FiltroDeseados") = 0 Then
                    iconoFiltroDeseados.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleUp
                    gvFiltroDeseados.Visibility = Visibility.Collapsed
                End If

                filtros.Sort(Function(x As FiltroDeseado, y As FiltroDeseado)
                                 Dim resultado As Integer = x.Titulo.CompareTo(y.Titulo)
                                 If resultado = 0 Then
                                     resultado = x.Titulo.CompareTo(y.Titulo)
                                 End If
                                 Return resultado
                             End Function)

                For Each filtro In filtros
                    Dim imagenFiltro As New ImageEx With {
                        .Source = filtro.Imagen,
                        .IsCacheEnabled = True
                    }

                    Dim botonFiltro As New Button With {
                        .Tag = filtro,
                        .Padding = New Thickness(0, 0, 0, 0),
                        .Content = imagenFiltro
                    }

                    AddHandler botonFiltro.Click, AddressOf AbrirFiltroDeseadoClick
                    AddHandler botonFiltro.PointerEntered, AddressOf EfectosHover.Entra_Boton_Imagen
                    AddHandler botonFiltro.PointerExited, AddressOf EfectosHover.Sale_Boton_Imagen

                    gvFiltroDeseados.Items.Add(botonFiltro)
                Next
            Else
                spFiltroDeseados.Visibility = Visibility.Collapsed
            End If

            '---------------------------------------------------------

            If entradas.Count > 0 Then
                Dim listaTiendas As New List(Of String)

                For Each entrada In entradas
                    Dim añadir As Boolean = True

                    If listaTiendas.Count > 0 Then
                        For Each tienda In listaTiendas
                            If tienda = entrada.Tienda Then
                                añadir = False
                            End If
                        Next
                    End If

                    If entrada.Tienda = Nothing Then
                        añadir = False
                    Else
                        If entrada.Tienda = "false" Then
                            añadir = False
                        End If
                    End If

                    If añadir = True Then
                        listaTiendas.Add(entrada.Tienda)
                    End If
                Next

                Dim spFiltroTiendas As StackPanel = pagina.FindName("spFiltroTiendas")

                If listaTiendas.Count > 0 Then
                    spFiltroTiendas.Visibility = Visibility.Visible

                    Dim iconoFiltroTiendas As FontAwesome5.FontAwesome = pagina.FindName("iconoFiltroTiendas")
                    Dim gvFiltroTiendas As AdaptiveGridView = pagina.FindName("gvFiltroTiendas")

                    If config.Values("FiltroTiendas") = 1 Then
                        iconoFiltroTiendas.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleDown
                        gvFiltroTiendas.Visibility = Visibility.Visible
                    ElseIf config.Values("FiltroTiendas") = 0 Then
                        iconoFiltroTiendas.Icon = FontAwesome5.EFontAwesomeIcon.Solid_AngleUp
                        gvFiltroTiendas.Visibility = Visibility.Collapsed
                    End If

                    listaTiendas.Sort()

                    For Each tienda In listaTiendas
                        Dim entradasTienda As New List(Of Entrada)

                        For Each entrada In entradas
                            If tienda = entrada.Tienda Then
                                entradasTienda.Add(entrada)
                            End If
                        Next

                        Dim fondoFiltro As New SolidColorBrush With {
                            .Color = App.Current.Resources("ColorCuarto"),
                            .Opacity = 0.4
                        }

                        Dim tbFiltro As New TextBlock With {
                            .Text = tienda,
                            .Foreground = New SolidColorBrush(Colors.White)
                        }

                        Dim botonFiltro As New Button With {
                            .Tag = entradasTienda,
                            .Padding = New Thickness(0, 0, 0, 0),
                            .Content = tbFiltro,
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .VerticalAlignment = VerticalAlignment.Stretch,
                            .Background = fondoFiltro
                        }

                        AddHandler botonFiltro.Click, AddressOf AbrirFiltroTiendaClick
                        AddHandler botonFiltro.PointerEntered, AddressOf EfectosHover.Entra_Basico
                        AddHandler botonFiltro.PointerExited, AddressOf EfectosHover.Sale_Basico

                        gvFiltroTiendas.Items.Add(botonFiltro)
                    Next
                Else
                    spFiltroTiendas.Visibility = Visibility.Collapsed
                End If
            End If

        End Sub

        Private Sub AbrirFiltroDeseadoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim filtro As FiltroDeseado = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
            spEntradas.Children.Clear()

            For Each entrada2 In filtro.Entradas
                If Not entrada2.Juego Is Nothing Then
                    Dim listaJuegos As New List(Of EntradaOfertasJuego) From {
                        entrada2.Juego
                    }

                    Dim json As String = GenerarJsonOfertas(listaJuegos, Nothing)

                    If entrada2.Juego Is Nothing Then
                        If entrada2.Entrada.Categorias(0) = 3 Then
                            json = entrada2.Entrada.Json
                        End If
                    End If

                    Dim entrada As New Entrada With {
                        .TiendaLogo = entrada2.Entrada.TiendaLogo,
                        .Json = json
                    }
                    entrada.Categorias = entrada2.Entrada.Categorias

                    spEntradas.Children.Add(Interfaz.Entradas.GenerarEntrada(entrada))
                Else
                    spEntradas.Children.Add(Interfaz.Entradas.GenerarEntrada(entrada2.Entrada))
                End If
            Next

        End Sub

        Private Sub AbrirFiltroTiendaClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim entradas As List(Of Entrada) = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
            spEntradas.Children.Clear()

            For Each entrada In entradas
                spEntradas.Children.Add(Interfaz.Entradas.GenerarEntrada(entrada))
            Next
        End Sub

    End Module
End Namespace