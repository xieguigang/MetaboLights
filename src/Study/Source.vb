Imports System.IO
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Text.Xml.Models

''' <summary>
''' the metadata for generates sampleinfo
''' </summary>
Public Class Source

    '<Column("Source Name")>
    '<Column("Characteristics[Organism]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>
    '<Column("Characteristics[Variant]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>
    '<Column("Characteristics[Organism part]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>
    '<Column("Protocol REF")>
    '<Column("Sample Name")>
    '<Column("Factor Value[Tissue]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>
    '<Column("Factor Value[Life cycle stage]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>
    '<Column("Factor Value[Genotype]")>
    '<Column("Term Source REF")>
    '<Column("Term Accession Number")>

    Public Property SourceName As String
    Public Property ProtocolREF As String
    Public Property SampleName As String
    Public Property Characteristics As Dictionary(Of String, [Property])
    Public Property FactorValue As Dictionary(Of String, [Property])

    Public Overrides Function ToString() As String
        Return SampleName
    End Function

    Public Shared Iterator Function LoadTsv(file As Stream) As IEnumerable(Of Source)
        Dim s As New StreamReader(file)
        Dim headers As String() = Tokenizer.CharsParser(s.ReadLine, delimiter:=ASCII.TAB).ToArray
        Dim line As Value(Of String) = ""
        Dim row As String()
        Dim sourceName As Integer = headers.IndexOf("Source Name")
        Dim protocolREF As Integer = headers.IndexOf("Protocol REF")
        Dim sampleName As Integer = headers.IndexOf("Sample Name")
        Dim characteristics = GetIndex(headers, "Characteristics").ToArray
        Dim factorValue = GetIndex(headers, "Factor Value").ToArray
        Dim i As Integer

        Do While (line = s.ReadLine) IsNot Nothing
            Dim chrs As New Dictionary(Of String, [Property])
            Dim factors As New Dictionary(Of String, [Property])

            row = Tokenizer _
                .CharsParser(line, delimiter:=ASCII.TAB) _
                .ToArray

            For Each offset As NamedValue(Of Integer) In characteristics
                i = offset
                chrs(offset.Name) = New [Property](row(i), row(i + 1), row(i + 2))

                If empty(chrs(offset.Name)) Then
                    chrs(offset.Name) = Nothing
                End If
            Next
            For Each offset As NamedValue(Of Integer) In factorValue
                i = offset
                factors(offset.Name) = New [Property](row(i), row(i + 1), row(i + 2))

                If empty(factors(offset.Name)) Then
                    factors(offset.Name) = Nothing
                End If
            Next

            Yield New Source With {
                .ProtocolREF = row(protocolREF),
                .SampleName = row(sampleName),
                .SourceName = row(sourceName),
                .Characteristics = chrs,
                .FactorValue = factors
            }
        Loop
    End Function

    Private Shared Function empty(p As [Property]) As Boolean
        Return p.name.Trim(" "c, """"c).StringEmpty AndAlso
            p.value.Trim(" "c, """"c).StringEmpty AndAlso
            p.comment.Trim(" "c, """"c).StringEmpty
    End Function

    Private Shared Iterator Function GetIndex(headers As String(), prefix As String) As IEnumerable(Of NamedValue(Of Integer))
        Dim key_size As Integer = prefix.Length
        Dim key As String

        For i As Integer = 0 To headers.Length - 1
            If headers(i).StartsWith(prefix) Then
                key = headers(i) _
                    .Substring(key_size) _
                    .Trim("["c, "]"c)

                Yield New NamedValue(Of Integer)(key, i)
            End If
        Next
    End Function

End Class