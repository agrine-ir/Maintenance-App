Imports System.Drawing
Imports System.Windows.Forms

' کلاس دکمه سفارشی
Public Class ALButton
    Inherits Button

    Public Sub New()
        ' تنظیمات پیش‌فرض دکمه
        Me.FlatStyle = FlatStyle.Flat
        Me.BackColor = Color.MediumSlateBlue
        Me.ForeColor = Color.White
        Me.Font = New Font("Tahoma", 12, FontStyle.Bold)
        Me.Size = New Size(120, 50)
        Me.FlatAppearance.BorderSize = 0
        Me.Cursor = Cursors.Hand
        Me.Text = "دکمه من"
    End Sub

    ' تغییر ظاهر هنگام هاور شدن
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        Me.BackColor = Color.DarkSlateBlue
    End Sub

    ' بازگرداندن ظاهر هنگام خروج موس
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        Me.BackColor = Color.MediumSlateBlue
    End Sub

    ' تغییر ظاهر هنگام کلیک
    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        MyBase.OnMouseDown(mevent)
        Me.BackColor = Color.Indigo
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        MyBase.OnMouseUp(mevent)
        Me.BackColor = Color.DarkSlateBlue
    End Sub
End Class
