Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.VisualBasic.Serialization.JSON.Formatter.Internals.Strategies

Namespace Serialization.JSON.Formatter.Internals

    Friend NotInheritable Class JsonFormatterStrategyContext
        Private Const Space As String = " "
        Private Const SpacesPerIndent As Integer = 4

        Private m_indent As String = String.Empty

        Private currentCharacter As Char
        Private previousChar As Char

        Private outputBuilder As StringBuilder

        Private ReadOnly scopeState As New FormatterScopeState()
        Private ReadOnly strategies As IDictionary(Of Char, ICharacterStrategy) = New Dictionary(Of Char, ICharacterStrategy)()


        Public ReadOnly Property Indent() As String
            Get
                If Me.m_indent = String.Empty Then
                    Me.InitializeIndent()
                End If

                Return Me.m_indent
            End Get
        End Property

        Private Sub InitializeIndent()
            For i As Integer = 0 To SpacesPerIndent - 1
                Me.m_indent += Space
            Next
        End Sub


        Public ReadOnly Property IsInArrayScope() As Boolean
            Get
                Return Me.scopeState.IsTopTypeArray
            End Get
        End Property

        Private Sub AppendIndents(indents As Integer)
            For i As Integer = 0 To indents - 1
                Me.outputBuilder.Append(Indent)
            Next
        End Sub

        Public IsProcessingVariableAssignment As Boolean
        Public Property IsProcessingDoubleQuoteInitiatedString() As Boolean
        Public Property IsProcessingSingleQuoteInitiatedString() As Boolean

        Public ReadOnly Property IsProcessingString() As Boolean
            Get
                Return Me.IsProcessingDoubleQuoteInitiatedString OrElse Me.IsProcessingSingleQuoteInitiatedString
            End Get
        End Property

        Public ReadOnly Property IsStart() As Boolean
            Get
                Return Me.outputBuilder.Length = 0
            End Get
        End Property

        Public ReadOnly Property WasLastCharacterABackSlash() As Boolean
            Get
                Return Me.previousChar = "\"c
            End Get
        End Property

        Public Sub PrettyPrintCharacter(curChar As Char, output As StringBuilder)
            Me.currentCharacter = curChar

            Dim strategy As ICharacterStrategy = If(Me.strategies.ContainsKey(curChar), strategies(curChar), New DefaultCharacterStrategy())

            Me.outputBuilder = output

            strategy.Execute(Me)

            Me.previousChar = curChar
        End Sub

        Public Sub AppendCurrentChar()
            Me.outputBuilder.Append(Me.currentCharacter)
        End Sub

        Public Sub AppendNewLine()
            Me.outputBuilder.Append(Environment.NewLine)
        End Sub

        Public Sub BuildContextIndents()
            Me.AppendNewLine()
            Me.AppendIndents(Me.scopeState.ScopeDepth)
        End Sub

        Public Sub EnterObjectScope()
            Me.scopeState.PushObjectContextOntoStack()
        End Sub

        Public Sub CloseCurrentScope()
            Me.scopeState.PopJsonType()
        End Sub

        Public Sub EnterArrayScope()
            Me.scopeState.PushJsonArrayType()
        End Sub

        Public Sub AppendSpace()
            Me.outputBuilder.Append(Space)
        End Sub

        Public Sub ClearStrategies()
            Me.strategies.Clear()
        End Sub

        Public Sub AddCharacterStrategy(strategy As ICharacterStrategy)
            Me.strategies(strategy.ForWhichCharacter) = strategy
        End Sub
    End Class
End Namespace
