Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.System
Imports Windows.UI

Namespace Interfaz

    Module EntradasBotones

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonActualizar As Button = pagina.FindName("botonActualizar")

            AddHandler botonActualizar.Click, AddressOf ActualizarClick
            AddHandler botonActualizar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonActualizar.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonSubir As Button = pagina.FindName("botonSubir")

            AddHandler botonSubir.Click, AddressOf SubirClick
            AddHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonComprar As Button = pagina.FindName("botonComprar")

            AddHandler botonComprar.Click, AddressOf Trial.BotonComprarClick
            AddHandler botonComprar.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonComprar.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonSorteos As Button = pagina.FindName("botonSorteosImagen")

            AddHandler botonSorteos.Click, AddressOf AbrirSorteosClick
            AddHandler botonSorteos.PointerEntered, AddressOf Entra_Boton_1_02
            AddHandler botonSorteos.PointerExited, AddressOf Sale_Boton_1_02

        End Sub

        Private Sub ActualizarClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")

            If tbTitulo.Text.Contains(recursos.GetString("Bundles2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Bundles2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Deals2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Free2"), True, False)
            ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
                Wordpress.CargarEntradas(100, recursos.GetString("Subscriptions2"), True, False)
            Else
                Wordpress.CargarEntradas(100, Nothing, True, False)
            End If

        End Sub

        Private Sub SubirClick(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim svEntradas As ScrollViewer = pagina.FindName("svEntradas")

            svEntradas.ChangeView(Nothing, 0, Nothing)

        End Sub

        Private Async Sub AbrirSorteosClick(sender As Object, e As RoutedEventArgs)

            Dim pagina2 As Entrada = Await Wordpress.RecuperarPagina("3643")

            If Not pagina2 Is Nothing Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim spEntradas As StackPanel = pagina.FindName("spEntradas")
                spEntradas.Children.Clear()

                Dim html As String = pagina2.Contenido.Texto
                Dim listaSorteos As New List(Of String)

                Dim i As Integer = 0
                While i < 100
                    If html.Contains("https://www.steamgifts.com/giveaway/") Then
                        Dim temp, temp2 As String
                        Dim int, int2 As Integer

                        int = html.IndexOf("https://www.steamgifts.com/giveaway/")
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf(ChrW(34))
                        temp2 = temp.Remove(int2, temp.Length - int2)

                        temp2 = temp2.Replace("/signature.png", Nothing)
                        temp2 = temp2.Trim

                        html = html.Remove(0, int + 2)

                        Dim añadir As Boolean = True
                        For Each sorteo In listaSorteos
                            If temp2 = sorteo Then
                                añadir = False
                            End If
                        Next

                        If añadir = True Then
                            listaSorteos.Add(temp2.Trim)
                        End If
                    End If
                    i += 1
                End While

                If listaSorteos.Count > 0 Then
                    Dim recursos As New Resources.ResourceLoader

                    Dim gridMensaje As New Grid With {
                        .Margin = New Thickness(0, 0, 0, 20),
                        .HorizontalAlignment = HorizontalAlignment.Center
                    }

                    Dim col1 As New ColumnDefinition
                    Dim col2 As New ColumnDefinition

                    col1.Width = New GridLength(1, GridUnitType.Star)
                    col2.Width = New GridLength(1, GridUnitType.Auto)

                    gridMensaje.ColumnDefinitions.Add(col1)
                    gridMensaje.ColumnDefinitions.Add(col2)

                    Dim tbMensaje As New TextBlock With {
                        .Text = recursos.GetString("GiveawaysMessage"),
                        .Foreground = New SolidColorBrush(Colors.White),
                        .HorizontalAlignment = HorizontalAlignment.Center,
                        .Margin = New Thickness(10, 10, 40, 10),
                        .TextWrapping = TextWrapping.Wrap
                    }

                    tbMensaje.SetValue(Grid.ColumnProperty, 0)

                    gridMensaje.Children.Add(tbMensaje)

                    Dim tbAbrirGrupoSteam As New TextBlock With {
                        .Text = recursos.GetString("GiveawaysOpenSteam"),
                        .Foreground = New SolidColorBrush(Colors.White)
                    }

                    Dim fondo As New SolidColorBrush With {
                        .Color = App.Current.Resources("ColorCuarto"),
                        .Opacity = 0.2
                    }

                    Dim botonMensaje As New Button With {
                        .Content = tbAbrirGrupoSteam,
                        .Background = fondo,
                        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                        .BorderThickness = New Thickness(1, 1, 1, 1),
                        .HorizontalAlignment = HorizontalAlignment.Center,
                        .Padding = New Thickness(15, 12, 15, 12)
                    }

                    botonMensaje.SetValue(Grid.ColumnProperty, 1)

                    AddHandler botonMensaje.Click, AddressOf AbrirGrupoSteamClick
                    AddHandler botonMensaje.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_02
                    AddHandler botonMensaje.PointerExited, AddressOf EfectosHover.Sale_Boton_1_02

                    gridMensaje.Children.Add(botonMensaje)

                    spEntradas.Children.Add(gridMensaje)

                    '-----------------------------------------------

                    For Each sorteo In listaSorteos
                        Dim imagen As New ImageEx With {
                            .Source = sorteo + "/signature.png",
                            .MaxWidth = 500,
                            .EnableLazyLoading = True
                        }

                        Dim boton As New Button With {
                            .Content = imagen,
                            .Margin = New Thickness(0, 0, 0, 30),
                            .Background = fondo,
                            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .HorizontalAlignment = HorizontalAlignment.Center,
                            .Tag = sorteo
                        }

                        AddHandler boton.Click, AddressOf AbrirSorteoClick
                        AddHandler boton.PointerEntered, AddressOf EfectosHover.Entra_Boton_1_02
                        AddHandler boton.PointerExited, AddressOf EfectosHover.Sale_Boton_1_02

                        spEntradas.Children.Add(boton)
                    Next
                End If

                If spEntradas.Children.Count > 0 Then
                    Dim boton As Button = spEntradas.Children(spEntradas.Children.Count - 1)
                    boton.Margin = New Thickness(0, 0, 0, 0)
                End If
            End If

        End Sub

        Private Async Sub AbrirSorteoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim enlace As String = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri(enlace))

        End Sub

        Private Async Sub AbrirGrupoSteamClick(sender As Object, e As RoutedEventArgs)

            Await Launcher.LaunchUriAsync(New Uri("https://steamcommunity.com/groups/pepeizqdeals"))

        End Sub

    End Module

End Namespace