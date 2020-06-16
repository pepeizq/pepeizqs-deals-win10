Imports Windows.ApplicationModel.Core
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Dim entradas As List(Of Entrada) = Await Wordpress.CargarEntradas

        For Each entrada In entradas
            lvTest.Items.Add(Interfaz.GenerarEntrada(entrada))
        Next

        '--------------------------------------------------------

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        Dim barra2 As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView().TitleBar
        barra2.ExtendViewIntoTitleBar = True

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        'gridAñadirTile.Visibility = Visibility.Collapsed
        'gridPersonalizarTiles.Visibility = Visibility.Collapsed
        'gridConfig.Visibility = Visibility.Collapsed
        'gridContactarAñadirJuegos.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Public Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Class
