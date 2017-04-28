Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Mathematical.LinearAlgebra
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Model

    ''' <summary>
    ''' ``w``�Ǵ�ֱ�ڳ�ƽ���һ������
    ''' 
    ''' @author Ralf Wondratschek
    ''' </summary>
    Public Class NormalVector : Implements ICloneable

        Public Property Vector As Vector

        Public ReadOnly Property W1 As Double
            Get
                Return Vector(0)
            End Get
        End Property

        Public ReadOnly Property W2 As Double
            Get
                Return Vector(1)
            End Get
        End Property

        Public Sub New(v As IEnumerable(Of Double))
            Vector = New Vector(v)
        End Sub

        Sub New()
            Vector = New Vector(2)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Overrides Function Equals(o As Object) As Boolean
            If Not TypeOf o Is NormalVector Then Return MyBase.Equals(o)

            Dim vector As Vector = TryCast(o, NormalVector).Vector
            Dim ppm#

            If vector.Dim <> Me.Vector.Dim Then
                Return False
            End If

            For Each x In vector.SeqIterator
                ppm = x.value / Me.Vector(x)

                If Not (ppm < 1.0001 AndAlso ppm > 0.999) Then
                    Return False
                End If
            Next

            Return True
        End Function

        Public Function Clone() As NormalVector
            Return New NormalVector(Me)
        End Function

        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Return Clone()
        End Function
    End Class
End Namespace