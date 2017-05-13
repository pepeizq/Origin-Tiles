Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.Core
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

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Acrilico.Generar(gridTopAcrilico)
        Acrilico.Generar(gridMenuAcrilico)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonPressedBackgroundColor = Colors.DarkOrange
        barra.ButtonInactiveBackgroundColor = Colors.Transparent
        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        '----------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()


        botonTilesTexto.Text = recursos.GetString("Tiles")
        botonConfigTexto.Text = recursos.GetString("Boton Config")
        botonVotarTexto.Text = recursos.GetString("Boton Votar")
        botonMasAppsTexto.Text = recursos.GetString("Boton Web")

        tbNoJuegosOrigin.Text = recursos.GetString("No Config")
        tbSiJuegosOrigin.Text = recursos.GetString("Si Config")
        tbAvisoSeleccionar.Text = recursos.GetString("Seleccionar")

        botonAñadirTileTexto.Text = recursos.GetString("Añadir Tile")

        cbTilesTitulo.Content = recursos.GetString("Tile Titulo")
        cbTilesIconos.Content = recursos.GetString("Tile Logo")

        tbOriginConfigInstrucciones.Text = recursos.GetString("Origin Carpeta Añadir")
        buttonAñadirCarpetaOriginTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigCarpeta.Text = recursos.GetString("Origin Carpeta No Config")

        '----------------------------------------------

        GridVisibilidad(gridTilesOrigin, botonTiles, recursos.GetString("Tiles"))
        Origin.CargarJuegos(False)
        Config.Generar()

    End Sub

    Public Sub GridVisibilidad(grid As Grid, boton As Button, seccion As String)

        tbTitulo.Text = "Horigin Tiles (" + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString + ") - " + seccion

        gridTilesOrigin.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonTiles.Background = New SolidColorBrush(Colors.Transparent)
        botonConfig.Background = New SolidColorBrush(Colors.Transparent)

        If Not boton Is Nothing Then
            boton.Background = New SolidColorBrush(Colors.Goldenrod)
        End If

    End Sub

    Private Sub BotonTiles_Click(sender As Object, e As RoutedEventArgs) Handles botonTiles.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        GridVisibilidad(gridTilesOrigin, botonTiles, recursos.GetString("Tiles"))

    End Sub

    Private Sub BotonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        GridVisibilidad(gridConfig, botonConfig, recursos.GetString("Boton Config"))
        GridConfigVisibilidad(gridConfigOrigin, buttonConfigOrigin)

    End Sub

    Private Async Sub BotonVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub BotonMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonMasApps.Click

        If popupMasApps.IsOpen = True Then
            botonMasApps.Background = New SolidColorBrush(Colors.Transparent)
            popupMasApps.IsOpen = False
        Else
            botonMasApps.Background = New SolidColorBrush(Colors.Goldenrod)
            popupMasApps.IsOpen = True
        End If

    End Sub

    Private Async Sub BotonAppSteamTiles_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamTiles.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh51sb3"))

    End Sub

    Private Async Sub BotonAppSteamDeals_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamDeals.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9p7836m1tw15"))

    End Sub

    Private Async Sub BotonAppSteamCategories_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamCategories.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9p54scg1n6bm"))

    End Sub

    Private Async Sub BotonAppSteamBridge_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamBridge.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh441c9"))

    End Sub

    Private Async Sub BotonAppSteamSkins_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamSkins.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh55b7f"))

    End Sub

    'TILES-------------------------------------------------------

    Private Sub BotonAñadirTile_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirTile.Click

        Dim tile As Tile = botonAñadirTile.Tag
        Tiles.Generar(tile)

    End Sub

    Private Sub LvOriginJuegos_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvOriginJuegos.ItemClick

        Dim grid As Grid = e.ClickedItem
        Dim juego As Tile = grid.Tag
        Buscador.CargarTiles(juego, "Origin")

        popupAvisoSeleccionar.IsOpen = True

    End Sub

    Private Sub PopupAvisoSeleccionar_LayoutUpdated(sender As Object, e As Object) Handles popupAvisoSeleccionar.LayoutUpdated

        popupAvisoSeleccionar.Width = panelAvisoSeleccionar.ActualWidth
        popupAvisoSeleccionar.Height = panelAvisoSeleccionar.ActualHeight

    End Sub

    'CONFIG-----------------------------------------------------------------------------

    Private Sub GridConfigVisibilidad(grid As Grid, boton As Button)

        If popupAvisoSeleccionar.IsOpen = True Then
            popupAvisoSeleccionar.IsOpen = False
        End If

        buttonConfigOrigin.Background = New SolidColorBrush(Colors.Transparent)

        boton.Background = New SolidColorBrush(Colors.DarkOrange)

        gridConfigOrigin.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub ButtonConfigOrigin_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigOrigin.Click

        GridConfigVisibilidad(gridConfigOrigin, buttonConfigOrigin)

    End Sub

    'CONFIGTILES-----------------------------------------------------------------------------

    Private Sub CbTilesTitulo_Checked(sender As Object, e As RoutedEventArgs) Handles cbTilesTitulo.Checked

        ApplicationData.Current.LocalSettings.Values("titulotile") = "on"
        Config.Generar()

    End Sub

    Private Sub CbTilesTitulo_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbTilesTitulo.Unchecked

        ApplicationData.Current.LocalSettings.Values("titulotile") = "off"
        Config.Generar()

    End Sub

    Private Sub CbTilesIconos_Checked(sender As Object, e As RoutedEventArgs) Handles cbTilesIconos.Checked

        ApplicationData.Current.LocalSettings.Values("logotile") = "on"
        Config.Generar()

    End Sub

    Private Sub CbTilesIconos_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbTilesIconos.Unchecked

        ApplicationData.Current.LocalSettings.Values("logotile") = "off"
        Config.Generar()

    End Sub

    Private Sub ButtonAñadirCarpetaOrigin_Click(sender As Object, e As RoutedEventArgs) Handles buttonAñadirCarpetaOrigin.Click

        Origin.CargarJuegos(True)

    End Sub

End Class
