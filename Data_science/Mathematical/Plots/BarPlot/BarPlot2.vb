﻿#Region "Microsoft.VisualBasic::5a14d7f31a271129e868707164bb3dbb, ..\sciBASIC#\Data_science\Mathematical\Plots\BarPlot\BarPlot2.vb"

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

Imports System.Drawing
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Vector.Shapes
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS

Partial Module BarPlot

    ''' <summary>
    ''' Plot bar plot in different direction compare with <see cref="Plot"/>
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="size"></param>
    ''' <param name="margin"></param>
    ''' <param name="bg$"></param>
    ''' <param name="showGrid"></param>
    ''' <param name="stacked"></param>
    ''' <param name="showLegend"></param>
    ''' <param name="legendPos"></param>
    ''' <param name="legendBorder"></param>
    ''' <returns></returns>
    Public Function Plot2(data As BarDataGroup,
                          Optional size As Size = Nothing,
                          Optional margin As Size = Nothing,
                          Optional bg$ = "white",
                          Optional showGrid As Boolean = True,
                          Optional stacked As Boolean = False,
                          Optional showLegend As Boolean = True,
                          Optional legendPos As Point = Nothing,
                          Optional legendBorder As Border = Nothing) As Bitmap

        Return GraphicsPlots(
            size, margin, bg,
            Sub(ByRef g, region)
                Dim top! = region.PlotRegion.Top
                Dim lefts! = region.PlotRegion.Left
                Dim mapper As New Scaling(data, stacked, True)
                Dim n As Integer = If(
                   stacked,
                   data.Samples.Length,
                   data.Samples.Sum(Function(x) x.data.Length))
                Dim dy As Double =
                  (size.Height - 2 * margin.Height - 2 * margin.Height) / n
                Dim interval As Double = 2 * margin.Height / n
                Dim sx As Func(Of Single, Single) =
                   mapper.XScaler(size, margin)

                Call g.DrawAxis(size, margin, mapper, showGrid)

                For Each sample As SeqValue(Of BarDataSample) In data.Samples.SeqIterator
                    Dim y = top + interval

                    If stacked Then ' 改变Y
                        Dim bottom! = y + dy
                        Dim right = sx(sample.value.StackedSum)
                        Dim canvasWidth = size.Height - (margin.Height * 2)

                        For Each val As SeqValue(Of Double) In sample.value.data.SeqIterator
                            Dim rect As Rectangle = Rectangle(y, lefts, right, bottom)

                            Call g.FillRectangle(New SolidBrush(data.Serials(val.i).Value), rect)

                            top += ((val.value - mapper.xmin) / mapper.dx) * canvasWidth
                        Next

                        top += dy
                    Else ' 改变X
                        For Each val As SeqValue(Of Double) In sample.value.data.SeqIterator
                            Dim bottom! = y
                            Dim right = sx(val.value)
                            Dim rect As Rectangle = Rectangle(bottom, lefts, right, bottom + dy)

                            Call g.FillRectangle(New SolidBrush(data.Serials(val.i).Value), rect)
                            Call g.DrawRectangle(Pens.Black, rect)

                            y += dy
                        Next
                    End If

                    top = y
                Next

                If showLegend Then
                    Dim legends As Legend() = LinqAPI.Exec(Of Legend) <=
 _
                        From x As NamedValue(Of Color)
                        In data.Serials
                        Select New Legend With {
                            .color = x.Value.RGBExpression,
                            .fontstyle = CSSFont.GetFontStyle(
                                FontFace.MicrosoftYaHei,
                                FontStyle.Regular,
                                30),
                            .style = LegendStyles.Circle,
                            .title = x.Name
                        }

                    If legendPos.IsEmpty Then
                        legendPos = New Point(CInt(size.Width * 0.8), margin.Height)
                    End If

                    Call g.DrawLegends(legendPos, legends,,, legendBorder)
                End If
            End Sub)
    End Function
End Module
