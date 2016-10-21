﻿Imports Microsoft.VisualBasic.Serialization.JSON

Namespace LP

    Public Structure ObjectiveFunction

        Dim type As OptimizationType
        Dim xyz#()
        Dim Z#

        Public Overrides Function ToString() As String
            Return $"{type.ToString}({xyz.GetJson})"
        End Function
    End Structure
End Namespace