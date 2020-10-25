Module GridVisibilidad

    Public Sub Mostrar(gridMostrar As String)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridCarga As Grid = pagina.FindName("gridCarga")
        gridCarga.Visibility = Visibility.Collapsed

        Dim gridEntradas As Grid = pagina.FindName("gridEntradas")
        gridEntradas.Visibility = Visibility.Collapsed

        Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
        gridBusqueda.Visibility = Visibility.Collapsed

        Dim gridBusquedaJuego As Grid = pagina.FindName("gridBusquedaJuego")
        gridBusquedaJuego.Visibility = Visibility.Collapsed

        Dim gridDeseados As Grid = pagina.FindName("gridDeseados")
        gridDeseados.Visibility = Visibility.Collapsed

        Dim gridMostrar2 As Grid = pagina.FindName(gridMostrar)
        gridMostrar2.Visibility = Visibility.Visible

    End Sub

End Module
