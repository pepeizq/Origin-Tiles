Public Class Tile

    Private _Titulo As String
    Private _Ejecutable As String
    Private _Imagen As String
    Private _Descripcion As String
    Private _Tile As Tile

    Public Property Titulo() As String
        Get
            Return _Titulo
        End Get
        Set(ByVal valor As String)
            _Titulo = valor
        End Set
    End Property

    Public Property Ejecutable() As String
        Get
            Return _Ejecutable
        End Get
        Set(ByVal valor As String)
            _Ejecutable = valor
        End Set
    End Property

    Public Property Imagen() As String
        Get
            Return _Imagen
        End Get
        Set(ByVal valor As String)
            _Imagen = valor
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal valor As String)
            _Descripcion = valor
        End Set
    End Property

    Public Property Tile() As Tile
        Get
            Return _Tile
        End Get
        Set(ByVal valor As Tile)
            _Tile = valor
        End Set
    End Property

    Public Sub New(ByVal titulo As String, ByVal ejecutable As String, ByVal imagen As String, ByVal descripcion As String, ByVal tile As Tile)
        _Titulo = titulo
        _Ejecutable = ejecutable
        _Imagen = imagen
        _Descripcion = descripcion
        _Tile = tile
    End Sub

End Class
