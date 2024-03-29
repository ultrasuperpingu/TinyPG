'Automatically generated from source file: simple expression2_vb.tpg
'By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

Imports System
Imports System.Collections.Generic


Namespace TinyPG
#Region "Parser"

	Partial Public Class Parser 
		Private m_scanner As Scanner
		Private m_tree As ParseTree

		Public Sub New(ByVal scanner As Scanner)
			m_scanner = scanner
		End Sub


		Public Function Parse(ByVal input As String) As ParseTree
			m_tree = New ParseTree()
			Return Parse(input, m_tree)
		End Function

		Public Function Parse(ByVal input As String, ByVal tree As ParseTree) As ParseTree
			m_scanner.Init(input)

			m_tree = tree
			ParseStart(m_tree)
			m_tree.Skipped = m_scanner.Skipped

			Return m_tree
		End Function

		Public Function ParseStart(ByVal input As String, ByVal tree As ParseTree) As ParseTree ' NonTerminalSymbol: Start
			m_scanner.Init(input)
			Me.m_tree = tree
			ParseStart(tree)
			tree.Skipped = m_scanner.Skipped
			Return tree
		End Function

		Private Sub ParseStart(ByVal parent As ParseNode) ' NonTerminalSymbol: Start
			Dim tok As Token
			Dim n As ParseNode
			Dim node As ParseNode = parent.CreateNode(m_scanner.GetToken(TokenType.Start), "Start")
			parent.Nodes.Add(node)


			 ' Concat Rule
			tok = m_scanner.LookAhead(TokenType.NUMBER, TokenType.BROPEN, TokenType.ID, TokenType.EOF) ' Option Rule
			If tok.Type = TokenType.NUMBER Or tok.Type = TokenType.BROPEN Or tok.Type = TokenType.ID Then
				ParseAddExpr(node) ' NonTerminal Rule: AddExpr
			End If

			 ' Concat Rule
			tok = m_scanner.Scan(TokenType.EOF) ' Terminal Rule: EOF
			n = node.CreateNode(tok, tok.ToString() )
			node.Token.UpdateRange(tok)
			node.Nodes.Add(n)
			If tok.Type <> TokenType.EOF Then
				m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'EOF'.", &H1001, tok))
				Return

			End If


			parent.Token.UpdateRange(node.Token)
		End Sub ' NonTerminalSymbol: Start

		Public Function ParseAddExpr(ByVal input As String, ByVal tree As ParseTree) As ParseTree ' NonTerminalSymbol: AddExpr
			m_scanner.Init(input)
			Me.m_tree = tree
			ParseAddExpr(tree)
			tree.Skipped = m_scanner.Skipped
			Return tree
		End Function

		Private Sub ParseAddExpr(ByVal parent As ParseNode) ' NonTerminalSymbol: AddExpr
			Dim tok As Token
			Dim n As ParseNode
			Dim node As ParseNode = parent.CreateNode(m_scanner.GetToken(TokenType.AddExpr), "AddExpr")
			parent.Nodes.Add(node)


			 ' Concat Rule
			ParseMultExpr(node) ' NonTerminal Rule: MultExpr

			 ' Concat Rule
			tok = m_scanner.LookAhead(TokenType.PLUSMINUS) ' ZeroOrMore Rule
			While tok.Type = TokenType.PLUSMINUS

				 ' Concat Rule
				tok = m_scanner.Scan(TokenType.PLUSMINUS) ' Terminal Rule: PLUSMINUS
				n = node.CreateNode(tok, tok.ToString() )
				node.Token.UpdateRange(tok)
				node.Nodes.Add(n)
				If tok.Type <> TokenType.PLUSMINUS Then
					m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'PLUSMINUS'.", &H1001, tok))
					Return

				End If


				 ' Concat Rule
				ParseMultExpr(node) ' NonTerminal Rule: MultExpr
			tok = m_scanner.LookAhead(TokenType.PLUSMINUS) ' ZeroOrMore Rule
			End While

			parent.Token.UpdateRange(node.Token)
		End Sub ' NonTerminalSymbol: AddExpr

		Public Function ParseMultExpr(ByVal input As String, ByVal tree As ParseTree) As ParseTree ' NonTerminalSymbol: MultExpr
			m_scanner.Init(input)
			Me.m_tree = tree
			ParseMultExpr(tree)
			tree.Skipped = m_scanner.Skipped
			Return tree
		End Function

		Private Sub ParseMultExpr(ByVal parent As ParseNode) ' NonTerminalSymbol: MultExpr
			Dim tok As Token
			Dim n As ParseNode
			Dim node As ParseNode = parent.CreateNode(m_scanner.GetToken(TokenType.MultExpr), "MultExpr")
			parent.Nodes.Add(node)


			 ' Concat Rule
			ParseAtom(node) ' NonTerminal Rule: Atom

			 ' Concat Rule
			tok = m_scanner.LookAhead(TokenType.MULTDIV) ' ZeroOrMore Rule
			While tok.Type = TokenType.MULTDIV

				 ' Concat Rule
				tok = m_scanner.Scan(TokenType.MULTDIV) ' Terminal Rule: MULTDIV
				n = node.CreateNode(tok, tok.ToString() )
				node.Token.UpdateRange(tok)
				node.Nodes.Add(n)
				If tok.Type <> TokenType.MULTDIV Then
					m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'MULTDIV'.", &H1001, tok))
					Return

				End If


				 ' Concat Rule
				ParseAtom(node) ' NonTerminal Rule: Atom
			tok = m_scanner.LookAhead(TokenType.MULTDIV) ' ZeroOrMore Rule
			End While

			parent.Token.UpdateRange(node.Token)
		End Sub ' NonTerminalSymbol: MultExpr

		Public Function ParseAtom(ByVal input As String, ByVal tree As ParseTree) As ParseTree ' NonTerminalSymbol: Atom
			m_scanner.Init(input)
			Me.m_tree = tree
			ParseAtom(tree)
			tree.Skipped = m_scanner.Skipped
			Return tree
		End Function

		Private Sub ParseAtom(ByVal parent As ParseNode) ' NonTerminalSymbol: Atom
			Dim tok As Token
			Dim n As ParseNode
			Dim node As ParseNode = parent.CreateNode(m_scanner.GetToken(TokenType.Atom), "Atom")
			parent.Nodes.Add(node)

			tok = m_scanner.LookAhead(TokenType.NUMBER, TokenType.BROPEN, TokenType.ID)
			Select Case tok.Type
			 ' Choice Rule
				Case TokenType.NUMBER
					tok = m_scanner.Scan(TokenType.NUMBER) ' Terminal Rule: NUMBER
					n = node.CreateNode(tok, tok.ToString() )
					node.Token.UpdateRange(tok)
					node.Nodes.Add(n)
					If tok.Type <> TokenType.NUMBER Then
						m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'NUMBER'.", &H1001, tok))
						Return

					End If

				Case TokenType.BROPEN

					 ' Concat Rule
					tok = m_scanner.Scan(TokenType.BROPEN) ' Terminal Rule: BROPEN
					n = node.CreateNode(tok, tok.ToString() )
					node.Token.UpdateRange(tok)
					node.Nodes.Add(n)
					If tok.Type <> TokenType.BROPEN Then
						m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'BROPEN'.", &H1001, tok))
						Return

					End If


					 ' Concat Rule
					ParseAddExpr(node) ' NonTerminal Rule: AddExpr

					 ' Concat Rule
					tok = m_scanner.Scan(TokenType.BRCLOSE) ' Terminal Rule: BRCLOSE
					n = node.CreateNode(tok, tok.ToString() )
					node.Token.UpdateRange(tok)
					node.Nodes.Add(n)
					If tok.Type <> TokenType.BRCLOSE Then
						m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'BRCLOSE'.", &H1001, tok))
						Return

					End If

				Case TokenType.ID
					tok = m_scanner.Scan(TokenType.ID) ' Terminal Rule: ID
					n = node.CreateNode(tok, tok.ToString() )
					node.Token.UpdateRange(tok)
					node.Nodes.Add(n)
					If tok.Type <> TokenType.ID Then
						m_tree.Errors.Add(New ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ID'.", &H1001, tok))
						Return

					End If

				Case Else
					m_tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.  Expected NUMBER, BROPEN, or ID.", &H0002, tok))
					Exit Select
			End Select ' Choice Rule

			parent.Token.UpdateRange(node.Token)
		End Sub ' NonTerminalSymbol: Atom



	End Class
#End Region
End Namespace

