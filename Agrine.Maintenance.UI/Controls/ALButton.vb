Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Serialization.Formatters
Imports System.Windows.Forms

''' <summary>
''' Custom button with rounded corners, border size, and border color options.
''' </summary>
Public Class ALButton
    Inherits Button

    ' ================================
    ' Properties
    ' ================================

    Private _borderRadius As Integer = 20
    ''' <summary>
    ''' Gets or sets the corner radius of the button (in pixels).
    ''' </summary>
    Public Property BorderRadius As Integer
        Get
            Return _borderRadius
        End Get
        Set(value As Integer)
            _borderRadius = value
            Me.Invalidate() ' Redraw the control
        End Set
    End Property

    Private _borderSize As Integer = 2
    ''' <summary>
    ''' Gets or sets the thickness of the button border.
    ''' </summary>
    Public Property BorderSize As Integer
        Get
            Return _borderSize
        End Get
        Set(value As Integer)
            _borderSize = value
            Me.Invalidate()
        End Set
    End Property

    Private _borderColor As Color = Color.Black
    ''' <summary>
    ''' Gets or sets the color of the button border.
    ''' </summary>
    Public Property BorderColor As Color
        Get
            Return _borderColor
        End Get
        Set(value As Color)
            _borderColor = value
            Me.Invalidate()
        End Set
    End Property

    ' ================================
    ' Constructor
    ' ================================
    ''' <summary>
    ''' Initializes a new instance of the <see cref="CustomButton"/> class with default settings.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        Me.FlatStyle = FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
        Me.Size = New Size(120, 45)
        Me.BackColor = Color.MediumSlateBlue
        Me.ForeColor = Color.White
        Me.Text = "دکمه"
    End Sub

    ' ================================
    ' Methods
    ' ================================

    ''' <summary>
    ''' Creates a GraphicsPath with rounded corners based on the given rectangle and radius.
    ''' </summary>
    ''' <param name="rect">The rectangle bounds of the button.</param>
    ''' <param name="radius">The corner radius.</param>
    ''' <returns>A <see cref="GraphicsPath"/> representing the rounded rectangle.</returns>
    Private Function GetFigurePath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim curveSize As Single = radius * 2.0F

        path.StartFigure()
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90)
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90)
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90)
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90)
        path.CloseFigure()

        Return path
    End Function

    ''' <summary>
    ''' Overrides the Paint event to draw a custom button with rounded corners and border.
    ''' </summary>
    ''' <param name="pevent">The paint event arguments.</param>
    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        MyBase.OnPaint(pevent)

        Dim rectSurface As Rectangle = Me.ClientRectangle
        Dim rectBorder As Rectangle = Rectangle.Inflate(rectSurface, -_borderSize, -_borderSize)

        Dim smoothSize As Integer = If(_borderSize > 0, _borderSize, 2)

        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        ' If rounded corners are enabled
        If _borderRadius > 2 Then
            Using pathSurface As GraphicsPath = GetFigurePath(rectSurface, _borderRadius)
                Using pathBorder As GraphicsPath = GetFigurePath(rectBorder, _borderRadius - _borderSize)
                    Using penSurface As New Pen(Me.Parent.BackColor, smoothSize)
                        Using penBorder As New Pen(_borderColor, _borderSize)
                            ' Draw surface
                            Me.Region = New Region(pathSurface)
                            pevent.Graphics.DrawPath(penSurface, pathSurface)

                            ' Draw border
                            If _borderSize >= 1 Then
                                pevent.Graphics.DrawPath(penBorder, pathBorder)
                            End If
                        End Using
                    End Using
                End Using
            End Using
        Else
            ' Normal rectangular button
            Me.Region = New Region(rectSurface)
            If _borderSize >= 1 Then
                Using penBorder As New Pen(_borderColor, _borderSize)
                    penBorder.Alignment = PenAlignment.Inset
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, rectSurface.Width - 1, rectSurface.Height - 1)
                End Using
            End If
        End If
    End Sub

End Class
