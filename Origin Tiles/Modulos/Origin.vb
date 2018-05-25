Imports System.Text.RegularExpressions
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
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

    Public Async Sub Generar(boolBuscarCarpeta As Boolean)

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonAñadirCarpetaTexto As TextBlock = pagina.FindName("botonAñadirCarpetaOriginTexto")

        Dim botonCarpetaTexto As TextBlock = pagina.FindName("tbOriginConfigCarpeta")

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCarpetaOrigin")
        botonAñadir.IsEnabled = False

        Dim pr As ProgressRing = pagina.FindName("prTilesOrigin")
        pr.Visibility = Visibility.Visible

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

        Dim listaJuegos As New List(Of Tile)

        If Not carpeta Is Nothing Then
            If carpeta.Path.Contains("Origin\LocalContent") Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginCarpeta", carpeta)
                botonCarpetaTexto.Text = carpeta.Path
                botonAñadirCarpetaTexto.Text = recursos.GetString("Change")

                Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

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
                                Dim html As String = Await Decompiladores.HttpClient(New Uri("https://api2.origin.com/ecommerce2/public/supercat/" + id + "/en_US?country=US"))

                                If Not html = Nothing Then
                                    Dim titulo As String = Nothing

                                    If html.Contains(ChrW(34) + "displayName" + ChrW(34)) Then
                                        Dim temp, temp2 As String
                                        Dim int, int2 As Integer

                                        int = html.IndexOf(ChrW(34) + "displayName" + ChrW(34))
                                        temp = html.Remove(0, int + 1)

                                        int = temp.IndexOf(":")
                                        temp = temp.Remove(0, int + 2)

                                        int2 = temp.IndexOf(ChrW(34))
                                        temp2 = temp.Remove(int2, temp.Length - int2)

                                        temp2 = Regex.Replace(temp2, "[^\u0000-\u007F]", String.Empty)

                                        titulo = temp2.Trim
                                    End If

                                    Dim imagenAlta As String = Nothing

                                    If html.Contains(ChrW(34) + "imageServer" + ChrW(34)) Then
                                        Dim temp, temp2 As String
                                        Dim int, int2 As Integer

                                        int = html.IndexOf(ChrW(34) + "imageServer" + ChrW(34))
                                        temp = html.Remove(0, int + 1)

                                        int = temp.IndexOf(":")
                                        temp = temp.Remove(0, int + 2)

                                        int2 = temp.IndexOf(ChrW(34))
                                        temp2 = temp.Remove(int2, temp.Length - int2)

                                        If html.Contains(ChrW(34) + "packArtLarge" + ChrW(34)) Then
                                            Dim temp3, temp4 As String
                                            Dim int3, int4 As Integer

                                            int3 = html.IndexOf(ChrW(34) + "packArtLarge" + ChrW(34))
                                            temp3 = html.Remove(0, int3 + 1)

                                            int3 = temp3.IndexOf(":")
                                            temp3 = temp3.Remove(0, int3 + 2)

                                            int4 = temp3.IndexOf(ChrW(34))
                                            temp4 = temp3.Remove(int4, temp3.Length - int4)

                                            imagenAlta = temp2.Trim + temp4.Trim
                                        End If
                                    End If

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

                                                imagenAncha = temp4.Trim
                                            End If
                                        End If
                                    End If

                                    If imagenAncha = Nothing Then
                                        imagenAncha = imagenAlta
                                    End If

                                    Dim tituloBool As Boolean = False
                                    Dim i As Integer = 0
                                    While i < listaJuegos.Count
                                        If listaJuegos(i).Titulo = titulo Then
                                            tituloBool = True
                                        End If

                                        If html.Contains(ChrW(34) + "originDisplayType" + ChrW(34) + ":" + ChrW(34) + "Addon") Then
                                            tituloBool = True
                                        End If
                                        i += 1
                                    End While

                                    If tituloBool = False Then
                                        Dim id2 As String = id
                                        id2 = id2.Replace(".", Nothing)
                                        id2 = id2.Replace(":", Nothing)
                                        id2 = id2.Replace("%", Nothing)

                                        Dim juego As New Tile(titulo, id2, "origin://launchgame/" + id, Nothing, New Uri(imagenAlta), New Uri(imagenAncha), New Uri(imagenAlta))
                                        listaJuegos.Add(juego)
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        End If

        Dim panelAvisoNoJuegos As Grid = pagina.FindName("panelAvisoNoJuegos")
        Dim gridSeleccionar As Grid = pagina.FindName("gridSeleccionarJuego")

        If listaJuegos.Count > 0 Then
            panelAvisoNoJuegos.Visibility = Visibility.Collapsed
            gridSeleccionar.Visibility = Visibility.Visible

            Dim gv As GridView = pagina.FindName("gridViewTilesOrigin")
            gv.IsEnabled = False

            Try
                gv.Items.Clear()
            Catch ex As Exception

            End Try

            listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

            For Each juego In listaJuegos
                Dim boton As New Button

                Dim imagen As New ImageEx

                Try
                    imagen.Source = New BitmapImage(juego.ImagenGrande)
                Catch ex As Exception

                End Try

                imagen.IsCacheEnabled = True
                imagen.Stretch = Stretch.UniformToFill
                imagen.Padding = New Thickness(0, 0, 0, 0)

                boton.Tag = juego
                boton.Content = imagen
                boton.Padding = New Thickness(0, 0, 0, 0)
                boton.BorderThickness = New Thickness(1, 1, 1, 1)
                boton.BorderBrush = New SolidColorBrush(Colors.Black)
                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim tbToolTip As TextBlock = New TextBlock With {
                    .Text = juego.Titulo,
                    .FontSize = 16
                }

                ToolTipService.SetToolTip(boton, tbToolTip)
                ToolTipService.SetPlacement(boton, PlacementMode.Mouse)

                AddHandler boton.Click, AddressOf BotonTile_Click
                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                gv.Items.Add(boton)
            Next

            If boolBuscarCarpeta = True Then
                Toast(listaJuegos.Count.ToString + " " + recursos.GetString("GamesDetected"), Nothing)
            End If

            gv.IsEnabled = True
        Else
            panelAvisoNoJuegos.Visibility = Visibility.Visible
            gridSeleccionar.Visibility = Visibility.Collapsed
        End If

        botonAñadir.IsEnabled = True
        pr.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = New BitmapImage(juego.ImagenGrande)

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

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

        Dim titulo1 As TextBlock = pagina.FindName("tituloTileAnchaEnseñar")
        Dim titulo2 As TextBlock = pagina.FindName("tituloTileAnchaPersonalizar")

        Dim titulo3 As TextBlock = pagina.FindName("tituloTileGrandeEnseñar")
        Dim titulo4 As TextBlock = pagina.FindName("tituloTileGrandePersonalizar")

        titulo1.Text = juego.Titulo
        titulo2.Text = juego.Titulo

        titulo3.Text = juego.Titulo
        titulo4.Text = juego.Titulo

        If juego.ImagenPequeña = Nothing Then
            juego.ImagenPequeña = New Uri("ms-appx:///Assets/Logos/AppList.scale-100.png")
        End If

        If Not juego.ImagenPequeña = Nothing Then
            Dim imagenPequeña1 As ImageEx = pagina.FindName("imagenTilePequeñaEnseñar")
            Dim imagenPequeña2 As ImageEx = pagina.FindName("imagenTilePequeñaGenerar")
            Dim imagenPequeña3 As ImageEx = pagina.FindName("imagenTilePequeñaPersonalizar")

            imagenPequeña1.Source = juego.ImagenPequeña
            imagenPequeña2.Source = juego.ImagenPequeña
            imagenPequeña3.Source = juego.ImagenPequeña

            imagenPequeña1.Tag = juego.ImagenPequeña
            imagenPequeña2.Tag = juego.ImagenPequeña
            imagenPequeña3.Tag = juego.ImagenPequeña
        End If

        If Not juego.ImagenMediana = Nothing Then
            Dim imagenMediana1 As ImageEx = pagina.FindName("imagenTileMedianaEnseñar")
            Dim imagenMediana2 As ImageEx = pagina.FindName("imagenTileMedianaGenerar")
            Dim imagenMediana3 As ImageEx = pagina.FindName("imagenTileMedianaPersonalizar")

            imagenMediana1.Source = juego.ImagenMediana
            imagenMediana2.Source = juego.ImagenMediana
            imagenMediana3.Source = juego.ImagenMediana

            imagenMediana1.Tag = juego.ImagenMediana
            imagenMediana2.Tag = juego.ImagenMediana
            imagenMediana3.Tag = juego.ImagenMediana
        End If

        If Not juego.ImagenAncha = Nothing Then
            Dim imagenAncha1 As ImageEx = pagina.FindName("imagenTileAnchaEnseñar")
            Dim imagenAncha2 As ImageEx = pagina.FindName("imagenTileAnchaGenerar")
            Dim imagenAncha3 As ImageEx = pagina.FindName("imagenTileAnchaPersonalizar")

            imagenAncha1.Source = juego.ImagenAncha
            imagenAncha2.Source = juego.ImagenAncha
            imagenAncha3.Source = juego.ImagenAncha

            imagenAncha1.Tag = juego.ImagenAncha
            imagenAncha2.Tag = juego.ImagenAncha
            imagenAncha3.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            Dim imagenGrande1 As ImageEx = pagina.FindName("imagenTileGrandeEnseñar")
            Dim imagenGrande2 As ImageEx = pagina.FindName("imagenTileGrandeGenerar")
            Dim imagenGrande3 As ImageEx = pagina.FindName("imagenTileGrandePersonalizar")

            imagenGrande1.Source = juego.ImagenGrande
            imagenGrande2.Source = juego.ImagenGrande
            imagenGrande3.Source = juego.ImagenGrande

            imagenGrande1.Tag = juego.ImagenGrande
            imagenGrande2.Tag = juego.ImagenGrande
            imagenGrande3.Tag = juego.ImagenGrande
        End If

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content

        imagen.Saturation(0).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content

        imagen.Saturation(1).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
