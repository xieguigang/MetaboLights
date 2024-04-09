
Imports MetaboLights.Metabolon
Imports MetaboLights.Metabolon.Models
Imports MetaboLights.Metabolon.Models.AssociationMatrix
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("Metabolon")>
<RTypeExport("association_matrix.v6", GetType(association_matrix_v6))>
<RTypeExport("metabolon_network", GetType(metabolon_network))>
Module MetabolonPathmap

    Sub Main()
        Call generic.add("plot", GetType(Render), AddressOf plotPathmap)
    End Sub

    <RGenericOverloads("plot")>
    Public Function plotPathmap(render As Render, args As list, env As Environment) As Object
        Dim highlights As list = args.getValue(Of list)({"highlight", "highlights"}, env, [default]:=Nothing)
        Dim defaultColor As String = args.getValue({"default.fill"}, env, [default]:="lightgray")
        Dim colors As Dictionary(Of String, String) = If(
            highlights Is Nothing,
            Nothing,
            highlights.AsGeneric(env, [default]:="#000000"))

        render.defaultFill = defaultColor

        Return render.RenderSvg(highlights:=colors)
    End Function

    <ExportAPI("as.render")>
    Public Function render(network As metabolon_network, association As association_matrix_v6) As Render
        Return New Render(association, network)
    End Function

    <ExportAPI("highlights")>
    <RApiReturn(GetType(NetworkGraph))>
    Public Function highlight_graph(render As Render, <RListObjectArgument> highlight As list, Optional env As Environment = Nothing) As Object
        If highlight Is Nothing Then
            highlight = list.empty
        End If
        If highlight.hasName(NameOf(highlight)) Then
            highlight = highlight(NameOf(highlight))

            If highlight Is Nothing Then
                highlight = list.empty
            End If
        End If

        Dim colors As Dictionary(Of String, String) = highlight.AsGeneric(env, [default]:="#FFFFFF")
        Dim graph As NetworkGraph = render.RenderGraph(colors)

        Return graph
    End Function

    <ExportAPI("matches_cas")>
    Public Function matches(<RRawVectorArgument> cas As String(), association As association_matrix_v6) As Object
        If cas.IsNullOrEmpty Then
            Return Nothing
        End If

        Dim metabolites_cas = association _
            .GetAllMetabolites _
            .Select(Iterator Function(r) As IEnumerable(Of (cas_id As String, meta As response))
                        For Each id As String In r.cas.StringSplit("[,;]+")
                            Yield (id, r)
                        Next
                    End Function) _
            .IteratesALL _
            .GroupBy(Function(r) r.cas_id) _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return a.ToArray
                          End Function)

        Dim result As response() = New response(cas.Length - 1) {}

        For i As Integer = 0 To result.Length - 1
            result(i) = metabolites_cas.TryGetValue(cas(i), [default]:={}).FirstOrDefault.meta
        Next

        Return result
    End Function

End Module
