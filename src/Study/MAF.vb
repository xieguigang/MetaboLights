﻿
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.Annotations
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework.StorageProvider.Reflection

Public Class MAF : Inherits DynamicPropertyBase(Of Double)
    Implements INamedValue
    Implements IReadOnlyId
    Implements ICompoundNameProvider, IExactMassProvider, IFormulaProvider

    <Column("database_identifier")> Public Property ID As String Implements INamedValue.Key, IReadOnlyId.Identity
    <Column("chemical_formula")> Public Property formula As String Implements IFormulaProvider.Formula
    <Column("smiles")> Public Property smiles As String
    <Column("inchi")> Public Property inchi As String
    ''' <summary>
    ''' the metabolite name
    ''' </summary>
    ''' <returns></returns>
    <Column("metabolite_identification")> Public Property metabolite_identification As String Implements ICompoundNameProvider.CommonName
    <Column("mass_to_charge")> Public Property mz As String
    <Column("fragmentation")> Public Property fragmentation As String
    <Column("modifications")> Public Property modifications As String
    <Column("charge")> Public Property charge As String
    <Column("retention_time")> Public Property retention_time As String
    <Column("taxid")> Public Property taxid As String
    <Column("species")> Public Property species As String
    <Column("database")> Public Property database As String
    <Column("database_version")> Public Property database_version As String
    <Column("reliability")> Public Property reliability As String
    <Column("uri")> Public Property uri As String
    <Column("search_engine")> Public Property search_engine As String
    <Column("search_engine_score")> Public Property search_engine_score As String
    <Column("smallmolecule_abundance_sub")> Public Property smallmolecule_abundance_sub As String
    <Column("smallmolecule_abundance_stdev_sub")> Public Property smallmolecule_abundance_stdev_sub As String
    <Column("smallmolecule_abundance_std_error_sub")> Public Property smallmolecule_abundance_std_error_sub As String

    Private ReadOnly Property ExactMass As Double Implements IExactMassProvider.ExactMass
        Get
            Return FormulaScanner.EvaluateExactMass(formula)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return metabolite_identification
    End Function

End Class
