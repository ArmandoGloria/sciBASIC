﻿Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

Module Program

    Sub Main()
        Call GetType(Program).RunCLI(App.CommandLine)
    End Sub

    <ExportAPI("/file", Usage:="/file /in <file.txt> [/out <out.txt>]")>
    Public Function OnlySupportsFile(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim out As String = args.GetValue("/out", [in].TrimSuffix & ".output.json")
        Return [in].ReadAllLines.GetJson.SaveTo(out).CLICode
    End Function

    <ExportAPI("/pipe.Test", Usage:="/pipe.Test /in <file.txt/std_in> [/out <out.txt/std_out>]")>
    Public Function SupportsBothFileAndPipeline(args As CommandLine) As Integer
        Using out = args.OpenStreamOutput("/out")
            Dim inData As String() = args.OpenStreamInput("/in").ReadToEnd.lTokens
            Call out.Write(inData.GetJson)
        End Using

        Return 0
    End Function
End Module
