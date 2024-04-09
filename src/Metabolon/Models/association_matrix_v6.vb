Imports MetaboLights.Metabolon.Models.AssociationMatrix
Imports Microsoft.VisualBasic.Linq

Namespace Metabolon.Models

    Public Class association_matrix_v6

        Public Property responses As response()
        Public Property predictors As predictor()
        Public Property associations As association()
        Public Property responsesns As response()

        Public Function GetAllMetabolites() As IEnumerable(Of response)
            Return responses.JoinIterates(responsesns)
        End Function

    End Class
End Namespace