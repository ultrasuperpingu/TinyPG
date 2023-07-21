'Automatically generated from source file: simple expression2_vb.tpg
'By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml.Serialization


Namespace TinyPG
#Region "ParseTree"
	<Serializable()> _
	Public Class ParseErrors
		Inherits List(Of ParseError)

		Public Sub New()

		End Sub
	End Class

	<Serializable()> _
	Public Class ParseError 
		Private m_message As String
		Private m_code As Integer
		Private m_line As Integer
		Private m_col As Integer
		Private m_pos As Integer
		Private m_length As Integer
		Private m_isWarning As Boolean

		Public ReadOnly Property Code() As Integer
			Get
				Return m_code
			End Get
		End Property

		Public ReadOnly Property Line() As Integer
			Get
				Return m_line
			End Get
		End Property

		Public ReadOnly Property Column() As Integer
			Get
				Return m_col
			End Get
		End Property

		Public ReadOnly Property Position() As Integer
			Get
				Return m_pos
			End Get
		End Property

		Public ReadOnly Property Length() As Integer
			Get
				Return m_length
			End Get
		End Property

		Public ReadOnly Property Message() As String
			Get
				Return m_message
			End Get
		End Property

		Public ReadOnly Property IsWarning() As Boolean
			Get
				Return m_isWarning
			End Get
		End Property

		Public Sub New(ByVal message As String, ByVal code As Integer, ByVal tok As Token)
			Me.New(message, code, 0, tok.StartPos, tok.StartPos, tok.Length, False)
		End Sub

		Public Sub New(ByVal message As String, ByVal code As Integer, ByVal tok As Token, ByVal isWarning As Boolean)
			Me.New(message, code, 0, tok.StartPos, tok.StartPos, tok.Length, isWarning)
		End Sub

		Public Sub New(ByVal message As String, ByVal code As Integer, ByVal line As Integer, ByVal col As Integer, ByVal pos As Integer, ByVal length As Integer, ByVal isWarning As Boolean)
			m_message = message
			m_code = code
			m_line = line
			m_col = col
			m_pos = pos
			m_length = length
			m_isWarning = isWarning
		End Sub
	End Class

	' rootlevel of the node tree
	<Serializable()> _
	Partial Public Class ParseTree
		Inherits ParseNode

		Public Errors As ParseErrors

		Public Skipped As List(Of Token)

		Public Sub New()
			MyBase.New(New Token(), "ParseTree")
			Token.Type = TokenType.Start
			Token.Text = "Root"
			Skipped = New List(Of Token)()
			Errors = New ParseErrors()
		End Sub

		Public Function PrintTree() As String
	Dim sb As New StringBuilder()
			Dim indent As Integer = 0
			PrintNode(sb, Me, indent)
			Return sb.ToString()
		End Function

		Private Sub PrintNode(ByVal sb As StringBuilder, ByVal node As ParseNode, ByVal indent As Integer)

			Dim space As String = "".PadLeft(indent, " "c)

			sb.Append(space)
			sb.AppendLine(node.Text)

			For Each n As ParseNode In node.Nodes
				PrintNode(sb, n, indent + 2)
			Next
		End Sub

		''' <summary>
		''' this is the entry point for executing and evaluating the parse tree.
		''' </summary>
		''' <param name="paramlist">additional optional input parameters</param>
		''' <returns>the output of the evaluation function</returns>
		Public Overloads Function Eval(ByVal ParamArray paramlist As Object()) As Object
		Return Nodes(0).Eval(paramlist)
		End Function
	End Class
#End Region

#Region "ParseNode"
	<Serializable()> _
	<XmlInclude(GetType(ParseTree))> _
	Partial Public Class ParseNode 
		Protected m_text As String
		Protected m_nodes As List(Of ParseNode)
		

		Public ReadOnly Property Nodes() As List(Of ParseNode)
			Get
				Return m_nodes
			End Get
		End Property

		
		<XMLIgnore()> _
		Public Parent As ParseNode
		Public Token As Token
		' the token/rule
		<XmlIgnore()> _
		Public Property Text() As String
			' text to display in parse tree 
			Get
				Return m_text
			End Get
			Set(ByVal value As String)
				m_text = value
			End Set
		End Property

		Public Overridable Function CreateNode(ByVal token As Token, ByVal text As String) As ParseNode
			Dim node As New ParseNode(token, text)
			node.Parent = Me
			Return node
		End Function

		Protected Sub New(ByVal token As Token, ByVal text As String)
			Me.Token = token
			m_text = text
			m_nodes = New List(Of ParseNode)()
		End Sub

		Protected Function GetTokenNode(ByVal type As TokenType, ByVal index As Integer) As ParseNode
			If index < 0 Then
				Return Nothing
			End If

			' left to right
			For Each node As ParseNode In Nodes
				If node.Token.Type = type Then
					System.Math.Max(System.Threading.Interlocked.Decrement(index), index + 1)
					If index < 0 Then
						Return node
					End If
				End If
			Next
			Return Nothing
		End Function

		Protected Function IsTokenPresent(ByVal type As TokenType, ByVal index As Integer) As Boolean
			Dim node As ParseNode = GetTokenNode(type, index)
			Return node IsNot Nothing
		End Function

		Protected Function GetTerminalValue(ByVal type As TokenType, ByVal index As Integer) As String
			Dim node As ParseNode = GetTokenNode(type, index)
			If node IsNot Nothing
				Return node.Token.Text
			End If
			Return Nothing
		End Function

		Protected Function GetValue(ByVal type As TokenType, ByVal index As Integer, ByVal ParamArray paramlist As Object()) As Object
			Return GetValueByRef(type, index, paramlist)
		End Function

		Protected Function GetValueByRef(ByVal type As TokenType, ByRef index As Integer, ByVal ParamArray paramlist As Object()) As Object
			Dim o As Object = Nothing
			If index < 0 Then
				Return o
			End If

			' left to right
			For Each node As ParseNode In Nodes
				If node.Token.Type = type Then
					System.Math.Max(System.Threading.Interlocked.Decrement(index), index + 1)
					If index < 0 Then
						o = node.Eval(paramlist)
						Exit For
					End If
				End If
			Next
			Return o
		End Function

		''' <summary>
		''' this implements the evaluation functionality, cannot be used directly
		''' </summary>
		''' <param name="tree">the parsetree itself</param>
		''' <param name="paramlist">optional input parameters</param>
		''' <returns>a partial result of the evaluation</returns>
		Friend Function Eval(ByVal ParamArray paramlist As Object()) As Object
			Dim Value As Object = Nothing

			Select Case Token.Type
				Case TokenType.Start
					Value = EvalStart(paramlist)
					Exit Select
				Case TokenType.AddExpr
					Value = EvalAddExpr(paramlist)
					Exit Select
				Case TokenType.MultExpr
					Value = EvalMultExpr(paramlist)
					Exit Select
				Case TokenType.Atom
					Value = EvalAtom(paramlist)
					Exit Select

				Case Else
					Value = Token.Text
					Exit Select
			End Select
			Return Value
		End Function

				Protected Overridable Function EvalStart(ByVal ParamArray paramlist As Object()) As Object
			Return Me.GetAddExprValue(0, paramlist)
		End Function

		Protected Overridable Function GetStartValue(index As Integer, ByVal ParamArray paramlist As Object()) As Object
			Dim o As Object = Nothing
			Dim node As ParseNode = GetTokenNode(TokenType.Start, index)
			If node IsNot Nothing
				o = node.EvalStart(paramlist)
			End If
			Return o
		End Function

		Protected Overridable Function EvalAddExpr(ByVal ParamArray paramlist As Object()) As Double
			Dim Value As Double = Convert.ToDouble(Me.GetMultExprValue(0, paramlist))
			Dim i As Double = 1
			While Me.IsTokenPresent(TokenType.MultExpr, i)
				Dim sign As String = Me.GetTerminalValue(TokenType.PLUSMINUS, i-1).ToString()
				If sign = "+" Then
					Value += Convert.ToDouble(Me.GetMultExprValue(i, paramlist))
				Else 
					Value -= Convert.ToDouble(Me.GetMultExprValue(i, paramlist))
				End If
				i=i+1
			End While
			Return Value
		End Function

		Protected Overridable Function GetAddExprValue(index As Integer, ByVal ParamArray paramlist As Object()) As Double
			Dim o As Double = Nothing
			Dim node As ParseNode = GetTokenNode(TokenType.AddExpr, index)
			If node IsNot Nothing
				o = node.EvalAddExpr(paramlist)
			End If
			Return o
		End Function

		Protected Overridable Function EvalMultExpr(ByVal ParamArray paramlist As Object()) As Double
			Dim Value As Double = Convert.ToDouble(Me.GetAtomValue(0, paramlist))
			Dim i As Double = 1
			While Me.IsTokenPresent(TokenType.Atom, i)
				Dim sign As String = Me.GetTerminalValue(TokenType.MULTDIV, i-1).ToString()
				If sign = "*" Then
					Value *= Convert.ToDouble(Me.GetAtomValue(i, paramlist))
				Else 
					Value /= Convert.ToDouble(Me.GetAtomValue(i, paramlist))
				End If
				i=i+1
			End While
		
			Return Value
		End Function

		Protected Overridable Function GetMultExprValue(index As Integer, ByVal ParamArray paramlist As Object()) As Double
			Dim o As Double = Nothing
			Dim node As ParseNode = GetTokenNode(TokenType.MultExpr, index)
			If node IsNot Nothing
				o = node.EvalMultExpr(paramlist)
			End If
			Return o
		End Function

		Protected Overridable Function EvalAtom(ByVal ParamArray paramlist As Object()) As Double
			If Me.IsTokenPresent(TokenType.NUMBER, 0) Then
				Return Me.GetTerminalValue(TokenType.NUMBER, 0)
			ElseIf Me.IsTokenPresent(TokenType.ID, 0)
				Return Me.GetVarValue(Me.GetTerminalValue(TokenType.ID, 0))
			Else 
				Return Me.GetAddExprValue(0, paramlist)
			End If
		End Function

		Protected Overridable Function GetAtomValue(index As Integer, ByVal ParamArray paramlist As Object()) As Double
			Dim o As Double = Nothing
			Dim node As ParseNode = GetTokenNode(TokenType.Atom, index)
			If node IsNot Nothing
				o = node.EvalAtom(paramlist)
			End If
			Return o
		End Function




		Protected _context as System.Collections.Generic.Dictionary(Of String, Integer)
		Public Property Context as System.Collections.Generic.Dictionary(Of String, Integer)
			Get
				If _context Is Nothing and Parent IsNot Nothing
					return Parent.Context
				End If
				return _context
			End Get
			Set
				_context = value
			End Set
		End Property

		Public Function GetVarValue(id as String) As Integer
			If Context Is Nothing
				Return 0
			Else
				Return Context.Item(id)
			End If
		End Function

	End Class
#End Region
End Namespace

