lexer grammar ArcSourceCodeLexer;

SYMBOL_AT : '@';
SYMBOL_DOT : '.';
SYMBOL_SEMICOLON : ';';
SYMBOL_COLON : ':';

// Keywords
KEYWORD_NUMBER : 'number';
KEYWORD_CHAR : 'char';
KEYWORD_STRING : 'string';
KEYWORD_BOOL : 'bool';
KEYWORD_NONE : 'none';
KEYWORD_ANY : 'any';
KEYWORD_AUTO : 'auto';
KEYWORD_TRUE : 'true';
KEYWORD_FALSE : 'false';
// KEYWORD_EXPORT : 'export';
KEYWORD_LINK : 'link';
KEYWORD_PRIVATE : 'private';
KEYWORD_PROTECTED : 'protected';
KEYWORD_INTERNAL : 'internal';
KEYWORD_PUBLIC : 'public';
KEYWORD_SEALED : 'sealed';
KEYWORD_DECLARE : 'declare';
KEYWORD_FUNCTION : 'function';
KEYWORD_METHOD : 'method';
KEYWORD_GROUP : 'group';
// KEYWORD_FRAME : 'frame';
KEYWORD_FIELD : 'field';
KEYWORD_EXTENDS : 'extends';
KEYWORD_IMPLEMENTS : 'implements';
KEYWORD_MACRO : 'macro';
KEYWORD_NAMESPACE : 'namespace';
KEYWORD_CONST : 'const';
KEYWORD_VAR : 'var';
KEYWORD_VAL : 'val';
KEYWORD_REF : 'ref';
KEYWORD_NEW : 'new';
KEYWORD_GET : 'get';
KEYWORD_SET : 'set';
KEYWORD_DEFAULT : 'default';
KEYWORD_SELF : 'self';
KEYWORD_AS : 'as';
KEYWORD_IF : 'if';
KEYWORD_ELSE_IF : 'elif';
KEYWORD_ELSE : 'else';
KEYWORD_WHILE : 'while';
KEYWORD_LOOP : 'loop';
KEYWORD_FOR : 'for';
KEYWORD_FOREACH : 'foreach';
KEYWORD_CONTINUE : 'continue';
KEYWORD_BREAK : 'break';
KEYWORD_CALL : 'call';
KEYWORD_RETURN : 'return';
KEYWORD_DEFER : 'defer';
KEYWORD_TRY : 'try';
KEYWORD_CATCH : 'catch';
KEYWORD_FINALLY : 'finally';
KEYWORD_THROW : 'throw';

// Operators
OPERATOR_PLUS : '+';
OPERATOR_MINUS : '-';
OPERATOR_MULTIPLY : '*';
OPERATOR_DIVIDE : '/';
OPERATOR_MODULO : '%';
OPERATOR_VALUE_EQUALS : '==';
OPERATOR_REF_EQUALS : '===';
OPERATOR_NOT_EQUALS : '<>';
// OPERATOR_LESS_THAN : '<';
// OPERATOR_GREATER_THAN : '>';
OPERATOR_LESS_THAN_EQUALS : '<=';
OPERATOR_GREATER_THAN_EQUALS : '>=';
OPERATOR_AND : '&&';
OPERATOR_OR : '||';
OPERATOR_NOT : '!';
OPERATOR_ASSIGN : '=';
OPERATOR_DOLLAR : '$';
OPERATOR_QUESTION_MARK : '?';
OPERATOR_COMMA : ',';
OPERATOR_SCOPE : '::';

// Containers
CONTAINER_OPEN_PAREN : '(';
CONTAINER_CLOSE_PAREN : ')';
CONTAINER_OPEN_BRACE : '{';
CONTAINER_CLOSE_BRACE : '}';
CONTAINER_OPEN_INDEXER : '[';
CONTAINER_CLOSE_INDEXER : ']';
// Also alternative for bigger and less operator
CONTAINER_OPEN_SHARP_PAREN : '<';
CONTAINER_CLOSE_SHARP_PAREN : '>';

// Flexible rules
NUMBER : INTEGER | OCTAL | HEX | FLOAT | BINARY ;
fragment INTEGER : [0-9]+ ;
fragment FLOAT : [0-9]+ '.' [0-9]* (('e' | 'E') ('+' | '-')? [0-9]+)? ;
fragment HEX : '0x' [0-9a-fA-F]+ ;
fragment OCTAL : '0o' [0-7]+ ;
fragment BINARY : '0b' [01]+ ;

LITERAL_STRING : '"' (~["\r\n\\] | '\\' .)* '"';

WHITESPACE : [ \t\r\n]+ -> skip ;
LINE_COMMENT : '#' .*? '\n' -> skip ;
IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9]* ;
