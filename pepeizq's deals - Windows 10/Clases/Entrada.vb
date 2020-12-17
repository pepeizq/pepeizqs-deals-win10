Imports Newtonsoft.Json

'https://pepeizqdeals.com/wp-json/wp/v2/posts

Public Class Entrada

    <JsonProperty("title")>
    Public Titulo As EntradaTitulo

    <JsonProperty("content")>
    Public Contenido As EntradaContenido

    <JsonProperty("status")>
    Public Estado As String

    <JsonProperty("id")>
    Public ID As Integer

    <JsonProperty("categories")>
    Public Categorias As List(Of Integer)

    <JsonProperty("tags")>
    Public Etiquetas As List(Of Integer)

    <JsonProperty("fifu_image_url")>
    Public Imagen As String

    <JsonProperty("redirect")>
    Public Redireccion As String

    <JsonProperty("image_v2")>
    Public Imagen2 As String

    <JsonProperty("image_v2_announcements")>
    Public ImagenAnuncio As String

    <JsonProperty("title2")>
    Public SubTitulo As String

    <JsonProperty("date")>
    Public Fecha As String

    <JsonProperty("link")>
    Public Enlace As String

    <JsonProperty("store_name")>
    Public Tienda As String

    <JsonProperty("store_logo")>
    Public TiendaLogo As String

    <JsonProperty("json")>
    Public Json As String

    <JsonProperty("json_expanded")>
    Public JsonExpandido As String

End Class

Public Class EntradaTitulo

    <JsonProperty("rendered")>
    Public Texto As String

End Class

Public Class EntradaContenido

    <JsonProperty("rendered")>
    Public Texto As String

End Class

Public Class EntradaOfertas

    <JsonProperty("message")>
    Public Mensaje As String

    <JsonProperty("games")>
    Public Juegos As List(Of EntradaOfertasJuego)

End Class

Public Class EntradaOfertasJuego

    <JsonProperty("title")>
    Public Titulo As String

    <JsonProperty("image")>
    Public Imagen As String

    <JsonProperty("link")>
    Public Enlace As String

    <JsonProperty("dscnt")>
    Public Descuento As String

    <JsonProperty("price")>
    Public Precio As String

    <JsonProperty("drm")>
    Public DRM As String

    <JsonProperty("revw1")>
    Public AnalisisPorcentaje As String

    <JsonProperty("revw2")>
    Public AnalisisCantidad As String

    <JsonProperty("revw3")>
    Public AnalisisEnlace As String

End Class

Public Class EntradaBundles

    <JsonProperty("moregames")>
    Public MasJuegos As String

    <JsonProperty("price")>
    Public Precio As String

    <JsonProperty("games")>
    Public Juegos As List(Of EntradaBundlesJuego)

End Class

Public Class EntradaBundlesJuego

    <JsonProperty("image")>
    Public Imagen As String

End Class

Public Class EntradaGratis

    <JsonProperty("image")>
    Public Imagen As String

End Class