Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Networking.BackgroundTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.StartScreen
Imports Windows.Web.Http

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar

        barra.BackgroundColor = Colors.DarkOrange
        barra.ForegroundColor = Colors.White
        barra.InactiveForegroundColor = Colors.White
        barra.ButtonBackgroundColor = Colors.DarkOrange
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveForegroundColor = Colors.White

        '----------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        menuItemConfig.Label = recursos.GetString("Boton Configuracion")
        menuItemVote.Label = recursos.GetString("Boton Votar")
        menuItemShare.Label = recursos.GetString("Boton Compartir")
        menuItemContact.Label = recursos.GetString("Boton Contactar")
        menuItemWeb.Label = recursos.GetString("Boton Web")

        tbMensajeTiles.Text = recursos.GetString("Texto Tiles")

        tbConfig.Text = recursos.GetString("Boton Configuracion")
        buttonConfigApp.Content = recursos.GetString("App")
        tbOriginConfigInstrucciones.Text = recursos.GetString("Texto Origin Config")
        buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigPath.Text = recursos.GetString("Texto Origin No Config")

        checkboxTilesTitulo.Content = recursos.GetString("Titulo Tile")

        '----------------------------------------------

        Dim boolOrigin As Boolean = Await Detector.Origin(tbOriginConfigPath, buttonOriginConfigPathTexto, False)

        If boolOrigin = True Then
            GridVisibilidad(gridTiles)
            GenerarJuegos()
        Else
            GridVisibilidad(gridConfig)
            GridConfigVisibilidad(gridConfigApp, buttonConfigApp)
        End If

        If ApplicationData.Current.LocalSettings.Values("titulotile") = "on" Then
            checkboxTilesTitulo.IsChecked = True
            imageTileTitulo.Source = New BitmapImage(New Uri(Me.BaseUri, "/Assets/Otros/titulo1.png"))
        Else
            imageTileTitulo.Source = New BitmapImage(New Uri(Me.BaseUri, "/Assets/Otros/titulo0.png"))
        End If

        '----------------------------------------------

        Dim coleccion As HamburgerMenuItemCollection = hamburgerMaestro.ItemsSource
        hamburgerMaestro.ItemsSource = Nothing
        hamburgerMaestro.ItemsSource = coleccion

        Dim coleccionOpciones As HamburgerMenuItemCollection = hamburgerMaestro.OptionsItemsSource
        hamburgerMaestro.OptionsItemsSource = Nothing
        hamburgerMaestro.OptionsItemsSource = coleccionOpciones

    End Sub

    Public Sub GridVisibilidad(grid As Grid)

        gridTiles.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub hamburgerMaestro_ItemClick(sender As Object, e As ItemClickEventArgs) Handles hamburgerMaestro.ItemClick

        Dim menuItem As HamburgerMenuGlyphItem = TryCast(e.ClickedItem, HamburgerMenuGlyphItem)

        If menuItem.Tag = 1 Then
            GridVisibilidad(gridTiles)
        End If

    End Sub

    Private Async Sub hamburgerMaestro_OptionsItemClick(sender As Object, e As ItemClickEventArgs) Handles hamburgerMaestro.OptionsItemClick

        Dim menuItem As HamburgerMenuGlyphItem = TryCast(e.ClickedItem, HamburgerMenuGlyphItem)

        If menuItem.Tag = 99 Then
            GridVisibilidad(gridConfig)
            GridConfigVisibilidad(gridConfigApp, buttonConfigApp)
        ElseIf menuItem.Tag = 100 Then
            Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))
        ElseIf menuItem.Tag = 101 Then
            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
            DataTransferManager.ShowShareUI()
        ElseIf menuItem.Tag = 102 Then
            GridVisibilidad(gridWebContacto)
        ElseIf menuItem.Tag = 103 Then
            GridVisibilidad(gridWeb)
        End If

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Origin Tiles")
        request.Data.Properties.Title = "Origin Tiles"
        request.Data.Properties.Description = "Add Tiles for your Origin games in the Start Menu of Windows 10"

    End Sub

    Private Async Sub buttonOriginConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonOriginConfigPath.Click

        Dim boolOrigin As Boolean = Await Detector.Origin(tbOriginConfigPath, buttonOriginConfigPathTexto, True)

        If boolOrigin = True Then
            GenerarJuegos()
        End If

    End Sub

    Private Async Sub GenerarJuegos()

        Dim listaJuegos As New List(Of Tile)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim ficheroext As String = fichero.DisplayName + fichero.FileType

                    If Not ficheroext = "map.crc" Then
                        Dim titulo As String = carpetaJuego.Name

                        titulo = titulo.Replace("Mirrors Edge", "Mirror's Edge")

                        Dim ejecutable As String = "origin://launchgame/" + fichero.DisplayName

                        If ejecutable.Contains("OFB-EAST") Then
                            ejecutable = ejecutable.Replace("OFB-EAST", "OFB-EAST:")
                        End If

                        If ejecutable.Contains("DR") Then
                            ejecutable = ejecutable.Replace("DR", "DR:")
                        End If

                        Dim tituloBool As Boolean = False
                        Dim i As Integer = 0
                        While i < listaJuegos.Count
                            If listaJuegos(i).Titulo = titulo Then
                                tituloBool = True
                            End If
                            i += 1
                        End While

                        If tituloBool = False Then
                            Dim juego As New Tile(titulo, ejecutable, Nothing)
                            listaJuegos.Add(juego)

                            Dim texto As New TextBlock
                            texto.Text = juego.Titulo
                            texto.Tag = juego

                            lvJuegos.Items.Add(texto)

                        End If
                    End If
                Next
            Next
        End If

    End Sub

    Private Async Sub lvJuegos_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvJuegos.ItemClick

        lvJuegos.IsEnabled = False
        gvTiles.IsEnabled = False
        prTiles.Visibility = Visibility.Visible

        Dim bloqueTitulo As TextBlock = e.ClickedItem
        Dim textoTitulo As String = bloqueTitulo.Text

        textoTitulo = textoTitulo.Replace(" ", "%20")

        Await WebView.ClearTemporaryWebDataAsync()

        Dim wb As New WebView
        wb.Tag = bloqueTitulo.Tag
        AddHandler wb.NavigationCompleted, AddressOf wb_NavigationCompleted

        wb.Navigate(New Uri("http://www.steamgriddb.com/game/" + textoTitulo + "/"))

    End Sub

    Private Async Sub wb_NavigationCompleted(sender As WebView, e As WebViewNavigationCompletedEventArgs)

        gvTiles.Items.Clear()

        Dim lista As New List(Of String)
        lista.Add("document.documentElement.outerHTML;")
        Dim argumentos As IEnumerable(Of String) = lista
        Dim html As String = Nothing

        Try
            html = Await sender.InvokeScriptAsync("eval", argumentos)
        Catch ex As Exception

        End Try

        Dim boolExito As Boolean = False

        If Not html = Nothing Then
            Dim i As Integer = 0
            While i < 100
                If html.Contains("<div class=" + ChrW(34) + "grid-row-inner" + ChrW(34)) Then
                    Dim temp, temp2, temp3 As String
                    Dim int, int2, int3 As Integer

                    int = html.IndexOf("<div class=" + ChrW(34) + "grid-row-inner" + ChrW(34))
                    temp = html.Remove(0, int + 5)

                    html = temp

                    int2 = temp.IndexOf("<a href=")
                    temp2 = temp.Remove(0, int2 + 9)

                    int3 = temp2.IndexOf(ChrW(34))
                    temp3 = temp2.Remove(int3, temp2.Length - int3)

                    '---------------------------------------------------

                    Dim imagenUri As Uri = New Uri(temp3.Trim, UriKind.RelativeOrAbsolute)
                    Dim client As New HttpClient
                    Dim response As Streams.IBuffer = Await client.GetBufferAsync(imagenUri)
                    Dim stream As Stream = response.AsStream
                    Dim mem As MemoryStream = New MemoryStream()
                    Await stream.CopyToAsync(mem)
                    mem.Position = 0
                    Dim bitmap As New BitmapImage
                    bitmap.SetSource(mem.AsRandomAccessStream)

                    '---------------------------------------------------

                    Dim imagen As New ImageEx
                    imagen.Source = bitmap
                    imagen.Stretch = Stretch.UniformToFill

                    Dim grid As New Grid
                    grid.Height = 150
                    grid.Width = 310
                    grid.Margin = New Thickness(5, 5, 5, 5)

                    Dim juego As Tile = sender.Tag
                    Dim juego_ As New Tile(juego.Titulo, juego.Ejecutable, temp3.Trim)

                    grid.Tag = juego_

                    grid.Children.Add(imagen)

                    gvTiles.Items.Add(grid)

                    boolExito = True
                End If
                i += 1
            End While

            If html.Contains("<h3>No grids found!</h3>") Then
                boolExito = True
                MessageBox("No grids found")
            End If
        End If

        If boolExito = False Then
            Await WebView.ClearTemporaryWebDataAsync()

            Dim wb As New WebView
            wb.Tag = sender.Tag

            AddHandler wb.NavigationCompleted, AddressOf wb_NavigationCompleted

            Try
                wb.Navigate(New Uri("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<    a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(    c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((    new Date()).getTime()-1e11).toGMTString());}}}})())"))
            Catch ex As Exception

            End Try

            wb.Navigate(sender.Source)
        Else
            lvJuegos.IsEnabled = True
            gvTiles.IsEnabled = True
            prTiles.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Async Sub gvTiles_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvTiles.ItemClick

        Dim grid As Grid = e.ClickedItem
        Dim tile As Tile = grid.Tag

        Dim ficheroImagen As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync("headerorigin.png", CreationCollisionOption.GenerateUniqueName)
        Dim downloader As BackgroundDownloader = New BackgroundDownloader()
        Dim descarga As DownloadOperation = downloader.CreateDownload(New Uri(tile.Imagen), ficheroImagen)
        Await descarga.StartAsync

        Dim codigo As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If codigo.Values("codigo") Is Nothing Then
            codigo.Values("codigo") = "0"
        End If

        Dim numCodigo As String = Integer.Parse(codigo.Values("codigo")) + 1
        codigo.Values("codigo") = numCodigo

        Dim nuevaTile As SecondaryTile = New SecondaryTile(numCodigo, tile.Titulo, tile.Ejecutable, New Uri("ms-appdata:///local/" + ficheroImagen.Name, UriKind.RelativeOrAbsolute), TileSize.Wide310x150)

        Dim frame As FrameworkElement = TryCast(sender, FrameworkElement)
        Dim button As GeneralTransform = frame.TransformToVisual(Nothing)
        Dim point As Point = button.TransformPoint(New Point())
        Dim rect As Rect = New Rect(point, New Size(frame.ActualWidth, frame.ActualHeight))

        nuevaTile.RoamingEnabled = False
        nuevaTile.VisualElements.Wide310x150Logo = New Uri("ms-appdata:///local/" + ficheroImagen.Name, UriKind.RelativeOrAbsolute)

        If ApplicationData.Current.LocalSettings.Values("titulotile") = "on" Then
            nuevaTile.VisualElements.ShowNameOnWide310x150Logo = True
        End If

        Await nuevaTile.RequestCreateForSelectionAsync(rect)

    End Sub

    Private Sub checkboxTilesTitulo_Checked(sender As Object, e As RoutedEventArgs) Handles checkboxTilesTitulo.Checked

        ApplicationData.Current.LocalSettings.Values("titulotile") = "on"
        imageTileTitulo.Source = New BitmapImage(New Uri(Me.BaseUri, "/Assets/Otros/titulo1.png"))

    End Sub

    Private Sub checkboxTilesTitulo_Unchecked(sender As Object, e As RoutedEventArgs) Handles checkboxTilesTitulo.Unchecked

        ApplicationData.Current.LocalSettings.Values("titulotile") = "off"
        imageTileTitulo.Source = New BitmapImage(New Uri(Me.BaseUri, "/Assets/Otros/titulo0.png"))

    End Sub

    '-----------------------------------------------------------------------------

    Private Sub GridConfigVisibilidad(grid As Grid, button As Button)

        buttonConfigApp.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigApp.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigTiles.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigTiles.BorderBrush = New SolidColorBrush(Colors.Transparent)

        button.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#bfbfbf"))
        button.BorderBrush = New SolidColorBrush(Colors.Black)

        gridConfigApp.Visibility = Visibility.Collapsed
        gridConfigTiles.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub


    Private Sub buttonConfigApp_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigApp.Click

        GridConfigVisibilidad(gridConfigApp, buttonConfigApp)

    End Sub

    Private Sub buttonConfigTiles_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigTiles.Click

        GridConfigVisibilidad(gridConfigTiles, buttonConfigTiles)

    End Sub
End Class
