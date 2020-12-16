Module EntradaExpandida

    Public Sub Generar(juegos As EntradaOfertas)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridEntradaExpandida As Grid = pagina.FindName("gridEntradaExpandida")
        Interfaz.Pestañas.Visibilidad_Pestañas(gridEntradaExpandida, Nothing)

        Dim botonVolver As Button = pagina.FindName("botonVolver")

        RemoveHandler botonVolver.Click, AddressOf VolverClick
        AddHandler botonVolver.Click, AddressOf VolverClick

    End Sub

    Private Sub VolverClick(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridEntradas As Grid = pagina.FindName("gridEntradas")
        Interfaz.Pestañas.Visibilidad_Pestañas(gridEntradas, Nothing)

    End Sub

End Module
