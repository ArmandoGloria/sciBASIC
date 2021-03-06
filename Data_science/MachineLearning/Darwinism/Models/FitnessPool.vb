﻿#Region "Microsoft.VisualBasic::62a6063413767198b254fdc2cb162dac, Data_science\MachineLearning\Darwinism\Models\FitnessPool.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
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



    ' /********************************************************************************/

    ' Summaries:

    '     Class FitnessPool
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: Fitness
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language.Default

Namespace Darwinism.Models

    ''' <summary>
    ''' Compute fitness and cache the result data in this pool.
    ''' </summary>
    ''' <typeparam name="Individual"></typeparam>
    Public Class FitnessPool(Of Individual)

        Protected Friend ReadOnly cache As New Dictionary(Of String, Double)
        Protected caclFitness As Func(Of Individual, Double)
        Protected indivToString As Func(Of Individual, String)
        Protected maxCapacity%

        Shared ReadOnly objToString As New DefaultValue(Of Func(Of Individual, String))(AddressOf Scripting.ToString)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cacl">Expression for descript how to calculate the fitness.</param>
        ''' <param name="toString">Obj to dictionary key</param>
        Sub New(cacl As Func(Of Individual, Double), capacity%, Optional toString As Func(Of Individual, String) = Nothing)
            caclFitness = cacl
            indivToString = toString Or objToString
            maxCapacity = capacity
        End Sub

        Sub New()
        End Sub

        ''' <summary>
        ''' This function tells how well given individual performs at given problem.
        ''' </summary>
        ''' <param name="[in]"></param>
        ''' <returns></returns>
        Public Function Fitness([in] As Individual) As Double
            Dim key$ = indivToString([in])
            Dim fit As Double

            SyncLock cache
                If cache.ContainsKey(key$) Then
                    fit = cache(key$)
                Else
                    fit = caclFitness([in])
                    cache.Add(key$, fit)

                    If cache.Count >= maxCapacity Then
                        Dim asc = From fitValue
                                  In cache
                                  Order By fitValue.Value Ascending
                                  Take CInt(maxCapacity * 0.9)
                                  Select ID = fitValue.Key

                        With asc.Indexing
                            For Each key In cache.Keys.ToArray
                                If .NotExists(key) Then
                                    Call cache.Remove(key)
                                End If
                            Next
                        End With
                    End If
                End If
            End SyncLock

            Return fit
        End Function
    End Class
End Namespace
