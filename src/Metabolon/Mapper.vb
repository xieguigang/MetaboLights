Imports System.Runtime.CompilerServices
Imports MetaboLights.Metabolon.Models

Namespace Metabolon

    Public Class Mapper

        ReadOnly association As association_matrix_v6
        ReadOnly network As metabolon_network

        Sub New(association As association_matrix_v6, network As metabolon_network)
            Me.network = network
            Me.association = association
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