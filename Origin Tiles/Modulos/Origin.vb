﻿Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.UI

Module Origin

    Public Async Sub CargarJuegos(boolBuscarCarpeta As Boolean)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonAñadirCarpetaTexto As TextBlock = pagina.FindName("buttonAñadirCarpetaOriginTexto")

        Dim botonCarpetaTexto As TextBlock = pagina.FindName("tbOriginConfigCarpeta")

        Dim lvJuegos As ListView = pagina.FindName("lvOriginJuegos")

        lvJuegos.Items.Clear()

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
                botonAñadirCarpetaTexto.Text = recursos.GetString("Boton Cambiar")

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
                                Dim juego As New Tile(titulo, Nothing, New Uri(ejecutable), Nothing, "Origin", Nothing)
                                listaJuegos.Add(juego)

                                Dim grid As New Grid

                                Dim texto As New TextBlock With {
                                     .Text = juego.Titulo,
                                     .Foreground = New SolidColorBrush(Colors.White)
                                    }

                                grid.Tag = juego
                                grid.Children.Add(texto)

                                lvJuegos.Items.Add(grid)
                            End If
                        End If
                    Next
                Next
            End If
        End If

        Dim panelSiJuegos As DropShadowPanel = pagina.FindName("panelAvisoSiJuegosOrigin")
        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosOrigin")

        If listaJuegos.Count > 0 Then
            panelSiJuegos.Visibility = Visibility.Visible
            panelNoJuegos.Visibility = Visibility.Collapsed
            lvJuegos.Visibility = Visibility.Visible
        Else
            panelSiJuegos.Visibility = Visibility.Collapsed
            panelNoJuegos.Visibility = Visibility.Visible
            lvJuegos.Visibility = Visibility.Collapsed
        End If

    End Sub

End Module