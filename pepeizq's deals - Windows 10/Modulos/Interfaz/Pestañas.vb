Imports Windows.UI.Xaml.Media.Animation

Namespace Interfaz
    Module Pestañas

        Public Sub Visibilidad(gridMostrar As Grid, tag As String, origen As Object)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

            If Not tag = Nothing Then
                If Not tag = 1208 Then
                    tbTitulo.Text = tbTitulo.Text + " • " + tag
                End If
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

            Dim gridConfig As Grid = pagina.FindName("gridConfig")
            gridConfig.Visibility = Visibility.Collapsed

            gridMostrar.Visibility = Visibility.Visible

            '--------------------------------------------------------

            If Not origen Is Nothing Then
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacion", origen)
                Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacion")

                If Not animacion Is Nothing Then
                    animacion.Configuration = New DirectConnectedAnimationConfiguration
                    animacion.TryStart(gridMostrar)
                End If
            End If

            '--------------------------------------------------------

            Dim spBusqueda As StackPanel = pagina.FindName("spBusqueda")

            If gridMostrar.Name = "gridCarga" Then
                spBusqueda.Visibility = Visibility.Collapsed
            Else
                spBusqueda.Visibility = Visibility.Visible
            End If

        End Sub

    End Module
End Namespace

