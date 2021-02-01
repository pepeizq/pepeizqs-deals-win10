Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Services.Store
Imports Windows.Storage
Imports Windows.System

Module Trial

    Public Async Sub Detectar(primeraVez As Boolean)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        Dim usuarios As IReadOnlyList(Of User) = Await User.FindAllAsync

        If Not usuarios Is Nothing Then
            If usuarios.Count > 0 Then
                Dim usuario As User = usuarios(0)

                Dim contexto As StoreContext = StoreContext.GetForUser(usuario)
                Dim licencia As StoreAppLicense = Await contexto.GetAppLicenseAsync

                If licencia.IsActive = True And licencia.IsTrial = False Then
                    config.Values("Estado_App") = 1
                Else
                    config.Values("Estado_App") = 0
                End If
            End If
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spFiltros As StackPanel = pagina.FindName("spFiltros")
        Dim panelComparadores As DropShadowPanel = pagina.FindName("panelComparadores")
        Dim spExportarEntradaExpandida As StackPanel = pagina.FindName("spExportarEntradaExpandida")
        Dim spComprarApp As StackPanel = pagina.FindName("spComprarApp")

        If config.Values("Estado_App") = 1 Then
            If primeraVez = True Then
                Divisas.Generar()
            End If

            spFiltros.Visibility = Visibility.Visible
            panelComparadores.Visibility = Visibility.Visible
            spExportarEntradaExpandida.Visibility = Visibility.Visible

            spComprarApp.Visibility = Visibility.Collapsed

            Configuracion.Trial(True)
        Else
            spFiltros.Visibility = Visibility.Collapsed
            panelComparadores.Visibility = Visibility.Collapsed
            spExportarEntradaExpandida.Visibility = Visibility.Collapsed

            spComprarApp.Visibility = Visibility.Visible

            Configuracion.Trial(False)
        End If

    End Sub

    Public Async Sub BotonComprarClick(sender As Object, e As RoutedEventArgs)

        Dim usuarios As IReadOnlyList(Of User) = Await User.FindAllAsync

        If Not usuarios Is Nothing Then
            If usuarios.Count > 0 Then
                Dim usuario As User = usuarios(0)

                Dim contexto As StoreContext = StoreContext.GetForUser(usuario)
                Await contexto.RequestPurchaseAsync("9P7836M1TW15")
            End If
        End If

    End Sub

End Module
