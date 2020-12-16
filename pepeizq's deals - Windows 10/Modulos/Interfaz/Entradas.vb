Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace Interfaz

    Module Entradas

        Public Function GenerarFecha(fecha As Date)

            Dim grid As New Grid With {
                .Margin = New Thickness(0, 20, 0, 40)
            }

            Dim col1 As New ColumnDefinition
            Dim col2 As New ColumnDefinition
            Dim col3 As New ColumnDefinition

            col1.Width = New GridLength(1, GridUnitType.Star)
            col2.Width = New GridLength(1, GridUnitType.Auto)
            col3.Width = New GridLength(1, GridUnitType.Star)

            grid.ColumnDefinitions.Add(col1)
            grid.ColumnDefinitions.Add(col2)
            grid.ColumnDefinitions.Add(col3)

            Dim subgrid1 As New Grid With {
                .BorderThickness = New Thickness(0, 0, 0, 1),
                .BorderBrush = New SolidColorBrush(Colors.White),
                .VerticalAlignment = VerticalAlignment.Center
            }

            subgrid1.SetValue(Grid.ColumnProperty, 0)
            grid.Children.Add(subgrid1)

            Dim tb As New TextBlock With {
                .Foreground = New SolidColorBrush(Colors.White),
                .Text = fecha.Day.ToString + "/" + fecha.Month.ToString + "/" + fecha.Year.ToString,
                .FontSize = 22,
                .Margin = New Thickness(25, 0, 25, 0)
            }

            tb.SetValue(Grid.ColumnProperty, 1)
            grid.Children.Add(tb)

            Dim subgrid2 As New Grid With {
                .BorderThickness = New Thickness(0, 0, 0, 1),
                .BorderBrush = New SolidColorBrush(Colors.White),
                .VerticalAlignment = VerticalAlignment.Center
            }

            subgrid2.SetValue(Grid.ColumnProperty, 2)
            grid.Children.Add(subgrid2)

            Return grid

        End Function

        Public Function GenerarEntrada(entrada As Entrada)

            Dim recursos As New Resources.ResourceLoader()

            Dim fondoMaestro As New SolidColorBrush With {
                .Color = App.Current.Resources("ColorCuarto"),
                .Opacity = 0.3
            }

            Dim gridMaestro As New Grid With {
                .Tag = entrada,
                .Margin = New Thickness(0, 0, 0, 60),
                .Background = fondoMaestro,
                .Padding = New Thickness(10, 10, 10, 10)
            }

            Dim col1 As New ColumnDefinition
            Dim col2 As New ColumnDefinition

            col1.Width = New GridLength(1, GridUnitType.Auto)
            col2.Width = New GridLength(1, GridUnitType.Star)

            gridMaestro.ColumnDefinitions.Add(col1)
            gridMaestro.ColumnDefinitions.Add(col2)

            Dim spIzquierda As New StackPanel With {
                .Orientation = Orientation.Vertical,
                .Margin = New Thickness(40, 30, 40, 30),
                .VerticalAlignment = VerticalAlignment.Center
            }

            Dim imagenTienda As New ImageEx With {
                .IsCacheEnabled = True,
                .Width = 180,
                .Source = entrada.TiendaLogo
            }

            spIzquierda.Children.Add(imagenTienda)

            If entrada.Categorias(0) = 12 Then
                Dim spGratis As New StackPanel With {
                    .Background = New SolidColorBrush(Colors.Black),
                    .Margin = New Thickness(0, 20, 0, 0),
                    .Padding = New Thickness(10, 8, 10, 8),
                    .HorizontalAlignment = HorizontalAlignment.Center
                }

                Dim tbGratis As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .FontSize = 20,
                    .Text = recursos.GetString("Free2"),
                    .FontWeight = Text.FontWeights.SemiBold
                }

                spGratis.Children.Add(tbGratis)
                spIzquierda.Children.Add(spGratis)
            End If

            spIzquierda.SetValue(Grid.ColumnProperty, 0)
            gridMaestro.Children.Add(spIzquierda)

            '------------------------------------

            Dim spDerecha As New StackPanel With {
                .Orientation = Orientation.Vertical
            }

            If entrada.Categorias(0) = 3 Then
                Dim json As EntradaOfertas = JsonConvert.DeserializeObject(Of EntradaOfertas)(entrada.Json)

                Dim gv As New AdaptiveGridView

                If json.Juegos.Count = 1 Then
                    gv.DesiredWidth = 400
                    gv.Padding = New Thickness(2, 2, 2, 2)
                ElseIf json.Juegos.Count > 1 Then
                    gv.DesiredWidth = 250
                End If

                For Each juego In json.Juegos
                    Dim fondoJuego As New SolidColorBrush With {
                        .Color = App.Current.Resources("ColorCuarto"),
                        .Opacity = 0.2
                    }

                    Dim spJuego As New StackPanel With {
                        .Orientation = Orientation.Vertical,
                        .Background = fondoJuego,
                        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                        .BorderThickness = New Thickness(1, 1, 1, 1)
                    }

                    Dim imagenJuego As New ImageEx With {
                        .IsCacheEnabled = True,
                        .Source = juego.Imagen,
                        .MaxHeight = 200,
                        .MaxWidth = 400
                    }

                    spJuego.Children.Add(imagenJuego)

                    Dim spDatos As New StackPanel With {
                        .Orientation = Orientation.Horizontal,
                        .HorizontalAlignment = HorizontalAlignment.Right
                    }

                    Dim tbDescuento As New TextBlock With {
                        .Text = juego.Descuento,
                        .Foreground = New SolidColorBrush(Colors.White),
                        .FontSize = 20,
                        .FontWeight = Text.FontWeights.SemiBold
                    }

                    Dim spDescuento As New StackPanel With {
                        .Background = New SolidColorBrush(Colors.ForestGreen),
                        .Padding = New Thickness(10, 8, 10, 8)
                    }

                    spDescuento.Children.Add(tbDescuento)

                    spDatos.Children.Add(spDescuento)

                    Dim tbPrecio As New TextBlock With {
                        .Text = juego.Precio,
                        .Foreground = New SolidColorBrush(Colors.White),
                        .FontSize = 20,
                        .FontWeight = Text.FontWeights.SemiBold
                    }

                    Dim spPrecio As New StackPanel With {
                        .Background = New SolidColorBrush(Colors.Black),
                        .Padding = New Thickness(10, 8, 10, 8)
                    }

                    spPrecio.Children.Add(tbPrecio)

                    spDatos.Children.Add(spPrecio)

                    spJuego.Children.Add(spDatos)

                    Dim boton As New Button With {
                        .Content = spJuego,
                        .Padding = New Thickness(10, 10, 10, 10),
                        .HorizontalAlignment = HorizontalAlignment.Stretch,
                        .HorizontalContentAlignment = HorizontalAlignment.Center,
                        .Tag = juego.Enlace,
                        .Background = New SolidColorBrush(Colors.Transparent),
                        .BorderBrush = New SolidColorBrush(Colors.Transparent),
                        .BorderThickness = New Thickness(0, 0, 0, 0)
                    }

                    If json.Juegos.Count = 1 Then
                        boton.Margin = New Thickness(10, 0, 10, 0)
                    ElseIf json.Juegos.Count > 1 Then
                        boton.Margin = New Thickness(0, 0, 0, 0)
                    End If

                    AddHandler boton.Click, AddressOf AbrirEnlaceClick
                    AddHandler boton.PointerEntered, AddressOf EfectosHover.Entra_Boton
                    AddHandler boton.PointerExited, AddressOf EfectosHover.Sale_Boton

                    gv.Items.Add(boton)
                Next

                spDerecha.Children.Add(gv)

                If Not entrada.JsonExpandido = Nothing Then
                    Dim jsonExpandido As EntradaOfertas = JsonConvert.DeserializeObject(Of EntradaOfertas)(entrada.JsonExpandido)

                    If jsonExpandido.Juegos.Count > 6 Then
                        Dim textoAmpliar As New TextBlock With {
                            .Text = recursos.GetString("ShowDeals"),
                            .Foreground = New SolidColorBrush(Colors.White),
                            .FontSize = 17
                        }

                        Dim botonAmpliar As New Button With {
                            .Content = textoAmpliar,
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .Margin = New Thickness(10, 10, 15, 15),
                            .Padding = New Thickness(0, 15, 0, 15),
                            .Tag = jsonExpandido
                        }

                        AddHandler botonAmpliar.Click, AddressOf AbrirEntradaExpandidaClick
                        AddHandler botonAmpliar.PointerEntered, AddressOf EfectosHover.Entra_Boton
                        AddHandler botonAmpliar.PointerExited, AddressOf EfectosHover.Sale_Boton

                        spDerecha.Children.Add(botonAmpliar)
                    End If
                End If
            ElseIf entrada.Categorias(0) = 12 Then
                Dim json As EntradaGratis = JsonConvert.DeserializeObject(Of EntradaGratis)(entrada.Json)

                Dim gv As New AdaptiveGridView With {
                    .DesiredWidth = 400,
                    .Padding = New Thickness(2, 2, 2, 2)
                }

                Dim imagenJuego As New ImageEx With {
                    .IsCacheEnabled = True,
                    .Source = json.Imagen,
                    .MaxHeight = 200,
                    .MaxWidth = 400,
                    .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                    .BorderThickness = New Thickness(1, 1, 1, 1)
                }

                Dim boton As New Button With {
                    .Content = imagenJuego,
                    .Padding = New Thickness(10, 10, 10, 10),
                    .Margin = New Thickness(10, 0, 10, 0),
                    .HorizontalAlignment = HorizontalAlignment.Stretch,
                    .HorizontalContentAlignment = HorizontalAlignment.Center,
                    .Tag = entrada.Redireccion,
                    .Background = New SolidColorBrush(Colors.Transparent),
                    .BorderBrush = New SolidColorBrush(Colors.Transparent),
                    .BorderThickness = New Thickness(0, 0, 0, 0)
                }

                AddHandler boton.Click, AddressOf AbrirEnlaceClick
                AddHandler boton.PointerEntered, AddressOf EfectosHover.Entra_Boton
                AddHandler boton.PointerExited, AddressOf EfectosHover.Sale_Boton

                gv.Items.Add(boton)

                spDerecha.Children.Add(gv)
            End If

            spDerecha.SetValue(Grid.ColumnProperty, 1)
            gridMaestro.Children.Add(spDerecha)

            Return gridMaestro

        End Function

        Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim enlace As String = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(Referidos.Generar(enlace)))

        End Sub

        Private Sub AbrirEntradaExpandidaClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim juegos As EntradaOfertas = boton.Tag

            EntradaExpandida.Generar(juegos)

        End Sub

        Public Sub UsuarioEntraBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content

            Dim subgrid As Grid = grid.Children(0)
            subgrid.Opacity = 0.2
            subgrid.Saturation(1).Scale(1.01, 1.01, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Dim titulo As TextBlock = grid.Children(1)
            titulo.Opacity = 1

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonEntrada(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content

            Dim subgrid As Grid = grid.Children(0)
            subgrid.Opacity = 1
            subgrid.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Dim titulo As TextBlock = grid.Children(1)
            titulo.Opacity = 0

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '------------------------------------------------------------------------------------------------

        Public Function GenerarCompartir(entrada As Entrada)

            Dim recursos As New Resources.ResourceLoader()

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(0, 0, 0, 20),
                .Padding = New Thickness(0, 10, 10, 10)
            }

            '------------------------------

            Dim panelCompartir As New DropShadowPanel With {
                .BlurRadius = 10,
                .ShadowOpacity = 0.9,
                .Color = App.Current.Resources("ColorCuarto"),
                .Margin = New Thickness(0, 0, 20, 0),
                .Tag = entrada
            }

            Dim tbCompartir As New TextBlock With {
                .Text = recursos.GetString("Share"),
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim compartir As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = tbCompartir,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler compartir.Click, AddressOf CompartirClick
            AddHandler compartir.PointerEntered, AddressOf EfectosHover.Entra_Boton_Texto
            AddHandler compartir.PointerExited, AddressOf EfectosHover.Sale_Boton_Texto

            panelCompartir.Content = compartir

            sp.Children.Add(panelCompartir)

            '------------------------------

            Dim panelTwitter As New DropShadowPanel With {
                .BlurRadius = 10,
                .ShadowOpacity = 0.9,
                .Color = App.Current.Resources("ColorCuarto"),
                .Margin = New Thickness(0, 0, 20, 0),
                .Tag = entrada
            }

            Dim iconoTwitter As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Twitter,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim twitter As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = iconoTwitter,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler twitter.Click, AddressOf TwitterClick
            AddHandler twitter.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono
            AddHandler twitter.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono

            panelTwitter.Content = twitter

            sp.Children.Add(panelTwitter)

            '------------------------------

            Dim panelReddit As New DropShadowPanel With {
                .BlurRadius = 10,
                .ShadowOpacity = 0.9,
                .Color = App.Current.Resources("ColorCuarto"),
                .Margin = New Thickness(0, 0, 20, 0),
                .Tag = entrada
            }

            Dim iconoReddit As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Reddit,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim reddit As New Button With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .HorizontalAlignment = HorizontalAlignment.Center,
                .Tag = entrada,
                .Content = iconoReddit,
                .Padding = New Thickness(15, 10, 15, 10),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler reddit.Click, AddressOf RedditClick
            AddHandler reddit.PointerEntered, AddressOf EfectosHover.Entra_Boton_Icono
            AddHandler reddit.PointerExited, AddressOf EfectosHover.Sale_Boton_Icono

            panelReddit.Content = reddit

            sp.Children.Add(panelReddit)

            '------------------------------

            Return sp

        End Function

        Private Sub CompartirClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim entrada As Entrada = boton.Tag

            Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
            ApplicationData.Current.LocalSettings.Values("compartir_titulo") = tituloTexto
            ApplicationData.Current.LocalSettings.Values("compartir_enlace") = entrada.Enlace

            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf CompartirVentana

            DataTransferManager.ShowShareUI()

        End Sub

        Public Sub CompartirVentana(sender As DataTransferManager, e As DataRequestedEventArgs)

            Dim request As DataRequest = e.Request
            request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("compartir_titulo")
            request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("compartir_enlace")))

        End Sub

        Private Async Sub TwitterClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim entrada As Entrada = boton.Tag

            Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
            Dim enlace As String = "https://twitter.com/intent/tweet?url=" + entrada.Enlace + "&title=" + WebUtility.UrlEncode(tituloTexto)

            Await Launcher.LaunchUriAsync(New Uri(enlace))

        End Sub

        Private Async Sub RedditClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim entrada As Entrada = boton.Tag

            Dim tituloTexto As String = WebUtility.HtmlDecode(entrada.Titulo.Texto)
            Dim enlace As String = "https://www.reddit.com/submit?url=" + entrada.Enlace + "&title=" + WebUtility.UrlEncode(tituloTexto)

            Await Launcher.LaunchUriAsync(New Uri(enlace))

        End Sub

    End Module

End Namespace