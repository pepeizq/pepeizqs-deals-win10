Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Function GenerarEntrada(entrada As Entrada)

        Dim panel As New DropShadowPanel With {
            .BlurRadius = 10,
            .ShadowOpacity = 0.9,
            .Color = Colors.Black,
            .Margin = New Thickness(0, 10, 0, 10)
        }

        Dim grid As New Grid With {
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorTercero")),
            .BorderThickness = New Thickness(2, 2, 2, 2)
        }

        Dim imagen As New ImageEx With {
            .Source = entrada.Imagen,
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill
        }

        grid.Children.Add(imagen)

        panel.Content = grid

        AddHandler panel.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler panel.PointerExited, AddressOf UsuarioSaleBoton

        Return panel
    End Function

    Public Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        Dim panel As DropShadowPanel = sender

    End Sub

    Public Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
