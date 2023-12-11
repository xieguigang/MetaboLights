
Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models

''' <summary>
''' the data model of the metabolights database
''' </summary>
''' <remarks>
''' data model for save and export csv file
''' </remarks>
Public Class Metabolite : Inherits MetaData

    Public Property formula As String
    Public Property inchi As String
    Public Property iupac As String
    Public Property organism As String()

    ''' <summary>
    ''' create the metabolite annotation data model of the mzkit package
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CreateMetabolite() As MetaLib
        Dim links = cross_references
        Dim chebi As String() = links.TryGetValue("ChEBI")

        Return New MetaLib With {
            .description = description,
            .formula = formula,
            .IUPACName = iupac,
            .ID = entry_id,
            .name = name,
            .xref = New xref With {
                .chebi = chebi.JoinBy("; "),
                .InChI = inchi
            },
            .organism = organism
        }
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return $"{entry_id}: {name}({formula})"
    End Function

End Class