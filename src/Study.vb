Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.genomics.GCModeller.Workbench.ExperimentDesigner
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' MTBLS study project data reader
''' </summary>
<Package("MTBLSStudy")>
<RTypeExport("MTBLS_maf", GetType(MAF))>
Module MTBLSStudy

    Sub Main()

    End Sub

    ''' <summary>
    ''' read the metabolights study file
    ''' </summary>
    ''' <param name="file">a file path to the metabolights study maf tsv table file.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("read.study_source")>
    <RApiReturn(GetType(Source))>
    Public Function readSourceInformation(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As Object
        Dim is_path As Boolean = False
        Dim s = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env, is_filepath:=is_path)

        If s Like GetType(Message) Then
            Return s.TryCast(Of Message)
        End If

        Dim metadata As Source() = Source.LoadTsv(s.TryCast(Of Stream)).ToArray

        If is_path Then
            Call s.TryCast(Of Stream).Close()
        End If

        Return metadata
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="metadata"></param>
    ''' <param name="group">
    ''' the group source, value could be characteristics or factor
    ''' </param>
    ''' <param name="property">
    ''' the group property data, value maybe various based on the study details
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' the sample group information is generates via the combination of 
    ''' <paramref name="group"/> and <paramref name="property"/> data, 
    ''' example as, there is data field named ``Factor Value[Cohort]`` in maf 
    ''' table file, then the group parameter value should be ``factor`` and 
    ''' the property parameter value should be ``Cohort``.
    ''' </remarks>
    <ExportAPI("sampleinfo")>
    <RApiReturn(GetType(SampleInfo))>
    Public Function convertSampleinfo(metadata As Source(), group As String, [property] As String) As Object
        Dim samples As New List(Of SampleInfo)
        Dim offset As i32 = 1

        For Each i As Source In metadata.SafeQuery
            Call samples.Add(New SampleInfo With {
                .batch = 1,
                .color = "black",
                .ID = i.SampleName,
                .injectionOrder = ++offset,
                .sample_info = i.GetGroupInformation(group, [property]),
                .sample_name = i.SampleName,
                .shape = "+"
            })
        Next

        Return samples.ToArray
    End Function

End Module
