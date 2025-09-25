Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

''' <summary>
''' Advanced custom button with:
''' - Rounded corners
''' - Custom border color and size
''' - Hover animation
''' - Predefined font options (via ALFont)
''' </summary>
Public Class ALButton
    Inherits Button

    ' ================================
    ' ENUM FOR FONT OPTIONS
    ' ================================
    Public Enum ALFontStyle
        SystemDefault
        IRANSansX
        SymbolFont
    End Enum

    ' ================================
    ' PRIVATE FIELDS
    ' ================================
    Private _alFont As ALFontStyle = ALFontStyle.SystemDefault
    Private _cornerRadius As Integer = 8
    Private _borderSize As Integer = 2
    Private _borderColor As Color = Color.DimGray

    ' For hover animation
    Private _isHovered As Boolean = False
    Private _hoverBackColor As Color = Color.LightBlue
    Private _normalBackColor As Color = Color.LightGray
    Private _animationTimer As Timer
    Private _currentBackColor As Color

    ' ================================
    ' PROPERTIES
    ' ================================
    <Category("Appearance"),
    Description("Choose a predefined font style for this button.")>
    Public Property ALFont As ALFontStyle
        Get
            Return _alFont
        End Get
        Set(value As ALFontStyle)
            _alFont = value
            ApplyCustomFont()
        End Set
    End Property

    <Category("Appearance"),
    Description("Corner radius of the button.")>
    Public Property CornerRadius As Integer
        Get
            Return _cornerRadius
        End Get
        Set(value As Integer)
            _cornerRadius = Math.Max(0, value)
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance"),
    Description("Border size of the button.")>
    Public Property BorderSize As Integer
        Get
            Return _borderSize
        End Get
        Set(value As Integer)
            _borderSize = Math.Max(0, value)
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance"),
    Description("Border color of the button.")>
    Public Property BorderColor As Color
        Get
            Return _borderColor
        End Get
        Set(value As Color)
            _borderColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance"),
    Description("Background color when hovered.")>
    Public Property HoverBackColor As Color
        Get
            Return _hoverBackColor
        End Get
        Set(value As Color)
            _hoverBackColor = value
        End Set
    End Property

    <Category("Appearance"),
    Description("Background color when not hovered.")>
    Public Property NormalBackColor As Color
        Get
            Return _normalBackColor
        End Get
        Set(value As Color)
            _normalBackColor = value
            _currentBackColor = value
            Me.Invalidate()
        End Set
    End Property

    ' Hide the base Font property
    <Browsable(False)>
    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(value As Font)
            ' Ignore direct font setting
            MyBase.Font = value
        End Set
    End Property

    ' ================================
    ' CONSTRUCTOR
    ' ================================
    Public Sub New()
        MyBase.New()
        Me.FlatStyle = FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
        Me.ForeColor = Color.Black

        _normalBackColor = Color.LightGray
        _hoverBackColor = Color.LightBlue
        _currentBackColor = _normalBackColor

        ' Setup animation timer
        _animationTimer = New Timer()
        _animationTimer.Interval = 15 ' ~60fps
        AddHandler _animationTimer.Tick, AddressOf AnimateBackground
    End Sub

    ' ================================
    ' FONT LOGIC
    ' ================================
    Private Sub ApplyCustomFont()
        If Me.DesignMode OrElse LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
            MyBase.Font = New Font("Arial", 12, FontStyle.Regular)
            Return
        End If

        Select Case _alFont
            Case ALFontStyle.IRANSansX
                MyBase.Font = FontManager.GetDefaultFont(12)
            Case ALFontStyle.SymbolFont
                MyBase.Font = FontManager.GetFontFrom("SymbolFont.ttf", 12)
            Case ALFontStyle.SystemDefault
                MyBase.Font = New Font(SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Regular)
        End Select
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        ApplyCustomFont()
    End Sub

    ' ================================
    ' ANIMATION EVENTS
    ' ================================
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        _isHovered = True
        _animationTimer.Start()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _isHovered = False
        _animationTimer.Start()
    End Sub

    Private Sub AnimateBackground(sender As Object, e As EventArgs)
        Try
            Dim targetColor As Color = If(_isHovered, _hoverBackColor, _normalBackColor)

            Dim t As Single = 0.1F
            Dim r As Integer = CInt(_currentBackColor.R + (targetColor.R - _currentBackColor.R) * t)
            Dim g As Integer = CInt(_currentBackColor.G + (targetColor.G - _currentBackColor.G) * t)
            Dim b As Integer = CInt(_currentBackColor.B + (targetColor.B - _currentBackColor.B) * t)

            _currentBackColor = Color.FromArgb(Clamp(r, 0, 255), Clamp(g, 0, 255), Clamp(b, 0, 255))
            Me.Invalidate()

            If _currentBackColor.ToArgb() = targetColor.ToArgb() Then
                _animationTimer.Stop()
            End If
        Catch ex As Exception
            _animationTimer.Stop()
            Debug.WriteLine("Animation Error: " & ex.Message)
        End Try
    End Sub


    Private Function Clamp(value As Integer, min As Integer, max As Integer) As Integer
        Return Math.Max(min, Math.Min(max, value))
    End Function


    ' ================================
    ' CUSTOM DRAW
    ' ================================
    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        Dim rect As Rectangle = Me.ClientRectangle
        rect.Width -= 1
        rect.Height -= 1

        Dim path As GraphicsPath = GetRoundedRectanglePath(rect, _cornerRadius)

        ' Fill background
        Using brush As New SolidBrush(_currentBackColor)
            pevent.Graphics.FillPath(brush, path)
        End Using

        ' Draw border
        If _borderSize > 0 Then
            Using pen As New Pen(_borderColor, _borderSize)
                pevent.Graphics.DrawPath(pen, path)
            End Using
        End If

        ' Draw text
        TextRenderer.DrawText(pevent.Graphics, Me.Text, Me.Font, rect, Me.ForeColor,
                              TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)

    End Sub

    Private Function GetRoundedRectanglePath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        If radius <= 0 Then
            path.AddRectangle(rect)
            Return path
        End If

        Dim d As Integer = radius * 2
        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function

End Class
