Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.UI

Module Buscador

    Dim tienda As String

    Public Async Sub CargarTiles(juego As Tile, tienda_ As String)

        tienda = tienda_

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lv" + tienda + "Juegos")
        lvJuegos.IsEnabled = False

        Dim prTiles As ProgressRing = pagina.FindName("prTiles" + tienda)
        prTiles.Visibility = Visibility.Visible

        Dim mensaje As DropShadowPanel = pagina.FindName("panelAvisoSiJuegos" + tienda)
        mensaje.Visibility = Visibility.Collapsed

        Dim textoTitulo As String = juego.Titulo

        textoTitulo = textoTitulo.Replace(" ", "%20")

        Await WebView.ClearTemporaryWebDataAsync()

        Dim wb As New WebView With {
            .Tag = juego
            }

        AddHandler wb.NavigationCompleted, AddressOf Wb_NavigationCompleted

        wb.Navigate(New Uri("https://www.google.com/search?q=" + textoTitulo + "+grid&biw=1280&bih=886&source=lnms&tbm=isch&sa=X&ved=0ahUKEwjw8KHftrrRAhUN8GMKHdzFBQMQ_AUICCgB&gws_rd=cr,ssl&ei=H1J2WLa_FIPcjwSRvLSQCg"))

    End Sub

    Private Async Sub Wb_NavigationCompleted(sender As WebView, e As WebViewNavigationCompletedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gvTiles As GridView = pagina.FindName("gridViewTiles" + tienda)

        Try
            gvTiles.Items.Clear()
        Catch ex As Exception

        End Try

        Dim lista As New List(Of String) From {
            "document.documentElement.outerHTML;"
        }

        Dim argumentos As IEnumerable(Of String) = lista
        Dim html As String = Nothing

        Try
            html = Await sender.InvokeScriptAsync("eval", argumentos)
        Catch ex As Exception

        End Try

        Dim boolExito As Boolean = False
        Dim listaFinal As New List(Of Tile)

        Dim tope As Integer = 50

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

                        Dim imagen As String = temp4.Trim

                        If Not imagen = Nothing Then
                            Dim boolImagen As Boolean = False

                            If imagen.Contains("steam.cryotank.net") Then
                                boolImagen = True
                            ElseIf imagen.Contains("steamgriddb.com") Then
                                boolImagen = True
                            ElseIf imagen.Contains("steamstatic.com") Then
                                boolImagen = True
                            ElseIf imagen.Contains("deviantart.net") Then
                                boolImagen = True
                            ElseIf imagen.Contains("abload.de") Then
                                boolImagen = True
                            ElseIf imagen.Contains("redd.it") Then
                                boolImagen = True
                            ElseIf Imagen.Contains("bp.blogspot.com") Then
                                boolImagen = True
                            ElseIf imagen.Contains("digitarius.net") Then
                                boolImagen = True
                            End If

                            If boolImagen = True Then
                                Dim codigo As ApplicationDataContainer = ApplicationData.Current.LocalSettings

                                If codigo.Values("codigo" + tienda) Is Nothing Then
                                    codigo.Values("codigo" + tienda) = "0"
                                End If

                                Dim numCodigo As String = Integer.Parse(codigo.Values("codigo" + tienda)) + 1
                                codigo.Values("codigo" + tienda) = numCodigo

                                Dim juego As Tile = sender.Tag
                                Dim juego_ As New Tile(juego.Titulo, tienda + codigo.Values("codigo" + tienda), juego.Enlace, New Uri(imagen), tienda, juego)
                                listaFinal.Add(juego_)
                            End If
                        End If

                        boolExito = True
                    End If
                End If
                i += 1
            End While
        End If

        If boolExito = False Then
            Await WebView.ClearTemporaryWebDataAsync()

            Dim wb As New WebView With {
                .Tag = sender.Tag
            }

            AddHandler wb.NavigationCompleted, AddressOf Wb_NavigationCompleted

            Try
                wb.Navigate(New Uri("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<    a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(    c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((    new Date()).getTime()-1e11).toGMTString());}}}})())"))
            Catch ex As Exception

            End Try

            wb.Navigate(sender.Source)
        Else
            For Each juego In listaFinal
                Dim boton As New Button

                Dim imagen As New ImageEx

                Try
                    imagen.Source = New BitmapImage(juego.Imagen)
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

                AddHandler boton.Click, AddressOf BotonTile_Click

                gvTiles.Items.Add(boton)
            Next

            gvTiles.IsEnabled = True

            Dim lvJuegos As ListView = pagina.FindName("lv" + tienda + "Juegos")
            lvJuegos.IsEnabled = True

            Dim prTiles As ProgressRing = pagina.FindName("prTiles" + tienda)
            prTiles.Visibility = Visibility.Collapsed
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

            Dim popupAviso As Popup = pagina.FindName("popupAvisoSeleccionar")
            popupAviso.IsOpen = True

            Dim grid As Grid = pagina.FindName("gridAñadirTiles")
            grid.Visibility = Visibility.Collapsed
        Else
            For Each item In gv.Items
                Dim itemBoton As Button = item
                itemBoton.BorderThickness = New Thickness(1, 1, 1, 1)
                itemBoton.BorderBrush = New SolidColorBrush(Colors.Black)
            Next

            botonJuego.BorderThickness = New Thickness(6, 6, 6, 6)
            botonJuego.BorderBrush = New SolidColorBrush(Colors.DarkOrange)

            Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
            Dim juego As Tile = botonJuego.Tag
            botonAñadirTile.Tag = juego

            Dim imageJuegoSeleccionado As ImageEx = pagina.FindName("imageJuegoSeleccionado")
            Dim imagenCapsula As String = juego.Imagen.ToString
            imageJuegoSeleccionado.Source = New BitmapImage(New Uri(imagenCapsula))

            Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
            tbJuegoSeleccionado.Text = juego.Titulo

            Dim popupAviso As Popup = pagina.FindName("popupAvisoSeleccionar")
            popupAviso.IsOpen = False

            Dim grid As Grid = pagina.FindName("gridAñadirTiles")
            grid.Visibility = Visibility.Visible
        End If

    End Sub

End Module
