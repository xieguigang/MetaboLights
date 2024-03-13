Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>

Public Class zzz

    Public Shared Sub onLoad()
        Call Rscript.Main()
        Call MTBLSStudy.Main()
        Call MetabolonPathmap.Main()
    End Sub
End Class
