Imports System.Text.RegularExpressions
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Animation

'https://api1.origin.com/supercarp/rating/offers/anonymous?country=ES&locale=es_ES&pid=&currency=EUR&offerIds=Origin.OFR.50.0002325
'https://data1.origin.com/ocd/battlefield/battlefield-1/standard-edition.es-es.esp.ocd
'https://urlscan.io/result/29449b3b-540f-4c5b-bde2-21f5405cfd1a/jsonview/
'https://pastebin.com/HvN2g7Qi
'https://api2.origin.com/ecommerce2/public/supercat/Origin.OFR.50.0002190/en_US?country=US

Module Origin

    Public anchoColumna As Integer = 231

    Public Async Sub Generar(boolBuscarCarpeta As Boolean)

        Dim helper As New LocalObjectStorageHelper

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spProgreso As StackPanel = pagina.FindName("spProgreso")
        spProgreso.Visibility = Visibility.Visible

        Dim pbProgreso As ProgressBar = pagina.FindName("pbProgreso")
        pbProgreso.Value = 0

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")
        tbProgreso.Text = String.Empty

        Dim botonCache As Button = pagina.FindName("botonConfigLimpiarCache")
        botonCache.IsEnabled = False

        Dim gridSeleccionarJuego As Grid = pagina.FindName("gridSeleccionarJuego")
        gridSeleccionarJuego.Visibility = Visibility.Collapsed

        Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
        gv.Items.Clear()

        Dim listaJuegos As New List(Of Tile)

        If Await helper.FileExistsAsync("juegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")
        End If

        Dim botonAñadirCarpetaTexto As TextBlock = pagina.FindName("botonAñadirCarpetaOriginTexto")

        Dim botonCarpetaTexto As TextBlock = pagina.FindName("tbOriginConfigCarpeta")

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCarpetaOrigin")
        botonAñadir.IsEnabled = False

        Dim carpeta As StorageFolder = Nothing

        Try
            If boolBuscarCarpeta = True Then
                Dim carpetapicker As New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginCarpeta")
            End If
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            If carpeta.Path.Contains("Origin\LocalContent") Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginCarpeta", carpeta)
                botonCarpetaTexto.Text = carpeta.Path
                botonAñadirCarpetaTexto.Text = recursos.GetString("Change")

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
                                End If

                                If id.Contains("DR") Then
                                    id = id.Replace("DR", "DR:")
                                End If
                            End If

                            If Not id = Nothing Then
                                Dim añadir As Boolean = True

                                For Each juegoGuardado In listaJuegos
                                    Dim id2 As String = juegoGuardado.ID
                                    id2 = id2.Replace(".", Nothing)
                                    id2 = id2.Replace(":", Nothing)
                                    id2 = id2.Replace("%", Nothing)

                                    If id2 = id Then
                                        añadir = False
                                    End If
                                Next

                                If añadir = True Then
                                    Dim id2 As String = id
                                    id2 = id2.Replace(".", Nothing)
                                    id2 = id2.Replace(":", Nothing)
                                    id2 = id2.Replace("%", Nothing)

                                    Dim html As String = Await Decompiladores.HttpClient(New Uri("https://api2.origin.com/ecommerce2/public/supercat/" + id + "/en_US?country=US"))

                                    If Not html = Nothing Then
                                        Dim juegoOrigin As OriginBBDDJuego = JsonConvert.DeserializeObject(Of OriginBBDDJuego)(html)

                                        Dim titulo As String = juegoOrigin.i18n.Titulo

                                        Dim imagenAlta As String = juegoOrigin.ImagenRaiz + juegoOrigin.i18n.ImagenGrande

                                        Dim imagenAncha As String = Nothing

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

                                                    Try
                                                        imagenAncha = Await Cache.DescargarImagen(temp4.Trim, id2, "ancha")
                                                    Catch ex As Exception

                                                    End Try
                                                End If
                                            End If
                                        End If

                                        If imagenAncha = Nothing Then
                                            imagenAncha = imagenAlta
                                        End If

                                        Dim añadir2 As Boolean = True
                                        Dim i As Integer = 0
                                        While i < listaJuegos.Count
                                            If listaJuegos(i).Titulo = titulo Then
                                                añadir2 = False
                                            End If

                                            If juegoOrigin.Distribucion = "Addon" Then
                                                añadir2 = False
                                            End If

                                            If juegoOrigin.Distribucion2 = "TRIAL" Then
                                                añadir2 = False
                                            End If
                                            i += 1
                                        End While

                                        If añadir2 = True Then
                                            Dim juego As New Tile(titulo, id2, "origin://launchgame/" + id, Nothing, imagenAlta, imagenAncha, imagenAlta)
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

        Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)

        spProgreso.Visibility = Visibility.Collapsed

        Dim gridTiles As Grid = pagina.FindName("gridTiles")
        Dim gridAvisoNoJuegos As Grid = pagina.FindName("gridAvisoNoJuegos")
        Dim spBuscador As StackPanel = pagina.FindName("spBuscador")

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                gridTiles.Visibility = Visibility.Visible
                gridAvisoNoJuegos.Visibility = Visibility.Collapsed
                gridSeleccionarJuego.Visibility = Visibility.Visible
                spBuscador.Visibility = Visibility.Visible

                gv.IsEnabled = False

                listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

                For Each juego In listaJuegos
                    BotonEstilo(juego, gv)
                Next

                'If boolBuscarCarpeta = True Then
                '    Toast(listaJuegos.Count.ToString + " " + recursos.GetString("GamesDetected"), Nothing)
                'End If

                gv.IsEnabled = True
            Else
                gridTiles.Visibility = Visibility.Collapsed
                gridAvisoNoJuegos.Visibility = Visibility.Visible
                gridSeleccionarJuego.Visibility = Visibility.Collapsed
                spBuscador.Visibility = Visibility.Collapsed
            End If
        Else
            gridTiles.Visibility = Visibility.Collapsed
            gridAvisoNoJuegos.Visibility = Visibility.Visible
            gridSeleccionarJuego.Visibility = Visibility.Collapsed
            spBuscador.Visibility = Visibility.Collapsed
        End If

        botonCache.IsEnabled = True
        botonAñadir.IsEnabled = True

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
            .Stretch = Stretch.Uniform,
            .Padding = New Thickness(0, 0, 0, 0),
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center
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
        AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

        gv.Items.Add(panel)

    End Sub

    Private Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spBuscador As StackPanel = pagina.FindName("spBuscador")
        spBuscador.Visibility = Visibility.Collapsed

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenGrande

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridSeleccionarJuego As Grid = pagina.FindName("gridSeleccionarJuego")
        gridSeleccionarJuego.Visibility = Visibility.Collapsed

        Dim gvTiles As GridView = pagina.FindName("gvTiles")

        If gvTiles.ActualWidth > anchoColumna Then
            ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles") = gvTiles.ActualWidth
        End If

        gvTiles.Width = anchoColumna
        gvTiles.Padding = New Thickness(0, 0, 15, 0)

        Dim gridAñadir As Grid = pagina.FindName("gridAñadirTile")
        gridAñadir.Visibility = Visibility.Visible

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("tile", botonJuego)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("tile")

        If Not animacion Is Nothing Then
            animacion.TryStart(gridAñadir)
        End If

        Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + juego.Titulo

        '---------------------------------------------

        Dim imagenPequeña As ImageEx = pagina.FindName("imagenTilePequeña")
        imagenPequeña.Source = Nothing

        Dim imagenMediana As ImageEx = pagina.FindName("imagenTileMediana")
        imagenMediana.Source = Nothing

        Dim imagenAncha As ImageEx = pagina.FindName("imagenTileAncha")
        imagenAncha.Source = Nothing

        Dim imagenGrande As ImageEx = pagina.FindName("imagenTileGrande")
        imagenGrande.Source = Nothing

        If juego.ImagenPequeña = Nothing Then
            juego.ImagenPequeña = "ms-appx:///Assets/Logos/AppList.scale-100.png"
        End If

        imagenPequeña.Source = juego.ImagenPequeña
        imagenPequeña.Tag = juego.ImagenPequeña

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

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvTiles As AdaptiveGridView = pagina.FindName("gvTiles")

        Dim boton As Button = sender

        boton.Saturation(0).Scale(1.05, 1.05, gvTiles.DesiredWidth / 2, gvTiles.ItemHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvTiles As AdaptiveGridView = pagina.FindName("gvTiles")

        Dim boton As Button = sender

        boton.Saturation(1).Scale(1, 1, gvTiles.DesiredWidth / 2, gvTiles.ItemHeight / 2).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

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

    End Class

    Public Class OriginBBDDJuegoi18n

        <JsonProperty("displayName")>
        Public Titulo As String

        <JsonProperty("packArtMedium")>
        Public ImagenPequeña As String

        <JsonProperty("packArtLarge")>
        Public ImagenGrande As String

    End Class

End Module
