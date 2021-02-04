Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Origin_Tiles.Configuracion
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.UI
Imports Windows.UI.Xaml.Media.Animation

'https://api1.origin.com/supercarp/rating/offers/anonymous?country=ES&locale=es_ES&pid=&currency=EUR&offerIds=Origin.OFR.50.0002325
'https://data1.origin.com/ocd/battlefield/battlefield-1/standard-edition.es-es.esp.ocd
'https://urlscan.io/result/29449b3b-540f-4c5b-bde2-21f5405cfd1a/jsonview/
'https://pastebin.com/HvN2g7Qi
'https://api2.origin.com/ecommerce2/public/supercat/Origin.OFR.50.0002190/en_US?country=US

Module Origin

    Public anchoColumna As Integer = 180
    Dim clave As String = "OriginCarpeta"
    Dim dominioImagenesSteam As String = "https://cdn.cloudflare.steamstatic.com"

    Public Async Sub Generar(buscarCarpeta As Boolean)

        Dim helper As New LocalObjectStorageHelper

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim pbProgreso As ProgressBar = pagina.FindName("pbProgreso")
        pbProgreso.Value = 0

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")
        tbProgreso.Text = String.Empty

        General.Estado(False)
        Cache.Estado(False)
        LimpiezaArchivos.Estado(False)

        Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
        gv.DesiredWidth = anchoColumna
        gv.Items.Clear()

        Dim listaJuegos As New List(Of Tile)

        If Await helper.FileExistsAsync("juegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")
        End If

        If listaJuegos Is Nothing Then
            listaJuegos = New List(Of Tile)
        End If

        Dim carpeta As StorageFolder = Nothing

        Try
            If buscarCarpeta = True Then
                Dim carpetapicker As New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()

                If Not carpeta Is Nothing Then
                    If carpeta.Path.Contains("Origin\LocalContent") Then
                        StorageApplicationPermissions.FutureAccessList.AddOrReplace(clave, carpeta)
                    End If
                End If
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(clave)
            End If
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            Dim gridProgreso As Grid = pagina.FindName("gridProgreso")
            Interfaz.Pestañas.Visibilidad(gridProgreso, Nothing, Nothing)

            If carpeta.Path.Contains("Origin\LocalContent") Then
                Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

                Dim k As Integer = 0
                For Each carpetaJuego As StorageFolder In carpetasJuegos
                    Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                    For Each fichero As StorageFile In ficheros
                        Dim ficheroext As String = fichero.DisplayName + fichero.FileType

                        If Not ficheroext = "map.crc" Then
                            Dim id As String = Nothing

                            If fichero.FileType = ".ddc" Then
                                Dim ficheroDDC As StorageFile = fichero

                                Dim texto As String = Await FileIO.ReadTextAsync(ficheroDDC)

                                If Not texto = Nothing Then
                                    If texto.Contains(ChrW(34) + "productId" + ChrW(34)) Then
                                        Dim temp, temp2 As String
                                        Dim int, int2 As Integer

                                        int = texto.IndexOf(ChrW(34) + "productId" + ChrW(34))
                                        temp = texto.Remove(0, int + 11)

                                        int = temp.IndexOf(ChrW(34))
                                        temp = temp.Remove(0, int + 1)

                                        int2 = temp.IndexOf(ChrW(34))
                                        temp2 = temp.Remove(int2, temp.Length - int2)

                                        id = temp2.Trim
                                    End If
                                End If
                            ElseIf fichero.FileType = ".mfst" Then
                                Dim ficheroMFST As StorageFile = fichero

                                Dim texto As String = Await FileIO.ReadTextAsync(ficheroMFST)

                                If Not texto = Nothing Then
                                    If texto.Contains("&id=") Then
                                        Dim temp, temp2 As String
                                        Dim int, int2 As Integer

                                        int = texto.IndexOf("&id=")
                                        temp = texto.Remove(0, int + 4)

                                        int2 = temp.IndexOf("&")
                                        temp2 = temp.Remove(int2, temp.Length - int2)

                                        id = temp2
                                    End If
                                End If
                            End If

                            If id = Nothing Then
                                id = fichero.DisplayName

                                If id.Contains("OFB-EAST") Then
                                    id = id.Replace("OFB-EAST", "OFB-EAST:")
                                    id = id.Replace("OFB-EAST%3a", "OFB-EAST:")
                                End If

                                If id.Contains("DR") Then
                                    id = id.Replace("DR", "DR:")
                                    id = id.Replace("DR%3a", "DR:")
                                End If
                            End If

                            If Not id = Nothing Then
                                Dim añadir As Boolean = True

                                For Each juegoGuardado In listaJuegos
                                    Dim id2 As String = juegoGuardado.ID
                                    id2 = id2.Replace(":", Nothing)
                                    id2 = id2.Replace("%3a", Nothing)

                                    If id2 = id Then
                                        añadir = False
                                    End If
                                Next

                                If añadir = True Then
                                    id = id.Replace("%3a", ":")
                                    Dim idEjecutable As String = id

                                    Dim id2 As String = id
                                    id2 = id2.Replace(":", Nothing)

                                    Dim html As String = Await Decompiladores.HttpClient(New Uri("https://api2.origin.com/ecommerce2/public/supercat/" + id + "/en_US?country=US"))

                                    If Not html = Nothing Then
                                        Dim juegoOrigin As OriginBBDDJuego = JsonConvert.DeserializeObject(Of OriginBBDDJuego)(html)

                                        Dim titulo As String = juegoOrigin.i18n.Titulo

                                        Dim imagenPequeña As String = Await Cache.DescargarImagen(Nothing, OriginBBDD.BuscarenListado(id, 1), "pequeña")
                                        Dim imagenMediana As String = Await Cache.DescargarImagen(Nothing, OriginBBDD.BuscarenListado(id, 1), "mediana")
                                        Dim imagenAncha As String = Await Cache.DescargarImagen(Nothing, OriginBBDD.BuscarenListado(id, 1), "ancha")
                                        Dim imagenGrande As String = Await Cache.DescargarImagen(Nothing, OriginBBDD.BuscarenListado(id, 1), "grande")

                                        Dim buscar As Integer = 0

                                        If imagenMediana = Nothing Then
                                            buscar += 1
                                        End If

                                        If imagenAncha = Nothing Then
                                            buscar += 1
                                        End If

                                        If imagenGrande = Nothing Then
                                            buscar += 1
                                        End If

                                        If buscar = 3 Then
                                            If Not OriginBBDD.BuscarenListado(id, 0) = String.Empty Then
                                                Dim idSteam As String = OriginBBDD.BuscarenListado(id, 0)

                                                If imagenMediana = Nothing Then
                                                    Try
                                                        imagenMediana = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + idSteam + "/logo.png", id2, "mediana")
                                                    Catch ex As Exception

                                                    End Try
                                                End If

                                                If imagenAncha = Nothing Then
                                                    Try
                                                        imagenAncha = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + idSteam + "/header.jpg", id2, "ancha")
                                                    Catch ex As Exception

                                                    End Try
                                                End If

                                                If imagenGrande = Nothing Then
                                                    Try
                                                        imagenGrande = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + idSteam + "/library_600x900.jpg", id2, "grande")
                                                    Catch ex As Exception

                                                    End Try
                                                End If
                                            Else
                                                Dim encontradoSteam As Boolean = False

                                                If Not juegoOrigin.Plataformas Is Nothing Then
                                                    If juegoOrigin.Plataformas.Count > 0 Then
                                                        For Each plataforma In juegoOrigin.Plataformas
                                                            If plataforma.Plataforma.ToLower = "steam" Then
                                                                encontradoSteam = True

                                                                If imagenMediana = Nothing Then
                                                                    Try
                                                                        imagenMediana = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + plataforma.PlataformaID + "/logo.png", id2, "logo")
                                                                    Catch ex As Exception

                                                                    End Try
                                                                End If

                                                                If imagenAncha = Nothing Then
                                                                    Try
                                                                        imagenAncha = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + plataforma.PlataformaID + "/header.jpg", id2, "ancha")
                                                                    Catch ex As Exception

                                                                    End Try
                                                                End If

                                                                If imagenGrande = Nothing Then
                                                                    Try
                                                                        imagenGrande = Await Cache.DescargarImagen(dominioImagenesSteam + "/steam/apps/" + plataforma.PlataformaID + "/library_600x900.jpg", id2, "grande")
                                                                    Catch ex As Exception

                                                                    End Try
                                                                End If
                                                            End If
                                                        Next
                                                    End If
                                                End If

                                                If encontradoSteam = False Then
                                                    If imagenGrande = Nothing Then
                                                        imagenGrande = juegoOrigin.ImagenRaiz + juegoOrigin.i18n.ImagenGrande
                                                    End If

                                                    If html.Contains(ChrW(34) + "path" + ChrW(34)) Then
                                                        Dim temp, temp2 As String
                                                        Dim int, int2 As Integer

                                                        int = html.IndexOf(ChrW(34) + "path" + ChrW(34))
                                                        temp = html.Remove(0, int + 1)

                                                        int = temp.IndexOf(":")
                                                        temp = temp.Remove(0, int + 2)

                                                        int2 = temp.IndexOf(ChrW(34))
                                                        temp2 = temp.Remove(int2, temp.Length - int2)

                                                        Dim html2 As String = Await Decompiladores.HttpClient(New Uri("https://data1.origin.com/ocd/" + temp2.Trim + ".en-us.esp.ocd"))

                                                        If Not html2 = Nothing Then
                                                            If html2.Contains(ChrW(34) + "download-background-image" + ChrW(34)) Then
                                                                Dim temp3, temp4 As String
                                                                Dim int3, int4 As Integer

                                                                int3 = html2.IndexOf(ChrW(34) + "download-background-image" + ChrW(34))
                                                                temp3 = html2.Remove(0, int3 + 1)

                                                                int3 = temp3.IndexOf(":")
                                                                temp3 = temp3.Remove(0, int3 + 2)

                                                                int4 = temp3.IndexOf(ChrW(34))
                                                                temp4 = temp3.Remove(int4, temp3.Length - int4)

                                                                If imagenAncha = Nothing Then
                                                                    Try
                                                                        imagenAncha = Await Cache.DescargarImagen(temp4.Trim, id2, "ancha")
                                                                    Catch ex As Exception

                                                                    End Try
                                                                End If
                                                            End If
                                                        End If
                                                    End If

                                                    If imagenAncha = Nothing Then
                                                        imagenAncha = imagenGrande
                                                    End If
                                                End If
                                            End If
                                        End If

                                        Dim añadir2 As Boolean = True
                                        Dim i As Integer = 0
                                        While i < listaJuegos.Count
                                            If listaJuegos(i).Titulo = titulo Then
                                                añadir2 = False
                                            End If
                                            i += 1
                                        End While

                                        If titulo.Contains("Soundtrack") = True Then
                                            añadir2 = False
                                        End If

                                        If titulo.Contains("Upgrade") = True Then
                                            añadir2 = False
                                        End If

                                        If juegoOrigin.Distribucion = "Addon" Then
                                            añadir2 = False
                                        End If

                                        If juegoOrigin.Tipo = "Extra Content" Then
                                            añadir2 = False
                                        End If

                                        If juegoOrigin.Distribucion2 = "TRIAL" Then
                                            añadir2 = False
                                        End If

                                        If añadir2 = True Then
                                            Dim juego As New Tile(titulo, id2, "origin://launchgame/" + idEjecutable, imagenPequeña, imagenMediana, imagenAncha, imagenGrande)
                                            listaJuegos.Add(juego)
                                            Exit For
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next

                    pbProgreso.Value = CInt((100 / carpetasJuegos.Count) * k)
                    tbProgreso.Text = k.ToString + "/" + carpetasJuegos.Count.ToString
                    k += 1
                Next
            End If
        End If

        Try
            Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)
        Catch ex As Exception

        End Try

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim gridJuegos As Grid = pagina.FindName("gridJuegos")
                Interfaz.Pestañas.Visibilidad(gridJuegos, recursos.GetString("Games"), Nothing)

                listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

                For Each juego In listaJuegos
                    BotonEstilo(juego, gv)
                Next
            Else
                Dim gridAvisoNoJuegos As Grid = pagina.FindName("gridAvisoNoJuegos")
                Interfaz.Pestañas.Visibilidad(gridAvisoNoJuegos, Nothing, Nothing)
            End If
        Else
            Dim gridAvisoNoJuegos As Grid = pagina.FindName("gridAvisoNoJuegos")
            Interfaz.Pestañas.Visibilidad(gridAvisoNoJuegos, Nothing, Nothing)
        End If

        General.Estado(True)
        Cache.Estado(True)
        LimpiezaArchivos.Estado(True)

    End Sub

    Public Sub BotonEstilo(juego As Tile, gv As GridView)

        Dim panel As New DropShadowPanel With {
            .Margin = New Thickness(10, 10, 10, 10),
            .ShadowOpacity = 0.9,
            .BlurRadius = 10,
            .MaxWidth = anchoColumna + 20,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim boton As New Button

        Dim imagen As New ImageEx With {
            .Source = juego.ImagenGrande,
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill,
            .Padding = New Thickness(0, 0, 0, 0),
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center,
            .EnableLazyLoading = True
        }

        boton.Tag = juego
        boton.Content = imagen
        boton.Padding = New Thickness(0, 0, 0, 0)
        boton.Background = New SolidColorBrush(Colors.Transparent)

        panel.Content = boton

        Dim tbToolTip As TextBlock = New TextBlock With {
            .Text = juego.Titulo,
            .FontSize = 16,
            .TextWrapping = TextWrapping.Wrap
        }

        ToolTipService.SetToolTip(boton, tbToolTip)
        ToolTipService.SetPlacement(boton, PlacementMode.Mouse)

        AddHandler boton.Click, AddressOf BotonTile_Click
        AddHandler boton.PointerEntered, AddressOf Interfaz.Entra_Boton_Imagen
        AddHandler boton.PointerExited, AddressOf Interfaz.Sale_Boton_Imagen

        gv.Items.Add(panel)

    End Sub

    Private Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Trial.Detectar()
        Interfaz.AñadirTile.ResetearValores()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenAncha

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridAñadirTile As Grid = pagina.FindName("gridAñadirTile")
        Interfaz.Pestañas.Visibilidad(gridAñadirTile, juego.Titulo, Nothing)

        '---------------------------------------------

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionJuego", botonJuego)
        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionJuego")

        If Not animacion Is Nothing Then
            animacion.Configuration = New BasicConnectedAnimationConfiguration
            animacion.TryStart(gridAñadirTile)
        End If

        '---------------------------------------------

        Dim tbImagenTituloTextoTileAncha As TextBox = pagina.FindName("tbImagenTituloTextoTileAncha")
        tbImagenTituloTextoTileAncha.Text = juego.Titulo
        tbImagenTituloTextoTileAncha.Tag = juego.Titulo

        Dim tbImagenTituloTextoTileGrande As TextBox = pagina.FindName("tbImagenTituloTextoTileGrande")
        tbImagenTituloTextoTileGrande.Text = juego.Titulo
        tbImagenTituloTextoTileGrande.Tag = juego.Titulo

        '---------------------------------------------

        Dim imagenPequeña As ImageEx = pagina.FindName("imagenTilePequeña")
        imagenPequeña.Source = Nothing

        Dim imagenMediana As ImageEx = pagina.FindName("imagenTileMediana")
        imagenMediana.Source = Nothing

        Dim imagenAncha As ImageEx = pagina.FindName("imagenTileAncha")
        imagenAncha.Source = Nothing

        Dim imagenGrande As ImageEx = pagina.FindName("imagenTileGrande")
        imagenGrande.Source = Nothing

        If Not juego.ImagenPequeña = Nothing Then
            imagenPequeña.Source = juego.ImagenPequeña
            imagenPequeña.Tag = juego.ImagenPequeña
        End If

        If Not juego.ImagenMediana = Nothing Then
            imagenMediana.Source = juego.ImagenMediana
            imagenMediana.Tag = juego.ImagenMediana
        End If

        If Not juego.ImagenAncha = Nothing Then
            imagenAncha.Source = juego.ImagenAncha
            imagenAncha.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            imagenGrande.Source = juego.ImagenGrande
            imagenGrande.Tag = juego.ImagenGrande
        End If

    End Sub

    Public Class OriginBBDDJuego

        <JsonProperty("offerId")>
        Public ID As String

        <JsonProperty("offerPath")>
        Public Enlace As String

        <JsonProperty("i18n")>
        Public i18n As OriginBBDDJuegoi18n

        <JsonProperty("imageServer")>
        Public ImagenRaiz As String

        <JsonProperty("originDisplayType")>
        Public Distribucion As String

        <JsonProperty("gameTypeFacetKey")>
        Public Distribucion2 As String

        <JsonProperty("offerType")>
        Public Tipo As String

        <JsonProperty("firstParties")>
        Public Plataformas As List(Of OriginPlataformas)

    End Class

    Public Class OriginBBDDJuegoi18n

        <JsonProperty("displayName")>
        Public Titulo As String

        <JsonProperty("packArtMedium")>
        Public ImagenPequeña As String

        <JsonProperty("packArtLarge")>
        Public ImagenGrande As String

    End Class

    Public Class OriginPlataformas

        <JsonProperty("partner")>
        Public Plataforma As String

        <JsonProperty("partnerId")>
        Public PlataformaID As String

    End Class

End Module
