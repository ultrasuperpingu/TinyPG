/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package TinyExe;


public enum TokenType
{

	//Non terminal tokens:
	_NONE_         ,
	_UNDETERMINED_ ,

	//Non terminal tokens:
	Start          ,
	Function       ,
	PrimaryExpression,
	ParenthesizedExpression,
	UnaryExpression,
	PowerExpression,
	MultiplicativeExpression,
	AdditiveExpression,
	ConcatEpression,
	RelationalExpression,
	EqualityExpression,
	ConditionalAndExpression,
	ConditionalOrExpression,
	AssignmentExpression,
	Expression     ,
	Params         ,
	Literal        ,
	IntegerLiteral ,
	RealLiteral    ,
	StringLiteral  ,
	Variable       ,

	//Terminal tokens:
	BOOLEANLITERAL ,
	DECIMALINTEGERLITERAL,
	REALLITERAL    ,
	HEXINTEGERLITERAL,
	STRINGLITERAL  ,
	FUNCTION       ,
	VARIABLE       ,
	CONSTANT       ,
	BRACEOPEN      ,
	BRACECLOSE     ,
	BRACKETOPEN    ,
	BRACKETCLOSE   ,
	SEMICOLON      ,
	PLUSPLUS       ,
	MINUSMINUS     ,
	PIPEPIPE       ,
	AMPAMP         ,
	AMP            ,
	POWER          ,
	PLUS           ,
	MINUS          ,
	EQUAL          ,
	ASSIGN         ,
	NOTEQUAL       ,
	NOT            ,
	ASTERIKS       ,
	SLASH          ,
	PERCENT        ,
	QUESTIONMARK   ,
	COMMA          ,
	LESSEQUAL      ,
	GREATEREQUAL   ,
	LESSTHAN       ,
	GREATERTHAN    ,
	COLON          ,
	EOF_           ,
	WHITESPACE     
}


