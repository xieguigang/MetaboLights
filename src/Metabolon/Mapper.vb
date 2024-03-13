Imports System.Runtime.CompilerServices
Imports MetaboLights.Metabolon.Models
Imports MetaboLights.Metabolon.Models.AssociationMatrix
Imports MetaboLights.Metabolon.Models.Network
Imports Microsoft.VisualBasic.Linq

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
        ReadOnly nodeId As Dictionary(Of String, String())

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.association = association
            Me.nodeId = network.nodes _
                .Where(Function(vi) Not vi.met_compid.StringEmpty) _
                .GroupBy(Function(v) $"M{v.met_compid}") _
                .ToDictionary(Function(v) v.Key,
                              Function(v)
                                  Return v.Select(Function(vi) vi.id) _
                                      .Distinct _
                                      .ToArray
                              End Function)

            Call SetResponseIndex()
        End Sub

        Private Sub SetResponseIndex()
            For Each item As response In association.responses
                If item.kegg IsNot Nothing Then responseIndex(item.kegg) = item.compid
                If item.hmdb IsNot Nothing Then responseIndex(item.hmdb) = item.compid
                If item.hmdb_ef IsNot Nothing Then responseIndex(item.hmdb_ef) = item.compid
                If item.cas IsNot Nothing Then responseIndex(item.cas) = item.compid
                If item.chemspider IsNot Nothing Then responseIndex(item.chemspider) = item.compid
                If item.pubchem IsNot Nothing Then responseIndex(item.pubchem) = item.compid
                If item.name IsNot Nothing Then responseIndex(item.name.ToLower) = item.compid
            Next
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
            Return highlights.SafeQuery _
                .Select(Iterator Function(map) As IEnumerable(Of (key As String, color As String))
                            For Each v_id As String In MapNode(map.Key).SafeQuery
                                Yield (v_id, map.Value)
                            Next
                        End Function) _
                .IteratesALL _
                .GroupBy(Function(v) v.key) _
                .ToDictionary(Function(i) i.Key,
                              Function(v)
                                  Return v.First.color
                              End Function)
        End Function

        Public Function MapNode(q As String) As String()
            If responseIndex.ContainsKey(q) Then
                Return nodeId.TryGetValue(responseIndex(q))
            ElseIf responseIndex.ContainsKey(q.ToLower) Then
                Return nodeId.TryGetValue(responseIndex(q.ToLower))
            Else
                Return {}
            End If
        End Function

    End Class
End Namespace