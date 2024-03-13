
Imports MetaboLights.Metabolon
Imports MetaboLights.Metabolon.Models
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
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
        Dim colors As Dictionary(Of String, String) = If(
            highlights Is Nothing,
            Nothing,
            highlights.AsGeneric(env, [default]:="#FFFFFF"))

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

End Module
