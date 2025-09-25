Imports System.Drawing
Imports System.Drawing.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices

''' <summary>
''' Manages embedded fonts for the UI library.
''' Place font files in "Characters" folder and set Build Action = Embedded Resource.
''' </summary>
Public NotInheritable Class FontManager

    ' ================================
    ' ENUM FOR FONT OPTIONS
    ' ================================
    Public Enum ALFontStyles
        SystemDefault
        IRANSansX
        SymbolFont
    End Enum

    Private Shared ReadOnly _fontCollection As New PrivateFontCollection()
    Private Shared ReadOnly _fonts As New Dictionary(Of String, FontFamily)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly _memoryPointers As New List(Of IntPtr)()

    ''' <summary>
    ''' Load a font from embedded resources into the collection.
    ''' </summary>
    Private Shared Sub LoadFont(fileName As String)
        If _fonts.ContainsKey(fileName) Then Return

        Dim asm As Assembly = GetType(FontManager).Assembly
        Dim resNames = asm.GetManifestResourceNames()

        ' Match by suffix or contains
        Dim resourceName = resNames.FirstOrDefault(Function(n) n.EndsWith("." & fileName, StringComparison.OrdinalIgnoreCase) _
                                                   OrElse n.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0)

        If String.IsNullOrEmpty(resourceName) Then
            Throw New Exception($"Font resource '{fileName}' not found. Available resources:{vbCrLf}{String.Join(vbCrLf, resNames)}")
        End If

        Using stream As Stream = asm.GetManifestResourceStream(resourceName)
            Dim data(CInt(stream.Length - 1)) As Byte
            stream.Read(data, 0, data.Length)

            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(data.Length)
            Marshal.Copy(data, 0, ptr, data.Length)
            _memoryPointers.Add(ptr)

            _fontCollection.AddMemoryFont(ptr, data.Length)
        End Using

        Dim fam As FontFamily = _fontCollection.Families.Last()
        _fonts(fileName) = fam
    End Sub

    ''' <summary>
    ''' Get a font by filename.
    ''' </summary>
    Public Shared Function GetFontFrom(fileName As String, size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        If Not _fonts.ContainsKey(fileName) Then LoadFont(fileName)
        Return New Font(_fonts(fileName), size, style, GraphicsUnit.Point)
    End Function

    ''' <summary>
    ''' Shortcut for default font IRANSansX-Regular.ttf.
    ''' </summary>
    Public Shared Function GetDefaultFont(size As Single, Optional style As FontStyle = FontStyle.Regular) As Font
        Return GetFontFrom("IRANSansX-Regular.ttf", size, style)
    End Function

End Class
