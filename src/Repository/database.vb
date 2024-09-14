Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text.Xml.Linq

Public Class database

    Public Property name As String
    Public Property description As String
    Public Property release As String
    Public Property release_date As String
    Public Property entry_count As Integer
    Public Property entries As entry()

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return description
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function LoadReferenceEntries(file As String, Optional tqdm As Boolean = True) As IEnumerable(Of entry)
        Return file.LoadUltraLargeXMLDataSet(Of entry)(tqdm:=tqdm)
    End Function

End Class
