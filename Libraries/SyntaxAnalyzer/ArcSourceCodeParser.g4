parser grammar ArcSourceCodeParser;

options {
	tokenVocab = ArcSourceCodeLexer;
}

arc_compilation_unit: arc_stmt_link* arc_namespace EOF;

arc_qualified_identifier:	IDENTIFIER (SYMBOL_DOT IDENTIFIER)*;
arc_scoped_identifier:		IDENTIFIER (OPERATOR_SCOPE IDENTIFIER)*;
arc_single_identifier:		IDENTIFIER;
arc_stmt_link:				KEYWORD_LINK arc_scoped_identifier SYMBOL_SEMICOLON;

arc_annotation: SYMBOL_AT arc_scoped_identifier arc_call_args?;

arc_accessibility:
	KEYWORD_PUBLIC
	| KEYWORD_INTERNAL
	| KEYWORD_PROTECTED
	| KEYWORD_PRIVATE;

arc_reassignability: KEYWORD_CONST | KEYWORD_VAR;

arc_param_type: KEYWORD_REF | KEYWORD_VAL;

arc_function_block: arc_function_declarator arc_wrapped_body;

arc_function_declarator:
	arc_annotation* arc_accessibility KEYWORD_FUNCTION arc_single_identifier CONTAINER_OPEN_PAREN arc_function_params?
		CONTAINER_CLOSE_PAREN SYMBOL_COLON arc_data_type;

arc_function_params:
	arc_data_declaration (OPERATOR_COMMA arc_data_declaration)*;

arc_wrapped_body:
	CONTAINER_OPEN_BRACE arc_exec_step* CONTAINER_CLOSE_BRACE;

arc_exec_step:
	arc_statement | arc_block;

arc_statement: (
		arc_call_stmt
		| arc_return_stmt
		| arc_assignment_stmt
		| arc_declaration_stmt
		| arc_break_stmt
		| arc_continue_stmt
	) SYMBOL_SEMICOLON;

arc_call_stmt:			KEYWORD_CALL arc_function_call;
arc_return_stmt:		KEYWORD_RETURN arc_expression;
arc_assignment_stmt:	arc_single_identifier OPERATOR_ASSIGN arc_expression;
arc_declaration_stmt:	arc_data_declaration;
arc_break_stmt:			KEYWORD_BREAK;
arc_continue_stmt:		KEYWORD_CONTINUE;

arc_block:
	arc_conditional_exec_block
	| arc_conditional_loop_block
	| arc_loop_block;

arc_conditional_block:
	CONTAINER_OPEN_PAREN arc_expression CONTAINER_CLOSE_PAREN arc_wrapped_body;

arc_if_block:
	KEYWORD_IF arc_conditional_block;
arc_elif_block:	KEYWORD_ELSE_IF arc_conditional_block;
arc_else_block:	KEYWORD_ELSE arc_wrapped_body;

arc_conditional_exec_block:	arc_if_block arc_elif_block* arc_else_block?;
arc_conditional_loop_block:	KEYWORD_WHILE arc_conditional_block;
arc_loop_block:				KEYWORD_LOOP arc_wrapped_body;

arc_data_declaration:
	arc_param_type arc_reassignability arc_single_identifier SYMBOL_COLON arc_data_type;

arc_interpolation_string:	OPERATOR_DOLLAR LITERAL_STRING;
arc_string_values:			LITERAL_STRING | arc_interpolation_string;
arc_bool_values: KEYWORD_TRUE | KEYWORD_FALSE;

arc_instant_value: NUMBER | arc_string_values | arc_bool_values | KEYWORD_NONE;

arc_value: arc_instant_value | arc_single_identifier | arc_function_call;

arc_call_args:
	CONTAINER_OPEN_PAREN (
		arc_expression (OPERATOR_COMMA arc_expression)*
	)? CONTAINER_CLOSE_PAREN;

arc_function_call: arc_scoped_identifier arc_call_args;

arc_data_type: (arc_primitive_data_type | arc_derivative_data_type) arc_array_data_flag?;

arc_primitive_data_type:
	KEYWORD_NUMBER
	| KEYWORD_CHAR
	| KEYWORD_STRING
	| KEYWORD_BOOL
	| KEYWORD_NONE
	| KEYWORD_ANY
	| KEYWORD_AUTO;

arc_derivative_data_type:
	arc_scoped_identifier;

arc_array_data_flag:
	CONTAINER_OPEN_INDEXER CONTAINER_CLOSE_INDEXER;

arc_expression:
	arc_value
	| arc_expression OPERATOR_PLUS arc_expression
	| arc_expression OPERATOR_MINUS arc_expression
	| arc_expression OPERATOR_MULTIPLY arc_expression
	| arc_expression OPERATOR_DIVIDE arc_expression
	| arc_expression OPERATOR_MODULO arc_expression
	| arc_expression OPERATOR_AND arc_expression
	| arc_expression OPERATOR_OR arc_expression
	| OPERATOR_NOT arc_value
	| arc_value OPERATOR_QUESTION_MARK
	| arc_expression OPERATOR_REF_EQUALS arc_expression
	| arc_expression OPERATOR_NOT_EQUALS arc_expression
	| arc_expression OPERATOR_LESS_THAN_EQUALS arc_expression
	| arc_expression OPERATOR_GREATER_THAN_EQUALS arc_expression
	// < LESS_THAN operator
	| arc_expression CONTAINER_OPEN_SHARP_PAREN arc_expression
	// > MORE_THAN operator
	| arc_expression CONTAINER_CLOSE_SHARP_PAREN arc_expression
	| CONTAINER_OPEN_PAREN arc_expression CONTAINER_CLOSE_PAREN;

arc_group_block:
	arc_annotation* arc_accessibility KEYWORD_GROUP arc_single_identifier CONTAINER_OPEN_BRACE arc_group_member*
		CONTAINER_CLOSE_BRACE;

arc_group_member: arc_group_field | arc_group_function | arc_group_method;

arc_group_field:
	arc_annotation* arc_accessibility KEYWORD_FIELD arc_data_declaration SYMBOL_SEMICOLON;

arc_method_declarator:
	arc_annotation* arc_accessibility KEYWORD_METHOD arc_single_identifier CONTAINER_OPEN_PAREN arc_function_params?
		CONTAINER_CLOSE_PAREN SYMBOL_COLON arc_data_type;

arc_group_method: arc_method_declarator arc_wrapped_body;

arc_group_function: arc_function_block;

arc_namespace:
	KEYWORD_NAMESPACE arc_scoped_identifier CONTAINER_OPEN_BRACE (arc_function_block | arc_group_block)* CONTAINER_CLOSE_BRACE;
