Namespace Metabolon.Models.Network

    Public Class node

        Public Property id As String
        Public Property x As  Double
        Public Property y As Double
        Public Property label As String
        Public Property compoundType As String
        Public Property canonicalName As String
        Public Property met_compid As String
        Public Property ecNumber As String
        Public Property title As String
        Public Property color As String
        Public Property size As Double
        Public Property fixed As  Boolean
        Public Property shape As String
        Public Property chosen as  Boolean

        Public Overrides Function ToString() As String
            Return $"[{x},{y}] {id}: {label}"
        End Function

    End Class
End Namespace