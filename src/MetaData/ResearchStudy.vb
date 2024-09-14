
''' <summary>
''' metadata of the metabolights research study
''' </summary>
Public Class ResearchStudy : Inherits MetaData

    Public Property authors As String()
    Public Property publication As String
    Public Property study_design As String()
    Public Property study_factor As String()
    Public Property keywords As String()
    Public Property Organism As String()
    Public Property OrganismPart As String()
    Public Property instrument_platform As String()
    Public Property technology_type As String
    Public Property omics_type As String
    Public Property protocols As Protocols

End Class

Public Class Protocols

    Public Property sample_collection As String
    Public Property extraction As String
    Public Property chromatography As String
    Public Property mass_spectrometry As String
    Public Property data_transformation As String
    Public Property metabolite_identification As String

    Sub New()
    End Sub

    Friend Sub New(additional_fields As Dictionary(Of String, field()))
        sample_collection = MetaData.getValues(additional_fields, "sample_collection_protocol").FirstOrDefault
        extraction = MetaData.getValues(additional_fields, "extraction_protocol").FirstOrDefault
        chromatography = MetaData.getValues(additional_fields, "chromatography_protocol").FirstOrDefault
        mass_spectrometry = MetaData.getValues(additional_fields, "mass_spectrometry_protocol").FirstOrDefault
        data_transformation = MetaData.getValues(additional_fields, "data_transformation_protocol").FirstOrDefault
        metabolite_identification = MetaData.getValues(additional_fields, "metabolite_identification_protocol").FirstOrDefault
    End Sub

End Class