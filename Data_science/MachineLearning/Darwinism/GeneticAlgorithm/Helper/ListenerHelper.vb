﻿#Region "Microsoft.VisualBasic::e88b18f5bee10863c1dec6900b86c9d6, Data_science\MachineLearning\Darwinism\GeneticAlgorithm\Helper\ListenerHelper.vb"

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

    '     Module ListenerHelper
    ' 
    '         Sub: AddDefaultListener
    '         Structure outPrint
    ' 
    '             Properties: chromosome, fit, iter
    ' 
    '             Function: ToString
    ' 
    '         Structure listenerHelper
    ' 
    '             Sub: Update
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models

Namespace Darwinism.GAF.Helper

    Public Module ListenerHelper

        Public Structure outPrint

            Public Property iter%
            Public Property fit#
            Public Property chromosome$

            Public Overrides Function ToString() As String
                Return $"{iter}{vbTab}{fit}{vbTab}{chromosome}"
            End Function
        End Structure

        ''' <summary>
        ''' After each iteration Genetic algorithm notifies listener
        ''' </summary>
        ''' 
        <Extension>
        Public Sub AddDefaultListener(Of T As Chromosome(Of T))(
                                  ByRef ga As GeneticAlgorithm(Of T),
                               Optional print As Action(Of outPrint) = Nothing,
                               Optional threshold# = DefaultThreshold)

            If print Is Nothing Then
                print = Sub(out)
                            Call Console.WriteLine(out.ToString)
                        End Sub
                ' just for pretty print
                Console.WriteLine($"{NameOf(outPrint.iter)}{vbTab}{NameOf(outPrint.fit)}{vbTab}{NameOf(outPrint.chromosome)}")
            End If

            ' Lets add listener, which prints best chromosome after each iteration
            ga.addIterationListener(
                New listenerHelper(Of T) With {
                    .print = print,
                    .threshold = threshold
                })
        End Sub

        Const DefaultThreshold# = 0.00001

        Private Structure listenerHelper(Of T As Chromosome(Of T))
            Implements IterartionListener(Of T)

            Public threshold As Double
            Public print As Action(Of outPrint)

            Public Sub Update(ga As GeneticAlgorithm(Of T)) Implements IterartionListener(Of T).Update
                Dim best As T = ga.Best
                Dim bestFit As Double = ga.GetFitness(best)
                Dim iteration As Integer = ga.Iteration

                ' Listener prints best achieved solution
                Call print(
                    New outPrint With {
                        .iter = iteration,
                        .fit = bestFit,
                        .chromosome = best.ToString
                    })

                ' If fitness is satisfying - we can stop Genetic algorithm
                If bestFit <= threshold Then
                    Call ga.Terminate()
                End If
            End Sub
        End Structure
    End Module
End Namespace
