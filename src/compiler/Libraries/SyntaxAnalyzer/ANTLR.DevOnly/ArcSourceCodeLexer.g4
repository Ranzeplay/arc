lexer grammar ArcSourceCodeLexer;

// Math operators
PLUS: '+';
MINUS: '-';
MULTIPLY: '*';
DIVIDE: '/';
MODULO: '%';
// Bitwise operators
BITWISE_AND: '&';
BITWISE_OR: '|';
BITWISE_LSHIFT: '<<';
BITWISE_RSHIFT: '>>';
BITWISE_XOR: '^';
BITWISE_NOT: '~';
// Comparison operators
COMP_OBJ_EQ: '==';
COMP_REF_EQ: '===';
COMP_OBJ_NEQ: '<>';
COMP_REF_NEQ: '<!>';
COMP_LT: '<';
COMP_GT: '>';
COMP_LTE: '<=';
COMP_GTE: '>=';
// Logical operators
LOGICAL_AND: '&&';
LOGICAL_OR: '||';
LOGICAL_NOT: '!';
// Assignment operators
ASSIGN: '=';
ASSIGN_IF_NULL: '??=';
SELF_INCREMENT: '++';
SELF_DECREMENT: '--';
INCREASE_BY: '+=';
DECREASE_BY: '-=';
MULTIPLY_BY: '*=';
DIVIDE_BY: '/=';
MOD_BY: '%=';

// Delimiters
LPAREN: '(';
RPAREN: ')';
LBRACKET: '[';
RBRACKET: ']';
LBRACE: '{';
RBRACE: '}';

// Malicious signs
QUESTION: '?';
SEMICOLON: ';';
RANGE: '..';
AT: '@';
COMMA: ',';
PIPE: '|>';

// Selective operators
NULL_COALESCING: '??';
COLON: ':';

// Limiters
DOT: '.';
ARROW: '=>';
OPTIONAL_CHAIN: '?.';
SCOPE: '::';

// Keywords
KW_IF: 'if';
KW_ELIF: 'elif';
KW_ELSE: 'else';
KW_WHILE: 'while';
KW_FOR: 'for';
KW_LOOP: 'loop';
KW_FOREACH: 'foreach';
KW_IN: 'in';
KW_BREAK: 'break';
KW_CONTINUE: 'continue';
KW_RETURN: 'return';
KW_THROW: 'throw';
KW_TRY: 'try';
KW_CATCH: 'catch';
KW_FINALLY: 'finally';
KW_MATCH: 'match';
KW_CASE: 'case';
KW_GET: 'get';
KW_SET: 'set';
KW_DEFAULT: 'default';
KW_AWAIT: 'await';
KW_ASYNC: 'async';
KW_FUNCTION: 'func';
KW_GROUP: 'group';
KW_FIELD: 'field';
KW_CONSTRUCTOR: 'constructor';
KW_DESTRUCTOR: 'destructor';
KW_OPERATOR: 'operator';
KW_PUBLIC: 'public';
KW_INTERNAL: 'internal';
KW_PROTECTED: 'protected';
KW_PRIVATE: 'private';
KW_STATIC: 'static';
KW_CONSTANT: 'const';
KW_VARIABLE: 'var';
KW_VALUE: 'val';
KW_REFERENCE: 'ref';
KW_CLONE: 'clone';
KW_CALL: 'call';
KW_NEW: 'new';
KW_DEFER: 'defer';
KW_MACRO: 'macro';
KW_NAMESPACE: 'namespace';
KW_EXTENDS: 'extends';
KW_IMPLEMENTS: 'implements';
KW_LINK: 'link';
KW_TRUE: 'true';
KW_FALSE: 'false';
KW_NONE: 'none';
KW_ANY: 'any';
KW_INFER: 'infer';
KW_CHAR: 'char';
KW_BOOL: 'bool';
KW_BYTE: 'byte';
KW_INT: 'int';
KW_DECIMAL: 'decimal';
KW_STRING: 'string';
KW_DISPOSE: 'dispose';
KW_WITH: 'with';
KW_LIFETIME: 'lifetime';
KW_TYPEOF: 'typeof';
KW_SELF: 'self';
KW_ENUM: 'enum';

LITERAL_STRING : '"' (~["\r\n\\] | '\\' .)* '"';
WHITESPACE : [ \t\r\n]+ -> skip ;
LINE_COMMENT : '#' .*? '\n' -> skip ;
IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9]* ;

NUMBER : INTEGER | OCTAL | HEX | DECIMAL | BINARY ;
fragment INTEGER : [0-9]+ ;
fragment DECIMAL : [0-9]+ '.' [0-9]* (('e' | 'E') ('+' | '-')? [0-9]+)? ;
fragment HEX : '0x' [0-9a-fA-F]+ ;
fragment OCTAL : '0o' [0-7]+ ;
fragment BINARY : '0b' [01]+ ;
