parser grammar ArcSourceCodeParser;

options {
	tokenVocab = ArcSourceCodeLexer;
}

compilation_unit: stmt_link* function_block* EOF;

qualified_identifier:	IDENTIFIER (SYMBOL_DOT IDENTIFIER)*;
scoped_identifier:		IDENTIFIER (OPERATOR_SCOPE IDENTIFIER)*;
single_identifier:		IDENTIFIER;
stmt_link:				KEYWORD_LINK scoped_identifier SYMBOL_SEMICOLON;

annotation: SYMBOL_AT qualified_identifier call_params?;

accessibility:
	KEYWORD_PUBLIC
	| KEYWORD_INTERNAL
	| KEYWORD_PROTECTED
	| KEYWORD_PRIVATE;

reassignability: KEYWORD_CONST | KEYWORD_VAR;

param_type: KEYWORD_REF | KEYWORD_VAL;

function_block: function_declarator wrapped_body;

function_declarator:
	annotation* accessibility KEYWORD_FUNCTION single_identifier CONTAINER_OPEN_PAREN
		function_args? CONTAINER_CLOSE_PAREN SYMBOL_COLON data_type;

function_args:
	data_declaration (OPERATOR_COMMA data_declaration)*;

wrapped_body:
	CONTAINER_OPEN_BRACE (statement | block)* CONTAINER_CLOSE_BRACE;

statement: (
		call_stmt
		| return_stmt
		| assignment_stmt
		| declaration_stmt
		| break_stmt
		| continue_stmt
	) SYMBOL_SEMICOLON;

call_stmt:			KEYWORD_CALL function_call;
return_stmt:		KEYWORD_RETURN expression;
assignment_stmt:	single_identifier OPERATOR_ASSIGN expression;
declaration_stmt:	data_declaration;
break_stmt:			KEYWORD_BREAK;
continue_stmt:		KEYWORD_CONTINUE;

block:
	conditional_exec_block
	| conditional_loop_block
	| loop_block;

conditional_block:
	CONTAINER_OPEN_PAREN expression CONTAINER_CLOSE_PAREN wrapped_body;

if_block:
	KEYWORD_IF CONTAINER_OPEN_PAREN expression CONTAINER_CLOSE_PAREN wrapped_body;
elif_block:	KEYWORD_ELSE_IF conditional_block;
else_block:	KEYWORD_ELSE wrapped_body;

conditional_exec_block:	if_block elif_block* else_block?;
conditional_loop_block:	KEYWORD_WHILE conditional_block;
loop_block:				KEYWORD_LOOP wrapped_body;

data_declaration:
	param_type reassignability single_identifier SYMBOL_COLON data_type;

interpolation_string:	OPERATOR_DOLLAR LITERAL_STRING;
string_values:			LITERAL_STRING | interpolation_string;

instant_value: NUMBER | string_values | KEYWORD_NONE;

value: instant_value | single_identifier | function_call;

call_params:
	CONTAINER_OPEN_PAREN (
		expression (OPERATOR_COMMA expression)*
	)? CONTAINER_CLOSE_PAREN;

function_call: scoped_identifier call_params;

data_type: singleton_data_type | array_data_type;

primitive_data_type:
	KEYWORD_NUMBER
	| KEYWORD_CHAR
	| KEYWORD_STRING
	| KEYWORD_BOOL
	| KEYWORD_NONE
	| KEYWORD_ANY
	| KEYWORD_AUTO;

singleton_data_type: primitive_data_type | single_identifier;

array_data_type:
	singleton_data_type CONTAINER_OPEN_INDEXER CONTAINER_CLOSE_INDEXER;

expression:
	value
	| expression OPERATOR_PLUS expression
	| expression OPERATOR_MINUS expression
	| expression OPERATOR_MULTIPLY expression
	| expression OPERATOR_DIVIDE expression
	| expression OPERATOR_MODULO expression
	| expression OPERATOR_AND expression
	| expression OPERATOR_OR expression
	| OPERATOR_NOT value
	| value OPERATOR_QUESTION_MARK
	| expression OPERATOR_REF_EQUALS expression
	| expression OPERATOR_NOT_EQUALS expression
	| expression OPERATOR_LESS_THAN_EQUALS expression
	| expression OPERATOR_GREATER_THAN_EQUALS expression
	// < LESS_THAN operator
	| expression CONTAINER_OPEN_SHARP_PAREN expression
	// > MORE_THAN operator
	| expression CONTAINER_CLOSE_SHARP_PAREN expression
	| CONTAINER_OPEN_PAREN expression CONTAINER_CLOSE_PAREN;

