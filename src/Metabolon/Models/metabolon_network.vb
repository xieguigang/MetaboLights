Imports MetaboLights.Metabolon.Models.Network
Imports Microsoft.VisualBasic.Data.visualize.Network

Namespace Metabolon.Models

    Public Class metabolon_network

        Public Property nodes As node()
        Public Property edges As edge()
        Public Property options As [option]
        Public Property width As Integer
        Public Property height As Integer

        Public Function CreateGraph() As Graph.NetworkGraph
            Dim g As New Graph.NetworkGraph



            Return g
        End Function

    End Class

    Public Class [option]
    End Class
End Namespace