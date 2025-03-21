﻿Imports System.Drawing
Imports MetaboLights.Metabolon.Models
Imports MetaboLights.Metabolon.Models.AssociationMatrix
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq

#If NET48 Then
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace Metabolon

    Public Class Render

        ReadOnly network As metabolon_network
        ReadOnly mapper As Mapper
        ReadOnly metadata As Dictionary(Of String, response)

        Public Property defaultFill As String = NameOf(Color.LightGray)
        Public Property scale As New SizeF(6.5, 10)
        ''' <summary>
        ''' the line width of the edge
        ''' </summary>
        ''' <returns></returns>
        Public Property lineWidth As Single = 4
        Public Property fontSize As Single = 24

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.mapper = New Mapper(association, network)
            Me.metadata = association.responses.ToDictionary(Function(a) a.compid)
        End Sub

        Public Function RenderGraph(highlights As Dictionary(Of String, String)) As NetworkGraph
            Dim graph As NetworkGraph = network.CreateGraph(metadata, scale, lineWidth)
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
                  canvasSize:=$"{network.width * scale.Width},{network.height * scale.Height}",
                  labelerIterations:=0,
                  fillConvexHullPolygon:=False,
                  labelTextStroke:=Nothing,
                  shapeRender:=AddressOf LegendPlotExtensions.DrawShape,
                  drawEdgeDirection:=True,
                  labelWordWrapWidth:=24,
                  fontSize:=fontSize,
                  driver:=Drivers.SVG)

            Return img
        End Function

    End Class
End Namespace