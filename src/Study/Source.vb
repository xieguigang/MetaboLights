Imports System.IO
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Text.Xml.Models

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

    <Column("Source Name")> Public Property SourceName As String
    <Column("Characteristics")> Public Property Characteristics As Characteristics
    <Column("Protocol REF")> Public Property ProtocolREF As String
    <Column("Sample Name")> Public Property SampleName As String
    <Column("Factor Value")> Public Property FactorValue As FactorValue

    Public Shared Iterator Function LoadTsv(file As Stream) As IEnumerable(Of Source)
        Dim s As New StreamReader(file)
        Dim headers As String() = Tokenizer.CharsParser(s.ReadLine, delimiter:=ASCII.TAB).ToArray
        Dim line As Value(Of String) = ""
        Dim row As String()
        Dim sourceName As Integer = headers.IndexOf("Source Name")
        Dim protocolREF As Integer = headers.IndexOf("Protocol REF")
        Dim sampleName As Integer = headers.IndexOf("Sample Name")

        Do While (line = s.ReadLine) IsNot Nothing
            row = Tokenizer.CharsParser(line, delimiter:=ASCII.TAB).ToArray

            Yield New Source
        Loop
    End Function

End Class

Public Class FactorValue

    Public Property Tissue As [Property]
    Public Property Life_cycle_stage As [Property]
    Public Property Genotype As [Property]

End Class

Public Class Characteristics

    Public Property Organism As [Property]
    Public Property [Variant] As [Property]
    Public Property Organism_part As [Property]

End Class