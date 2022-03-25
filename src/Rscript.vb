Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("repository")>
Public Module Rscript

    <ExportAPI("loadMetaEntries")>
    Public Function loadMetaEntries(file As String) As pipeline
        Return pipeline.CreateFromPopulator(database.LoadReferenceEntries(file))
    End Function

    <ExportAPI("as.metaSet")>
    <RApiReturn(GetType(MetaData))>
    Public Function createMetaSet(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of entry)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return (From i As entry
                In repo _
                    .populates(Of entry)(env) _
                    .AsParallel
                Select MetaData.CreateMeta(i)).ToArray
    End Function

    <ExportAPI("metabolites")>
    <RApiReturn(GetType(Metabolite))>
    Public Function metabolites(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return repo _
            .populates(Of MetaData)(env) _
            .Where(Function(m) TypeOf m Is Metabolite) _
            .ToArray
    End Function

    <ExportAPI("experiments")>
    <RApiReturn(GetType(ResearchStudy))>
    Public Function experiments(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return repo _
            .populates(Of MetaData)(env) _
            .Where(Function(m) TypeOf m Is ResearchStudy) _
            .ToArray
    End Function
End Module
