Module Pais

    Public Function DetectarEuro()

        Dim dentroEuro As Boolean = False
        Dim pais As New Windows.Globalization.GeographicRegion

        For Each moneda In pais.CurrenciesInUse
            If moneda = "EUR" Then
                dentroEuro = True
            End If
        Next

        Return dentroEuro

    End Function

    Public Function DevolverMoneda()

        Dim moneda As String = String.Empty

        Dim pais As New Windows.Globalization.GeographicRegion

        If pais.CurrenciesInUse.Count > 0 Then
            moneda = pais.CurrenciesInUse(0)
        End If

        Return moneda

    End Function

End Module
