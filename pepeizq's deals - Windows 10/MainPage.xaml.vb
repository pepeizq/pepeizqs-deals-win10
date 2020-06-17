﻿Imports Windows.ApplicationModel.Core
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        '--------------------------------------------------------

        CargaListado(Nothing, 0, spSeleccionarTodo)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        Dim barra2 As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView().TitleBar
        barra2.ExtendViewIntoTitleBar = True

    End Sub

    Public Async Sub CargaListado(categoria As String, tipo As Integer, sp As StackPanel)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        If Not categoria = Nothing Then
            tbTitulo.Text = tbTitulo.Text + " • " + categoria
        End If

        spBotonesSeleccion.IsHitTestVisible = False

        spSeleccionarTodo.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarBundles.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarOfertas.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarGratis.BorderThickness = New Thickness(0, 0, 0, 0)
        spSeleccionarSuscripciones.BorderThickness = New Thickness(0, 0, 0, 0)

        sp.BorderThickness = New Thickness(0, 0, 0, 2)

        botonActualizar.IsHitTestVisible = False

        '--------------------------------------

        lvPrincipal.IsItemClickEnabled = False

        prPrincipal.Visibility = Visibility.Visible
        prPrincipal.IsActive = True

        lvPrincipal.Items.Clear()

        Dim entradas As List(Of Entrada) = Await Wordpress.CargarEntradas

        If entradas.Count > 0 Then
            For Each entrada In entradas
                Dim añadirPrincipal As Boolean = False

                If tipo = 0 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Or categoria = 3 Or categoria = 12 Or categoria = 13 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 1 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 4 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 2 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 3 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 3 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 12 Then
                            añadirPrincipal = True
                        End If
                    Next
                ElseIf tipo = 4 Then
                    For Each categoria In entrada.Categorias
                        If categoria = 13 Then
                            añadirPrincipal = True
                        End If
                    Next
                End If

                If añadirPrincipal = True Then
                    lvPrincipal.Items.Add(Interfaz.GenerarEntrada(entrada))
                    lvPrincipal.Items.Add(Interfaz.GenerarCompartir(entrada))
                End If
            Next
        End If

        prPrincipal.Visibility = Visibility.Collapsed
        prPrincipal.IsActive = False

        botonActualizar.IsHitTestVisible = True
        spBotonesSeleccion.IsHitTestVisible = True
        lvPrincipal.IsItemClickEnabled = True

    End Sub

    Private Sub BotonSeleccionarTodo_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarTodo.Click

        CargaListado(Nothing, 0, spSeleccionarTodo)

    End Sub

    Private Sub BotonSeleccionarBundles_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarBundles.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(recursos.GetString("Bundles2"), 1, spSeleccionarBundles)

    End Sub

    Private Sub BotonSeleccionarOfertas_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarOfertas.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(recursos.GetString("Deals2"), 2, spSeleccionarOfertas)

    End Sub

    Private Sub BotonSeleccionarGratis_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarGratis.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(recursos.GetString("Free2"), 3, spSeleccionarGratis)

    End Sub

    Private Sub BotonSeleccionarSuscripciones_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionarSuscripciones.Click

        Dim recursos As New Resources.ResourceLoader()
        CargaListado(recursos.GetString("Subscriptions2"), 4, spSeleccionarSuscripciones)

    End Sub

    Private Sub BotonActualizar_Click(sender As Object, e As RoutedEventArgs) Handles botonActualizar.Click

        Dim recursos As New Resources.ResourceLoader()

        If tbTitulo.Text.Contains(recursos.GetString("Bundles2")) Then
            CargaListado(recursos.GetString("Bundles2"), 1, spSeleccionarBundles)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Deals2")) Then
            CargaListado(recursos.GetString("Deals2"), 2, spSeleccionarOfertas)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Free2")) Then
            CargaListado(recursos.GetString("Free2"), 3, spSeleccionarGratis)
        ElseIf tbTitulo.Text.Contains(recursos.GetString("Subscriptions2")) Then
            CargaListado(recursos.GetString("Subscriptions2"), 4, spSeleccionarSuscripciones)
        Else
            CargaListado(Nothing, 0, spSeleccionarTodo)
        End If

    End Sub

    Public Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Public Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Class
