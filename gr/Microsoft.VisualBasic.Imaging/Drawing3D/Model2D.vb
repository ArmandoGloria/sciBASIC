﻿Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing3D.IsoMetric

Namespace Drawing3D

    Public Class Model2D
        Friend path As Path3D
        Friend baseColor As Color
        Friend paint As Pen
        Friend drawn As Integer
        Friend transformedPoints As Point3D()
        Friend drawPath As Path2D

        Friend Sub New(ByVal item As Model2D)
            transformedPoints = item.transformedPoints
            drawPath = item.drawPath
            drawn = item.drawn
            Me.paint = item.paint
            Me.path = item.path
            Me.baseColor = item.baseColor
        End Sub

        Friend Sub New(ByVal ___path As Path3D, ByVal baseColor As Color)
            drawPath = New Path2D
            drawn = 0
            Me.baseColor = baseColor
            Me.paint = New Pen(Color.FromArgb(CInt(Fix(baseColor.A)), CInt(Fix(baseColor.R)), CInt(Fix(baseColor.G)), CInt(Fix(baseColor.B))), 1)
            Me.path = ___path
        End Sub
    End Class
End Namespace