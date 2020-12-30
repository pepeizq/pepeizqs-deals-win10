Imports Windows.Storage

Module Configuracion

    Public Sub Cargar()

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tsConfigDeseados As ToggleSwitch = pagina.FindName("tsConfigDeseados")
        tsConfigDeseados.OnContent = recursos.GetString("ConfigWishlistMode1")
        tsConfigDeseados.OffContent = recursos.GetString("ConfigWishlistMode2")

        If Not config.Values("Deseados") Is Nothing Then
            If config.Values("Deseados") = 1 Then
                tsConfigDeseados.IsOn = True
            ElseIf config.Values("Deseados") = 0 Then
                tsConfigDeseados.IsOn = False
            End If
        Else
            config.Values("Deseados") = 1
            tsConfigDeseados.IsOn = True
        End If

        AddHandler tsConfigDeseados.Toggled, AddressOf DeseadosSwitch
        AddHandler tsConfigDeseados.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
        AddHandler tsConfigDeseados.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

        Dim tsConfigNotificaciones As ToggleSwitch = pagina.FindName("tsConfigNotificaciones")
        tsConfigNotificaciones.OnContent = recursos.GetString("Yes")
        tsConfigNotificaciones.OffContent = recursos.GetString("No")

        If Not config.Values("Notificaciones") Is Nothing Then
            If config.Values("Notificaciones") = 1 Then
                tsConfigNotificaciones.IsOn = True
            ElseIf config.Values("Notificaciones") = 0 Then
                tsConfigNotificaciones.IsOn = False
            End If
        Else
            config.Values("Notificaciones") = 1
            tsConfigNotificaciones.IsOn = True
        End If

        AddHandler tsConfigNotificaciones.Toggled, AddressOf NotificacionesSwitch
        AddHandler tsConfigNotificaciones.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
        AddHandler tsConfigNotificaciones.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

        Dim tsConfigBusqueda As ToggleSwitch = pagina.FindName("tsConfigBusqueda")
        tsConfigBusqueda.OnContent = recursos.GetString("ConfigSearchMode1")
        tsConfigBusqueda.OffContent = recursos.GetString("ConfigSearchMode2")

        If Not config.Values("Busqueda") Is Nothing Then
            If config.Values("Busqueda") = 1 Then
                tsConfigBusqueda.IsOn = True
            ElseIf config.Values("Busqueda") = 0 Then
                tsConfigBusqueda.IsOn = False
            End If
        Else
            config.Values("Busqueda") = 0
            tsConfigBusqueda.IsOn = False
        End If

        AddHandler tsConfigBusqueda.Toggled, AddressOf BusquedaSwitch
        AddHandler tsConfigBusqueda.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
        AddHandler tsConfigBusqueda.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

        Dim tsConfigBusquedaSteam As ToggleSwitch = pagina.FindName("tsConfigBusquedaSteam")
        tsConfigBusquedaSteam.OnContent = recursos.GetString("Yes")
        tsConfigBusquedaSteam.OffContent = recursos.GetString("No")

        If Not config.Values("BusquedaSteam") Is Nothing Then
            If config.Values("BusquedaSteam") = 1 Then
                tsConfigBusquedaSteam.IsOn = True
            ElseIf config.Values("BusquedaSteam") = 0 Then
                tsConfigBusquedaSteam.IsOn = False
            End If
        Else
            config.Values("BusquedaSteam") = 1
            tsConfigBusquedaSteam.IsOn = True
        End If

        AddHandler tsConfigBusquedaSteam.Toggled, AddressOf BusquedaSteamSwitch
        AddHandler tsConfigBusquedaSteam.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
        AddHandler tsConfigBusquedaSteam.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

        Dim tsConfigSorteos As ToggleSwitch = pagina.FindName("tsConfigSorteos")
        tsConfigSorteos.OnContent = recursos.GetString("Yes")
        tsConfigSorteos.OffContent = recursos.GetString("No")

        If Not config.Values("Sorteos") Is Nothing Then
            If config.Values("Sorteos") = 1 Then
                tsConfigSorteos.IsOn = True
            ElseIf config.Values("Sorteos") = 0 Then
                tsConfigSorteos.IsOn = False
            End If
        Else
            config.Values("Sorteos") = 1
            tsConfigSorteos.IsOn = True
        End If

        AddHandler tsConfigSorteos.Toggled, AddressOf SorteosSwitch
        AddHandler tsConfigSorteos.PointerEntered, AddressOf Interfaz.EfectosHover.Entra_Basico
        AddHandler tsConfigSorteos.PointerExited, AddressOf Interfaz.EfectosHover.Sale_Basico

    End Sub

    Private Sub DeseadosSwitch(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim ts As ToggleSwitch = sender

        If ts.IsOn = True Then
            config.Values("Deseados") = 1
        Else
            config.Values("Deseados") = 0
        End If

    End Sub

    Private Sub NotificacionesSwitch(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim ts As ToggleSwitch = sender

        If ts.IsOn = True Then
            config.Values("Notificaciones") = 1
        Else
            config.Values("Notificaciones") = 0
        End If

    End Sub

    Private Sub BusquedaSwitch(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim ts As ToggleSwitch = sender

        If ts.IsOn = True Then
            config.Values("Busqueda") = 1
        Else
            config.Values("Busqueda") = 0
        End If

    End Sub

    Private Sub BusquedaSteamSwitch(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim ts As ToggleSwitch = sender

        If ts.IsOn = True Then
            config.Values("BusquedaSteam") = 1
        Else
            config.Values("BusquedaSteam") = 0
        End If

    End Sub

    Private Sub SorteosSwitch(sender As Object, e As RoutedEventArgs)

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        Dim ts As ToggleSwitch = sender

        If ts.IsOn = True Then
            config.Values("Sorteos") = 1
        Else
            config.Values("Sorteos") = 0
        End If

    End Sub

    Public Sub Trial(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridConfigTrialMensaje As Grid = pagina.FindName("gridConfigTrialMensaje")

        If estado = True Then
            gridConfigTrialMensaje.Visibility = Visibility.Collapsed
        Else
            gridConfigTrialMensaje.Visibility = Visibility.Visible
        End If

        Dim tsConfigDeseados As ToggleSwitch = pagina.FindName("tsConfigDeseados")
        tsConfigDeseados.IsEnabled = estado

        Dim tsConfigNotificaciones As ToggleSwitch = pagina.FindName("tsConfigNotificaciones")
        tsConfigNotificaciones.IsEnabled = estado

        Dim tsConfigSorteos As ToggleSwitch = pagina.FindName("tsConfigSorteos")
        tsConfigSorteos.IsEnabled = estado

    End Sub

End Module
