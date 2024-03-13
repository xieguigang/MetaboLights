Imports MetaboLights.Metabolon.Models
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

        End Sub

        Public Function RenderSvg(highlights As Dictionary(Of String, String)) As SVGData
            Dim graph As NetworkGraph = RenderGraph(highlights)
            ' rendering the network graph as svg

        End Function

    End Class
End Namespace