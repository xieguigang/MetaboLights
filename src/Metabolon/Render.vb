Imports System.Drawing
Imports MetaboLights.Metabolon.Models
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq

Namespace Metabolon

    Public Class Render

        ReadOnly network As metabolon_network
        ReadOnly mapper As Mapper

        Public Property defaultFill As String = NameOf(Color.LightGray)

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.mapper = New Mapper(association, network)
        End Sub

        Public Function RenderGraph(highlights As Dictionary(Of String, String)) As NetworkGraph
            Dim graph As NetworkGraph = network.CreateGraph
            Dim gray As Brush = defaultFill.GetBrush

            ' reset colors
            For Each v As Node In graph.vertex
                v.data.color = gray
            Next

            ' rendering the hightlights data
            If Not highlights.IsNullOrEmpty Then
                Call RenderGraph(graph, highlights)
            End If

            Return graph
        End Function

        Private Sub RenderGraph(ByRef graph As NetworkGraph, hightlights As Dictionary(Of String, String))
            For Each target In hightlights
                Dim vs As String() = mapper.MapNode(target.Key)
                Dim color As Brush = target.Value.GetBrush

                If vs.IsNullOrEmpty Then
                    Call VBDebugger.EchoLine($"'{target.Key}' has no id mapping!")
                Else
                    Call VBDebugger.EchoLine($"get {vs.Length} vertex id mapping for target '{target.Key}'.")
                End If

                For Each v_id As String In vs.SafeQuery
                    graph.GetElementByID(v_id).data.color = color
                Next
            Next
        End Sub

        Public Function RenderSvg(highlights As Dictionary(Of String, String)) As SVGData
            Dim graph As NetworkGraph = RenderGraph(highlights)
            ' rendering the network graph as svg
            Dim img = NetworkVisualizer.DrawImage(graph,
                  canvasSize:=$"{network.width * 6.5},{network.height * 6.5}",
                  labelerIterations:=0,
                  fillConvexHullPolygon:=False,
                  labelTextStroke:=Nothing,
                  shapeRender:=AddressOf LegendPlotExtensions.DrawShape,
                  labelWordWrapWidth:=24,
                  fontSize:=16,
                  driver:=Drivers.SVG)

            Return img
        End Function

    End Class
End Namespace