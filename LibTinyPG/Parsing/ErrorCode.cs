using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TinyPG.Parsing
{
	public enum ErrorCode : uint
	{
		NoStart = 0x0024, //(E) "The grammar requires 'Start' to be a production rule."
		VariableCodeBlockNotExists = 0x1016, //(W) "Variable $" + match.Groups["var"].Value + " cannot be matched."
		RegexError = 0x1020, //(E)"Regular expression '" + pattern + "'for '" + name + "' rule results in error: " + ex.Message
		StartTerminal = 0x1021, // "'Start' symbol cannot be a regular expression."
		TerminalAlreadyDefined = 0x1022, //(E) "Terminal already declared: " + terminal.Name
		NonTerminalAlreadyDeclared = 0x1023, //(E) "Non terminal already declared: " + nts.Name
		TinyPGDirectiveNotFirst = 0x1030, //(W) "Directive '" + name + "' should be the first directive declared"
		DirectiveAlreadyDefined = 0x1030, //(W) "Directive '" + name + "' is already defined"
		DirectiveUnallowedForLanguage = 0x1030, //(W) "Directive '" + name + "' is not supported with language " + decl["Language"]
		DirectiveNotSupported = 0x1031, //(W) "Directive '" + name + "' is not supported"
		TemplatePathNotExist = 0x1060, //(E) "Template path '" + value + "' does not exist"
		OutputPathNotExist = 0x1061, //(E) "Output path '" + value + "' does not exist"
		LanguageNotSupported = 0x1062, //(E) "Language '" + value + "' is not supported"
		DirectiveAttributeNotSupported = 0x1034, //(W) "Directive attribute '" + key + "' is not supported"
		DirectiveAttributeAlreadyDefined = 0x1039, //(W) "Attribute already defined for this symbol: " + node.Nodes[1].Token.Text
		DirectiveAttributeNotAloowedForNonTerminal = 0x1035, //(W) "Attribute " + node.Nodes[1].Token.Text + " for non-terminal rule not allowed: " + node.Nodes[1].Token.Text
		DirectiveAttributeInvalidParametersCount = 0x103A, //(W) "Attribute " + node.Nodes[1].Token.Text + " has too many or missing parameters"
		DirectiveAttributeInvalidParametersType = 0x103B, //(W) "Parameter " + node.Nodes[3].Nodes[i * 2].Nodes[0].Token.Text + " is of incorrect type", 
		__DirectiveAttributeNotSupported2 = 0x1036, //(W) "Attribute not supported: " + node.Nodes[1].Token.Text, 
		__DirectiveAttributeInvalidParametersType2 = 0x1037, //(W) "Attribute parameter is not a valid value: " + node.Token.Text
		__DirectiveAttributeInvalidParametersType3 = 0x1038, //(W) "Attribute parameter is not a valid value: " + node.Token.Text
		SymbolUnknown = 0x1040, //(E) "Symbol '" + Nodes[0].Token.Text + "' is not declared. "
		TerminalSymbolReturnTypeDeclared = 0x1040, //(E) "Terminal Symbol '" + Nodes[0].Token.Text + "' cannot declare a return type. "
		TerminalSymbolCodeBlockDeclared = 0x1040, //(E) "Terminal Symbol '" + Nodes[0].Token.Text + "' cannot declare a code block. "
		_SymbolUnknown2 = 0x1041, //"Symbol '" + Nodes[0].Token.Text + "' is not declared. "
		_SymbolUnknown3 = 0x1042, //(E) "Symbol '" + Nodes[0].Token.Text + "' is not declared. ",
		_SymbolUnknown4 = 0x1043, //(E) "Symbol '" + Nodes[0].Token.Text + "' is not declared.", 
		UnexpectedToken = 0x0002, //(E) "Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected STRING or CODEBLOCK."
		_UnexpectedToken2 = 0x1001, //(E) "Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected STRING or CODEBLOCK."
	}
}
