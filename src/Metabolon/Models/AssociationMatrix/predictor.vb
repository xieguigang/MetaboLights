Namespace Metabolon.Models.AssociationMatrix

    Public Class predictor

        Public Property locus As String
        Public Property metabotype As String
        Public Property name As String
        Public Property chr As String
        Public Property bp_from As Long
        Public Property bp_to As Long
        Public Property genes As gene()
        Public Property key As String

    End Class

    Public Class gene

        Public Property literature As String()
        Public Property vep As String()
        Public Property vep_proxies As String()
        Public Property snipa_proxies As String()
        Public Property snipa As String()

    End Class
End Namespace