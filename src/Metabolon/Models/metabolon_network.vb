Imports System.Drawing
Imports MetaboLights.Metabolon.Models.Network
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Imaging
Imports edge_data = Microsoft.VisualBasic.Data.visualize.Network.Graph.EdgeData
Imports node_data = Microsoft.VisualBasic.Data.visualize.Network.Graph.NodeData
Imports V = Microsoft.VisualBasic.Data.visualize.Network.Graph.Node

Namespace Metabolon.Models

    Public Class metabolon_network

        Public Property nodes As node()
        Public Property edges As edge()
        Public Property options As [option]
        Public Property width As Integer
        Public Property height As Integer

        Public Function CreateGraph() As Graph.NetworkGraph
            Dim g As New Graph.NetworkGraph

            For Each node As node In nodes
                Dim meta As New node_data() With {
                    .color = node.color.GetBrush,
                    .initialPostion = New FDGVector2(node.x, node.y),
                    .label = node.label,
                    .mass = 1,
                    .origID = node.id,
                    .size = {node.size},
                    .Properties = New Dictionary(Of String, String) From {
                        {NamesOf.REFLECTION_ID_MAPPING_NODETYPE, node.compoundType},
                        {"shape", shape_data(node.shape)}
                    }
                }

                Call g.CreateNode(node.id, meta)
            Next

            For Each edge As edge In edges
                Dim u As V = g.GetElementByID(edge.from)
                Dim v As V = g.GetElementByID(edge.to)
                Dim meta As New edge_data With {
                    .label = edge.title,
                    .style = New Pen(edge.color.GetBrush, 1.0)
                }

                Call g.CreateEdge(u, v, 1, meta)
            Next

            Return g
        End Function

        Private Shared Function shape_data(shape As String) As String
            Select Case Strings.LCase(shape)
                Case "square" : Return LegendStyles.Square.Description
                Case "dot", "diamond" : Return LegendStyles.Diamond.Description
                Case "triangle", "triangledown" : Return LegendStyles.Triangle.Description
                Case "star" : Return LegendStyles.Pentacle.Description
                Case "box" : Return LegendStyles.Rectangle.Description

                    ' default is circle for render a node
                Case "" : Return LegendStyles.Circle.Description

                Case Else
                    Throw New NotImplementedException(shape)
            End Select
        End Function

    End Class

    Public Class [option]
    End Class
End Namespace