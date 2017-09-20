Public Class Tree(Of T) : Inherits Vertex

    Public Property Childs As Tree(Of T)()
    Public Property Parent As Tree(Of T)
    Public Property Data As T

    Public ReadOnly Property Count As Integer
        Get
            If Childs.IsNullOrEmpty Then
                Return 1  ' �Լ���һ���ڵ㣬������������1��
            Else
                Dim n% = Childs.Length

                For Each node In Childs
                    n += node.Count ' ����ڵ�û��childs����᷵��1����Ϊ���������һ���ڵ�
                Next

                Return n
            End If
        End Get
    End Property

    Public ReadOnly Property QualifyName As String
        Get
            If Not Parent Is Nothing Then
                Return Parent.QualifyName & "." & Label
            Else
                Return Label
            End If
        End Get
    End Property

    Public ReadOnly Property IsRoot As Boolean
        Get
            Return Parent Is Nothing
        End Get
    End Property

    Public ReadOnly Property IsLeaf As Boolean
        Get
            Return Childs.IsNullOrEmpty
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return QualifyName
    End Function
End Class