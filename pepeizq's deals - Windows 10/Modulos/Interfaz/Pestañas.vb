Namespace Interfaz
    Module Pestañas

        Public Sub Visibilidad_Pestañas(gridMostrar As Grid, tag As String)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

            If Not tag = Nothing Then
                tbTitulo.Text = tbTitulo.Text + " • " + tag
            End If

            Dim gridCarga As Grid = pagina.FindName("gridCarga")
            gridCarga.Visibility = Visibility.Collapsed

            Dim gridEntradas As Grid = pagina.FindName("gridEntradas")
            gridEntradas.Visibility = Visibility.Collapsed

            Dim gridEntradaExpandida As Grid = pagina.FindName("gridEntradaExpandida")
            gridEntradaExpandida.Visibility = Visibility.Collapsed

            Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
            gridBusqueda.Visibility = Visibility.Collapsed

            Dim gridBusquedaJuego As Grid = pagina.FindName("gridBusquedaJuego")
            gridBusquedaJuego.Visibility = Visibility.Collapsed

            Dim gridDeseados As Grid = pagina.FindName("gridDeseados")
            gridDeseados.Visibility = Visibility.Collapsed

            Dim gridMasCosas As Grid = pagina.FindName("gridMasCosas")
            gridMasCosas.Visibility = Visibility.Collapsed

            gridMostrar.Visibility = Visibility.Visible

        End Sub

    End Module
End Namespace

