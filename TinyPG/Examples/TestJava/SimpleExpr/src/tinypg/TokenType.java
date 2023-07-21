// Automatically generated from source file: simple expression2_java.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

package tinypg;


public enum TokenType
{

	//Non terminal tokens:
	_NONE_         ,
	_UNDETERMINED_ ,

	//Non terminal tokens:
	Start          ,
	AddExpr        ,
	MultExpr       ,
	Atom           ,

	//Terminal tokens:
	EOF            ,
	NUMBER         ,
	ID             ,
	PLUSMINUS      ,
	MULTDIV        ,
	BROPEN         ,
	BRCLOSE        ,
	WHITESPACE     
}
