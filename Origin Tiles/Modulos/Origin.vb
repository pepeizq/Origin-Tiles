Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.UI

Module Origin

    Public Async Sub Generar(boolBuscarCarpeta As Boolean)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

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
                Dim carpetapicker As FolderPicker = New FolderPicker()

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
                            Dim titulo As String = carpetaJuego.Name

                            titulo = titulo.Replace("Mirrors Edge", "Mirror's Edge")

                            Dim ejecutable As String = Nothing

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

                                        ejecutable = "origin://launchgame/" + temp2
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

                                        ejecutable = "origin://launchgame/" + temp2
                                    End If
                                End If
                            End If

                            If ejecutable = Nothing Then
                                ejecutable = "origin://launchgame/" + fichero.DisplayName

                                If ejecutable.Contains("OFB-EAST") Then
                                    ejecutable = ejecutable.Replace("OFB-EAST", "OFB-EAST:")
                                End If

                                If ejecutable.Contains("DR") Then
                                    ejecutable = ejecutable.Replace("DR", "DR:")
                                End If
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
                                Dim juego As New Tile(titulo, Nothing, New Uri(ejecutable), Nothing, "Origin", Nothing)
                                listaJuegos.Add(juego)
                            End If
                        End If
                    Next
                Next
            End If
        End If

        Dim panelAvisoNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegos")
        Dim gridSeleccionar As Grid = pagina.FindName("gridSeleccionarJuego")

        If listaJuegos.Count > 0 Then
            panelAvisoNoJuegos.Visibility = Visibility.Collapsed
            gridSeleccionar.Visibility = Visibility.Visible

            Dim gvTiles As GridView = pagina.FindName("gridViewTilesOrigin")

            Try
                gvTiles.Items.Clear()
            Catch ex As Exception

            End Try

            listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

            For Each juego In listaJuegos
                CargarTile(juego)
            Next

            If boolBuscarCarpeta = True Then
                Toast(listaJuegos.Count.ToString + " " + recursos.GetString("GamesDetected"), Nothing)
            End If
        Else
            panelAvisoNoJuegos.Visibility = Visibility.Visible
            gridSeleccionar.Visibility = Visibility.Collapsed
        End If

        botonAñadir.IsEnabled = True
        pr.Visibility = Visibility.Collapsed

    End Sub

    Public Sub CargarTile(juego As Tile)

        Dim textoTitulo As String = juego.Titulo

        textoTitulo = textoTitulo.Replace(" ", "%20")

        'Try
        '    Await WebView.ClearTemporaryWebDataAsync()
        'Catch ex As Exception

        'End Try

        Dim wb As New WebView With {
            .Tag = juego
            }

        AddHandler wb.NavigationCompleted, AddressOf Wb_NavigationCompleted

        wb.Navigate(New Uri("https://www.google.com/search?q=" + textoTitulo + "%20originassets.akamaized.net&biw=1280&bih=886&source=lnms&tbm=isch&sa=X&ved=0ahUKEwjw8KHftrrRAhUN8GMKHdzFBQMQ_AUICCgB&gws_rd=cr,ssl&ei=H1J2WLa_FIPcjwSRvLSQCg"))

    End Sub

    Private Async Sub Wb_NavigationCompleted(sender As WebView, e As WebViewNavigationCompletedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvTiles As GridView = pagina.FindName("gridViewTilesOrigin")


        Dim lista As New List(Of String) From {
            "document.documentElement.outerHTML;"
        }

        Dim argumentos As IEnumerable(Of String) = lista
        Dim html As String = Nothing

        Try
            html = Await sender.InvokeScriptAsync("eval", argumentos)
        Catch ex As Exception

        End Try

        Dim tope As Integer = 20

        If Not html = Nothing Then
            Dim i As Integer = 0
            While i < tope
                If html.Contains("<div class=" + ChrW(34) + "rg_meta") Then
                    Dim temp, temp2, temp3, temp4 As String
                    Dim int, int2, int3, int4 As Integer

                    int = html.IndexOf("<div class=" + ChrW(34) + "rg_meta")
                    temp = html.Remove(0, int + 5)

                    html = temp

                    int2 = temp.IndexOf("</div>")
                    temp2 = temp.Remove(int2, temp.Length - int2)

                    If temp2.Contains(ChrW(34) + "ou" + ChrW(34)) Then
                        int3 = temp2.IndexOf(ChrW(34) + "ou" + ChrW(34))
                        temp3 = temp2.Remove(0, int3 + 6)

                        int4 = temp3.IndexOf(ChrW(34))
                        temp4 = temp3.Remove(int4, temp3.Length - int4)

                        Dim imagenUrl As String = temp4.Trim

                        If Not imagenUrl = Nothing Then
                            Dim boolImagen As Boolean = False

                            If imagenUrl.Contains("originassets.akamaized.net") Then
                                boolImagen = True
                            End If

                            If boolImagen = True Then
                                Dim codigo As ApplicationDataContainer = ApplicationData.Current.LocalSettings

                                If codigo.Values("codigoOrigin") Is Nothing Then
                                    codigo.Values("codigoOrigin") = "0"
                                End If

                                Dim numCodigo As String = Integer.Parse(codigo.Values("codigoOrigin")) + 1
                                codigo.Values("codigoOrigin") = numCodigo

                                Dim juego As Tile = sender.Tag

                                juego.ID = numCodigo
                                juego.Imagen = New Uri(imagenUrl)

                                Dim gv As GridView = pagina.FindName("gridViewTilesOrigin")

                                Dim boton As New Button

                                Dim imagen As New ImageEx

                                Try
                                    imagen.Source = New BitmapImage(juego.Imagen)
                                Catch ex As Exception

                                End Try

                                imagen.IsCacheEnabled = True
                                imagen.Stretch = Stretch.Uniform
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

                                gv.Items.Add(boton)
                                Exit While
                            End If
                        End If
                    End If
                End If
                i += 1
            End While
        End If

    End Sub

    Private Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gv As GridView = pagina.FindName("gridViewTilesOrigin")

        Dim botonJuego As Button = e.OriginalSource

        Dim borde As Thickness = New Thickness(6, 6, 6, 6)
        If botonJuego.BorderThickness = borde Then
            botonJuego.BorderThickness = New Thickness(1, 1, 1, 1)
            botonJuego.BorderBrush = New SolidColorBrush(Colors.Black)

            Dim grid As Grid = pagina.FindName("gridAñadirTiles")
            grid.Visibility = Visibility.Collapsed

            Dim gridSeleccionar As Grid = pagina.FindName("gridSeleccionarJuego")
            gridSeleccionar.Visibility = Visibility.Visible
        Else
            For Each item In gv.Items
                Dim itemBoton As Button = item
                itemBoton.BorderThickness = New Thickness(1, 1, 1, 1)
                itemBoton.BorderBrush = New SolidColorBrush(Colors.Black)
            Next

            botonJuego.BorderThickness = New Thickness(6, 6, 6, 6)
            botonJuego.BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

            Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
            Dim juego As Tile = botonJuego.Tag
            botonAñadirTile.Tag = juego

            Dim imageJuegoSeleccionado As ImageEx = pagina.FindName("imageJuegoSeleccionado")
            Dim imagenCapsula As String = juego.Imagen.ToString
            imageJuegoSeleccionado.Source = New BitmapImage(New Uri(imagenCapsula))

            Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
            tbJuegoSeleccionado.Text = juego.Titulo

            Dim grid As Grid = pagina.FindName("gridAñadirTiles")
            grid.Visibility = Visibility.Visible

            Dim gridSeleccionar As Grid = pagina.FindName("gridSeleccionarJuego")
            gridSeleccionar.Visibility = Visibility.Collapsed
        End If

    End Sub

End Module
