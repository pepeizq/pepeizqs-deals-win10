Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq_s_deals___Windows_10.Buscador
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace Interfaz
    Module Buscador

        Public Function ResultadoWeb(resultado As pepeizqdeals)

            Dim spJuego As New StackPanel With {
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 2),
                .Orientation = Orientation.Vertical,
                .Padding = New Thickness(5, 0, 5, 0)
            }

            Dim tbPrecio As New TextBlock With {
                .Text = WebUtility.HtmlDecode(resultado.Titulo),
                .Foreground = New SolidColorBrush(Colors.White),
                .HorizontalAlignment = HorizontalAlignment.Stretch,
                .FontSize = 16,
                .Margin = New Thickness(5, 8, 5, 8),
                .TextWrapping = TextWrapping.Wrap
            }

            spJuego.Children.Add(tbPrecio)

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(5, 10, 5, 10),
                .Content = spJuego,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler boton.Click, AddressOf AbrirEnlaceClick
            AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaWeb
            AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaWeb

            Return boton

        End Function

        Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As pepeizqdeals = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(resultado.Enlace))

        End Sub

        Public Sub UsuarioEntraBotonBusquedaWeb(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content
            sp.BorderBrush = New SolidColorBrush(Colors.White)

            Dim tb As TextBlock = sp.Children(0)
            tb.Saturation(1).Scale(1.05, 1.05, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonBusquedaWeb(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content
            sp.BorderBrush = New SolidColorBrush(Colors.Transparent)

            Dim tb As TextBlock = sp.Children(0)
            tb.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '---------------------------------------------------------------

        Public Function ResultadoSteam(resultado As SteamWeb)

            Dim imagen As New ImageEx With {
                .Source = resultado.Imagen,
                .IsCacheEnabled = True,
                .Tag = resultado
            }

            AddHandler imagen.ImageExFailed, AddressOf ImagenFalla

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(5, 5, 5, 5),
                .Content = imagen,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler boton.Click, AddressOf AbrirJuegoClick
            AddHandler boton.PointerEntered, AddressOf Entra_Boton_Imagen
            AddHandler boton.PointerExited, AddressOf Sale_Boton_Imagen

            Return boton

        End Function

        Private Sub ImagenFalla(sender As Object, e As ImageExFailedEventArgs)

            Dim borrar As Boolean = True

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim imagen As ImageEx = sender
            Dim imagenFuente As String = imagen.Source

            If imagenFuente.Contains("/library_600x900.jpg") Then
                imagen.Source = imagenFuente.Replace("/library_600x900.jpg", "/header.jpg")
                borrar = False
            End If

            If borrar = True Then
                Dim juego As SteamWeb = imagen.Tag

                Dim gv1 As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")

                For Each item In gv1.Items
                    Dim sp As Button = item
                    Dim juegoItem As SteamWeb = sp.Tag

                    If juego.ID = juegoItem.ID Then
                        gv1.Items.Remove(item)
                    End If
                Next

                Dim gv2 As AdaptiveGridView = pagina.FindName("gvDeseadosJuegos")

                For Each item In gv2.Items
                    Dim sp As Button = item
                    Dim juegoItem As SteamWeb = sp.Tag

                    If juego.ID = juegoItem.ID Then
                        gv2.Items.Remove(item)
                    End If
                Next
            End If

        End Sub

        Private Sub AbrirJuegoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As SteamWeb = boton.Tag

            BuscarJuego(resultado)

        End Sub

        '---------------------------------------------------------------

        Public Sub AñadirTienda(tiendas As List(Of Tienda), nuevaTienda As Tienda)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim añadir As Boolean = True

            For Each tienda In tiendas
                If tienda.Enlace = nuevaTienda.Enlace Then
                    añadir = False
                End If
            Next

            If añadir = True Then
                tiendas.Add(nuevaTienda)

                If config.Values("Estado_App") = 1 Then
                    If Pais.DetectarEuro = True Then
                        tiendas.Sort(Function(x As Tienda, y As Tienda)
                                         Dim precioX As String = x.Precio

                                         If precioX.Length = 6 Then
                                             precioX = "0" + precioX
                                         End If

                                         Dim precioY As String = y.Precio

                                         If precioY.Length = 6 Then
                                             precioY = "0" + precioY
                                         End If

                                         Dim resultado As Integer = precioX.CompareTo(precioY)
                                         Return resultado
                                     End Function)
                    End If
                End If

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim lvTiendas As ListView = pagina.FindName("lvBusquedaJuegoTiendas")
                lvTiendas.Items.Clear()

                For Each tienda In tiendas
                    Dim grid As New Grid

                    Dim col1 As New ColumnDefinition
                    Dim col2 As New ColumnDefinition
                    Dim col3 As New ColumnDefinition
                    Dim col4 As New ColumnDefinition

                    col1.Width = New GridLength(1, GridUnitType.Auto)
                    col2.Width = New GridLength(1, GridUnitType.Auto)
                    col3.Width = New GridLength(1, GridUnitType.Star)
                    col4.Width = New GridLength(1, GridUnitType.Auto)

                    grid.ColumnDefinitions.Add(col1)
                    grid.ColumnDefinitions.Add(col2)
                    grid.ColumnDefinitions.Add(col3)
                    grid.ColumnDefinitions.Add(col4)

                    Dim imagenTienda As New ImageEx With {
                        .Source = tienda.Imagen,
                        .IsCacheEnabled = True,
                        .Tag = tienda,
                        .Width = 200
                    }

                    imagenTienda.SetValue(Grid.ColumnProperty, 0)
                    grid.Children.Add(imagenTienda)

                    '-------------------------------

                    If Not tienda.Pais = Nothing Then
                        Dim imagenPais As New ImageEx With {
                            .Source = "Assets/Paises/" + tienda.Pais + ".png",
                            .IsCacheEnabled = True,
                            .Margin = New Thickness(30, 0, 0, 0),
                            .Width = 32,
                            .Height = 24,
                            .Opacity = 0.7
                        }

                        imagenPais.SetValue(Grid.ColumnProperty, 1)
                        grid.Children.Add(imagenPais)
                    End If

                    '-------------------------------

                    Dim tbPrecio As New TextBlock With {
                        .Text = tienda.Precio,
                        .Margin = New Thickness(0, 0, 0, 0),
                        .Foreground = New SolidColorBrush(Colors.White),
                        .FontSize = 22,
                        .HorizontalAlignment = HorizontalAlignment.Center,
                        .VerticalAlignment = VerticalAlignment.Center
                    }

                    tbPrecio.SetValue(Grid.ColumnProperty, 3)
                    grid.Children.Add(tbPrecio)

                    '-------------------------------

                    Dim fondoBoton As New SolidColorBrush With {
                        .Color = App.Current.Resources("ColorPrimario"),
                        .Opacity = 0.5
                    }

                    Dim boton As New Button With {
                        .Background = fondoBoton,
                        .Padding = New Thickness(40, 20, 40, 20),
                        .BorderThickness = New Thickness(0, 0, 0, 0),
                        .Tag = tienda,
                        .Margin = New Thickness(15, 15, 15, 15),
                        .Content = grid,
                        .HorizontalAlignment = HorizontalAlignment.Stretch,
                        .HorizontalContentAlignment = HorizontalAlignment.Stretch
                    }

                    AddHandler boton.Click, AddressOf AbrirTiendaClick
                    AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusquedaTienda
                    AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusquedaTienda

                    If Not tienda.Mensaje = Nothing Then
                        Dim tbMensaje As New TextBlock With {
                            .Text = tienda.Mensaje,
                            .TextWrapping = TextWrapping.Wrap
                        }

                        ToolTipService.SetToolTip(boton, tienda.Mensaje)
                        ToolTipService.SetPlacement(boton, PlacementMode.Bottom)
                    End If

                    lvTiendas.Items.Add(boton)
                Next
            End If

        End Sub

        Private Async Sub AbrirTiendaClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As Tienda = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(resultado.Enlace))

        End Sub

        Public Sub UsuarioEntraBotonBusquedaTienda(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content
            Dim imagen As ImageEx = grid.Children(0)
            imagen.Saturation(1).Scale(1.05, 1.05, imagen.ActualWidth / 2, imagen.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub UsuarioSaleBotonBusquedaTienda(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Content
            Dim imagen As ImageEx = grid.Children(0)
            imagen.Saturation(1).Scale(1, 1, imagen.ActualWidth / 2, imagen.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace

