Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Windows.UI
Imports Windows.UI.Core

Module NavigationViewItems

    Public Function Generar(titulo As String, simbolo As FontAwesome5.EFontAwesomeIcon, tag As String)

        Dim tb As New TextBlock With {
            .Text = titulo,
            .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimero"))
        }

        Dim item As New NavigationViewItem With {
            .Content = tb,
            .Foreground = New SolidColorBrush(Colors.White),
            .Tag = tag
        }

        If Not simbolo = Nothing Then
            Dim icono As New FontAwesome5.FontAwesome With {
                .Icon = simbolo,
                .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimero"))
            }

            item.Icon = icono
        End If

        Dim tbToolTip As TextBlock = New TextBlock With {
            .Text = titulo
        }

        ToolTipService.SetToolTip(item, tbToolTip)
        ToolTipService.SetPlacement(item, PlacementMode.Mouse)

        AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

        Return item

    End Function

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim item As NavigationViewItem = sender

        If Not item.Icon Is Nothing Then
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1.2, 1.2, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()
        End If

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim item As NavigationViewItem = sender

        If Not item.Icon Is Nothing Then
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()
        End If

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
