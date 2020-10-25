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

End Module
