''' <summary>
''' the base object type of the metabolights metadata
''' </summary>
Public Class MetaData

    Public Property entry_id As String
    Public Property name As String
    Public Property description As String
    Public Property cross_references As Dictionary(Of String, String())

    Default Public ReadOnly Property xrefs(src As String) As String()
        Get
            Return cross_references.TryGetValue(src)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return $"{entry_id}: {name}"
    End Function

    Public Shared Function CreateMeta(entry As entry) As MetaData
        Dim metadata = loadMetaSet(entry)
        Dim publication = getValues(metadata, "publication").FirstOrDefault
        Dim study_design = getValues(metadata, "study_design")
        Dim cross_references = entry.cross_references _
            .GroupBy(Function(a) a.dbname) _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return (From i In a Select i.dbkey).ToArray
                          End Function)

        If publication Is Nothing AndAlso study_design.Length = 0 Then
            ' is metabolites
            Return New Metabolite With {
                .entry_id = entry.id,
                .name = entry.name,
                .description = entry.description,
                .cross_references = cross_references,
                .formula = getValues(metadata, "formula").FirstOrDefault,
                .inchi = getValues(metadata, "inchi").FirstOrDefault,
                .iupac = getValues(metadata, "iupac").FirstOrDefault,
                .organism = getValues(metadata, "organism")
            }
        Else
            ' is study
            Return New ResearchStudy With {
                .authors = getValues(metadata, "author"),
                .cross_references = cross_references,
                .description = entry.description,
                .entry_id = entry.id,
                .keywords = getValues(metadata, "curator_keywords"),
                .name = entry.name,
                .Organism = getValues(metadata, "Organism"),
                .OrganismPart = getValues(metadata, "Organism Part"),
                .publication = publication,
                .study_design = study_design
            }
        End If
    End Function

    Protected Shared Function loadMetaSet(entry As entry) As Dictionary(Of String, field())
        Return entry.additional_fields _
            .GroupBy(Function(a) a.name) _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return a.ToArray
                          End Function)
    End Function

    Protected Shared Function getValues(metadata As Dictionary(Of String, field()), ref As String) As String()
        If metadata.ContainsKey(ref) Then
            Return metadata(ref).Select(Function(a) a.value).ToArray
        Else
            Return {}
        End If
    End Function
End Class


