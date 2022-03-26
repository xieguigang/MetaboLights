Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Text
Imports SMRUCC.genomics.Assembly.ELIXIR.EBI.ChEBI.WebServices
Imports SMRUCC.genomics.Assembly.ELIXIR.EBI.ChEBI.XML
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

        Static organismIgnores As Index(Of String) = {"blank", "experimental blank", "reference compound", "culture media", "sample preparation blank", "reference compound mix", "solvent blank"}
        Static tissueIgnores As Index(Of String) = {"dmem (medium)", "solvent", "mixture", "blank", "pure substance", "commercial glucose", "standard", "matrix", "no injection"}

        Dim organismTags As String() = study _
            .Select(Function(d)
                        Return d.Organism _
                            .Select(Function(s) s.StringSplit("[\\/]")) _
                            .IteratesALL _
                            .Where(Function(r) Not Strings.LCase(r) Like organismIgnores) _
                            .Select(AddressOf Strings.LCase) _
                            .Distinct _
                            .JoinBy("; ") _
                            .trimString
                    End Function) _
            .ToArray
        Dim tissueTags As String() = study _
            .Select(Function(d)
                        Return d.OrganismPart _
                            .Where(Function(r)
                                       Return Not Strings.LCase(r) Like tissueIgnores
                                   End Function) _
                            .Select(AddressOf Strings.LCase) _
                            .Distinct _
                            .JoinBy("; ") _
                            .trimString
                    End Function) _
            .ToArray

        table.columns("name") = study.Select(Function(d) trimString(d.name)).ToArray
        table.columns("keywords") = study.Select(Function(d) trimString(d.keywords.JoinBy("; "))).ToArray
        table.columns("study") = study.Select(Function(d) trimString(d.study_design.JoinBy("; "))).ToArray
        table.columns("publication") = study.Select(Function(d) trimString(d.publication)).ToArray
        table.columns("organism") = organismTags
        table.columns("tissue") = tissueTags
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
    <Extension>
    Private Function trimString(str As String) As String
        Dim raw = str.LineTokens.Select(Function(s) s.StringReplace("\s{2,}", " ")).ToArray
        Dim asc = raw _
            .Select(Function(s)
                        Return GreekAlphabets _
                            .StripGreek(s) _
                            .Where(Function(c) ASCII.IsAsciiChar(c)) _
                            .JoinBy("")
                    End Function) _
            .ToArray

        Return asc.JoinBy("; ")
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

    <ExportAPI("parseChEBIEntity")>
    <RApiReturn(GetType(ChEBIEntity))>
    Public Function ParseChebiEntity(xml As String) As Object
        Dim data = REST.ParsingRESTData(xml)

        If data.Length = 1 Then
            Return data(Scan0)
        Else
            Return data
        End If
    End Function
End Module
