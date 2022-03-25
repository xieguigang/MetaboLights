Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("repository")>
Public Module Rscript

    <ExportAPI("loadMetaEntries")>
    Public Function loadMetaEntries(file As String) As entry()
        Return database.LoadReferenceEntries(file).ToArray
    End Function

    <ExportAPI("as.metaSet")>
    Public Function createMetaSet(repo As entry()) As MetaData()
        Return (From i As entry
                In repo.AsParallel
                Select MetaData.CreateMeta(i)).ToArray
    End Function

    <ExportAPI("metabolites")>
    <RApiReturn(GetType(Metabolites))>
    Public Function metabolites(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return repo _
            .populates(Of MetaData)(env) _
            .Where(Function(m) TypeOf m Is Metabolites) _
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
