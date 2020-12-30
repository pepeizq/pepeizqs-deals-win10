Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.System
Imports Windows.UI

Namespace Interfaz
    Module EntradaExpandida

        Public Sub Generar(json As EntradaOfertas)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridEntradaExpandida As Grid = pagina.FindName("gridEntradaExpandida")
            Pestañas.Visibilidad(gridEntradaExpandida, Nothing, Nothing)

            Dim botonVolver As Button = pagina.FindName("botonVolver")

            RemoveHandler botonVolver.Click, AddressOf VolverClick
            AddHandler botonVolver.Click, AddressOf VolverClick

            RemoveHandler botonVolver.PointerEntered, AddressOf EfectosHover.Entra_Boton_IconoTexto
            AddHandler botonVolver.PointerEntered, AddressOf EfectosHover.Entra_Boton_IconoTexto

            RemoveHandler botonVolver.PointerExited, AddressOf EfectosHover.Sale_Boton_IconoTexto
            AddHandler botonVolver.PointerExited, AddressOf EfectosHover.Sale_Boton_IconoTexto

            Dim cbOrdenarEntradaExpandida As ComboBox = pagina.FindName("cbOrdenarEntradaExpandida")
            cbOrdenarEntradaExpandida.Tag = json

            RemoveHandler cbOrdenarEntradaExpandida.SelectionChanged, AddressOf CambiarOrdenarListado
            AddHandler cbOrdenarEntradaExpandida.SelectionChanged, AddressOf CambiarOrdenarListado

            RemoveHandler cbOrdenarEntradaExpandida.PointerEntered, AddressOf EfectosHover.Entra_Basico
            AddHandler cbOrdenarEntradaExpandida.PointerEntered, AddressOf EfectosHover.Entra_Basico

            RemoveHandler cbOrdenarEntradaExpandida.PointerExited, AddressOf EfectosHover.Sale_Basico
            AddHandler cbOrdenarEntradaExpandida.PointerExited, AddressOf EfectosHover.Sale_Basico

            Dim spEntradaExpandida As StackPanel = pagina.FindName("spEntradaExpandida")
            spEntradaExpandida.Children.Clear()

            json = GenerarOrdenado(json, cbOrdenarEntradaExpandida.SelectedIndex)
            GenerarListado(spEntradaExpandida, json)

            Dim tbMensajeEntradaExpandida As TextBlock = pagina.FindName("tbMensajeEntradaExpandida")

            If Not json.Mensaje = Nothing Then
                If json.Mensaje.Trim.Length > 4 Then
                    tbMensajeEntradaExpandida.Visibility = Visibility.Visible
                    tbMensajeEntradaExpandida.Text = json.Mensaje.Trim
                Else
                    tbMensajeEntradaExpandida.Visibility = Visibility.Collapsed
                End If
            Else
                tbMensajeEntradaExpandida.Visibility = Visibility.Collapsed
            End If

            Dim svEntradaExpandida As ScrollViewer = pagina.FindName("svEntradaExpandida")

            RemoveHandler svEntradaExpandida.ViewChanging, AddressOf SvScroll
            AddHandler svEntradaExpandida.ViewChanging, AddressOf SvScroll

            Dim botonSubirEntradaExpandida As Button = pagina.FindName("botonSubirEntradaExpandida")

            RemoveHandler botonSubirEntradaExpandida.Click, AddressOf SubirClick
            AddHandler botonSubirEntradaExpandida.Click, AddressOf SubirClick

            RemoveHandler botonSubirEntradaExpandida.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono
            AddHandler botonSubirEntradaExpandida.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono

            RemoveHandler botonSubirEntradaExpandida.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono
            AddHandler botonSubirEntradaExpandida.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono

            Dim botonExportarTexto As Button = pagina.FindName("botonExportarTexto")
            botonExportarTexto.Tag = json

            RemoveHandler botonExportarTexto.Click, AddressOf ExportarTextoClick
            AddHandler botonExportarTexto.Click, AddressOf ExportarTextoClick

            RemoveHandler botonExportarTexto.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto
            AddHandler botonExportarTexto.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto

            RemoveHandler botonExportarTexto.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto
            AddHandler botonExportarTexto.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto

            Dim botonExportarBBCode As Button = pagina.FindName("botonExportarBBCode")
            botonExportarBBCode.Tag = json

            RemoveHandler botonExportarBBCode.Click, AddressOf ExportarBBCodeClick
            AddHandler botonExportarBBCode.Click, AddressOf ExportarBBCodeClick

            RemoveHandler botonExportarBBCode.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto
            AddHandler botonExportarBBCode.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto

            RemoveHandler botonExportarBBCode.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto
            AddHandler botonExportarBBCode.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto

            Dim botonExportarReddit As Button = pagina.FindName("botonExportarReddit")
            botonExportarReddit.Tag = json

            RemoveHandler botonExportarReddit.Click, AddressOf ExportarRedditClick
            AddHandler botonExportarReddit.Click, AddressOf ExportarRedditClick

            RemoveHandler botonExportarReddit.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto
            AddHandler botonExportarReddit.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto

            RemoveHandler botonExportarReddit.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto
            AddHandler botonExportarReddit.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto

        End Sub

        Private Sub CambiarOrdenarListado(sender As Object, e As SelectionChangedEventArgs)

            Dim cb As ComboBox = sender
            Dim json As EntradaOfertas = cb.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spEntradaExpandida As StackPanel = pagina.FindName("spEntradaExpandida")
            spEntradaExpandida.Children.Clear()

            json = GenerarOrdenado(json, cb.SelectedIndex)
            GenerarListado(spEntradaExpandida, json)

        End Sub

        Private Function GenerarOrdenado(json As EntradaOfertas, ordenado As Integer)

            If ordenado = 0 Then
                json.Juegos.Sort(Function(x As EntradaOfertasJuego, y As EntradaOfertasJuego)
                                     Dim resultado As Integer = x.Titulo.CompareTo(y.Titulo)
                                     Return resultado
                                 End Function)
            ElseIf ordenado = 1 Then
                json.Juegos.Sort(Function(x As EntradaOfertasJuego, y As EntradaOfertasJuego)
                                     Dim resultado As Integer = y.Descuento.CompareTo(x.Descuento)
                                     If resultado = 0 Then
                                         resultado = x.Titulo.CompareTo(y.Titulo)
                                     End If
                                     Return resultado
                                 End Function)
            ElseIf ordenado = 2 Then
                json.Juegos.Sort(Function(x As EntradaOfertasJuego, y As EntradaOfertasJuego)
                                     If x.AnalisisPorcentaje = Nothing Or x.AnalisisPorcentaje = "null" Then
                                         x.AnalisisPorcentaje = "0"
                                     End If

                                     If y.AnalisisPorcentaje = Nothing Or y.AnalisisPorcentaje = "null" Then
                                         y.AnalisisPorcentaje = "0"
                                     End If

                                     Dim resultado As Integer = y.AnalisisPorcentaje.CompareTo(x.AnalisisPorcentaje)
                                     If resultado = 0 Then
                                         resultado = x.Titulo.CompareTo(y.Titulo)
                                     End If
                                     Return resultado
                                 End Function)

            End If

            Return json

        End Function

        Private Sub GenerarListado(spEntradaExpandida As StackPanel, json As EntradaOfertas)

            spEntradaExpandida.Tag = json

            Dim recursos As New Resources.ResourceLoader()

            Dim i As Integer = spEntradaExpandida.Children.Count
            Dim j As Integer = spEntradaExpandida.Children.Count + 50

            For Each juego In json.Juegos
                Dim añadir As Boolean = True

                For Each boton In spEntradaExpandida.Children
                    Dim boton2 As Button = boton
                    Dim enlace As String = boton2.Tag

                    If enlace = juego.Enlace Then
                        añadir = False
                    End If
                Next

                If añadir = True Then
                    i += 1

                    If i < j Then
                        Dim fondoMaestro As New SolidColorBrush With {
                            .Color = App.Current.Resources("ColorCuarto"),
                            .Opacity = 0.3
                        }

                        Dim gridMaestro As New Grid With {
                            .Tag = juego,
                            .Background = fondoMaestro,
                            .Padding = New Thickness(20, 20, 20, 20),
                            .IsHitTestVisible = False
                        }

                        Dim col1 As New ColumnDefinition
                        Dim col2 As New ColumnDefinition

                        col1.Width = New GridLength(1, GridUnitType.Auto)
                        col2.Width = New GridLength(1, GridUnitType.Star)

                        gridMaestro.ColumnDefinitions.Add(col1)
                        gridMaestro.ColumnDefinitions.Add(col2)

                        Dim spIzquierda As New StackPanel With {
                            .Orientation = Orientation.Vertical,
                            .Margin = New Thickness(10, 10, 30, 10),
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        Dim imagenJuego As New ImageEx With {
                            .IsCacheEnabled = True,
                            .MaxWidth = 250,
                            .MaxHeight = 150,
                            .Source = juego.Imagen,
                            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .EnableLazyLoading = True
                        }

                        spIzquierda.Children.Add(imagenJuego)

                        spIzquierda.SetValue(Grid.ColumnProperty, 0)
                        gridMaestro.Children.Add(spIzquierda)

                        '-----------------------------------------------

                        Dim spDerecha As New StackPanel With {
                            .Orientation = Orientation.Vertical,
                            .Margin = New Thickness(10, 10, 10, 10),
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        Dim tbTitulo As New TextBlock With {
                            .Text = juego.Titulo,
                            .Foreground = New SolidColorBrush(Colors.White),
                            .FontSize = 17,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontWeight = Text.FontWeights.SemiBold,
                            .Margin = New Thickness(0, 0, 0, 20)
                        }

                        spDerecha.Children.Add(tbTitulo)

                        Dim spDatos As New StackPanel With {
                            .Orientation = Orientation.Horizontal
                        }

                        Dim tbDescuento As New TextBlock With {
                            .Text = juego.Descuento,
                            .Foreground = New SolidColorBrush(Colors.White),
                            .FontSize = 17,
                            .FontWeight = Text.FontWeights.SemiBold
                        }

                        Dim spDescuento As New StackPanel With {
                            .Background = New SolidColorBrush(Colors.ForestGreen),
                            .Padding = New Thickness(10, 6, 8, 8)
                        }

                        spDescuento.Children.Add(tbDescuento)

                        spDatos.Children.Add(spDescuento)

                        Dim tbPrecio As New TextBlock With {
                            .Text = juego.Precio,
                            .Foreground = New SolidColorBrush(Colors.White),
                            .FontSize = 17,
                            .FontWeight = Text.FontWeights.SemiBold
                        }

                        Dim spPrecio As New StackPanel With {
                            .Background = New SolidColorBrush(Colors.Black),
                            .Padding = New Thickness(10, 6, 8, 8)
                        }

                        spPrecio.Children.Add(tbPrecio)

                        spDatos.Children.Add(spPrecio)

                        If Not juego.DRM = Nothing Then
                            Dim imagenOrigen As String = String.Empty

                            If juego.DRM.ToLower.Contains("steam") Then
                                imagenOrigen = "Assets/DRM/steam.png"
                            ElseIf juego.DRM.ToLower.Contains("ubi") Or juego.DRM.ToLower.Contains("uplay") Then
                                imagenOrigen = "Assets/DRM/ubi.png"
                            ElseIf juego.DRM.ToLower.Contains("bethesda") Then
                                imagenOrigen = "Assets/DRM/bethesda.png"
                            End If

                            If Not imagenOrigen = String.Empty Then
                                Dim imagenDRM As New ImageEx With {
                                    .Source = imagenOrigen,
                                    .MaxHeight = 20,
                                    .Margin = New Thickness(20, 0, 0, 0),
                                    .VerticalAlignment = VerticalAlignment.Center
                                }

                                spDatos.Children.Add(imagenDRM)
                            End If
                        End If

                        If Not juego.AnalisisPorcentaje = Nothing Then
                            If Not juego.AnalisisPorcentaje = "null" And Not juego.AnalisisPorcentaje = "0" Then
                                Dim imagenAnalisis As New ImageEx With {
                                    .Width = 16,
                                    .Height = 16,
                                    .IsCacheEnabled = True,
                                    .Margin = New Thickness(20, 0, 0, 0),
                                    .VerticalAlignment = VerticalAlignment.Center
                                }

                                If juego.AnalisisPorcentaje > 74 Then
                                    imagenAnalisis.Source = "Assets/Reviews/positive.png"
                                ElseIf juego.AnalisisPorcentaje > 49 And juego.AnalisisPorcentaje < 75 Then
                                    imagenAnalisis.Source = "Assets/Reviews/mixed.png"
                                ElseIf juego.AnalisisPorcentaje < 50 Then
                                    imagenAnalisis.Source = "Assets/Reviews/negative.png"
                                End If

                                spDatos.Children.Add(imagenAnalisis)

                                Dim tbAnalisis As New TextBlock With {
                                    .Foreground = New SolidColorBrush(Colors.White),
                                    .FontSize = 13,
                                    .Text = juego.AnalisisPorcentaje + "% • " + juego.AnalisisCantidad + " " + recursos.GetString("Reviews"),
                                    .VerticalAlignment = VerticalAlignment.Center,
                                    .Margin = New Thickness(10, 0, 0, 2),
                                    .FontWeight = Text.FontWeights.SemiBold
                                }

                                spDatos.Children.Add(tbAnalisis)
                            End If
                        End If

                        spDerecha.Children.Add(spDatos)

                        spDerecha.SetValue(Grid.ColumnProperty, 1)
                        gridMaestro.Children.Add(spDerecha)

                        '-----------------------------------------------

                        Dim boton As New Button With {
                            .Content = gridMaestro,
                            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .Margin = New Thickness(0, 0, 0, 30),
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .HorizontalContentAlignment = HorizontalAlignment.Stretch,
                            .Padding = New Thickness(0, 0, 0, 0),
                            .Tag = juego.Enlace
                        }

                        AddHandler boton.Click, AddressOf AbrirEnlaceClick
                        AddHandler boton.PointerEntered, AddressOf EfectosHover.Entra_Boton2
                        AddHandler boton.PointerExited, AddressOf EfectosHover.Sale_Boton2

                        spEntradaExpandida.Children.Add(boton)
                    End If
                End If
            Next

            If spEntradaExpandida.Children.Count > 1 Then
                If json.Juegos.Count = spEntradaExpandida.Children.Count Then
                    Dim boton As Button = spEntradaExpandida.Children(spEntradaExpandida.Children.Count - 1)
                    boton.Margin = New Thickness(0, 0, 0, 0)
                End If
            End If

        End Sub

        Private Sub VolverClick(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridEntradas As Grid = pagina.FindName("gridEntradas")
            Pestañas.Visibilidad(gridEntradas, Nothing, Nothing)

        End Sub

        Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim enlace As String = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(Referidos.Generar(enlace)))

        End Sub

        Private Sub SvScroll(sender As Object, e As ScrollViewerViewChangingEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim sv As ScrollViewer = sender

            Dim botonSubir As Button = pagina.FindName("botonSubirEntradaExpandida")

            If sv.VerticalOffset > 50 Then
                botonSubir.Visibility = Visibility.Visible
            Else
                botonSubir.Visibility = Visibility.Collapsed
            End If

            If sv.ScrollableHeight < sv.VerticalOffset + 100 Then
                Dim spEntradaExpandida As StackPanel = pagina.FindName("spEntradaExpandida")
                Dim json As EntradaOfertas = spEntradaExpandida.Tag

                GenerarListado(spEntradaExpandida, json)
            End If

        End Sub

        Private Sub SubirClick(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim sv As ScrollViewer = pagina.FindName("svEntradaExpandida")
            sv.ChangeView(Nothing, 0, Nothing)

        End Sub

        Private Sub ExportarTextoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim json As EntradaOfertas = boton.Tag

            Exportar.Texto.Generar(json)

        End Sub

        Private Sub ExportarBBCodeClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim json As EntradaOfertas = boton.Tag

            Exportar.BBCode.Generar(json)

        End Sub

        Private Sub ExportarRedditClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim json As EntradaOfertas = boton.Tag

            Exportar.Reddit.Generar(json)

        End Sub

    End Module
End Namespace