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

        botonPrincipal.Label = recursos.GetString("Tiles")
        botonConfig.Label = recursos.GetString("Boton Configuracion")
        botonVotar.Label = recursos.GetString("Boton Votar")
        botonCompartir.Label = recursos.GetString("Boton Compartir")
        botonContacto.Label = recursos.GetString("Boton Contactar")
        botonMasApps.Label = recursos.GetString("Boton Web")

        commadBarTop.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right

        tbMensajeTiles.Text = recursos.GetString("Texto Tiles")

        tbConfig.Text = recursos.GetString("Boton Configuracion")
        buttonConfigApp.Content = recursos.GetString("App")
        tbOriginConfigInstrucciones.Text = recursos.GetString("Texto Origin Config")
        buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigPath.Text = recursos.GetString("Texto Origin No Config")

        checkboxTilesTitulo.Content = recursos.GetString("Titulo Tile")

        tbTilesNumeroAviso.Text = recursos.GetString("Numero Tiles")

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

        If tbTilesNumero.Text = Nothing Then
            If Not ApplicationData.Current.LocalSettings.Values("numerostiles") = Nothing Then
                tbTilesNumero.Text = ApplicationData.Current.LocalSettings.Values("numerostiles")
            Else
                tbTilesNumero.Text = 12
                ApplicationData.Current.LocalSettings.Values("numerostiles") = tbTilesNumero.Text
            End If
        End If

    End Sub

    Public Sub GridVisibilidad(grid As Grid)

        gridTiles.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub botonPrincipal_Click(sender As Object, e As RoutedEventArgs) Handles botonPrincipal.Click

        GridVisibilidad(gridTiles)

    End Sub

    Private Sub botonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        GridVisibilidad(gridConfig)
        GridConfigVisibilidad(gridConfigApp, buttonConfigApp)

    End Sub

    Private Async Sub botonVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub botonCompartir_Click(sender As Object, e As RoutedEventArgs) Handles botonCompartir.Click

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Download: https://www.microsoft.com/store/apps/9pfklrbzj4gr")
        request.Data.Properties.Title = "Origin Tiles"
        request.Data.Properties.Description = "Add Tiles for your Origin games in the Start Menu of Windows 10"

    End Sub

    Private Sub botonContacto_Click(sender As Object, e As RoutedEventArgs) Handles botonContacto.Click

        GridVisibilidad(gridWebContacto)

    End Sub

    Private Sub botonMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonMasApps.Click

        GridVisibilidad(gridWeb)

    End Sub

    '-------------------------------------------------------

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
                            Dim juego As New Tile(titulo, ejecutable, Nothing, Nothing)
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

        tbMensajeTiles.Visibility = Visibility.Collapsed

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

        wb.Navigate(New Uri("https://www.google.com/search?q=" + textoTitulo + "+steam+grid&biw=1280&bih=886&source=lnms&tbm=isch&sa=X&ved=0ahUKEwjw8KHftrrRAhUN8GMKHdzFBQMQ_AUICCgB&gws_rd=cr,ssl&ei=H1J2WLa_FIPcjwSRvLSQCg"))

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
        Dim listaFinal As New List(Of Tile)
        Dim tope As Integer = 12

        Try
            tope = ApplicationData.Current.LocalSettings.Values("numerostiles")
        Catch ex As Exception

        End Try

        If Not html = Nothing Then
            Dim i As Integer = 0
            While i < tope
                If html.Contains("<div class=" + ChrW(34) + "rg_meta" + ChrW(34) + ">") Then
                    Dim temp, temp2, temp3, temp4 As String
                    Dim int, int2, int3, int4 As Integer

                    int = html.IndexOf("<div class=" + ChrW(34) + "rg_meta" + ChrW(34) + ">")
                    temp = html.Remove(0, int + 5)

                    html = temp

                    int2 = temp.IndexOf("</div>")
                    temp2 = temp.Remove(int2, temp.Length - int2)

                    If temp2.Contains(ChrW(34) + "ou" + ChrW(34)) Then
                        int3 = temp2.IndexOf(ChrW(34) + "ou" + ChrW(34))
                        temp3 = temp2.Remove(0, int3 + 6)

                        int4 = temp3.IndexOf(ChrW(34))
                        temp4 = temp3.Remove(int4, temp3.Length - int4)

                        temp4 = temp4.Trim
                        Dim juego As Tile = sender.Tag
                        Dim juego_ As New Tile(juego.Titulo, juego.Ejecutable, temp4, juego)
                        listaFinal.Add(juego_)

                        boolExito = True
                    End If
                End If
                i += 1
            End While
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
            gvTiles.ItemsSource = listaFinal

            lvJuegos.IsEnabled = True
            gvTiles.IsEnabled = True
            prTiles.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Async Sub gvTiles_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvTiles.ItemClick

        Dim tile As Tile = e.ClickedItem

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

        Try
            Await nuevaTile.RequestCreateForSelectionAsync(rect)
        Catch ex As Exception

        End Try

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

    Private Sub tbTilesNumero_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbTilesNumero.TextChanged

        Dim numero As Integer

        Try
            numero = Integer.Parse(tbTilesNumero.Text)
            ApplicationData.Current.LocalSettings.Values("numerostiles") = numero
        Catch ex As Exception

        End Try

    End Sub
End Class
