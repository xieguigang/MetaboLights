Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>

Public Class zzz

    Public Shared Sub onLoad()
        Call Rscript.Main()
        Call MTBLSStudy.Main()
    End Sub
End Class
