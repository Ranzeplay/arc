parser grammar ArcSourceCodeParser;
options { tokenVocab=ArcSourceCodeLexer; }

arc_compilation_unit: arc_stmt_link* arc_namespace_block EOF;

arc_accessibility: KW_PUBLIC | KW_INTERNAL | KW_PROTECTED | KW_PRIVATE;
arc_mutability: KW_CONSTANT | KW_VARIABLE;
arc_mem_store_type: KW_VALUE | KW_REFERENCE;

arc_namespace_identifier: IDENTIFIER (SCOPE IDENTIFIER)*;
arc_namespace_declarator: KW_NAMESPACE arc_namespace_identifier;
arc_namespace_limiter: LBRACKET arc_namespace_identifier RBRACKET;
arc_single_identifier: IDENTIFIER;
arc_full_identifier: arc_namespace_limiter DOT arc_single_identifier;
arc_flexible_identifier: arc_full_identifier | arc_single_identifier;

arc_primitive_data_type: KW_INT | KW_DECIMAL | KW_CHAR | KW_STRING | KW_BOOL | KW_BYTE | KW_NONE | KW_ANY | KW_INFER;
arc_array_indicator: LBRACKET RBRACKET;
arc_data_type: arc_mem_store_type (arc_primitive_data_type | arc_flexible_identifier) arc_generic_specialization_wrapper? arc_array_indicator*;

arc_data_declarator: arc_mutability arc_single_identifier COLON arc_data_type;
arc_self_data_declarator: arc_mutability arc_self_wrapper COLON arc_data_type;

arc_arg_list: (arc_data_declarator | arc_self_data_declarator) (COMMA arc_data_declarator)*;
arc_wrapped_arg_list: LPAREN arc_arg_list? RPAREN;

arc_param_list: arc_expression (COMMA arc_expression)*;
arc_wrapped_param_list: LPAREN arc_param_list? RPAREN;

arc_annotation: AT arc_flexible_identifier arc_wrapped_param_list?;

arc_function_declarator: arc_annotation* arc_accessibility KW_FUNCTION arc_single_identifier arc_generic_declaration_wrapper? arc_wrapped_arg_list COLON arc_data_type;
arc_function_call_base: arc_flexible_identifier arc_generic_specialization_wrapper? arc_wrapped_param_list;

arc_wrapped_function_body: LBRACE arc_statement* RBRACE;
arc_function_block: arc_function_declarator arc_wrapped_function_body;

arc_bool_value: KW_TRUE | KW_FALSE;
arc_instant_value: NUMBER | LITERAL_STRING | arc_bool_value | KW_NONE | KW_ANY;
arc_type_value: KW_TYPEOF LPAREN arc_data_type RPAREN;
arc_data_value: arc_instant_value | arc_type_value | arc_call_chain | arc_enum_accessor;

arc_constructor_call: KW_NEW arc_flexible_identifier arc_generic_specialization_wrapper arc_wrapped_param_list;

arc_statement: ((arc_stmt_assign | arc_stmt_decl | arc_stmt_return | arc_stmt_assign | arc_stmt_break | arc_stmt_continue | arc_stmt_call | arc_stmt_throw) SEMICOLON) | (arc_stmt_while | arc_stmt_loop | arc_stmt_for | arc_stmt_foreach | arc_stmt_if);

arc_stmt_link: KW_LINK arc_namespace_identifier SEMICOLON;
arc_stmt_return: KW_RETURN arc_expression?;
arc_stmt_decl: arc_data_declarator (COMP_OBJ_EQ arc_expression)?;
// TODO: incremental
arc_stmt_assign: arc_call_chain (ASSIGN | ASSIGN_IF_NULL) arc_expression;
arc_stmt_break: KW_BREAK;
arc_stmt_continue: KW_CONTINUE;
arc_stmt_call: KW_CALL (arc_function_call_base | arc_call_chain);
arc_stmt_while: KW_WHILE LPAREN arc_expression RPAREN arc_wrapped_function_body;
arc_stmt_for: KW_FOR LPAREN arc_stmt_decl SEMICOLON arc_expression SEMICOLON arc_stmt_assign RPAREN arc_wrapped_function_body;
arc_stmt_loop: KW_LOOP arc_wrapped_function_body;
arc_stmt_foreach: KW_FOREACH LPAREN arc_data_declarator KW_IN arc_expression RPAREN arc_wrapped_function_body;
arc_stmt_throw: KW_THROW arc_expression;

arc_stmt_if: KW_IF LPAREN arc_expression RPAREN arc_wrapped_function_body (KW_ELIF LPAREN arc_expression RPAREN arc_wrapped_function_body)* (KW_ELSE arc_wrapped_function_body)?;

// TODO
arc_expression: 
    arc_data_value |
    arc_wrapped_expression |
    arc_expression MULTIPLY arc_expression |
    arc_expression DIVIDE arc_expression |
    arc_expression MODULO arc_expression |
    arc_expression PLUS arc_expression |
    arc_expression MINUS arc_expression |
    arc_expression BITWISE_LSHIFT arc_expression |
    arc_expression BITWISE_RSHIFT arc_expression |
    arc_expression BITWISE_AND arc_expression |
    arc_expression BITWISE_OR arc_expression |
    arc_expression BITWISE_XOR arc_expression |
    arc_expression COMP_LT arc_expression |
    arc_expression COMP_GT arc_expression |
    arc_expression COMP_LTE arc_expression |
    arc_expression COMP_GTE arc_expression |
    arc_expression COMP_OBJ_EQ arc_expression |
    arc_expression COMP_REF_EQ arc_expression |
    arc_expression COMP_OBJ_NEQ arc_expression |
    arc_expression COMP_REF_NEQ arc_expression |
    arc_expression LOGICAL_AND arc_expression |
    arc_expression LOGICAL_OR arc_expression |
    arc_expression QUESTION arc_expression COLON arc_expression |
    arc_expression RANGE arc_expression |
    arc_expression ARROW arc_expression |
    LOGICAL_NOT (arc_data_value | arc_wrapped_expression) |
    arc_data_value BITWISE_NOT |
    arc_wrapped_expression BITWISE_NOT;
arc_wrapped_expression: LPAREN arc_expression RPAREN;

arc_namespace_block: arc_namespace_declarator LBRACE arc_namespace_member* RBRACE;
arc_namespace_member: arc_function_block | arc_group_block | arc_enum_declarator;

arc_group_block: arc_annotation* arc_accessibility KW_GROUP arc_single_identifier arc_generic_declaration_wrapper? arc_wrapped_group_member;
arc_wrapped_group_member: LBRACE arc_group_member* RBRACE;
arc_group_member: arc_group_constructor | arc_group_destructor | arc_group_function | arc_group_field;
arc_group_field: arc_annotation* arc_accessibility KW_FIELD arc_data_declarator SEMICOLON;
arc_group_constructor: arc_annotation* arc_accessibility KW_CONSTRUCTOR arc_wrapped_arg_list arc_wrapped_function_body;
arc_group_destructor: arc_annotation* arc_accessibility KW_DESTRUCTOR arc_wrapped_function_body;
arc_group_function: arc_function_block;

arc_index: LBRACKET arc_expression RBRACKET;

// Call chain
arc_call_chain: (arc_call_chain_term | arc_constructor_call) (DOT arc_call_chain_term)*;
arc_call_chain_term: (arc_flexible_identifier | arc_function_call_base | arc_self_wrapper) arc_index*;

arc_self_wrapper: KW_SELF;

arc_generic_declaration_wrapper: COMP_LT arc_single_identifier (COMMA arc_single_identifier)* COMP_GT;
arc_generic_specialization_wrapper: COMP_LT ((arc_data_type (COMMA arc_data_type)*) | QUESTION) COMP_GT;

arc_enum_declarator: arc_annotation* arc_accessibility KW_ENUM arc_single_identifier LBRACE (arc_enum_member (COMMA arc_enum_member)*)? RBRACE;
arc_enum_member: arc_annotation* arc_single_identifier;
arc_enum_accessor: arc_flexible_identifier DOT arc_single_identifier;
