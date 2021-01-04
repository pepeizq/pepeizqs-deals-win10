Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq_s_deals___Windows_10.Buscador
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace Interfaz
    Module Buscador

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbBuscador As TextBox = pagina.FindName("tbBuscador")
            tbBuscador.PlaceholderText = recursos.GetString("SearchGame")

            AddHandler tbBuscador.TextChanged, AddressOf Motor.Buscar

            Dim botonBusquedaWeb As Button = pagina.FindName("botonBusquedaWeb")

            AddHandler botonBusquedaWeb.Click, AddressOf MostrarResultadosWeb
            AddHandler botonBusquedaWeb.PointerEntered, AddressOf EfectosHover.Entra_Boton2
            AddHandler botonBusquedaWeb.PointerExited, AddressOf EfectosHover.Sale_Boton2

            Dim botonBusquedaSteam As Button = pagina.FindName("botonBusquedaSteam")

            AddHandler botonBusquedaSteam.Click, AddressOf MostrarResultadosSteam
            AddHandler botonBusquedaSteam.PointerEntered, AddressOf EfectosHover.Entra_Boton2
            AddHandler botonBusquedaSteam.PointerExited, AddressOf EfectosHover.Sale_Boton2

        End Sub

        Private Sub MostrarResultadosWeb(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spResultados As StackPanel = pagina.FindName("spBusquedaEntradasResultados")
            Dim iconoBusquedaWeb As FontAwesome5.FontAwesome = pagina.FindName("iconoBusquedaWeb")

            If spResultados.Visibility = Visibility.Visible Then
                spResultados.Visibility = Visibility.Collapsed
                iconoBusquedaWeb.Icon = FontAwesome5.EFontAwesomeIcon.Solid_CaretUp
            Else
                spResultados.Visibility = Visibility.Visible
                iconoBusquedaWeb.Icon = FontAwesome5.EFontAwesomeIcon.Solid_CaretDown
            End If

        End Sub

        Private Sub MostrarResultadosSteam(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gvResultados As AdaptiveGridView = pagina.FindName("gvBuscadorJuegos")
            Dim iconoBusquedaSteam As FontAwesome5.FontAwesome = pagina.FindName("iconoBusquedaSteam")

            If gvResultados.Visibility = Visibility.Visible Then
                gvResultados.Visibility = Visibility.Collapsed
                iconoBusquedaSteam.Icon = FontAwesome5.EFontAwesomeIcon.Solid_CaretUp
            Else
                gvResultados.Visibility = Visibility.Visible
                iconoBusquedaSteam.Icon = FontAwesome5.EFontAwesomeIcon.Solid_CaretDown
            End If

        End Sub

        '---------------------------------------------------------------

        Public Function ResultadoSteam(resultado As SteamWeb)

            Dim recursos As New Resources.ResourceLoader

            Dim imagen As New ImageEx With {
                .Source = resultado.Imagen,
                .IsCacheEnabled = True,
                .Tag = resultado
            }

            AddHandler imagen.ImageExFailed, AddressOf ImagenFalla

            Dim boton As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(0, 0, 0, 0),
                .Tag = resultado,
                .Margin = New Thickness(8, 8, 8, 8),
                .Content = imagen,
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .BorderThickness = New Thickness(1, 1, 1, 1)
            }

            AddHandler boton.Tapped, AddressOf AbrirJuegoClick
            AddHandler boton.PointerEntered, AddressOf Entra_Boton_Imagen
            AddHandler boton.PointerExited, AddressOf Sale_Boton_Imagen

            '------------------------------------------------

            Dim estilo As New Style(GetType(FlyoutPresenter))
            estilo.Setters.Add(New Setter(FlyoutPresenter.PaddingProperty, 0))
            estilo.Setters.Add(New Setter(FlyoutPresenter.BorderThicknessProperty, 0))
            estilo.Setters.Add(New Setter(FlyoutPresenter.BackgroundProperty, Colors.Transparent))

            Dim menu As New Flyout With {
                .ShowMode = FlyoutShowMode.Transient,
                .Placement = FlyoutPlacementMode.Bottom,
                .FlyoutPresenterStyle = estilo
            }

            Dim fondoSpMenu As New SolidColorBrush With {
                .Color = App.Current.Resources("ColorCuarto"),
                .Opacity = 0.95
            }

            Dim spMenu As New StackPanel With {
                .Orientation = Orientation.Vertical,
                .Background = fondoSpMenu,
                .Padding = New Thickness(15, 15, 15, 15)
            }

            Dim tbBuscador As New TextBlock With {
                .Text = recursos.GetString("OpenSearch"),
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim botonBuscador As New Button With {
                .Content = tbBuscador,
                .Padding = New Thickness(15, 12, 15, 12),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .Tag = resultado,
                .HorizontalAlignment = HorizontalAlignment.Stretch
            }
            spMenu.Children.Add(botonBuscador)

            AddHandler botonBuscador.Click, AddressOf AbrirBuscadorClick
            AddHandler botonBuscador.PointerEntered, AddressOf EfectosHover.Entra_Boton2
            AddHandler botonBuscador.PointerExited, AddressOf EfectosHover.Sale_Boton2

            Dim tbSteamDB As New TextBlock With {
                .Text = recursos.GetString("OpenSteamDB"),
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim botonSteamDB As New Button With {
                .Content = tbSteamDB,
                .Padding = New Thickness(15, 12, 15, 12),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .Margin = New Thickness(0, 10, 0, 0),
                .Tag = resultado.ID,
                .HorizontalAlignment = HorizontalAlignment.Stretch
            }
            spMenu.Children.Add(botonSteamDB)

            AddHandler botonSteamDB.Click, AddressOf AbrirSteamDBClick
            AddHandler botonSteamDB.PointerEntered, AddressOf EfectosHover.Entra_Boton2
            AddHandler botonSteamDB.PointerExited, AddressOf EfectosHover.Sale_Boton2

            Dim tbIsthereanydeal As New TextBlock With {
                .Text = recursos.GetString("OpenIsthereanydeal"),
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim botonIsthereanydeal As New Button With {
                .Content = tbIsthereanydeal,
                .Padding = New Thickness(15, 12, 15, 12),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .Margin = New Thickness(0, 10, 0, 0),
                .Tag = resultado.ID,
                .HorizontalAlignment = HorizontalAlignment.Stretch
            }
            spMenu.Children.Add(botonIsthereanydeal)

            AddHandler botonIsthereanydeal.Click, AddressOf AbrirIsthereanydealClick
            AddHandler botonIsthereanydeal.PointerEntered, AddressOf EfectosHover.Entra_Boton2
            AddHandler botonIsthereanydeal.PointerExited, AddressOf EfectosHover.Sale_Boton2

            menu.Content = spMenu
            FlyoutBase.SetAttachedFlyout(boton, menu)

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

            FlyoutBase.ShowAttachedFlyout(sender)

        End Sub

        Private Sub AbrirBuscadorClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim resultado As SteamWeb = boton.Tag

            BuscarJuego(resultado)

        End Sub

        Private Async Sub AbrirSteamDBClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim id As String = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri("https://steamdb.info/app/" + id + "/"))

        End Sub

        Private Async Sub AbrirIsthereanydealClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim id As String = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri("https://new.isthereanydeal.com/steam/app/" + id))

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

