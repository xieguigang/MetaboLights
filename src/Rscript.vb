Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("repository")>
Public Module Rscript

    Sub New()
        Call Internal.Object.Converts.makeDataframe.addHandler(GetType(ResearchStudy()), AddressOf CreateStudyTable)
    End Sub

    Public Function CreateStudyTable(study As ResearchStudy(), args As list, env As Environment) As dataframe
        Dim table As New dataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = study _
                .Select(Function(d) d.entry_id) _
                .ToArray
        }

        Static organismIgnores As Index(Of String) = {"blank", "experimental blank", "reference compound", "culture media"}
        Static tissueIgnores As Index(Of String) = {"dmem (medium)", "solvent", "mixture", "blank", "pure substance", "commercial glucose"}

        table.columns("name") = study.Select(Function(d) trimString(d.name)).ToArray
        table.columns("keywords") = study.Select(Function(d) trimString(d.keywords.JoinBy("; "))).ToArray
        table.columns("study") = study.Select(Function(d) trimString(d.study_design.JoinBy("; "))).ToArray
        table.columns("publication") = study.Select(Function(d) trimString(d.publication)).ToArray
        table.columns("organism") = study.Select(Function(d) trimString(d.Organism.Where(Function(r) Not Strings.LCase(r) Like organismIgnores).Select(AddressOf Strings.LCase).Distinct.JoinBy("; "))).ToArray
        table.columns("tissue") = study.Select(Function(d) trimString(d.OrganismPart.Where(Function(r) Not Strings.LCase(r) Like tissueIgnores).Select(AddressOf Strings.LCase).Distinct.JoinBy("; "))).ToArray
        table.columns("metabolites") = study _
            .Select(Function(d)
                        Return d.cross_references _
                            .TryGetValue("MetaboLights", [default]:={}) _
                            .JoinBy("; ")
                    End Function) _
            .ToArray

        Return table
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function trimString(str As String) As String
        Return str.LineTokens.Select(Function(s) s.StringReplace("\s{2,}", " ")).JoinBy(" ")
    End Function

    <ExportAPI("loadMetaEntries")>
    Public Function loadMetaEntries(file As String) As pipeline
        Return pipeline.CreateFromPopulator(database.LoadReferenceEntries(file))
    End Function

    <ExportAPI("as.metaSet")>
    <RApiReturn(GetType(MetaData))>
    Public Function createMetaSet(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of entry)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return (From i As entry
                In repo _
                    .populates(Of entry)(env) _
                    .AsParallel
                Select MetaData.CreateMeta(i)).ToArray
    End Function

    <ExportAPI("metabolites")>
    <RApiReturn(GetType(Metabolite))>
    Public Function metabolites(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return repo _
            .populates(Of MetaData)(env) _
            .Where(Function(m) TypeOf m Is Metabolite) _
            .ToArray
    End Function

    <ExportAPI("experiments")>
    <RApiReturn(GetType(ResearchStudy))>
    Public Function experiments(<RRawVectorArgument> database As Object, Optional env As Environment = Nothing) As Object
        Dim repo As pipeline = pipeline.TryCreatePipeline(Of MetaData)(database, env)

        If repo.isError Then
            Return repo.getError
        End If

        Return repo _
            .populates(Of MetaData)(env) _
            .Where(Function(m) TypeOf m Is ResearchStudy) _
            .Select(Function(d) DirectCast(d, ResearchStudy)) _
            .ToArray
    End Function

    <ExportAPI("keywordFilters")>
    Public Function keywordFilters(<RRawVectorArgument>
                                   studies As Object,
                                   keywords As String(),
                                   Optional ignoreCase As Boolean = True,
                                   Optional env As Environment = Nothing) As Object

        Dim repo As pipeline = pipeline.TryCreatePipeline(Of ResearchStudy)(studies, env)
        Dim test As Func(Of String(), Boolean)

        If repo.isError Then
            Return repo.getError
        ElseIf ignoreCase Then
            test = Function(factors)
                       Return factors.Any(Function(wd) wd.InStrAny(keywords) > 0)
                   End Function
        Else
            test = Function(factors)
                       Return factors _
                           .Any(Function(wd)
                                    Return keywords _
                                        .Any(Function(str)
                                                 Return wd.IndexOf(str) > -1
                                             End Function)
                                End Function)
                   End Function
        End If

        Return repo.populates(Of ResearchStudy)(env) _
            .AsParallel _
            .Where(Function(study)
                       Return test(study.keywords) OrElse test(study.study_design)
                   End Function) _
            .ToArray
    End Function
End Module
