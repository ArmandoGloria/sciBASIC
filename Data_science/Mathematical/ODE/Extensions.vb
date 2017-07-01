﻿#Region "Microsoft.VisualBasic::ce8159355e8d90d4b409069edb441313, ..\sciBASIC#\Data_science\Mathematical\ODE\Extensions.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xieguigang (xie.guigang@live.com)
'       xie (genetics@smrucc.org)
' 
' Copyright (c) 2016 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Language

Public Module Extensions

    ''' <summary>
    ''' 使用PCC来了解各个变量之间的相关度
    ''' </summary>
    ''' <param name="df"></param>
    ''' <returns></returns>
    <Extension> Public Function Pcc(df As ODEsOut) As DataSet()
        Dim out As New List(Of DataSet)
        Dim vars$() = df.y.Keys.ToArray

        For Each var As NamedValue(Of Double()) In df
            Dim x As New DataSet With {
                .ID = var.Name,
                .Properties = New Dictionary(Of String, Double)
            }

            For Each name$ In vars
                Dim __pcc# = Correlations _
                    .GetPearson(var.Value, df.y(name).Value)
                x.Properties(name$) = __pcc
            Next

            out += x
        Next

        Return out
    End Function

    ''' <summary>
    ''' 使用sPCC来了解各个变量之间的相关度
    ''' </summary>
    ''' <param name="df"></param>
    ''' <returns></returns>
    <Extension> Public Function SPcc(df As ODEsOut) As DataSet()
        Dim out As New List(Of DataSet)
        Dim vars$() = df.y.Keys.ToArray

        For Each var As NamedValue(Of Double()) In df
            Dim x As New DataSet With {
                .ID = var.Name,
                .Properties = New Dictionary(Of String, Double)
            }

            For Each name$ In vars
                Dim __spcc# = Correlations _
                    .Spearman(var.Value, df.y(name).Value)
                x.Properties(name$) = __spcc
            Next

            out += x
        Next

        Return out
    End Function

    ''' <summary>
    ''' Solve the target ODEs dynamics system by using the RK4 solver.
    ''' </summary>
    ''' <param name="system"></param>
    ''' <returns></returns>
    <Extension> Public Function Solve(system As IEnumerable(Of var), t As (from#, to#, step#)) As ODEsOut

    End Function

    <Extension> Public Sub lapply(list As Expression(Of Func(Of var())))
        Dim unaryExpression As NewArrayExpression = DirectCast(list.Body, NewArrayExpression)
        Dim arrayData = unaryExpression _
            .Expressions _
            .Select(Function(e) DirectCast(e, BinaryExpression)) _
            .ToArray

        For Each expr As BinaryExpression In arrayData
            Dim member = DirectCast(expr.Left, MemberExpression)
            Dim name As String = member.Member.Name.Replace("$VB$Local_", "")
            Dim field As FieldInfo = DirectCast(member.Member, FieldInfo)
            Dim value As Object = DirectCast(expr.Right, ConstantExpression).Value
            Dim constantExpression As ConstantExpression = DirectCast(member.Expression, ConstantExpression)

            Call field.SetValue(constantExpression.Value, New var(name, CDbl(value)))
        Next
    End Sub
End Module
