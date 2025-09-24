Imports System.Drawing
Imports System.Drawing.Text
Imports System.Reflection
Imports System.Runtime.InteropServices

''' <summary>
''' Manages custom embedded fonts for the UI library.
''' </summary>
Public NotInheritable Class FontManager

    Private Shared _privateFonts As PrivateFontCollection
    Private Shared _customFontFamily As FontFamily

    Shared Sub New()
        LoadFont()
    End Sub

    ''' <summary>
    ''' Loads the embedded custom font from resources.
    ''' </summary>
    Private Shared Sub LoadFont()
        _privateFonts = New PrivateFontCollection()

        ' Adjust the namespace and folder path to your actual project structure
        Dim resourceName As String = "YourLibraryNamespace.Characters.MyCustomFont.ttf"

        Using fontStream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            If fontStream Is Nothing Then
                Throw New Exception($"Font resource '{resourceName}' not found. Make sure the Build Action is set to Embedded Resource.")
            End If

            ' Copy font data into unmanaged memory
            Dim fontData(CInt(fontStream.Length) - 1) As Byte
            fontStream.Read(fontData, 0, fontData.Length)
            Dim fontPtr As IntPtr = Marshal.AllocCoTaskMem(fontData.Length)
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length)

            ' Add font to collection
            _privateFonts.AddMemoryFont(fontPtr, fontData.Length)

            ' Free unmanaged memory
            Marshal.FreeCoTaskMem(fontPtr)

            _customFontFamily = _privateFonts.Families(0)
        End Using
    End Sub

    ''' <summary>
    ''' Gets the custom font family.
    ''' </summary>
    Public Shared ReadOnly Property CustomFontFamily As FontFamily
        Get
            Return _customFontFamily
        End Get
    End Property

    ''' <summary>
    ''' Creates a font instance from the custom font family.
    ''' </summary>
    ''' <param name="size">Font size.</param>
    ''' <param name="style">Font style.</param>
    ''' <returns>A <see cref="Font"/> instance.</returns>
    Public Shared Function GetFont(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Return New Font(CustomFontFamily, size, style, GraphicsUnit.Point)
    End Function

End Class
