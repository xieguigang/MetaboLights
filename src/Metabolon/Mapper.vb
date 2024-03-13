Imports System.Runtime.CompilerServices
Imports MetaboLights.Metabolon.Models
Imports MetaboLights.Metabolon.Models.AssociationMatrix
Imports MetaboLights.Metabolon.Models.Network

Namespace Metabolon

    Public Class Mapper

        ReadOnly association As association_matrix_v6
        ReadOnly network As metabolon_network
        ''' <summary>
        ''' mapping from xref to <see cref="response.compid"/>
        ''' </summary>
        ReadOnly responseIndex As New Dictionary(Of String, String)
        ''' <summary>
        ''' mapping from <see cref="node.met_compid"/> to <see cref="node.id"/>
        ''' </summary>
        ReadOnly nodeId As Dictionary(Of String, String)

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.association = association
            Me.nodeId = network.nodes _
                .Where(Function(vi) Not vi.met_compid.StringEmpty) _
                .ToDictionary(Function(v) $"M{v.met_compid}",
                              Function(v)
                                  Return v.id
                              End Function)

            Call SetResponseIndex()
        End Sub

        Private Sub SetResponseIndex()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns>
        ''' a mapping tuple list of [item_key => color]
        ''' </returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetMapping(highlights As Dictionary(Of String, String)) As Dictionary(Of String, String)
            Return highlights.ToDictionary(Function(i) MapNode(i.Key), Function(color) color.Value)
        End Function

        Public Function MapNode(q As String) As String

        End Function

    End Class
End Namespace