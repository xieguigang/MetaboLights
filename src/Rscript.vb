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
        Return repo.AsParallel.Select(Function(i) New MetaData(i)).ToArray
    End Function

    <ExportAPI("metabolites")>
    <RApiReturn(GetType(MetaData))>
    Public Function metabolites(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If


    End Function

    <ExportAPI("experiments")>
    <RApiReturn(GetType(MetaData))>
    Public Function experiments(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If
    End Function
End Module
