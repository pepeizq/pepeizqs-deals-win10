Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.System
Imports Windows.UI

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
                .Padding = New Thickness(20, 20, 20, 20)
            }

            Dim col1 As New ColumnDefinition
            Dim col2 As New ColumnDefinition

            col1.Width = New GridLength(1, GridUnitType.Auto)
            col2.Width = New GridLength(1, GridUnitType.Star)

            gridMaestro.ColumnDefinitions.Add(col1)
            gridMaestro.ColumnDefinitions.Add(col2)

            Dim spIzquierda As New StackPanel With {
                .Orientation = Orientation.Vertical,
                .Margin = New Thickness(30, 30, 40, 30),
                .VerticalAlignment = VerticalAlignment.Center
            }

            Dim imagenTienda As New ImageEx With {
                .IsCacheEnabled = True,
                .Width = 180,
                .Source = entrada.TiendaLogo
            }

            spIzquierda.Children.Add(imagenTienda)

            If entrada.Categorias(0) = 4 Then
                Dim temp As String = entrada.Titulo.Texto
                Dim int As Integer = temp.IndexOf("•")
                temp = temp.Remove(0, int + 1)

                Dim int2 As Integer = temp.IndexOf("•")
                Dim temp2 As String = temp.Remove(int2, temp.Length - int2)

                Dim precioBundle As String = temp2.Trim

                Dim spBundles As New StackPanel With {
                    .Background = New SolidColorBrush(Colors.Black),
                    .Margin = New Thickness(0, 20, 0, 0),
                    .Padding = New Thickness(10, 8, 10, 8),
                    .HorizontalAlignment = HorizontalAlignment.Center
                }

                Dim tbBundles As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .FontSize = 20,
                    .Text = precioBundle,
                    .FontWeight = Text.FontWeights.SemiBold
                }

                spBundles.Children.Add(tbBundles)
                spIzquierda.Children.Add(spBundles)
            ElseIf entrada.Categorias(0) = 12 Then
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
                        Dim fondoAmpliar As New SolidColorBrush With {
                            .Color = App.Current.Resources("ColorCuarto"),
                            .Opacity = 0.2
                        }

                        Dim textoAmpliar As New TextBlock With {
                            .Text = recursos.GetString("ShowDeals") + " (" + jsonExpandido.Juegos.Count.ToString + ")",
                            .Foreground = New SolidColorBrush(Colors.White),
                            .FontSize = 17
                        }

                        Dim botonAmpliar As New Button With {
                            .Content = textoAmpliar,
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .Margin = New Thickness(10, 10, 15, 15),
                            .Padding = New Thickness(0, 15, 0, 15),
                            .Tag = jsonExpandido,
                            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .Background = fondoAmpliar
                        }

                        AddHandler botonAmpliar.Click, AddressOf AbrirEntradaExpandidaClick
                        AddHandler botonAmpliar.PointerEntered, AddressOf EfectosHover.Entra_Boton
                        AddHandler botonAmpliar.PointerExited, AddressOf EfectosHover.Sale_Boton

                        spDerecha.Children.Add(botonAmpliar)
                    End If
                End If
            ElseIf entrada.Categorias(0) = 4 Then
                Dim json As EntradaBundles = JsonConvert.DeserializeObject(Of EntradaBundles)(entrada.Json)

                Dim gv As New AdaptiveGridView With {
                    .DesiredWidth = 250,
                    .Padding = New Thickness(5, 5, 0, 0),
                    .IsHitTestVisible = False
                }

                For Each juego In json.Juegos
                    Dim imagenJuego As New ImageEx With {
                        .IsCacheEnabled = True,
                        .Source = juego.Imagen,
                        .MaxHeight = 200,
                        .MaxWidth = 400,
                        .Margin = New Thickness(10, 10, 10, 10),
                        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                        .BorderThickness = New Thickness(1, 1, 1, 1)
                    }

                    gv.Items.Add(imagenJuego)
                Next

                Dim fondoBundle As New SolidColorBrush With {
                    .Color = App.Current.Resources("ColorCuarto"),
                    .Opacity = 0.2
                }

                Dim boton As New Button With {
                    .Content = gv,
                    .HorizontalAlignment = HorizontalAlignment.Stretch,
                    .HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    .Background = fondoBundle,
                    .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                    .BorderThickness = New Thickness(1, 1, 1, 1),
                    .Margin = New Thickness(10, 10, 10, 10),
                    .Tag = entrada.Redireccion
                }

                AddHandler boton.Click, AddressOf AbrirEnlaceClick
                AddHandler boton.PointerEntered, AddressOf EfectosHover.Entra_Boton
                AddHandler boton.PointerExited, AddressOf EfectosHover.Sale_Boton

                spDerecha.Children.Add(boton)

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

    End Module

End Namespace