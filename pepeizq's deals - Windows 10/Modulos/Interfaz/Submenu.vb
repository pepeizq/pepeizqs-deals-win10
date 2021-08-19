Imports Windows.System

Namespace Interfaz

    Module Submenu

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonOfertas As Button = pagina.FindName("botonOfertas")
            botonOfertas.Tag = recursos.GetString("Deals2")

            AddHandler botonOfertas.Click, AddressOf AbrirSeccionClick
            AddHandler botonOfertas.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonOfertas.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonBundles As Button = pagina.FindName("botonBundles")
            botonBundles.Tag = recursos.GetString("Bundles2")

            AddHandler botonBundles.Click, AddressOf AbrirSeccionClick
            AddHandler botonBundles.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonBundles.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonGratis As Button = pagina.FindName("botonGratis")
            botonGratis.Tag = recursos.GetString("Free2")

            AddHandler botonGratis.Click, AddressOf AbrirSeccionClick
            AddHandler botonGratis.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonGratis.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonSuscripciones As Button = pagina.FindName("botonSuscripciones")
            botonSuscripciones.Tag = recursos.GetString("Subscriptions2")

            AddHandler botonSuscripciones.Click, AddressOf AbrirSeccionClick
            AddHandler botonSuscripciones.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonSuscripciones.PointerExited, AddressOf Sale_Boton_Texto

        End Sub

        Private Sub AbrirSeccionClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Wordpress.CargarEntradas(100, boton.Tag, False, False)

        End Sub

    End Module

End Namespace