// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>

namespace <%Namespace%>
{
	class ParseTree;
	class ParseNode<%IParseNode%>
	{
		protected:
		<%ITokenGet%>
		public:
		std::vector<ParseNode*> Nodes;
		<%INodesGet%>
		//[XmlIgnore] // avoid circular references when serializing
		ParseNode* Parent;
		Token TokenVal; // the token/rule

		std::string Text;

		inline virtual ParseNode* CreateNode(Token token, std::string text)
		{
			ParseNode* node = new ParseNode(token, text);
			node->Parent = this;
			return node;
		}

		protected:
		inline ParseNode(Token token, const std::string& text)
		{
			this->TokenVal = token;
			this->Text = text;
			//this->nodes = new List<ParseNode>();
		}

		inline virtual ~ParseNode()
		{
			for (ParseNode* node : Nodes)
			{
				delete node;
			}
		}

		inline virtual ParseNode* GetTokenNode(TokenType type, int index)
		{
			if (index < 0)
				return NULL;
			// left to right
			for (ParseNode* node : Nodes)
			{
				if (node->TokenVal.Type == type)
				{
					index--;
					if (index < 0)
					{
						return node;
					}
				}
			}
			return NULL;
		}
		inline virtual bool IsTokenPresent(TokenType type, int index)
		{
			ParseNode* node = GetTokenNode(type, index);
			return node != NULL;
		}
		
		inline virtual std::string GetTerminalValue(TokenType type, int index)
		{
			ParseNode* node = GetTokenNode(type, index);
			if (node != NULL)
				return node->TokenVal.Text;
			return "";
		}

		/*inline void* GetValue(const ParseTree& tree, TokenType type, int index)
		{
			GetValueRefIndex(tree, type, index);
		}

		inline void* GetValueRefIndex(const ParseTree& tree, TokenType type, int& index)
		{
			void* o = NULL;
			if (index < 0) return o;

			// left to right
			for (ParseNode* node : Nodes)
			{
				if (node->TokenVal.Type == type)
				{
					index--;
					if (index < 0)
					{
						o = node->Eval(tree, {});
						break;
					}
				}
			}
			return o;
		}*/

		public:
		/// <summary>
		/// this implements the evaluation functionality, cannot be used directly
		/// </summary>
		/// <param name="tree">the parsetree itself</param>
		/// <param name="paramlist">optional input parameters</param>
		/// <returns>a partial result of the evaluation</returns>
		/*inline void* Eval(const ParseTree& tree, std::vector<void*> paramlist)
		{
			void* Value = NULL;

			switch (TokenVal.Type)
			{
<%EvalSymbols%>
				default:
					Value = &TokenVal.Text;
					break;
			}
			return Value;
		}*/

<%VirtualEvalMethods%>

<%ParseTreeCustomCode%>
	};
	
	class ParseError<%ParseError%>
	{
		public:
		std::string File;
		std::string Message;
		int Code;
		int Line;
		int Column;
		int Position;
		int Length;

		
		// just for the sake of serialization
		inline ParseError() : ParseError("", 0, "", 0, 0, 0, 0)
		{
		}

		inline ParseError(std::string message, int code, ParseNode& node) : ParseError(message, code, node.TokenVal)
		{
		}

		inline ParseError(std::string message, int code, Token token) : ParseError(message, code, token.File, token.Line, token.Column, token.StartPos, token.Length)
		{
		}

		inline ParseError(std::string message, int code) : ParseError(message, code, "", 0, 0, 0, 0)
		{
		}

		inline ParseError(std::string message, int code, std::string file, int line, int col, int pos, int length)
		{
			this->File = file;
			this->Message = message;
			this->Code = code;
			this->Line = line;
			this->Column = col;
			this->Position = pos;
			this->Length = length;
		}
	};
	
	class ParseErrors : public <%ParseErrors%>
	{
	};

	
	// rootlevel of the node tree
	class ParseTree : public ParseNode<%IParseTree%>
	{
	public:
		ParseErrors Errors;

		std::vector<Token> Skipped;

		inline ParseTree() : ParseNode<%IParseTree%>(Token(), "ParseTree")
		{
			TokenVal.Type = TokenType::Start;
			TokenVal.Text = "Root";
			//Errors = new ParseErrors();
		}
		
		virtual inline ~ParseTree()
		{
			// Cleanup Children done in parent class
		}

		inline std::string PrintTree()
		{
			std::string sb;
			int indent = 0;
			PrintNode(sb, this, indent);
			return sb;
		}

		inline void PrintNode(std::string& sb, ParseNode* node, int indent)
		{

			std::string space = "";
			space.insert(0, indent, ' ');

			sb += space;
			sb += node->Text + "\n";

			for (ParseNode* n : node->Nodes)
				PrintNode(sb, n, indent + 2);
		}

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		// TODO: template the class and/or use std::any
		/*inline void* Eval(std::vector<void*> paramlist)
		{
			return Nodes[0]->EvalStart(*this, paramlist);
		}*/
	};
 
}
