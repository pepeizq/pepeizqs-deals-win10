Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Module Buscador

    Public Async Sub Busca(sender As Object, e As TextChangedEventArgs)

        Dim tb As TextBox = sender

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim grid As Grid = pagina.FindName("gridBusqueda")
        Dim spCarga As StackPanel = pagina.FindName("spBusquedaCarga")
        Dim sv As ScrollViewer = pagina.FindName("svBusquedaEntradas")

        Dim tbNoResultados As TextBlock = pagina.FindName("tbBusquedaNoResultados")
        tbNoResultados.Visibility = Visibility.Collapsed

        If tb.Text.Trim.Length > 0 Then
            grid.Visibility = Visibility.Visible
            spCarga.Visibility = Visibility.Visible
            sv.Visibility = Visibility.Collapsed

            Dim resultados As List(Of Busqueda) = Await Wordpress.Buscar(tb.Text.Trim)

            If resultados.Count > 0 Then
                Dim spResultados As StackPanel = pagina.FindName("spBusquedaEntradas")
                spResultados.Children.Clear()

                For Each resultado In resultados
                    spResultados.Children.Add(InterfazResultado(resultado))
                Next

                spCarga.Visibility = Visibility.Collapsed
                sv.Visibility = Visibility.Visible
            Else
                spCarga.Visibility = Visibility.Collapsed
                tbNoResultados.Visibility = Visibility.Visible
            End If
        Else
            grid.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Function InterfazResultado(resultado As Busqueda)

        Dim spJuego As New StackPanel With {
            .BorderBrush = New SolidColorBrush(Colors.White),
            .BorderThickness = New Thickness(0, 0, 0, 0),
            .Orientation = Orientation.Vertical,
            .Padding = New Thickness(5, 0, 5, 0)
        }

        Dim tbPrecio As New TextBlock With {
            .Text = resultado.Titulo,
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
            .Content = spJuego
        }

        AddHandler boton.Click, AddressOf AbrirEnlaceClick
        AddHandler boton.PointerEntered, AddressOf UsuarioEntraBotonBusqueda
        AddHandler boton.PointerExited, AddressOf UsuarioSaleBotonBusqueda

        Return boton

    End Function

    Private Async Sub AbrirEnlaceClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim resultado As Busqueda = boton.Tag

        Await Launcher.LaunchUriAsync(New Uri(resultado.Enlace))

    End Sub

    Public Sub UsuarioEntraBotonBusqueda(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content
        sp.BorderThickness = New Thickness(0, 0, 0, 2)

        Dim tb As TextBlock = sp.Children(0)
        tb.Saturation(1).Scale(1.05, 1.05, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBotonBusqueda(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content
        sp.BorderThickness = New Thickness(0, 0, 0, 0)

        Dim tb As TextBlock = sp.Children(0)
        tb.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
