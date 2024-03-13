Imports System.Drawing
Imports MetaboLights.Metabolon.Models
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging.Driver

Namespace Metabolon

    Public Class Render

        ReadOnly association As association_matrix_v6
        ReadOnly network As metabolon_network

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.association = association
        End Sub

        Public Function RenderGraph(highlights As Dictionary(Of String, String)) As NetworkGraph
            Dim graph As NetworkGraph = network.CreateGraph

            ' rendering the hightlights data
            If Not highlights.IsNullOrEmpty Then
                Call RenderGraph(graph, highlights)
            End If

            Return graph
        End Function

        Private Sub RenderGraph(ByRef graph As NetworkGraph, hightlights As Dictionary(Of String, String))
            Dim gray As Brush = Brushes.Gray

            For Each v As Node In graph.vertex
                v.data.color = gray
            Next
        End Sub

        Public Function RenderSvg(highlights As Dictionary(Of String, String)) As SVGData
            Dim graph As NetworkGraph = RenderGraph(highlights)
            ' rendering the network graph as svg
            Dim img = NetworkVisualizer.DrawImage(graph,
                  canvasSize:=$"{network.width * 5},{network.height * 5}",
                  labelerIterations:=0,
                  fillConvexHullPolygon:=False,
                  labelTextStroke:=Nothing,
                  shapeRender:=AddressOf LegendPlotExtensions.DrawShape,
                  driver:=Drivers.SVG)

            Return img
        End Function

    End Class
End Namespace