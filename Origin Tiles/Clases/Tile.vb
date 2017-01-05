Public Class Tile

    Private _Titulo As String
    Private _Ejecutable As String
    Private _Imagen As String

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

    Public Sub New(ByVal titulo As String, ByVal ejecutable As String, ByVal imagen As String)
        _Titulo = titulo
        _Ejecutable = ejecutable
        _Imagen = imagen
    End Sub

End Class
