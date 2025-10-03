// Generated from src/parser/ArcSourceCodeParser.g4 by ANTLR 4.9.0-SNAPSHOT


import { ATN } from "antlr4ts/atn/ATN";
import { ATNDeserializer } from "antlr4ts/atn/ATNDeserializer";
import { FailedPredicateException } from "antlr4ts/FailedPredicateException";
import { NotNull } from "antlr4ts/Decorators";
import { NoViableAltException } from "antlr4ts/NoViableAltException";
import { Override } from "antlr4ts/Decorators";
import { Parser } from "antlr4ts/Parser";
import { ParserRuleContext } from "antlr4ts/ParserRuleContext";
import { ParserATNSimulator } from "antlr4ts/atn/ParserATNSimulator";
import { ParseTreeListener } from "antlr4ts/tree/ParseTreeListener";
import { ParseTreeVisitor } from "antlr4ts/tree/ParseTreeVisitor";
import { RecognitionException } from "antlr4ts/RecognitionException";
import { RuleContext } from "antlr4ts/RuleContext";
//import { RuleVersion } from "antlr4ts/RuleVersion";
import { TerminalNode } from "antlr4ts/tree/TerminalNode";
import { Token } from "antlr4ts/Token";
import { TokenStream } from "antlr4ts/TokenStream";
import { Vocabulary } from "antlr4ts/Vocabulary";
import { VocabularyImpl } from "antlr4ts/VocabularyImpl";

import * as Utils from "antlr4ts/misc/Utils";

import { ArcSourceCodeParserListener } from "./ArcSourceCodeParserListener";
import { ArcSourceCodeParserVisitor } from "./ArcSourceCodeParserVisitor";


export class ArcSourceCodeParser extends Parser {
	public static readonly PLUS = 1;
	public static readonly MINUS = 2;
	public static readonly MULTIPLY = 3;
	public static readonly DIVIDE = 4;
	public static readonly MODULO = 5;
	public static readonly BITWISE_AND = 6;
	public static readonly BITWISE_OR = 7;
	public static readonly BITWISE_LSHIFT = 8;
	public static readonly BITWISE_RSHIFT = 9;
	public static readonly BITWISE_XOR = 10;
	public static readonly BITWISE_NOT = 11;
	public static readonly COMP_OBJ_EQ = 12;
	public static readonly COMP_REF_EQ = 13;
	public static readonly COMP_OBJ_NEQ = 14;
	public static readonly COMP_REF_NEQ = 15;
	public static readonly COMP_LT = 16;
	public static readonly COMP_GT = 17;
	public static readonly COMP_LTE = 18;
	public static readonly COMP_GTE = 19;
	public static readonly LOGICAL_AND = 20;
	public static readonly LOGICAL_OR = 21;
	public static readonly LOGICAL_NOT = 22;
	public static readonly ASSIGN = 23;
	public static readonly ASSIGN_IF_NULL = 24;
	public static readonly SELF_INCREMENT = 25;
	public static readonly SELF_DECREMENT = 26;
	public static readonly INCREASE_BY = 27;
	public static readonly DECREASE_BY = 28;
	public static readonly MULTIPLY_BY = 29;
	public static readonly DIVIDE_BY = 30;
	public static readonly MOD_BY = 31;
	public static readonly LPAREN = 32;
	public static readonly RPAREN = 33;
	public static readonly LBRACKET = 34;
	public static readonly RBRACKET = 35;
	public static readonly LBRACE = 36;
	public static readonly RBRACE = 37;
	public static readonly QUESTION = 38;
	public static readonly SEMICOLON = 39;
	public static readonly RANGE = 40;
	public static readonly AT = 41;
	public static readonly COMMA = 42;
	public static readonly PIPE = 43;
	public static readonly NULL_COALESCING = 44;
	public static readonly COLON = 45;
	public static readonly DOT = 46;
	public static readonly ARROW = 47;
	public static readonly OPTIONAL_CHAIN = 48;
	public static readonly SCOPE = 49;
	public static readonly KW_IF = 50;
	public static readonly KW_ELIF = 51;
	public static readonly KW_ELSE = 52;
	public static readonly KW_WHILE = 53;
	public static readonly KW_FOR = 54;
	public static readonly KW_LOOP = 55;
	public static readonly KW_FOREACH = 56;
	public static readonly KW_IN = 57;
	public static readonly KW_BREAK = 58;
	public static readonly KW_CONTINUE = 59;
	public static readonly KW_RETURN = 60;
	public static readonly KW_THROW = 61;
	public static readonly KW_TRY = 62;
	public static readonly KW_CATCH = 63;
	public static readonly KW_FINALLY = 64;
	public static readonly KW_MATCH = 65;
	public static readonly KW_CASE = 66;
	public static readonly KW_GET = 67;
	public static readonly KW_SET = 68;
	public static readonly KW_DEFAULT = 69;
	public static readonly KW_AWAIT = 70;
	public static readonly KW_ASYNC = 71;
	public static readonly KW_FUNCTION = 72;
	public static readonly KW_GROUP = 73;
	public static readonly KW_FIELD = 74;
	public static readonly KW_CONSTRUCTOR = 75;
	public static readonly KW_DESTRUCTOR = 76;
	public static readonly KW_OPERATOR = 77;
	public static readonly KW_PUBLIC = 78;
	public static readonly KW_INTERNAL = 79;
	public static readonly KW_PROTECTED = 80;
	public static readonly KW_PRIVATE = 81;
	public static readonly KW_STATIC = 82;
	public static readonly KW_CONSTANT = 83;
	public static readonly KW_VARIABLE = 84;
	public static readonly KW_VALUE = 85;
	public static readonly KW_REFERENCE = 86;
	public static readonly KW_CLONE = 87;
	public static readonly KW_CALL = 88;
	public static readonly KW_NEW = 89;
	public static readonly KW_DEFER = 90;
	public static readonly KW_MACRO = 91;
	public static readonly KW_NAMESPACE = 92;
	public static readonly KW_LINK = 93;
	public static readonly KW_TRUE = 94;
	public static readonly KW_FALSE = 95;
	public static readonly KW_NONE = 96;
	public static readonly KW_ANY = 97;
	public static readonly KW_INFER = 98;
	public static readonly KW_CHAR = 99;
	public static readonly KW_BOOL = 100;
	public static readonly KW_BYTE = 101;
	public static readonly KW_INT = 102;
	public static readonly KW_DECIMAL = 103;
	public static readonly KW_STRING = 104;
	public static readonly KW_DISPOSE = 105;
	public static readonly KW_WITH = 106;
	public static readonly KW_LIFETIME = 107;
	public static readonly KW_TYPEOF = 108;
	public static readonly KW_SELF = 109;
	public static readonly KW_ENUM = 110;
	public static readonly LITERAL_STRING = 111;
	public static readonly WHITESPACE = 112;
	public static readonly LINE_COMMENT = 113;
	public static readonly IDENTIFIER = 114;
	public static readonly NUMBER = 115;
	public static readonly RULE_arc_compilation_unit = 0;
	public static readonly RULE_arc_accessibility = 1;
	public static readonly RULE_arc_mutability = 2;
	public static readonly RULE_arc_namespace_identifier = 3;
	public static readonly RULE_arc_namespace_declarator = 4;
	public static readonly RULE_arc_namespace_limiter = 5;
	public static readonly RULE_arc_single_identifier = 6;
	public static readonly RULE_arc_full_identifier = 7;
	public static readonly RULE_arc_flexible_identifier = 8;
	public static readonly RULE_arc_primitive_data_type = 9;
	public static readonly RULE_arc_array_indicator = 10;
	public static readonly RULE_arc_data_type = 11;
	public static readonly RULE_arc_data_declarator = 12;
	public static readonly RULE_arc_self_data_declarator = 13;
	public static readonly RULE_arc_arg_list = 14;
	public static readonly RULE_arc_wrapped_arg_list = 15;
	public static readonly RULE_arc_param_list = 16;
	public static readonly RULE_arc_wrapped_param_list = 17;
	public static readonly RULE_arc_annotation = 18;
	public static readonly RULE_arc_function_declarator = 19;
	public static readonly RULE_arc_function_call_base = 20;
	public static readonly RULE_arc_wrapped_function_body = 21;
	public static readonly RULE_arc_function_block = 22;
	public static readonly RULE_arc_bool_value = 23;
	public static readonly RULE_arc_instant_value = 24;
	public static readonly RULE_arc_type_value = 25;
	public static readonly RULE_arc_data_value = 26;
	public static readonly RULE_arc_constructor_call = 27;
	public static readonly RULE_arc_statement = 28;
	public static readonly RULE_arc_stmt_link = 29;
	public static readonly RULE_arc_stmt_return = 30;
	public static readonly RULE_arc_stmt_decl = 31;
	public static readonly RULE_arc_stmt_assign = 32;
	public static readonly RULE_arc_stmt_break = 33;
	public static readonly RULE_arc_stmt_continue = 34;
	public static readonly RULE_arc_stmt_call = 35;
	public static readonly RULE_arc_stmt_while = 36;
	public static readonly RULE_arc_stmt_for = 37;
	public static readonly RULE_arc_stmt_loop = 38;
	public static readonly RULE_arc_stmt_foreach = 39;
	public static readonly RULE_arc_stmt_throw = 40;
	public static readonly RULE_arc_stmt_if = 41;
	public static readonly RULE_arc_expression = 42;
	public static readonly RULE_arc_wrapped_expression = 43;
	public static readonly RULE_arc_namespace_block = 44;
	public static readonly RULE_arc_namespace_member = 45;
	public static readonly RULE_arc_group_block = 46;
	public static readonly RULE_arc_group_derive_list = 47;
	public static readonly RULE_arc_wrapped_group_member = 48;
	public static readonly RULE_arc_group_member = 49;
	public static readonly RULE_arc_group_field = 50;
	public static readonly RULE_arc_group_function = 51;
	public static readonly RULE_arc_group_lifecycle_keyword = 52;
	public static readonly RULE_arc_group_lifecycle_function = 53;
	public static readonly RULE_arc_index = 54;
	public static readonly RULE_arc_call_chain = 55;
	public static readonly RULE_arc_call_chain_term = 56;
	public static readonly RULE_arc_self_wrapper = 57;
	public static readonly RULE_arc_generic_declaration_wrapper = 58;
	public static readonly RULE_arc_generic_specialization_wrapper = 59;
	public static readonly RULE_arc_enum_declarator = 60;
	public static readonly RULE_arc_enum_member = 61;
	public static readonly RULE_arc_enum_accessor = 62;
	// tslint:disable:no-trailing-whitespace
	public static readonly ruleNames: string[] = [
		"arc_compilation_unit", "arc_accessibility", "arc_mutability", "arc_namespace_identifier", 
		"arc_namespace_declarator", "arc_namespace_limiter", "arc_single_identifier", 
		"arc_full_identifier", "arc_flexible_identifier", "arc_primitive_data_type", 
		"arc_array_indicator", "arc_data_type", "arc_data_declarator", "arc_self_data_declarator", 
		"arc_arg_list", "arc_wrapped_arg_list", "arc_param_list", "arc_wrapped_param_list", 
		"arc_annotation", "arc_function_declarator", "arc_function_call_base", 
		"arc_wrapped_function_body", "arc_function_block", "arc_bool_value", "arc_instant_value", 
		"arc_type_value", "arc_data_value", "arc_constructor_call", "arc_statement", 
		"arc_stmt_link", "arc_stmt_return", "arc_stmt_decl", "arc_stmt_assign", 
		"arc_stmt_break", "arc_stmt_continue", "arc_stmt_call", "arc_stmt_while", 
		"arc_stmt_for", "arc_stmt_loop", "arc_stmt_foreach", "arc_stmt_throw", 
		"arc_stmt_if", "arc_expression", "arc_wrapped_expression", "arc_namespace_block", 
		"arc_namespace_member", "arc_group_block", "arc_group_derive_list", "arc_wrapped_group_member", 
		"arc_group_member", "arc_group_field", "arc_group_function", "arc_group_lifecycle_keyword", 
		"arc_group_lifecycle_function", "arc_index", "arc_call_chain", "arc_call_chain_term", 
		"arc_self_wrapper", "arc_generic_declaration_wrapper", "arc_generic_specialization_wrapper", 
		"arc_enum_declarator", "arc_enum_member", "arc_enum_accessor",
	];

	private static readonly _LITERAL_NAMES: Array<string | undefined> = [
		undefined, "'+'", "'-'", "'*'", "'/'", "'%'", "'&'", "'|'", "'<<'", "'>>'", 
		"'^'", "'~'", "'=='", "'==='", "'<>'", "'<!>'", "'<'", "'>'", "'<='", 
		"'>='", "'&&'", "'||'", "'!'", "'='", "'??='", "'++'", "'--'", "'+='", 
		"'-='", "'*='", "'/='", "'%='", "'('", "')'", "'['", "']'", "'{'", "'}'", 
		"'?'", "';'", "'..'", "'@'", "','", "'|>'", "'??'", "':'", "'.'", "'=>'", 
		"'?.'", "'::'", "'if'", "'elif'", "'else'", "'while'", "'for'", "'loop'", 
		"'foreach'", "'in'", "'break'", "'continue'", "'return'", "'throw'", "'try'", 
		"'catch'", "'finally'", "'match'", "'case'", "'get'", "'set'", "'default'", 
		"'await'", "'async'", "'func'", "'group'", "'field'", "'constructor'", 
		"'destructor'", "'operator'", "'public'", "'internal'", "'protected'", 
		"'private'", "'static'", "'const'", "'var'", "'val'", "'ref'", "'clone'", 
		"'call'", "'new'", "'defer'", "'macro'", "'namespace'", "'link'", "'true'", 
		"'false'", "'none'", "'any'", "'infer'", "'char'", "'bool'", "'byte'", 
		"'int'", "'decimal'", "'string'", "'dispose'", "'with'", "'lifetime'", 
		"'typeof'", "'self'", "'enum'",
	];
	private static readonly _SYMBOLIC_NAMES: Array<string | undefined> = [
		undefined, "PLUS", "MINUS", "MULTIPLY", "DIVIDE", "MODULO", "BITWISE_AND", 
		"BITWISE_OR", "BITWISE_LSHIFT", "BITWISE_RSHIFT", "BITWISE_XOR", "BITWISE_NOT", 
		"COMP_OBJ_EQ", "COMP_REF_EQ", "COMP_OBJ_NEQ", "COMP_REF_NEQ", "COMP_LT", 
		"COMP_GT", "COMP_LTE", "COMP_GTE", "LOGICAL_AND", "LOGICAL_OR", "LOGICAL_NOT", 
		"ASSIGN", "ASSIGN_IF_NULL", "SELF_INCREMENT", "SELF_DECREMENT", "INCREASE_BY", 
		"DECREASE_BY", "MULTIPLY_BY", "DIVIDE_BY", "MOD_BY", "LPAREN", "RPAREN", 
		"LBRACKET", "RBRACKET", "LBRACE", "RBRACE", "QUESTION", "SEMICOLON", "RANGE", 
		"AT", "COMMA", "PIPE", "NULL_COALESCING", "COLON", "DOT", "ARROW", "OPTIONAL_CHAIN", 
		"SCOPE", "KW_IF", "KW_ELIF", "KW_ELSE", "KW_WHILE", "KW_FOR", "KW_LOOP", 
		"KW_FOREACH", "KW_IN", "KW_BREAK", "KW_CONTINUE", "KW_RETURN", "KW_THROW", 
		"KW_TRY", "KW_CATCH", "KW_FINALLY", "KW_MATCH", "KW_CASE", "KW_GET", "KW_SET", 
		"KW_DEFAULT", "KW_AWAIT", "KW_ASYNC", "KW_FUNCTION", "KW_GROUP", "KW_FIELD", 
		"KW_CONSTRUCTOR", "KW_DESTRUCTOR", "KW_OPERATOR", "KW_PUBLIC", "KW_INTERNAL", 
		"KW_PROTECTED", "KW_PRIVATE", "KW_STATIC", "KW_CONSTANT", "KW_VARIABLE", 
		"KW_VALUE", "KW_REFERENCE", "KW_CLONE", "KW_CALL", "KW_NEW", "KW_DEFER", 
		"KW_MACRO", "KW_NAMESPACE", "KW_LINK", "KW_TRUE", "KW_FALSE", "KW_NONE", 
		"KW_ANY", "KW_INFER", "KW_CHAR", "KW_BOOL", "KW_BYTE", "KW_INT", "KW_DECIMAL", 
		"KW_STRING", "KW_DISPOSE", "KW_WITH", "KW_LIFETIME", "KW_TYPEOF", "KW_SELF", 
		"KW_ENUM", "LITERAL_STRING", "WHITESPACE", "LINE_COMMENT", "IDENTIFIER", 
		"NUMBER",
	];
	public static readonly VOCABULARY: Vocabulary = new VocabularyImpl(ArcSourceCodeParser._LITERAL_NAMES, ArcSourceCodeParser._SYMBOLIC_NAMES, []);

	// @Override
	// @NotNull
	public get vocabulary(): Vocabulary {
		return ArcSourceCodeParser.VOCABULARY;
	}
	// tslint:enable:no-trailing-whitespace

	// @Override
	public get grammarFileName(): string { return "ArcSourceCodeParser.g4"; }

	// @Override
	public get ruleNames(): string[] { return ArcSourceCodeParser.ruleNames; }

	// @Override
	public get serializedATN(): string { return ArcSourceCodeParser._serializedATN; }

	protected createFailedPredicateException(predicate?: string, message?: string): FailedPredicateException {
		return new FailedPredicateException(this, predicate, message);
	}

	constructor(input: TokenStream) {
		super(input);
		this._interp = new ParserATNSimulator(ArcSourceCodeParser._ATN, this);
	}
	// @RuleVersion(0)
	public arc_compilation_unit(): Arc_compilation_unitContext {
		let _localctx: Arc_compilation_unitContext = new Arc_compilation_unitContext(this._ctx, this.state);
		this.enterRule(_localctx, 0, ArcSourceCodeParser.RULE_arc_compilation_unit);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 129;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.KW_LINK) {
				{
				{
				this.state = 126;
				this.arc_stmt_link();
				}
				}
				this.state = 131;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 132;
			this.arc_namespace_block();
			this.state = 133;
			this.match(ArcSourceCodeParser.EOF);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_accessibility(): Arc_accessibilityContext {
		let _localctx: Arc_accessibilityContext = new Arc_accessibilityContext(this._ctx, this.state);
		this.enterRule(_localctx, 2, ArcSourceCodeParser.RULE_arc_accessibility);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 135;
			_la = this._input.LA(1);
			if (!(((((_la - 78)) & ~0x1F) === 0 && ((1 << (_la - 78)) & ((1 << (ArcSourceCodeParser.KW_PUBLIC - 78)) | (1 << (ArcSourceCodeParser.KW_INTERNAL - 78)) | (1 << (ArcSourceCodeParser.KW_PROTECTED - 78)) | (1 << (ArcSourceCodeParser.KW_PRIVATE - 78)))) !== 0))) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_mutability(): Arc_mutabilityContext {
		let _localctx: Arc_mutabilityContext = new Arc_mutabilityContext(this._ctx, this.state);
		this.enterRule(_localctx, 4, ArcSourceCodeParser.RULE_arc_mutability);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 137;
			_la = this._input.LA(1);
			if (!(_la === ArcSourceCodeParser.KW_CONSTANT || _la === ArcSourceCodeParser.KW_VARIABLE)) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_namespace_identifier(): Arc_namespace_identifierContext {
		let _localctx: Arc_namespace_identifierContext = new Arc_namespace_identifierContext(this._ctx, this.state);
		this.enterRule(_localctx, 6, ArcSourceCodeParser.RULE_arc_namespace_identifier);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 139;
			this.match(ArcSourceCodeParser.IDENTIFIER);
			this.state = 144;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.SCOPE) {
				{
				{
				this.state = 140;
				this.match(ArcSourceCodeParser.SCOPE);
				this.state = 141;
				this.match(ArcSourceCodeParser.IDENTIFIER);
				}
				}
				this.state = 146;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_namespace_declarator(): Arc_namespace_declaratorContext {
		let _localctx: Arc_namespace_declaratorContext = new Arc_namespace_declaratorContext(this._ctx, this.state);
		this.enterRule(_localctx, 8, ArcSourceCodeParser.RULE_arc_namespace_declarator);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 147;
			this.match(ArcSourceCodeParser.KW_NAMESPACE);
			this.state = 148;
			this.arc_namespace_identifier();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_namespace_limiter(): Arc_namespace_limiterContext {
		let _localctx: Arc_namespace_limiterContext = new Arc_namespace_limiterContext(this._ctx, this.state);
		this.enterRule(_localctx, 10, ArcSourceCodeParser.RULE_arc_namespace_limiter);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 150;
			this.match(ArcSourceCodeParser.LBRACKET);
			this.state = 151;
			this.arc_namespace_identifier();
			this.state = 152;
			this.match(ArcSourceCodeParser.RBRACKET);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_single_identifier(): Arc_single_identifierContext {
		let _localctx: Arc_single_identifierContext = new Arc_single_identifierContext(this._ctx, this.state);
		this.enterRule(_localctx, 12, ArcSourceCodeParser.RULE_arc_single_identifier);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 154;
			this.match(ArcSourceCodeParser.IDENTIFIER);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_full_identifier(): Arc_full_identifierContext {
		let _localctx: Arc_full_identifierContext = new Arc_full_identifierContext(this._ctx, this.state);
		this.enterRule(_localctx, 14, ArcSourceCodeParser.RULE_arc_full_identifier);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 156;
			this.arc_namespace_limiter();
			this.state = 157;
			this.match(ArcSourceCodeParser.DOT);
			this.state = 158;
			this.arc_single_identifier();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_flexible_identifier(): Arc_flexible_identifierContext {
		let _localctx: Arc_flexible_identifierContext = new Arc_flexible_identifierContext(this._ctx, this.state);
		this.enterRule(_localctx, 16, ArcSourceCodeParser.RULE_arc_flexible_identifier);
		try {
			this.state = 162;
			this._errHandler.sync(this);
			switch (this._input.LA(1)) {
			case ArcSourceCodeParser.LBRACKET:
				this.enterOuterAlt(_localctx, 1);
				{
				this.state = 160;
				this.arc_full_identifier();
				}
				break;
			case ArcSourceCodeParser.IDENTIFIER:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 161;
				this.arc_single_identifier();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_primitive_data_type(): Arc_primitive_data_typeContext {
		let _localctx: Arc_primitive_data_typeContext = new Arc_primitive_data_typeContext(this._ctx, this.state);
		this.enterRule(_localctx, 18, ArcSourceCodeParser.RULE_arc_primitive_data_type);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 164;
			_la = this._input.LA(1);
			if (!(((((_la - 96)) & ~0x1F) === 0 && ((1 << (_la - 96)) & ((1 << (ArcSourceCodeParser.KW_NONE - 96)) | (1 << (ArcSourceCodeParser.KW_ANY - 96)) | (1 << (ArcSourceCodeParser.KW_INFER - 96)) | (1 << (ArcSourceCodeParser.KW_CHAR - 96)) | (1 << (ArcSourceCodeParser.KW_BOOL - 96)) | (1 << (ArcSourceCodeParser.KW_BYTE - 96)) | (1 << (ArcSourceCodeParser.KW_INT - 96)) | (1 << (ArcSourceCodeParser.KW_DECIMAL - 96)) | (1 << (ArcSourceCodeParser.KW_STRING - 96)))) !== 0))) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_array_indicator(): Arc_array_indicatorContext {
		let _localctx: Arc_array_indicatorContext = new Arc_array_indicatorContext(this._ctx, this.state);
		this.enterRule(_localctx, 20, ArcSourceCodeParser.RULE_arc_array_indicator);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 166;
			this.match(ArcSourceCodeParser.LBRACKET);
			this.state = 167;
			this.match(ArcSourceCodeParser.RBRACKET);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_data_type(): Arc_data_typeContext {
		let _localctx: Arc_data_typeContext = new Arc_data_typeContext(this._ctx, this.state);
		this.enterRule(_localctx, 22, ArcSourceCodeParser.RULE_arc_data_type);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 171;
			this._errHandler.sync(this);
			switch (this._input.LA(1)) {
			case ArcSourceCodeParser.KW_NONE:
			case ArcSourceCodeParser.KW_ANY:
			case ArcSourceCodeParser.KW_INFER:
			case ArcSourceCodeParser.KW_CHAR:
			case ArcSourceCodeParser.KW_BOOL:
			case ArcSourceCodeParser.KW_BYTE:
			case ArcSourceCodeParser.KW_INT:
			case ArcSourceCodeParser.KW_DECIMAL:
			case ArcSourceCodeParser.KW_STRING:
				{
				this.state = 169;
				this.arc_primitive_data_type();
				}
				break;
			case ArcSourceCodeParser.LBRACKET:
			case ArcSourceCodeParser.IDENTIFIER:
				{
				this.state = 170;
				this.arc_flexible_identifier();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			this.state = 174;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COMP_LT) {
				{
				this.state = 173;
				this.arc_generic_specialization_wrapper();
				}
			}

			this.state = 179;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.LBRACKET) {
				{
				{
				this.state = 176;
				this.arc_array_indicator();
				}
				}
				this.state = 181;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_data_declarator(): Arc_data_declaratorContext {
		let _localctx: Arc_data_declaratorContext = new Arc_data_declaratorContext(this._ctx, this.state);
		this.enterRule(_localctx, 24, ArcSourceCodeParser.RULE_arc_data_declarator);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 182;
			this.arc_mutability();
			this.state = 183;
			this.arc_single_identifier();
			this.state = 184;
			this.match(ArcSourceCodeParser.COLON);
			this.state = 185;
			this.arc_data_type();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_self_data_declarator(): Arc_self_data_declaratorContext {
		let _localctx: Arc_self_data_declaratorContext = new Arc_self_data_declaratorContext(this._ctx, this.state);
		this.enterRule(_localctx, 26, ArcSourceCodeParser.RULE_arc_self_data_declarator);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 187;
			this.arc_mutability();
			this.state = 188;
			this.arc_self_wrapper();
			this.state = 189;
			this.match(ArcSourceCodeParser.COLON);
			this.state = 190;
			this.arc_data_type();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_arg_list(): Arc_arg_listContext {
		let _localctx: Arc_arg_listContext = new Arc_arg_listContext(this._ctx, this.state);
		this.enterRule(_localctx, 28, ArcSourceCodeParser.RULE_arc_arg_list);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 194;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 6, this._ctx) ) {
			case 1:
				{
				this.state = 192;
				this.arc_data_declarator();
				}
				break;

			case 2:
				{
				this.state = 193;
				this.arc_self_data_declarator();
				}
				break;
			}
			this.state = 200;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.COMMA) {
				{
				{
				this.state = 196;
				this.match(ArcSourceCodeParser.COMMA);
				this.state = 197;
				this.arc_data_declarator();
				}
				}
				this.state = 202;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_wrapped_arg_list(): Arc_wrapped_arg_listContext {
		let _localctx: Arc_wrapped_arg_listContext = new Arc_wrapped_arg_listContext(this._ctx, this.state);
		this.enterRule(_localctx, 30, ArcSourceCodeParser.RULE_arc_wrapped_arg_list);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 203;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 205;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.KW_CONSTANT || _la === ArcSourceCodeParser.KW_VARIABLE) {
				{
				this.state = 204;
				this.arc_arg_list();
				}
			}

			this.state = 207;
			this.match(ArcSourceCodeParser.RPAREN);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_param_list(): Arc_param_listContext {
		let _localctx: Arc_param_listContext = new Arc_param_listContext(this._ctx, this.state);
		this.enterRule(_localctx, 32, ArcSourceCodeParser.RULE_arc_param_list);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 209;
			this.arc_expression(0);
			this.state = 214;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.COMMA) {
				{
				{
				this.state = 210;
				this.match(ArcSourceCodeParser.COMMA);
				this.state = 211;
				this.arc_expression(0);
				}
				}
				this.state = 216;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_wrapped_param_list(): Arc_wrapped_param_listContext {
		let _localctx: Arc_wrapped_param_listContext = new Arc_wrapped_param_listContext(this._ctx, this.state);
		this.enterRule(_localctx, 34, ArcSourceCodeParser.RULE_arc_wrapped_param_list);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 217;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 219;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (((((_la - 22)) & ~0x1F) === 0 && ((1 << (_la - 22)) & ((1 << (ArcSourceCodeParser.LOGICAL_NOT - 22)) | (1 << (ArcSourceCodeParser.LPAREN - 22)) | (1 << (ArcSourceCodeParser.LBRACKET - 22)))) !== 0) || ((((_la - 89)) & ~0x1F) === 0 && ((1 << (_la - 89)) & ((1 << (ArcSourceCodeParser.KW_NEW - 89)) | (1 << (ArcSourceCodeParser.KW_TRUE - 89)) | (1 << (ArcSourceCodeParser.KW_FALSE - 89)) | (1 << (ArcSourceCodeParser.KW_NONE - 89)) | (1 << (ArcSourceCodeParser.KW_ANY - 89)) | (1 << (ArcSourceCodeParser.KW_TYPEOF - 89)) | (1 << (ArcSourceCodeParser.KW_SELF - 89)) | (1 << (ArcSourceCodeParser.LITERAL_STRING - 89)) | (1 << (ArcSourceCodeParser.IDENTIFIER - 89)) | (1 << (ArcSourceCodeParser.NUMBER - 89)))) !== 0)) {
				{
				this.state = 218;
				this.arc_param_list();
				}
			}

			this.state = 221;
			this.match(ArcSourceCodeParser.RPAREN);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_annotation(): Arc_annotationContext {
		let _localctx: Arc_annotationContext = new Arc_annotationContext(this._ctx, this.state);
		this.enterRule(_localctx, 36, ArcSourceCodeParser.RULE_arc_annotation);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 223;
			this.match(ArcSourceCodeParser.AT);
			this.state = 224;
			this.arc_flexible_identifier();
			this.state = 226;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.LPAREN) {
				{
				this.state = 225;
				this.arc_wrapped_param_list();
				}
			}

			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_function_declarator(): Arc_function_declaratorContext {
		let _localctx: Arc_function_declaratorContext = new Arc_function_declaratorContext(this._ctx, this.state);
		this.enterRule(_localctx, 38, ArcSourceCodeParser.RULE_arc_function_declarator);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 231;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 228;
				this.arc_annotation();
				}
				}
				this.state = 233;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 234;
			this.arc_accessibility();
			this.state = 235;
			this.match(ArcSourceCodeParser.KW_FUNCTION);
			this.state = 236;
			this.arc_single_identifier();
			this.state = 238;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COMP_LT) {
				{
				this.state = 237;
				this.arc_generic_declaration_wrapper();
				}
			}

			this.state = 240;
			this.arc_wrapped_arg_list();
			this.state = 241;
			this.match(ArcSourceCodeParser.COLON);
			this.state = 242;
			this.arc_data_type();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_function_call_base(): Arc_function_call_baseContext {
		let _localctx: Arc_function_call_baseContext = new Arc_function_call_baseContext(this._ctx, this.state);
		this.enterRule(_localctx, 40, ArcSourceCodeParser.RULE_arc_function_call_base);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 244;
			this.arc_flexible_identifier();
			this.state = 246;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COMP_LT) {
				{
				this.state = 245;
				this.arc_generic_specialization_wrapper();
				}
			}

			this.state = 248;
			this.arc_wrapped_param_list();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		let _localctx: Arc_wrapped_function_bodyContext = new Arc_wrapped_function_bodyContext(this._ctx, this.state);
		this.enterRule(_localctx, 42, ArcSourceCodeParser.RULE_arc_wrapped_function_body);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 250;
			this.match(ArcSourceCodeParser.LBRACE);
			this.state = 254;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (((((_la - 34)) & ~0x1F) === 0 && ((1 << (_la - 34)) & ((1 << (ArcSourceCodeParser.LBRACKET - 34)) | (1 << (ArcSourceCodeParser.KW_IF - 34)) | (1 << (ArcSourceCodeParser.KW_WHILE - 34)) | (1 << (ArcSourceCodeParser.KW_FOR - 34)) | (1 << (ArcSourceCodeParser.KW_LOOP - 34)) | (1 << (ArcSourceCodeParser.KW_FOREACH - 34)) | (1 << (ArcSourceCodeParser.KW_BREAK - 34)) | (1 << (ArcSourceCodeParser.KW_CONTINUE - 34)) | (1 << (ArcSourceCodeParser.KW_RETURN - 34)) | (1 << (ArcSourceCodeParser.KW_THROW - 34)))) !== 0) || ((((_la - 83)) & ~0x1F) === 0 && ((1 << (_la - 83)) & ((1 << (ArcSourceCodeParser.KW_CONSTANT - 83)) | (1 << (ArcSourceCodeParser.KW_VARIABLE - 83)) | (1 << (ArcSourceCodeParser.KW_CALL - 83)) | (1 << (ArcSourceCodeParser.KW_NEW - 83)) | (1 << (ArcSourceCodeParser.KW_SELF - 83)) | (1 << (ArcSourceCodeParser.IDENTIFIER - 83)))) !== 0)) {
				{
				{
				this.state = 251;
				this.arc_statement();
				}
				}
				this.state = 256;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 257;
			this.match(ArcSourceCodeParser.RBRACE);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_function_block(): Arc_function_blockContext {
		let _localctx: Arc_function_blockContext = new Arc_function_blockContext(this._ctx, this.state);
		this.enterRule(_localctx, 44, ArcSourceCodeParser.RULE_arc_function_block);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 259;
			this.arc_function_declarator();
			this.state = 260;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_bool_value(): Arc_bool_valueContext {
		let _localctx: Arc_bool_valueContext = new Arc_bool_valueContext(this._ctx, this.state);
		this.enterRule(_localctx, 46, ArcSourceCodeParser.RULE_arc_bool_value);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 262;
			_la = this._input.LA(1);
			if (!(_la === ArcSourceCodeParser.KW_TRUE || _la === ArcSourceCodeParser.KW_FALSE)) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_instant_value(): Arc_instant_valueContext {
		let _localctx: Arc_instant_valueContext = new Arc_instant_valueContext(this._ctx, this.state);
		this.enterRule(_localctx, 48, ArcSourceCodeParser.RULE_arc_instant_value);
		try {
			this.state = 269;
			this._errHandler.sync(this);
			switch (this._input.LA(1)) {
			case ArcSourceCodeParser.NUMBER:
				this.enterOuterAlt(_localctx, 1);
				{
				this.state = 264;
				this.match(ArcSourceCodeParser.NUMBER);
				}
				break;
			case ArcSourceCodeParser.LITERAL_STRING:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 265;
				this.match(ArcSourceCodeParser.LITERAL_STRING);
				}
				break;
			case ArcSourceCodeParser.KW_TRUE:
			case ArcSourceCodeParser.KW_FALSE:
				this.enterOuterAlt(_localctx, 3);
				{
				this.state = 266;
				this.arc_bool_value();
				}
				break;
			case ArcSourceCodeParser.KW_NONE:
				this.enterOuterAlt(_localctx, 4);
				{
				this.state = 267;
				this.match(ArcSourceCodeParser.KW_NONE);
				}
				break;
			case ArcSourceCodeParser.KW_ANY:
				this.enterOuterAlt(_localctx, 5);
				{
				this.state = 268;
				this.match(ArcSourceCodeParser.KW_ANY);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_type_value(): Arc_type_valueContext {
		let _localctx: Arc_type_valueContext = new Arc_type_valueContext(this._ctx, this.state);
		this.enterRule(_localctx, 50, ArcSourceCodeParser.RULE_arc_type_value);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 271;
			this.match(ArcSourceCodeParser.KW_TYPEOF);
			this.state = 272;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 273;
			this.arc_data_type();
			this.state = 274;
			this.match(ArcSourceCodeParser.RPAREN);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_data_value(): Arc_data_valueContext {
		let _localctx: Arc_data_valueContext = new Arc_data_valueContext(this._ctx, this.state);
		this.enterRule(_localctx, 52, ArcSourceCodeParser.RULE_arc_data_value);
		try {
			this.state = 280;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 17, this._ctx) ) {
			case 1:
				this.enterOuterAlt(_localctx, 1);
				{
				this.state = 276;
				this.arc_instant_value();
				}
				break;

			case 2:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 277;
				this.arc_type_value();
				}
				break;

			case 3:
				this.enterOuterAlt(_localctx, 3);
				{
				this.state = 278;
				this.arc_call_chain();
				}
				break;

			case 4:
				this.enterOuterAlt(_localctx, 4);
				{
				this.state = 279;
				this.arc_enum_accessor();
				}
				break;
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_constructor_call(): Arc_constructor_callContext {
		let _localctx: Arc_constructor_callContext = new Arc_constructor_callContext(this._ctx, this.state);
		this.enterRule(_localctx, 54, ArcSourceCodeParser.RULE_arc_constructor_call);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 282;
			this.match(ArcSourceCodeParser.KW_NEW);
			this.state = 283;
			this.arc_flexible_identifier();
			this.state = 285;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COMP_LT) {
				{
				this.state = 284;
				this.arc_generic_specialization_wrapper();
				}
			}

			this.state = 287;
			this.arc_wrapped_param_list();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_statement(): Arc_statementContext {
		let _localctx: Arc_statementContext = new Arc_statementContext(this._ctx, this.state);
		this.enterRule(_localctx, 56, ArcSourceCodeParser.RULE_arc_statement);
		try {
			this.state = 308;
			this._errHandler.sync(this);
			switch (this._input.LA(1)) {
			case ArcSourceCodeParser.LBRACKET:
			case ArcSourceCodeParser.KW_BREAK:
			case ArcSourceCodeParser.KW_CONTINUE:
			case ArcSourceCodeParser.KW_RETURN:
			case ArcSourceCodeParser.KW_THROW:
			case ArcSourceCodeParser.KW_CONSTANT:
			case ArcSourceCodeParser.KW_VARIABLE:
			case ArcSourceCodeParser.KW_CALL:
			case ArcSourceCodeParser.KW_NEW:
			case ArcSourceCodeParser.KW_SELF:
			case ArcSourceCodeParser.IDENTIFIER:
				this.enterOuterAlt(_localctx, 1);
				{
				{
				this.state = 297;
				this._errHandler.sync(this);
				switch ( this.interpreter.adaptivePredict(this._input, 19, this._ctx) ) {
				case 1:
					{
					this.state = 289;
					this.arc_stmt_decl();
					}
					break;

				case 2:
					{
					this.state = 290;
					this.arc_stmt_assign();
					}
					break;

				case 3:
					{
					this.state = 291;
					this.arc_stmt_return();
					}
					break;

				case 4:
					{
					this.state = 292;
					this.arc_stmt_assign();
					}
					break;

				case 5:
					{
					this.state = 293;
					this.arc_stmt_break();
					}
					break;

				case 6:
					{
					this.state = 294;
					this.arc_stmt_continue();
					}
					break;

				case 7:
					{
					this.state = 295;
					this.arc_stmt_call();
					}
					break;

				case 8:
					{
					this.state = 296;
					this.arc_stmt_throw();
					}
					break;
				}
				this.state = 299;
				this.match(ArcSourceCodeParser.SEMICOLON);
				}
				}
				break;
			case ArcSourceCodeParser.KW_IF:
			case ArcSourceCodeParser.KW_WHILE:
			case ArcSourceCodeParser.KW_FOR:
			case ArcSourceCodeParser.KW_LOOP:
			case ArcSourceCodeParser.KW_FOREACH:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 306;
				this._errHandler.sync(this);
				switch (this._input.LA(1)) {
				case ArcSourceCodeParser.KW_WHILE:
					{
					this.state = 301;
					this.arc_stmt_while();
					}
					break;
				case ArcSourceCodeParser.KW_LOOP:
					{
					this.state = 302;
					this.arc_stmt_loop();
					}
					break;
				case ArcSourceCodeParser.KW_FOR:
					{
					this.state = 303;
					this.arc_stmt_for();
					}
					break;
				case ArcSourceCodeParser.KW_FOREACH:
					{
					this.state = 304;
					this.arc_stmt_foreach();
					}
					break;
				case ArcSourceCodeParser.KW_IF:
					{
					this.state = 305;
					this.arc_stmt_if();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_link(): Arc_stmt_linkContext {
		let _localctx: Arc_stmt_linkContext = new Arc_stmt_linkContext(this._ctx, this.state);
		this.enterRule(_localctx, 58, ArcSourceCodeParser.RULE_arc_stmt_link);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 310;
			this.match(ArcSourceCodeParser.KW_LINK);
			this.state = 311;
			this.arc_namespace_identifier();
			this.state = 312;
			this.match(ArcSourceCodeParser.SEMICOLON);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_return(): Arc_stmt_returnContext {
		let _localctx: Arc_stmt_returnContext = new Arc_stmt_returnContext(this._ctx, this.state);
		this.enterRule(_localctx, 60, ArcSourceCodeParser.RULE_arc_stmt_return);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 314;
			this.match(ArcSourceCodeParser.KW_RETURN);
			this.state = 316;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (((((_la - 22)) & ~0x1F) === 0 && ((1 << (_la - 22)) & ((1 << (ArcSourceCodeParser.LOGICAL_NOT - 22)) | (1 << (ArcSourceCodeParser.LPAREN - 22)) | (1 << (ArcSourceCodeParser.LBRACKET - 22)))) !== 0) || ((((_la - 89)) & ~0x1F) === 0 && ((1 << (_la - 89)) & ((1 << (ArcSourceCodeParser.KW_NEW - 89)) | (1 << (ArcSourceCodeParser.KW_TRUE - 89)) | (1 << (ArcSourceCodeParser.KW_FALSE - 89)) | (1 << (ArcSourceCodeParser.KW_NONE - 89)) | (1 << (ArcSourceCodeParser.KW_ANY - 89)) | (1 << (ArcSourceCodeParser.KW_TYPEOF - 89)) | (1 << (ArcSourceCodeParser.KW_SELF - 89)) | (1 << (ArcSourceCodeParser.LITERAL_STRING - 89)) | (1 << (ArcSourceCodeParser.IDENTIFIER - 89)) | (1 << (ArcSourceCodeParser.NUMBER - 89)))) !== 0)) {
				{
				this.state = 315;
				this.arc_expression(0);
				}
			}

			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_decl(): Arc_stmt_declContext {
		let _localctx: Arc_stmt_declContext = new Arc_stmt_declContext(this._ctx, this.state);
		this.enterRule(_localctx, 62, ArcSourceCodeParser.RULE_arc_stmt_decl);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 318;
			this.arc_data_declarator();
			this.state = 321;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.ASSIGN) {
				{
				this.state = 319;
				this.match(ArcSourceCodeParser.ASSIGN);
				this.state = 320;
				this.arc_expression(0);
				}
			}

			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_assign(): Arc_stmt_assignContext {
		let _localctx: Arc_stmt_assignContext = new Arc_stmt_assignContext(this._ctx, this.state);
		this.enterRule(_localctx, 64, ArcSourceCodeParser.RULE_arc_stmt_assign);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 323;
			this.arc_call_chain();
			this.state = 324;
			_la = this._input.LA(1);
			if (!(_la === ArcSourceCodeParser.ASSIGN || _la === ArcSourceCodeParser.ASSIGN_IF_NULL)) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			this.state = 325;
			this.arc_expression(0);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_break(): Arc_stmt_breakContext {
		let _localctx: Arc_stmt_breakContext = new Arc_stmt_breakContext(this._ctx, this.state);
		this.enterRule(_localctx, 66, ArcSourceCodeParser.RULE_arc_stmt_break);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 327;
			this.match(ArcSourceCodeParser.KW_BREAK);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_continue(): Arc_stmt_continueContext {
		let _localctx: Arc_stmt_continueContext = new Arc_stmt_continueContext(this._ctx, this.state);
		this.enterRule(_localctx, 68, ArcSourceCodeParser.RULE_arc_stmt_continue);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 329;
			this.match(ArcSourceCodeParser.KW_CONTINUE);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_call(): Arc_stmt_callContext {
		let _localctx: Arc_stmt_callContext = new Arc_stmt_callContext(this._ctx, this.state);
		this.enterRule(_localctx, 70, ArcSourceCodeParser.RULE_arc_stmt_call);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 331;
			this.match(ArcSourceCodeParser.KW_CALL);
			this.state = 334;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 24, this._ctx) ) {
			case 1:
				{
				this.state = 332;
				this.arc_function_call_base();
				}
				break;

			case 2:
				{
				this.state = 333;
				this.arc_call_chain();
				}
				break;
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_while(): Arc_stmt_whileContext {
		let _localctx: Arc_stmt_whileContext = new Arc_stmt_whileContext(this._ctx, this.state);
		this.enterRule(_localctx, 72, ArcSourceCodeParser.RULE_arc_stmt_while);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 336;
			this.match(ArcSourceCodeParser.KW_WHILE);
			this.state = 337;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 338;
			this.arc_expression(0);
			this.state = 339;
			this.match(ArcSourceCodeParser.RPAREN);
			this.state = 340;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_for(): Arc_stmt_forContext {
		let _localctx: Arc_stmt_forContext = new Arc_stmt_forContext(this._ctx, this.state);
		this.enterRule(_localctx, 74, ArcSourceCodeParser.RULE_arc_stmt_for);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 342;
			this.match(ArcSourceCodeParser.KW_FOR);
			this.state = 343;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 344;
			this.arc_stmt_decl();
			this.state = 345;
			this.match(ArcSourceCodeParser.SEMICOLON);
			this.state = 346;
			this.arc_expression(0);
			this.state = 347;
			this.match(ArcSourceCodeParser.SEMICOLON);
			this.state = 348;
			this.arc_stmt_assign();
			this.state = 349;
			this.match(ArcSourceCodeParser.RPAREN);
			this.state = 350;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_loop(): Arc_stmt_loopContext {
		let _localctx: Arc_stmt_loopContext = new Arc_stmt_loopContext(this._ctx, this.state);
		this.enterRule(_localctx, 76, ArcSourceCodeParser.RULE_arc_stmt_loop);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 352;
			this.match(ArcSourceCodeParser.KW_LOOP);
			this.state = 353;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_foreach(): Arc_stmt_foreachContext {
		let _localctx: Arc_stmt_foreachContext = new Arc_stmt_foreachContext(this._ctx, this.state);
		this.enterRule(_localctx, 78, ArcSourceCodeParser.RULE_arc_stmt_foreach);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 355;
			this.match(ArcSourceCodeParser.KW_FOREACH);
			this.state = 356;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 357;
			this.arc_data_declarator();
			this.state = 358;
			this.match(ArcSourceCodeParser.KW_IN);
			this.state = 359;
			this.arc_expression(0);
			this.state = 360;
			this.match(ArcSourceCodeParser.RPAREN);
			this.state = 361;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_throw(): Arc_stmt_throwContext {
		let _localctx: Arc_stmt_throwContext = new Arc_stmt_throwContext(this._ctx, this.state);
		this.enterRule(_localctx, 80, ArcSourceCodeParser.RULE_arc_stmt_throw);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 363;
			this.match(ArcSourceCodeParser.KW_THROW);
			this.state = 364;
			this.arc_expression(0);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_stmt_if(): Arc_stmt_ifContext {
		let _localctx: Arc_stmt_ifContext = new Arc_stmt_ifContext(this._ctx, this.state);
		this.enterRule(_localctx, 82, ArcSourceCodeParser.RULE_arc_stmt_if);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 366;
			this.match(ArcSourceCodeParser.KW_IF);
			this.state = 367;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 368;
			this.arc_expression(0);
			this.state = 369;
			this.match(ArcSourceCodeParser.RPAREN);
			this.state = 370;
			this.arc_wrapped_function_body();
			this.state = 379;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.KW_ELIF) {
				{
				{
				this.state = 371;
				this.match(ArcSourceCodeParser.KW_ELIF);
				this.state = 372;
				this.match(ArcSourceCodeParser.LPAREN);
				this.state = 373;
				this.arc_expression(0);
				this.state = 374;
				this.match(ArcSourceCodeParser.RPAREN);
				this.state = 375;
				this.arc_wrapped_function_body();
				}
				}
				this.state = 381;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 384;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.KW_ELSE) {
				{
				this.state = 382;
				this.match(ArcSourceCodeParser.KW_ELSE);
				this.state = 383;
				this.arc_wrapped_function_body();
				}
			}

			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}

	public arc_expression(): Arc_expressionContext;
	public arc_expression(_p: number): Arc_expressionContext;
	// @RuleVersion(0)
	public arc_expression(_p?: number): Arc_expressionContext {
		if (_p === undefined) {
			_p = 0;
		}

		let _parentctx: ParserRuleContext = this._ctx;
		let _parentState: number = this.state;
		let _localctx: Arc_expressionContext = new Arc_expressionContext(this._ctx, _parentState);
		let _prevctx: Arc_expressionContext = _localctx;
		let _startState: number = 84;
		this.enterRecursionRule(_localctx, 84, ArcSourceCodeParser.RULE_arc_expression, _p);
		try {
			let _alt: number;
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 400;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 28, this._ctx) ) {
			case 1:
				{
				this.state = 387;
				this.arc_data_value();
				}
				break;

			case 2:
				{
				this.state = 388;
				this.arc_wrapped_expression();
				}
				break;

			case 3:
				{
				this.state = 389;
				this.match(ArcSourceCodeParser.LOGICAL_NOT);
				this.state = 392;
				this._errHandler.sync(this);
				switch (this._input.LA(1)) {
				case ArcSourceCodeParser.LBRACKET:
				case ArcSourceCodeParser.KW_NEW:
				case ArcSourceCodeParser.KW_TRUE:
				case ArcSourceCodeParser.KW_FALSE:
				case ArcSourceCodeParser.KW_NONE:
				case ArcSourceCodeParser.KW_ANY:
				case ArcSourceCodeParser.KW_TYPEOF:
				case ArcSourceCodeParser.KW_SELF:
				case ArcSourceCodeParser.LITERAL_STRING:
				case ArcSourceCodeParser.IDENTIFIER:
				case ArcSourceCodeParser.NUMBER:
					{
					this.state = 390;
					this.arc_data_value();
					}
					break;
				case ArcSourceCodeParser.LPAREN:
					{
					this.state = 391;
					this.arc_wrapped_expression();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;

			case 4:
				{
				this.state = 394;
				this.arc_data_value();
				this.state = 395;
				this.match(ArcSourceCodeParser.BITWISE_NOT);
				}
				break;

			case 5:
				{
				this.state = 397;
				this.arc_wrapped_expression();
				this.state = 398;
				this.match(ArcSourceCodeParser.BITWISE_NOT);
				}
				break;
			}
			this._ctx._stop = this._input.tryLT(-1);
			this.state = 476;
			this._errHandler.sync(this);
			_alt = this.interpreter.adaptivePredict(this._input, 30, this._ctx);
			while (_alt !== 2 && _alt !== ATN.INVALID_ALT_NUMBER) {
				if (_alt === 1) {
					if (this._parseListeners != null) {
						this.triggerExitRuleEvent();
					}
					_prevctx = _localctx;
					{
					this.state = 474;
					this._errHandler.sync(this);
					switch ( this.interpreter.adaptivePredict(this._input, 29, this._ctx) ) {
					case 1:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 402;
						if (!(this.precpred(this._ctx, 26))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 26)");
						}
						this.state = 403;
						this.match(ArcSourceCodeParser.MULTIPLY);
						this.state = 404;
						this.arc_expression(27);
						}
						break;

					case 2:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 405;
						if (!(this.precpred(this._ctx, 25))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 25)");
						}
						this.state = 406;
						this.match(ArcSourceCodeParser.DIVIDE);
						this.state = 407;
						this.arc_expression(26);
						}
						break;

					case 3:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 408;
						if (!(this.precpred(this._ctx, 24))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 24)");
						}
						this.state = 409;
						this.match(ArcSourceCodeParser.MODULO);
						this.state = 410;
						this.arc_expression(25);
						}
						break;

					case 4:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 411;
						if (!(this.precpred(this._ctx, 23))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 23)");
						}
						this.state = 412;
						this.match(ArcSourceCodeParser.PLUS);
						this.state = 413;
						this.arc_expression(24);
						}
						break;

					case 5:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 414;
						if (!(this.precpred(this._ctx, 22))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 22)");
						}
						this.state = 415;
						this.match(ArcSourceCodeParser.MINUS);
						this.state = 416;
						this.arc_expression(23);
						}
						break;

					case 6:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 417;
						if (!(this.precpred(this._ctx, 21))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 21)");
						}
						this.state = 418;
						this.match(ArcSourceCodeParser.BITWISE_LSHIFT);
						this.state = 419;
						this.arc_expression(22);
						}
						break;

					case 7:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 420;
						if (!(this.precpred(this._ctx, 20))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 20)");
						}
						this.state = 421;
						this.match(ArcSourceCodeParser.BITWISE_RSHIFT);
						this.state = 422;
						this.arc_expression(21);
						}
						break;

					case 8:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 423;
						if (!(this.precpred(this._ctx, 19))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 19)");
						}
						this.state = 424;
						this.match(ArcSourceCodeParser.BITWISE_AND);
						this.state = 425;
						this.arc_expression(20);
						}
						break;

					case 9:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 426;
						if (!(this.precpred(this._ctx, 18))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 18)");
						}
						this.state = 427;
						this.match(ArcSourceCodeParser.BITWISE_OR);
						this.state = 428;
						this.arc_expression(19);
						}
						break;

					case 10:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 429;
						if (!(this.precpred(this._ctx, 17))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 17)");
						}
						this.state = 430;
						this.match(ArcSourceCodeParser.BITWISE_XOR);
						this.state = 431;
						this.arc_expression(18);
						}
						break;

					case 11:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 432;
						if (!(this.precpred(this._ctx, 16))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 16)");
						}
						this.state = 433;
						this.match(ArcSourceCodeParser.COMP_LT);
						this.state = 434;
						this.arc_expression(17);
						}
						break;

					case 12:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 435;
						if (!(this.precpred(this._ctx, 15))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 15)");
						}
						this.state = 436;
						this.match(ArcSourceCodeParser.COMP_GT);
						this.state = 437;
						this.arc_expression(16);
						}
						break;

					case 13:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 438;
						if (!(this.precpred(this._ctx, 14))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 14)");
						}
						this.state = 439;
						this.match(ArcSourceCodeParser.COMP_LTE);
						this.state = 440;
						this.arc_expression(15);
						}
						break;

					case 14:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 441;
						if (!(this.precpred(this._ctx, 13))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 13)");
						}
						this.state = 442;
						this.match(ArcSourceCodeParser.COMP_GTE);
						this.state = 443;
						this.arc_expression(14);
						}
						break;

					case 15:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 444;
						if (!(this.precpred(this._ctx, 12))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 12)");
						}
						this.state = 445;
						this.match(ArcSourceCodeParser.COMP_OBJ_EQ);
						this.state = 446;
						this.arc_expression(13);
						}
						break;

					case 16:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 447;
						if (!(this.precpred(this._ctx, 11))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 11)");
						}
						this.state = 448;
						this.match(ArcSourceCodeParser.COMP_REF_EQ);
						this.state = 449;
						this.arc_expression(12);
						}
						break;

					case 17:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 450;
						if (!(this.precpred(this._ctx, 10))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 10)");
						}
						this.state = 451;
						this.match(ArcSourceCodeParser.COMP_OBJ_NEQ);
						this.state = 452;
						this.arc_expression(11);
						}
						break;

					case 18:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 453;
						if (!(this.precpred(this._ctx, 9))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 9)");
						}
						this.state = 454;
						this.match(ArcSourceCodeParser.COMP_REF_NEQ);
						this.state = 455;
						this.arc_expression(10);
						}
						break;

					case 19:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 456;
						if (!(this.precpred(this._ctx, 8))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 8)");
						}
						this.state = 457;
						this.match(ArcSourceCodeParser.LOGICAL_AND);
						this.state = 458;
						this.arc_expression(9);
						}
						break;

					case 20:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 459;
						if (!(this.precpred(this._ctx, 7))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 7)");
						}
						this.state = 460;
						this.match(ArcSourceCodeParser.LOGICAL_OR);
						this.state = 461;
						this.arc_expression(8);
						}
						break;

					case 21:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 462;
						if (!(this.precpred(this._ctx, 6))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 6)");
						}
						this.state = 463;
						this.match(ArcSourceCodeParser.QUESTION);
						this.state = 464;
						this.arc_expression(0);
						this.state = 465;
						this.match(ArcSourceCodeParser.COLON);
						this.state = 466;
						this.arc_expression(7);
						}
						break;

					case 22:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 468;
						if (!(this.precpred(this._ctx, 5))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 5)");
						}
						this.state = 469;
						this.match(ArcSourceCodeParser.RANGE);
						this.state = 470;
						this.arc_expression(6);
						}
						break;

					case 23:
						{
						_localctx = new Arc_expressionContext(_parentctx, _parentState);
						this.pushNewRecursionContext(_localctx, _startState, ArcSourceCodeParser.RULE_arc_expression);
						this.state = 471;
						if (!(this.precpred(this._ctx, 4))) {
							throw this.createFailedPredicateException("this.precpred(this._ctx, 4)");
						}
						this.state = 472;
						this.match(ArcSourceCodeParser.ARROW);
						this.state = 473;
						this.arc_expression(5);
						}
						break;
					}
					}
				}
				this.state = 478;
				this._errHandler.sync(this);
				_alt = this.interpreter.adaptivePredict(this._input, 30, this._ctx);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_wrapped_expression(): Arc_wrapped_expressionContext {
		let _localctx: Arc_wrapped_expressionContext = new Arc_wrapped_expressionContext(this._ctx, this.state);
		this.enterRule(_localctx, 86, ArcSourceCodeParser.RULE_arc_wrapped_expression);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 479;
			this.match(ArcSourceCodeParser.LPAREN);
			this.state = 480;
			this.arc_expression(0);
			this.state = 481;
			this.match(ArcSourceCodeParser.RPAREN);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_namespace_block(): Arc_namespace_blockContext {
		let _localctx: Arc_namespace_blockContext = new Arc_namespace_blockContext(this._ctx, this.state);
		this.enterRule(_localctx, 88, ArcSourceCodeParser.RULE_arc_namespace_block);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 483;
			this.arc_namespace_declarator();
			this.state = 484;
			this.match(ArcSourceCodeParser.LBRACE);
			this.state = 488;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT || ((((_la - 78)) & ~0x1F) === 0 && ((1 << (_la - 78)) & ((1 << (ArcSourceCodeParser.KW_PUBLIC - 78)) | (1 << (ArcSourceCodeParser.KW_INTERNAL - 78)) | (1 << (ArcSourceCodeParser.KW_PROTECTED - 78)) | (1 << (ArcSourceCodeParser.KW_PRIVATE - 78)))) !== 0)) {
				{
				{
				this.state = 485;
				this.arc_namespace_member();
				}
				}
				this.state = 490;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 491;
			this.match(ArcSourceCodeParser.RBRACE);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_namespace_member(): Arc_namespace_memberContext {
		let _localctx: Arc_namespace_memberContext = new Arc_namespace_memberContext(this._ctx, this.state);
		this.enterRule(_localctx, 90, ArcSourceCodeParser.RULE_arc_namespace_member);
		try {
			this.state = 496;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 32, this._ctx) ) {
			case 1:
				this.enterOuterAlt(_localctx, 1);
				{
				this.state = 493;
				this.arc_function_block();
				}
				break;

			case 2:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 494;
				this.arc_group_block();
				}
				break;

			case 3:
				this.enterOuterAlt(_localctx, 3);
				{
				this.state = 495;
				this.arc_enum_declarator();
				}
				break;
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_block(): Arc_group_blockContext {
		let _localctx: Arc_group_blockContext = new Arc_group_blockContext(this._ctx, this.state);
		this.enterRule(_localctx, 92, ArcSourceCodeParser.RULE_arc_group_block);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 501;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 498;
				this.arc_annotation();
				}
				}
				this.state = 503;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 504;
			this.arc_accessibility();
			this.state = 505;
			this.match(ArcSourceCodeParser.KW_GROUP);
			this.state = 506;
			this.arc_single_identifier();
			this.state = 508;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COMP_LT) {
				{
				this.state = 507;
				this.arc_generic_declaration_wrapper();
				}
			}

			this.state = 512;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.COLON) {
				{
				this.state = 510;
				this.match(ArcSourceCodeParser.COLON);
				this.state = 511;
				this.arc_group_derive_list();
				}
			}

			this.state = 514;
			this.arc_wrapped_group_member();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_derive_list(): Arc_group_derive_listContext {
		let _localctx: Arc_group_derive_listContext = new Arc_group_derive_listContext(this._ctx, this.state);
		this.enterRule(_localctx, 94, ArcSourceCodeParser.RULE_arc_group_derive_list);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 516;
			this.arc_data_type();
			this.state = 521;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.COMMA) {
				{
				{
				this.state = 517;
				this.match(ArcSourceCodeParser.COMMA);
				this.state = 518;
				this.arc_data_type();
				}
				}
				this.state = 523;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_wrapped_group_member(): Arc_wrapped_group_memberContext {
		let _localctx: Arc_wrapped_group_memberContext = new Arc_wrapped_group_memberContext(this._ctx, this.state);
		this.enterRule(_localctx, 96, ArcSourceCodeParser.RULE_arc_wrapped_group_member);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 524;
			this.match(ArcSourceCodeParser.LBRACE);
			this.state = 528;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT || ((((_la - 78)) & ~0x1F) === 0 && ((1 << (_la - 78)) & ((1 << (ArcSourceCodeParser.KW_PUBLIC - 78)) | (1 << (ArcSourceCodeParser.KW_INTERNAL - 78)) | (1 << (ArcSourceCodeParser.KW_PROTECTED - 78)) | (1 << (ArcSourceCodeParser.KW_PRIVATE - 78)))) !== 0)) {
				{
				{
				this.state = 525;
				this.arc_group_member();
				}
				}
				this.state = 530;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 531;
			this.match(ArcSourceCodeParser.RBRACE);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_member(): Arc_group_memberContext {
		let _localctx: Arc_group_memberContext = new Arc_group_memberContext(this._ctx, this.state);
		this.enterRule(_localctx, 98, ArcSourceCodeParser.RULE_arc_group_member);
		try {
			this.state = 536;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 38, this._ctx) ) {
			case 1:
				this.enterOuterAlt(_localctx, 1);
				{
				this.state = 533;
				this.arc_group_lifecycle_function();
				}
				break;

			case 2:
				this.enterOuterAlt(_localctx, 2);
				{
				this.state = 534;
				this.arc_group_function();
				}
				break;

			case 3:
				this.enterOuterAlt(_localctx, 3);
				{
				this.state = 535;
				this.arc_group_field();
				}
				break;
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_field(): Arc_group_fieldContext {
		let _localctx: Arc_group_fieldContext = new Arc_group_fieldContext(this._ctx, this.state);
		this.enterRule(_localctx, 100, ArcSourceCodeParser.RULE_arc_group_field);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 541;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 538;
				this.arc_annotation();
				}
				}
				this.state = 543;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 544;
			this.arc_accessibility();
			this.state = 545;
			this.match(ArcSourceCodeParser.KW_FIELD);
			this.state = 546;
			this.arc_data_declarator();
			this.state = 547;
			this.match(ArcSourceCodeParser.SEMICOLON);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_function(): Arc_group_functionContext {
		let _localctx: Arc_group_functionContext = new Arc_group_functionContext(this._ctx, this.state);
		this.enterRule(_localctx, 102, ArcSourceCodeParser.RULE_arc_group_function);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 549;
			this.arc_function_block();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_lifecycle_keyword(): Arc_group_lifecycle_keywordContext {
		let _localctx: Arc_group_lifecycle_keywordContext = new Arc_group_lifecycle_keywordContext(this._ctx, this.state);
		this.enterRule(_localctx, 104, ArcSourceCodeParser.RULE_arc_group_lifecycle_keyword);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 551;
			_la = this._input.LA(1);
			if (!(((((_la - 75)) & ~0x1F) === 0 && ((1 << (_la - 75)) & ((1 << (ArcSourceCodeParser.KW_CONSTRUCTOR - 75)) | (1 << (ArcSourceCodeParser.KW_DESTRUCTOR - 75)) | (1 << (ArcSourceCodeParser.KW_VALUE - 75)) | (1 << (ArcSourceCodeParser.KW_CLONE - 75)))) !== 0))) {
			this._errHandler.recoverInline(this);
			} else {
				if (this._input.LA(1) === Token.EOF) {
					this.matchedEOF = true;
				}

				this._errHandler.reportMatch(this);
				this.consume();
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_group_lifecycle_function(): Arc_group_lifecycle_functionContext {
		let _localctx: Arc_group_lifecycle_functionContext = new Arc_group_lifecycle_functionContext(this._ctx, this.state);
		this.enterRule(_localctx, 106, ArcSourceCodeParser.RULE_arc_group_lifecycle_function);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 556;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 553;
				this.arc_annotation();
				}
				}
				this.state = 558;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 559;
			this.arc_accessibility();
			this.state = 560;
			this.arc_group_lifecycle_keyword();
			this.state = 561;
			this.arc_wrapped_arg_list();
			this.state = 562;
			this.match(ArcSourceCodeParser.COLON);
			this.state = 563;
			this.arc_data_type();
			this.state = 564;
			this.arc_wrapped_function_body();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_index(): Arc_indexContext {
		let _localctx: Arc_indexContext = new Arc_indexContext(this._ctx, this.state);
		this.enterRule(_localctx, 108, ArcSourceCodeParser.RULE_arc_index);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 566;
			this.match(ArcSourceCodeParser.LBRACKET);
			this.state = 567;
			this.arc_expression(0);
			this.state = 568;
			this.match(ArcSourceCodeParser.RBRACKET);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_call_chain(): Arc_call_chainContext {
		let _localctx: Arc_call_chainContext = new Arc_call_chainContext(this._ctx, this.state);
		this.enterRule(_localctx, 110, ArcSourceCodeParser.RULE_arc_call_chain);
		try {
			let _alt: number;
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 572;
			this._errHandler.sync(this);
			switch (this._input.LA(1)) {
			case ArcSourceCodeParser.LBRACKET:
			case ArcSourceCodeParser.KW_SELF:
			case ArcSourceCodeParser.IDENTIFIER:
				{
				this.state = 570;
				this.arc_call_chain_term();
				}
				break;
			case ArcSourceCodeParser.KW_NEW:
				{
				this.state = 571;
				this.arc_constructor_call();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			this.state = 578;
			this._errHandler.sync(this);
			_alt = this.interpreter.adaptivePredict(this._input, 42, this._ctx);
			while (_alt !== 2 && _alt !== ATN.INVALID_ALT_NUMBER) {
				if (_alt === 1) {
					{
					{
					this.state = 574;
					this.match(ArcSourceCodeParser.DOT);
					this.state = 575;
					this.arc_call_chain_term();
					}
					}
				}
				this.state = 580;
				this._errHandler.sync(this);
				_alt = this.interpreter.adaptivePredict(this._input, 42, this._ctx);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_call_chain_term(): Arc_call_chain_termContext {
		let _localctx: Arc_call_chain_termContext = new Arc_call_chain_termContext(this._ctx, this.state);
		this.enterRule(_localctx, 112, ArcSourceCodeParser.RULE_arc_call_chain_term);
		try {
			let _alt: number;
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 584;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 43, this._ctx) ) {
			case 1:
				{
				this.state = 581;
				this.arc_function_call_base();
				}
				break;

			case 2:
				{
				this.state = 582;
				this.arc_flexible_identifier();
				}
				break;

			case 3:
				{
				this.state = 583;
				this.arc_self_wrapper();
				}
				break;
			}
			this.state = 589;
			this._errHandler.sync(this);
			_alt = this.interpreter.adaptivePredict(this._input, 44, this._ctx);
			while (_alt !== 2 && _alt !== ATN.INVALID_ALT_NUMBER) {
				if (_alt === 1) {
					{
					{
					this.state = 586;
					this.arc_index();
					}
					}
				}
				this.state = 591;
				this._errHandler.sync(this);
				_alt = this.interpreter.adaptivePredict(this._input, 44, this._ctx);
			}
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_self_wrapper(): Arc_self_wrapperContext {
		let _localctx: Arc_self_wrapperContext = new Arc_self_wrapperContext(this._ctx, this.state);
		this.enterRule(_localctx, 114, ArcSourceCodeParser.RULE_arc_self_wrapper);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 592;
			this.match(ArcSourceCodeParser.KW_SELF);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_generic_declaration_wrapper(): Arc_generic_declaration_wrapperContext {
		let _localctx: Arc_generic_declaration_wrapperContext = new Arc_generic_declaration_wrapperContext(this._ctx, this.state);
		this.enterRule(_localctx, 116, ArcSourceCodeParser.RULE_arc_generic_declaration_wrapper);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 594;
			this.match(ArcSourceCodeParser.COMP_LT);
			this.state = 595;
			this.arc_single_identifier();
			this.state = 600;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.COMMA) {
				{
				{
				this.state = 596;
				this.match(ArcSourceCodeParser.COMMA);
				this.state = 597;
				this.arc_single_identifier();
				}
				}
				this.state = 602;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 603;
			this.match(ArcSourceCodeParser.COMP_GT);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_generic_specialization_wrapper(): Arc_generic_specialization_wrapperContext {
		let _localctx: Arc_generic_specialization_wrapperContext = new Arc_generic_specialization_wrapperContext(this._ctx, this.state);
		this.enterRule(_localctx, 118, ArcSourceCodeParser.RULE_arc_generic_specialization_wrapper);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 605;
			this.match(ArcSourceCodeParser.COMP_LT);
			this.state = 616;
			this._errHandler.sync(this);
			switch ( this.interpreter.adaptivePredict(this._input, 47, this._ctx) ) {
			case 1:
				{
				{
				this.state = 606;
				this.arc_data_type();
				this.state = 611;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
				while (_la === ArcSourceCodeParser.COMMA) {
					{
					{
					this.state = 607;
					this.match(ArcSourceCodeParser.COMMA);
					this.state = 608;
					this.arc_data_type();
					}
					}
					this.state = 613;
					this._errHandler.sync(this);
					_la = this._input.LA(1);
				}
				}
				}
				break;

			case 2:
				{
				this.state = 614;
				this.match(ArcSourceCodeParser.QUESTION);
				}
				break;

			case 3:
				{
				this.state = 615;
				this.arc_single_identifier();
				}
				break;
			}
			this.state = 618;
			this.match(ArcSourceCodeParser.COMP_GT);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_enum_declarator(): Arc_enum_declaratorContext {
		let _localctx: Arc_enum_declaratorContext = new Arc_enum_declaratorContext(this._ctx, this.state);
		this.enterRule(_localctx, 120, ArcSourceCodeParser.RULE_arc_enum_declarator);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 623;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 620;
				this.arc_annotation();
				}
				}
				this.state = 625;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 626;
			this.arc_accessibility();
			this.state = 627;
			this.match(ArcSourceCodeParser.KW_ENUM);
			this.state = 628;
			this.arc_single_identifier();
			this.state = 629;
			this.match(ArcSourceCodeParser.LBRACE);
			this.state = 638;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			if (_la === ArcSourceCodeParser.AT || _la === ArcSourceCodeParser.IDENTIFIER) {
				{
				this.state = 630;
				this.arc_enum_member();
				this.state = 635;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
				while (_la === ArcSourceCodeParser.COMMA) {
					{
					{
					this.state = 631;
					this.match(ArcSourceCodeParser.COMMA);
					this.state = 632;
					this.arc_enum_member();
					}
					}
					this.state = 637;
					this._errHandler.sync(this);
					_la = this._input.LA(1);
				}
				}
			}

			this.state = 640;
			this.match(ArcSourceCodeParser.RBRACE);
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_enum_member(): Arc_enum_memberContext {
		let _localctx: Arc_enum_memberContext = new Arc_enum_memberContext(this._ctx, this.state);
		this.enterRule(_localctx, 122, ArcSourceCodeParser.RULE_arc_enum_member);
		let _la: number;
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 645;
			this._errHandler.sync(this);
			_la = this._input.LA(1);
			while (_la === ArcSourceCodeParser.AT) {
				{
				{
				this.state = 642;
				this.arc_annotation();
				}
				}
				this.state = 647;
				this._errHandler.sync(this);
				_la = this._input.LA(1);
			}
			this.state = 648;
			this.arc_single_identifier();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}
	// @RuleVersion(0)
	public arc_enum_accessor(): Arc_enum_accessorContext {
		let _localctx: Arc_enum_accessorContext = new Arc_enum_accessorContext(this._ctx, this.state);
		this.enterRule(_localctx, 124, ArcSourceCodeParser.RULE_arc_enum_accessor);
		try {
			this.enterOuterAlt(_localctx, 1);
			{
			this.state = 650;
			this.arc_flexible_identifier();
			this.state = 651;
			this.match(ArcSourceCodeParser.DOT);
			this.state = 652;
			this.arc_single_identifier();
			}
		}
		catch (re) {
			if (re instanceof RecognitionException) {
				_localctx.exception = re;
				this._errHandler.reportError(this, re);
				this._errHandler.recover(this, re);
			} else {
				throw re;
			}
		}
		finally {
			this.exitRule();
		}
		return _localctx;
	}

	public sempred(_localctx: RuleContext, ruleIndex: number, predIndex: number): boolean {
		switch (ruleIndex) {
		case 42:
			return this.arc_expression_sempred(_localctx as Arc_expressionContext, predIndex);
		}
		return true;
	}
	private arc_expression_sempred(_localctx: Arc_expressionContext, predIndex: number): boolean {
		switch (predIndex) {
		case 0:
			return this.precpred(this._ctx, 26);

		case 1:
			return this.precpred(this._ctx, 25);

		case 2:
			return this.precpred(this._ctx, 24);

		case 3:
			return this.precpred(this._ctx, 23);

		case 4:
			return this.precpred(this._ctx, 22);

		case 5:
			return this.precpred(this._ctx, 21);

		case 6:
			return this.precpred(this._ctx, 20);

		case 7:
			return this.precpred(this._ctx, 19);

		case 8:
			return this.precpred(this._ctx, 18);

		case 9:
			return this.precpred(this._ctx, 17);

		case 10:
			return this.precpred(this._ctx, 16);

		case 11:
			return this.precpred(this._ctx, 15);

		case 12:
			return this.precpred(this._ctx, 14);

		case 13:
			return this.precpred(this._ctx, 13);

		case 14:
			return this.precpred(this._ctx, 12);

		case 15:
			return this.precpred(this._ctx, 11);

		case 16:
			return this.precpred(this._ctx, 10);

		case 17:
			return this.precpred(this._ctx, 9);

		case 18:
			return this.precpred(this._ctx, 8);

		case 19:
			return this.precpred(this._ctx, 7);

		case 20:
			return this.precpred(this._ctx, 6);

		case 21:
			return this.precpred(this._ctx, 5);

		case 22:
			return this.precpred(this._ctx, 4);
		}
		return true;
	}

	private static readonly _serializedATNSegments: number = 2;
	private static readonly _serializedATNSegment0: string =
		"\x03\uC91D\uCABA\u058D\uAFBA\u4F53\u0607\uEA8B\uC241\x03u\u0291\x04\x02" +
		"\t\x02\x04\x03\t\x03\x04\x04\t\x04\x04\x05\t\x05\x04\x06\t\x06\x04\x07" +
		"\t\x07\x04\b\t\b\x04\t\t\t\x04\n\t\n\x04\v\t\v\x04\f\t\f\x04\r\t\r\x04" +
		"\x0E\t\x0E\x04\x0F\t\x0F\x04\x10\t\x10\x04\x11\t\x11\x04\x12\t\x12\x04" +
		"\x13\t\x13\x04\x14\t\x14\x04\x15\t\x15\x04\x16\t\x16\x04\x17\t\x17\x04" +
		"\x18\t\x18\x04\x19\t\x19\x04\x1A\t\x1A\x04\x1B\t\x1B\x04\x1C\t\x1C\x04" +
		"\x1D\t\x1D\x04\x1E\t\x1E\x04\x1F\t\x1F\x04 \t \x04!\t!\x04\"\t\"\x04#" +
		"\t#\x04$\t$\x04%\t%\x04&\t&\x04\'\t\'\x04(\t(\x04)\t)\x04*\t*\x04+\t+" +
		"\x04,\t,\x04-\t-\x04.\t.\x04/\t/\x040\t0\x041\t1\x042\t2\x043\t3\x044" +
		"\t4\x045\t5\x046\t6\x047\t7\x048\t8\x049\t9\x04:\t:\x04;\t;\x04<\t<\x04" +
		"=\t=\x04>\t>\x04?\t?\x04@\t@\x03\x02\x07\x02\x82\n\x02\f\x02\x0E\x02\x85" +
		"\v\x02\x03\x02\x03\x02\x03\x02\x03\x03\x03\x03\x03\x04\x03\x04\x03\x05" +
		"\x03\x05\x03\x05\x07\x05\x91\n\x05\f\x05\x0E\x05\x94\v\x05\x03\x06\x03" +
		"\x06\x03\x06\x03\x07\x03\x07\x03\x07\x03\x07\x03\b\x03\b\x03\t\x03\t\x03" +
		"\t\x03\t\x03\n\x03\n\x05\n\xA5\n\n\x03\v\x03\v\x03\f\x03\f\x03\f\x03\r" +
		"\x03\r\x05\r\xAE\n\r\x03\r\x05\r\xB1\n\r\x03\r\x07\r\xB4\n\r\f\r\x0E\r" +
		"\xB7\v\r\x03\x0E\x03\x0E\x03\x0E\x03\x0E\x03\x0E\x03\x0F\x03\x0F\x03\x0F" +
		"\x03\x0F\x03\x0F\x03\x10\x03\x10\x05\x10\xC5\n\x10\x03\x10\x03\x10\x07" +
		"\x10\xC9\n\x10\f\x10\x0E\x10\xCC\v\x10\x03\x11\x03\x11\x05\x11\xD0\n\x11" +
		"\x03\x11\x03\x11\x03\x12\x03\x12\x03\x12\x07\x12\xD7\n\x12\f\x12\x0E\x12" +
		"\xDA\v\x12\x03\x13\x03\x13\x05\x13\xDE\n\x13\x03\x13\x03\x13\x03\x14\x03" +
		"\x14\x03\x14\x05\x14\xE5\n\x14\x03\x15\x07\x15\xE8\n\x15\f\x15\x0E\x15" +
		"\xEB\v\x15\x03\x15\x03\x15\x03\x15\x03\x15\x05\x15\xF1\n\x15\x03\x15\x03" +
		"\x15\x03\x15\x03\x15\x03\x16\x03\x16\x05\x16\xF9\n\x16\x03\x16\x03\x16" +
		"\x03\x17\x03\x17\x07\x17\xFF\n\x17\f\x17\x0E\x17\u0102\v\x17\x03\x17\x03" +
		"\x17\x03\x18\x03\x18\x03\x18\x03\x19\x03\x19\x03\x1A\x03\x1A\x03\x1A\x03" +
		"\x1A\x03\x1A\x05\x1A\u0110\n\x1A\x03\x1B\x03\x1B\x03\x1B\x03\x1B\x03\x1B" +
		"\x03\x1C\x03\x1C\x03\x1C\x03\x1C\x05\x1C\u011B\n\x1C\x03\x1D\x03\x1D\x03" +
		"\x1D\x05\x1D\u0120\n\x1D\x03\x1D\x03\x1D\x03\x1E\x03\x1E\x03\x1E\x03\x1E" +
		"\x03\x1E\x03\x1E\x03\x1E\x03\x1E\x05\x1E\u012C\n\x1E\x03\x1E\x03\x1E\x03" +
		"\x1E\x03\x1E\x03\x1E\x03\x1E\x03\x1E\x05\x1E\u0135\n\x1E\x05\x1E\u0137" +
		"\n\x1E\x03\x1F\x03\x1F\x03\x1F\x03\x1F\x03 \x03 \x05 \u013F\n \x03!\x03" +
		"!\x03!\x05!\u0144\n!\x03\"\x03\"\x03\"\x03\"\x03#\x03#\x03$\x03$\x03%" +
		"\x03%\x03%\x05%\u0151\n%\x03&\x03&\x03&\x03&\x03&\x03&\x03\'\x03\'\x03" +
		"\'\x03\'\x03\'\x03\'\x03\'\x03\'\x03\'\x03\'\x03(\x03(\x03(\x03)\x03)" +
		"\x03)\x03)\x03)\x03)\x03)\x03)\x03*\x03*\x03*\x03+\x03+\x03+\x03+\x03" +
		"+\x03+\x03+\x03+\x03+\x03+\x03+\x07+\u017C\n+\f+\x0E+\u017F\v+\x03+\x03" +
		"+\x05+\u0183\n+\x03,\x03,\x03,\x03,\x03,\x03,\x05,\u018B\n,\x03,\x03," +
		"\x03,\x03,\x03,\x03,\x05,\u0193\n,\x03,\x03,\x03,\x03,\x03,\x03,\x03," +
		"\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03" +
		",\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03" +
		",\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03" +
		",\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03" +
		",\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x03,\x07,\u01DD\n,\f,\x0E,\u01E0" +
		"\v,\x03-\x03-\x03-\x03-\x03.\x03.\x03.\x07.\u01E9\n.\f.\x0E.\u01EC\v." +
		"\x03.\x03.\x03/\x03/\x03/\x05/\u01F3\n/\x030\x070\u01F6\n0\f0\x0E0\u01F9" +
		"\v0\x030\x030\x030\x030\x050\u01FF\n0\x030\x030\x050\u0203\n0\x030\x03" +
		"0\x031\x031\x031\x071\u020A\n1\f1\x0E1\u020D\v1\x032\x032\x072\u0211\n" +
		"2\f2\x0E2\u0214\v2\x032\x032\x033\x033\x033\x053\u021B\n3\x034\x074\u021E" +
		"\n4\f4\x0E4\u0221\v4\x034\x034\x034\x034\x034\x035\x035\x036\x036\x03" +
		"7\x077\u022D\n7\f7\x0E7\u0230\v7\x037\x037\x037\x037\x037\x037\x037\x03" +
		"8\x038\x038\x038\x039\x039\x059\u023F\n9\x039\x039\x079\u0243\n9\f9\x0E" +
		"9\u0246\v9\x03:\x03:\x03:\x05:\u024B\n:\x03:\x07:\u024E\n:\f:\x0E:\u0251" +
		"\v:\x03;\x03;\x03<\x03<\x03<\x03<\x07<\u0259\n<\f<\x0E<\u025C\v<\x03<" +
		"\x03<\x03=\x03=\x03=\x03=\x07=\u0264\n=\f=\x0E=\u0267\v=\x03=\x03=\x05" +
		"=\u026B\n=\x03=\x03=\x03>\x07>\u0270\n>\f>\x0E>\u0273\v>\x03>\x03>\x03" +
		">\x03>\x03>\x03>\x03>\x07>\u027C\n>\f>\x0E>\u027F\v>\x05>\u0281\n>\x03" +
		">\x03>\x03?\x07?\u0286\n?\f?\x0E?\u0289\v?\x03?\x03?\x03@\x03@\x03@\x03" +
		"@\x03@\x02\x02\x03VA\x02\x02\x04\x02\x06\x02\b\x02\n\x02\f\x02\x0E\x02" +
		"\x10\x02\x12\x02\x14\x02\x16\x02\x18\x02\x1A\x02\x1C\x02\x1E\x02 \x02" +
		"\"\x02$\x02&\x02(\x02*\x02,\x02.\x020\x022\x024\x026\x028\x02:\x02<\x02" +
		">\x02@\x02B\x02D\x02F\x02H\x02J\x02L\x02N\x02P\x02R\x02T\x02V\x02X\x02" +
		"Z\x02\\\x02^\x02`\x02b\x02d\x02f\x02h\x02j\x02l\x02n\x02p\x02r\x02t\x02" +
		"v\x02x\x02z\x02|\x02~\x02\x02\b\x03\x02PS\x03\x02UV\x03\x02bj\x03\x02" +
		"`a\x03\x02\x19\x1A\x05\x02MNWWYY\x02\u02AF\x02\x83\x03\x02\x02\x02\x04" +
		"\x89\x03\x02\x02\x02\x06\x8B\x03\x02\x02\x02\b\x8D\x03\x02\x02\x02\n\x95" +
		"\x03\x02\x02\x02\f\x98\x03\x02\x02\x02\x0E\x9C\x03\x02\x02\x02\x10\x9E" +
		"\x03\x02\x02\x02\x12\xA4\x03\x02\x02\x02\x14\xA6\x03\x02\x02\x02\x16\xA8" +
		"\x03\x02\x02\x02\x18\xAD\x03\x02\x02\x02\x1A\xB8\x03\x02\x02\x02\x1C\xBD" +
		"\x03\x02\x02\x02\x1E\xC4\x03\x02\x02\x02 \xCD\x03\x02\x02\x02\"\xD3\x03" +
		"\x02\x02\x02$\xDB\x03\x02\x02\x02&\xE1\x03\x02\x02\x02(\xE9\x03\x02\x02" +
		"\x02*\xF6\x03\x02\x02\x02,\xFC\x03\x02\x02\x02.\u0105\x03\x02\x02\x02" +
		"0\u0108\x03\x02\x02\x022\u010F\x03\x02\x02\x024\u0111\x03\x02\x02\x02" +
		"6\u011A\x03\x02\x02\x028\u011C\x03\x02\x02\x02:\u0136\x03\x02\x02\x02" +
		"<\u0138\x03\x02\x02\x02>\u013C\x03\x02\x02\x02@\u0140\x03\x02\x02\x02" +
		"B\u0145\x03\x02\x02\x02D\u0149\x03\x02\x02\x02F\u014B\x03\x02\x02\x02" +
		"H\u014D\x03\x02\x02\x02J\u0152\x03\x02\x02\x02L\u0158\x03\x02\x02\x02" +
		"N\u0162\x03\x02\x02\x02P\u0165\x03\x02\x02\x02R\u016D\x03\x02\x02\x02" +
		"T\u0170\x03\x02\x02\x02V\u0192\x03\x02\x02\x02X\u01E1\x03\x02\x02\x02" +
		"Z\u01E5\x03\x02\x02\x02\\\u01F2\x03\x02\x02\x02^\u01F7\x03\x02\x02\x02" +
		"`\u0206\x03\x02\x02\x02b\u020E\x03\x02\x02\x02d\u021A\x03\x02\x02\x02" +
		"f\u021F\x03\x02\x02\x02h\u0227\x03\x02\x02\x02j\u0229\x03\x02\x02\x02" +
		"l\u022E\x03\x02\x02\x02n\u0238\x03\x02\x02\x02p\u023E\x03\x02\x02\x02" +
		"r\u024A\x03\x02\x02\x02t\u0252\x03\x02\x02\x02v\u0254\x03\x02\x02\x02" +
		"x\u025F\x03\x02\x02\x02z\u0271\x03\x02\x02\x02|\u0287\x03\x02\x02\x02" +
		"~\u028C\x03\x02\x02\x02\x80\x82\x05<\x1F\x02\x81\x80\x03\x02\x02\x02\x82" +
		"\x85\x03\x02\x02\x02\x83\x81\x03\x02\x02\x02\x83\x84\x03\x02\x02\x02\x84" +
		"\x86\x03\x02\x02\x02\x85\x83\x03\x02\x02\x02\x86\x87\x05Z.\x02\x87\x88" +
		"\x07\x02\x02\x03\x88\x03\x03\x02\x02\x02\x89\x8A\t\x02\x02\x02\x8A\x05" +
		"\x03\x02\x02\x02\x8B\x8C\t\x03\x02\x02\x8C\x07\x03\x02\x02\x02\x8D\x92" +
		"\x07t\x02\x02\x8E\x8F\x073\x02\x02\x8F\x91\x07t\x02\x02\x90\x8E\x03\x02" +
		"\x02\x02\x91\x94\x03\x02\x02\x02\x92\x90\x03\x02\x02\x02\x92\x93\x03\x02" +
		"\x02\x02\x93\t\x03\x02\x02\x02\x94\x92\x03\x02\x02\x02\x95\x96\x07^\x02" +
		"\x02\x96\x97\x05\b\x05\x02\x97\v\x03\x02\x02\x02\x98\x99\x07$\x02\x02" +
		"\x99\x9A\x05\b\x05\x02\x9A\x9B\x07%\x02\x02\x9B\r\x03\x02\x02\x02\x9C" +
		"\x9D\x07t\x02\x02\x9D\x0F\x03\x02\x02\x02\x9E\x9F\x05\f\x07\x02\x9F\xA0" +
		"\x070\x02\x02\xA0\xA1\x05\x0E\b\x02\xA1\x11\x03\x02\x02\x02\xA2\xA5\x05" +
		"\x10\t\x02\xA3\xA5\x05\x0E\b\x02\xA4\xA2\x03\x02\x02\x02\xA4\xA3\x03\x02" +
		"\x02\x02\xA5\x13\x03\x02\x02\x02\xA6\xA7\t\x04\x02\x02\xA7\x15\x03\x02" +
		"\x02\x02\xA8\xA9\x07$\x02\x02\xA9\xAA\x07%\x02\x02\xAA\x17\x03\x02\x02" +
		"\x02\xAB\xAE\x05\x14\v\x02\xAC\xAE\x05\x12\n\x02\xAD\xAB\x03\x02\x02\x02" +
		"\xAD\xAC\x03\x02\x02\x02\xAE\xB0\x03\x02\x02\x02\xAF\xB1\x05x=\x02\xB0" +
		"\xAF\x03\x02\x02\x02\xB0\xB1\x03\x02\x02\x02\xB1\xB5\x03\x02\x02\x02\xB2" +
		"\xB4\x05\x16\f\x02\xB3\xB2\x03\x02\x02\x02\xB4\xB7\x03\x02\x02\x02\xB5" +
		"\xB3\x03\x02\x02\x02\xB5\xB6\x03\x02\x02\x02\xB6\x19\x03\x02\x02\x02\xB7" +
		"\xB5\x03\x02\x02\x02\xB8\xB9\x05\x06\x04\x02\xB9\xBA\x05\x0E\b\x02\xBA" +
		"\xBB\x07/\x02\x02\xBB\xBC\x05\x18\r\x02\xBC\x1B\x03\x02\x02\x02\xBD\xBE" +
		"\x05\x06\x04\x02\xBE\xBF\x05t;\x02\xBF\xC0\x07/\x02\x02\xC0\xC1\x05\x18" +
		"\r\x02\xC1\x1D\x03\x02\x02\x02\xC2\xC5\x05\x1A\x0E\x02\xC3\xC5\x05\x1C" +
		"\x0F\x02\xC4\xC2\x03\x02\x02\x02\xC4\xC3\x03\x02\x02\x02\xC5\xCA\x03\x02" +
		"\x02\x02\xC6\xC7\x07,\x02\x02\xC7\xC9\x05\x1A\x0E\x02\xC8\xC6\x03\x02" +
		"\x02\x02\xC9\xCC\x03\x02\x02\x02\xCA\xC8\x03\x02\x02\x02\xCA\xCB\x03\x02" +
		"\x02\x02\xCB\x1F\x03\x02\x02\x02\xCC\xCA\x03\x02\x02\x02\xCD\xCF\x07\"" +
		"\x02\x02\xCE\xD0\x05\x1E\x10\x02\xCF\xCE\x03\x02\x02\x02\xCF\xD0\x03\x02" +
		"\x02\x02\xD0\xD1\x03\x02\x02\x02\xD1\xD2\x07#\x02\x02\xD2!\x03\x02\x02" +
		"\x02\xD3\xD8\x05V,\x02\xD4\xD5\x07,\x02\x02\xD5\xD7\x05V,\x02\xD6\xD4" +
		"\x03\x02\x02\x02\xD7\xDA\x03\x02\x02\x02\xD8\xD6\x03\x02\x02\x02\xD8\xD9" +
		"\x03\x02\x02\x02\xD9#\x03\x02\x02\x02\xDA\xD8\x03\x02\x02\x02\xDB\xDD" +
		"\x07\"\x02\x02\xDC\xDE\x05\"\x12\x02\xDD\xDC\x03\x02\x02\x02\xDD\xDE\x03" +
		"\x02\x02\x02\xDE\xDF\x03\x02\x02\x02\xDF\xE0\x07#\x02\x02\xE0%\x03\x02" +
		"\x02\x02\xE1\xE2\x07+\x02\x02\xE2\xE4\x05\x12\n\x02\xE3\xE5\x05$\x13\x02" +
		"\xE4\xE3\x03\x02\x02\x02\xE4\xE5\x03\x02\x02\x02\xE5\'\x03\x02\x02\x02" +
		"\xE6\xE8\x05&\x14\x02\xE7\xE6\x03\x02\x02\x02\xE8\xEB\x03\x02\x02\x02" +
		"\xE9\xE7\x03\x02\x02\x02\xE9\xEA\x03\x02\x02\x02\xEA\xEC\x03\x02\x02\x02" +
		"\xEB\xE9\x03\x02\x02\x02\xEC\xED\x05\x04\x03\x02\xED\xEE\x07J\x02\x02" +
		"\xEE\xF0\x05\x0E\b\x02\xEF\xF1\x05v<\x02\xF0\xEF\x03\x02\x02\x02\xF0\xF1" +
		"\x03\x02\x02\x02\xF1\xF2\x03\x02\x02\x02\xF2\xF3\x05 \x11\x02\xF3\xF4" +
		"\x07/\x02\x02\xF4\xF5\x05\x18\r\x02\xF5)\x03\x02\x02\x02\xF6\xF8\x05\x12" +
		"\n\x02\xF7\xF9\x05x=\x02\xF8\xF7\x03\x02\x02\x02\xF8\xF9\x03\x02\x02\x02" +
		"\xF9\xFA\x03\x02\x02\x02\xFA\xFB\x05$\x13\x02\xFB+\x03\x02\x02\x02\xFC" +
		"\u0100\x07&\x02\x02\xFD\xFF\x05:\x1E\x02\xFE\xFD\x03\x02\x02\x02\xFF\u0102" +
		"\x03\x02\x02\x02\u0100\xFE\x03\x02\x02\x02\u0100\u0101\x03\x02\x02\x02" +
		"\u0101\u0103\x03\x02\x02\x02\u0102\u0100\x03\x02\x02\x02\u0103\u0104\x07" +
		"\'\x02\x02\u0104-\x03\x02\x02\x02\u0105\u0106\x05(\x15\x02\u0106\u0107" +
		"\x05,\x17\x02\u0107/\x03\x02\x02\x02\u0108\u0109\t\x05\x02\x02\u01091" +
		"\x03\x02\x02\x02\u010A\u0110\x07u\x02\x02\u010B\u0110\x07q\x02\x02\u010C" +
		"\u0110\x050\x19\x02\u010D\u0110\x07b\x02\x02\u010E\u0110\x07c\x02\x02" +
		"\u010F\u010A\x03\x02\x02\x02\u010F\u010B\x03\x02\x02\x02\u010F\u010C\x03" +
		"\x02\x02\x02\u010F\u010D\x03\x02\x02\x02\u010F\u010E\x03\x02\x02\x02\u0110" +
		"3\x03\x02\x02\x02\u0111\u0112\x07n\x02\x02\u0112\u0113\x07\"\x02\x02\u0113" +
		"\u0114\x05\x18\r\x02\u0114\u0115\x07#\x02\x02\u01155\x03\x02\x02\x02\u0116" +
		"\u011B\x052\x1A\x02\u0117\u011B\x054\x1B\x02\u0118\u011B\x05p9\x02\u0119" +
		"\u011B\x05~@\x02\u011A\u0116\x03\x02\x02\x02\u011A\u0117\x03\x02\x02\x02" +
		"\u011A\u0118\x03\x02\x02\x02\u011A\u0119\x03\x02\x02\x02\u011B7\x03\x02" +
		"\x02\x02\u011C\u011D\x07[\x02\x02\u011D\u011F\x05\x12\n\x02\u011E\u0120" +
		"\x05x=\x02\u011F\u011E\x03\x02\x02\x02\u011F\u0120\x03\x02\x02\x02\u0120" +
		"\u0121\x03\x02\x02\x02\u0121\u0122\x05$\x13\x02\u01229\x03\x02\x02\x02" +
		"\u0123\u012C\x05@!\x02\u0124\u012C\x05B\"\x02\u0125\u012C\x05> \x02\u0126" +
		"\u012C\x05B\"\x02\u0127\u012C\x05D#\x02\u0128\u012C\x05F$\x02\u0129\u012C" +
		"\x05H%\x02\u012A\u012C\x05R*\x02\u012B\u0123\x03\x02\x02\x02\u012B\u0124" +
		"\x03\x02\x02\x02\u012B\u0125\x03\x02\x02\x02\u012B\u0126\x03\x02\x02\x02" +
		"\u012B\u0127\x03\x02\x02\x02\u012B\u0128\x03\x02\x02\x02\u012B\u0129\x03" +
		"\x02\x02\x02\u012B\u012A\x03\x02\x02\x02\u012C\u012D\x03\x02\x02\x02\u012D" +
		"\u012E\x07)\x02\x02\u012E\u0137\x03\x02\x02\x02\u012F\u0135\x05J&\x02" +
		"\u0130\u0135\x05N(\x02\u0131\u0135\x05L\'\x02\u0132\u0135\x05P)\x02\u0133" +
		"\u0135\x05T+\x02\u0134\u012F\x03\x02\x02\x02\u0134\u0130\x03\x02\x02\x02" +
		"\u0134\u0131\x03\x02\x02\x02\u0134\u0132\x03\x02\x02\x02\u0134\u0133\x03" +
		"\x02\x02\x02\u0135\u0137\x03\x02\x02\x02\u0136\u012B\x03\x02\x02\x02\u0136" +
		"\u0134\x03\x02\x02\x02\u0137;\x03\x02\x02\x02\u0138\u0139\x07_\x02\x02" +
		"\u0139\u013A\x05\b\x05\x02\u013A\u013B\x07)\x02\x02\u013B=\x03\x02\x02" +
		"\x02\u013C\u013E\x07>\x02\x02\u013D\u013F\x05V,\x02\u013E\u013D\x03\x02" +
		"\x02\x02\u013E\u013F\x03\x02\x02\x02\u013F?\x03\x02\x02\x02\u0140\u0143" +
		"\x05\x1A\x0E\x02\u0141\u0142\x07\x19\x02\x02\u0142\u0144\x05V,\x02\u0143" +
		"\u0141\x03\x02\x02\x02\u0143\u0144\x03\x02\x02\x02\u0144A\x03\x02\x02" +
		"\x02\u0145\u0146\x05p9\x02\u0146\u0147\t\x06\x02\x02\u0147\u0148\x05V" +
		",\x02\u0148C\x03\x02\x02\x02\u0149\u014A\x07<\x02\x02\u014AE\x03\x02\x02" +
		"\x02\u014B\u014C\x07=\x02\x02\u014CG\x03\x02\x02\x02\u014D\u0150\x07Z" +
		"\x02\x02\u014E\u0151\x05*\x16\x02\u014F\u0151\x05p9\x02\u0150\u014E\x03" +
		"\x02\x02\x02\u0150\u014F\x03\x02\x02\x02\u0151I\x03\x02\x02\x02\u0152" +
		"\u0153\x077\x02\x02\u0153\u0154\x07\"\x02\x02\u0154\u0155\x05V,\x02\u0155" +
		"\u0156\x07#\x02\x02\u0156\u0157\x05,\x17\x02\u0157K\x03\x02\x02\x02\u0158" +
		"\u0159\x078\x02\x02\u0159\u015A\x07\"\x02\x02\u015A\u015B\x05@!\x02\u015B" +
		"\u015C\x07)\x02\x02\u015C\u015D\x05V,\x02\u015D\u015E\x07)\x02\x02\u015E" +
		"\u015F\x05B\"\x02\u015F\u0160\x07#\x02\x02\u0160\u0161\x05,\x17\x02\u0161" +
		"M\x03\x02\x02\x02\u0162\u0163\x079\x02\x02\u0163\u0164\x05,\x17\x02\u0164" +
		"O\x03\x02\x02\x02\u0165\u0166\x07:\x02\x02\u0166\u0167\x07\"\x02\x02\u0167" +
		"\u0168\x05\x1A\x0E\x02\u0168\u0169\x07;\x02\x02\u0169\u016A\x05V,\x02" +
		"\u016A\u016B\x07#\x02\x02\u016B\u016C\x05,\x17\x02\u016CQ\x03\x02\x02" +
		"\x02\u016D\u016E\x07?\x02\x02\u016E\u016F\x05V,\x02\u016FS\x03\x02\x02" +
		"\x02\u0170\u0171\x074\x02\x02\u0171\u0172\x07\"\x02\x02\u0172\u0173\x05" +
		"V,\x02\u0173\u0174\x07#\x02\x02\u0174\u017D\x05,\x17\x02\u0175\u0176\x07" +
		"5\x02\x02\u0176\u0177\x07\"\x02\x02\u0177\u0178\x05V,\x02\u0178\u0179" +
		"\x07#\x02\x02\u0179\u017A\x05,\x17\x02\u017A\u017C\x03\x02\x02\x02\u017B" +
		"\u0175\x03\x02\x02\x02\u017C\u017F\x03\x02\x02\x02\u017D\u017B\x03\x02" +
		"\x02\x02\u017D\u017E\x03\x02\x02\x02\u017E\u0182\x03\x02\x02\x02\u017F" +
		"\u017D\x03\x02\x02\x02\u0180\u0181\x076\x02\x02\u0181\u0183\x05,\x17\x02" +
		"\u0182\u0180\x03\x02\x02\x02\u0182\u0183\x03\x02\x02\x02\u0183U\x03\x02" +
		"\x02\x02\u0184\u0185\b,\x01\x02\u0185\u0193\x056\x1C\x02\u0186\u0193\x05" +
		"X-\x02\u0187\u018A\x07\x18\x02\x02\u0188\u018B\x056\x1C\x02\u0189\u018B" +
		"\x05X-\x02\u018A\u0188\x03\x02\x02\x02\u018A\u0189\x03\x02\x02\x02\u018B" +
		"\u0193\x03\x02\x02\x02\u018C\u018D\x056\x1C\x02\u018D\u018E\x07\r\x02" +
		"\x02\u018E\u0193\x03\x02\x02\x02\u018F\u0190\x05X-\x02\u0190\u0191\x07" +
		"\r\x02\x02\u0191\u0193\x03\x02\x02\x02\u0192\u0184\x03\x02\x02\x02\u0192" +
		"\u0186\x03\x02\x02\x02\u0192\u0187\x03\x02\x02\x02\u0192\u018C\x03\x02" +
		"\x02\x02\u0192\u018F\x03\x02\x02\x02\u0193\u01DE\x03\x02\x02\x02\u0194" +
		"\u0195\f\x1C\x02\x02\u0195\u0196\x07\x05\x02\x02\u0196\u01DD\x05V,\x1D" +
		"\u0197\u0198\f\x1B\x02\x02\u0198\u0199\x07\x06\x02\x02\u0199\u01DD\x05" +
		"V,\x1C\u019A\u019B\f\x1A\x02\x02\u019B\u019C\x07\x07\x02\x02\u019C\u01DD" +
		"\x05V,\x1B\u019D\u019E\f\x19\x02\x02\u019E\u019F\x07\x03\x02\x02\u019F" +
		"\u01DD\x05V,\x1A\u01A0\u01A1\f\x18\x02\x02\u01A1\u01A2\x07\x04\x02\x02" +
		"\u01A2\u01DD\x05V,\x19\u01A3\u01A4\f\x17\x02\x02\u01A4\u01A5\x07\n\x02" +
		"\x02\u01A5\u01DD\x05V,\x18\u01A6\u01A7\f\x16\x02\x02\u01A7\u01A8\x07\v" +
		"\x02\x02\u01A8\u01DD\x05V,\x17\u01A9\u01AA\f\x15\x02\x02\u01AA\u01AB\x07" +
		"\b\x02\x02\u01AB\u01DD\x05V,\x16\u01AC\u01AD\f\x14\x02\x02\u01AD\u01AE" +
		"\x07\t\x02\x02\u01AE\u01DD\x05V,\x15\u01AF\u01B0\f\x13\x02\x02\u01B0\u01B1" +
		"\x07\f\x02\x02\u01B1\u01DD\x05V,\x14\u01B2\u01B3\f\x12\x02\x02\u01B3\u01B4" +
		"\x07\x12\x02\x02\u01B4\u01DD\x05V,\x13\u01B5\u01B6\f\x11\x02\x02\u01B6" +
		"\u01B7\x07\x13\x02\x02\u01B7\u01DD\x05V,\x12\u01B8\u01B9\f\x10\x02\x02" +
		"\u01B9\u01BA\x07\x14\x02\x02\u01BA\u01DD\x05V,\x11\u01BB\u01BC\f\x0F\x02" +
		"\x02\u01BC\u01BD\x07\x15\x02\x02\u01BD\u01DD\x05V,\x10\u01BE\u01BF\f\x0E" +
		"\x02\x02\u01BF\u01C0\x07\x0E\x02\x02\u01C0\u01DD\x05V,\x0F\u01C1\u01C2" +
		"\f\r\x02\x02\u01C2\u01C3\x07\x0F\x02\x02\u01C3\u01DD\x05V,\x0E\u01C4\u01C5" +
		"\f\f\x02\x02\u01C5\u01C6\x07\x10\x02\x02\u01C6\u01DD\x05V,\r\u01C7\u01C8" +
		"\f\v\x02\x02\u01C8\u01C9\x07\x11\x02\x02\u01C9\u01DD\x05V,\f\u01CA\u01CB" +
		"\f\n\x02\x02\u01CB\u01CC\x07\x16\x02\x02\u01CC\u01DD\x05V,\v\u01CD\u01CE" +
		"\f\t\x02\x02\u01CE\u01CF\x07\x17\x02\x02\u01CF\u01DD\x05V,\n\u01D0\u01D1" +
		"\f\b\x02\x02\u01D1\u01D2\x07(\x02\x02\u01D2\u01D3\x05V,\x02\u01D3\u01D4" +
		"\x07/\x02\x02\u01D4\u01D5\x05V,\t\u01D5\u01DD\x03\x02\x02\x02\u01D6\u01D7" +
		"\f\x07\x02\x02\u01D7\u01D8\x07*\x02\x02\u01D8\u01DD\x05V,\b\u01D9\u01DA" +
		"\f\x06\x02\x02\u01DA\u01DB\x071\x02\x02\u01DB\u01DD\x05V,\x07\u01DC\u0194" +
		"\x03\x02\x02\x02\u01DC\u0197\x03\x02\x02\x02\u01DC\u019A\x03\x02\x02\x02" +
		"\u01DC\u019D\x03\x02\x02\x02\u01DC\u01A0\x03\x02\x02\x02\u01DC\u01A3\x03" +
		"\x02\x02\x02\u01DC\u01A6\x03\x02\x02\x02\u01DC\u01A9\x03\x02\x02\x02\u01DC" +
		"\u01AC\x03\x02\x02\x02\u01DC\u01AF\x03\x02\x02\x02\u01DC\u01B2\x03\x02" +
		"\x02\x02\u01DC\u01B5\x03\x02\x02\x02\u01DC\u01B8\x03\x02\x02\x02\u01DC" +
		"\u01BB\x03\x02\x02\x02\u01DC\u01BE\x03\x02\x02\x02\u01DC\u01C1\x03\x02" +
		"\x02\x02\u01DC\u01C4\x03\x02\x02\x02\u01DC\u01C7\x03\x02\x02\x02\u01DC" +
		"\u01CA\x03\x02\x02\x02\u01DC\u01CD\x03\x02\x02\x02\u01DC\u01D0\x03\x02" +
		"\x02\x02\u01DC\u01D6\x03\x02\x02\x02\u01DC\u01D9\x03\x02\x02\x02\u01DD" +
		"\u01E0\x03\x02\x02\x02\u01DE\u01DC\x03\x02\x02\x02\u01DE\u01DF\x03\x02" +
		"\x02\x02\u01DFW\x03\x02\x02\x02\u01E0\u01DE\x03\x02\x02\x02\u01E1\u01E2" +
		"\x07\"\x02\x02\u01E2\u01E3\x05V,\x02\u01E3\u01E4\x07#\x02\x02\u01E4Y\x03" +
		"\x02\x02\x02\u01E5\u01E6\x05\n\x06\x02\u01E6\u01EA\x07&\x02\x02\u01E7" +
		"\u01E9\x05\\/\x02\u01E8\u01E7\x03\x02\x02\x02\u01E9\u01EC\x03\x02\x02" +
		"\x02\u01EA\u01E8\x03\x02\x02\x02\u01EA\u01EB\x03\x02\x02\x02\u01EB\u01ED" +
		"\x03\x02\x02\x02\u01EC\u01EA\x03\x02\x02\x02\u01ED\u01EE\x07\'\x02\x02" +
		"\u01EE[\x03\x02\x02\x02\u01EF\u01F3\x05.\x18\x02\u01F0\u01F3\x05^0\x02" +
		"\u01F1\u01F3\x05z>\x02\u01F2\u01EF\x03\x02\x02\x02\u01F2\u01F0\x03\x02" +
		"\x02\x02\u01F2\u01F1\x03\x02\x02\x02\u01F3]\x03\x02\x02\x02\u01F4\u01F6" +
		"\x05&\x14\x02\u01F5\u01F4\x03\x02\x02\x02\u01F6\u01F9\x03\x02\x02\x02" +
		"\u01F7\u01F5\x03\x02\x02\x02\u01F7\u01F8\x03\x02\x02\x02\u01F8\u01FA\x03" +
		"\x02\x02\x02\u01F9\u01F7\x03\x02\x02\x02\u01FA\u01FB\x05\x04\x03\x02\u01FB" +
		"\u01FC\x07K\x02\x02\u01FC\u01FE\x05\x0E\b\x02\u01FD\u01FF\x05v<\x02\u01FE" +
		"\u01FD\x03\x02\x02\x02\u01FE\u01FF\x03\x02\x02\x02\u01FF\u0202\x03\x02" +
		"\x02\x02\u0200\u0201\x07/\x02\x02\u0201\u0203\x05`1\x02\u0202\u0200\x03" +
		"\x02\x02\x02\u0202\u0203\x03\x02\x02\x02\u0203\u0204\x03\x02\x02\x02\u0204" +
		"\u0205\x05b2\x02\u0205_\x03\x02\x02\x02\u0206\u020B\x05\x18\r\x02\u0207" +
		"\u0208\x07,\x02\x02\u0208\u020A\x05\x18\r\x02\u0209\u0207\x03\x02\x02" +
		"\x02\u020A\u020D\x03\x02\x02\x02\u020B\u0209\x03\x02\x02\x02\u020B\u020C" +
		"\x03\x02\x02\x02\u020Ca\x03\x02\x02\x02\u020D\u020B\x03\x02\x02\x02\u020E" +
		"\u0212\x07&\x02\x02\u020F\u0211\x05d3\x02\u0210\u020F\x03\x02\x02\x02" +
		"\u0211\u0214\x03\x02\x02\x02\u0212\u0210\x03\x02\x02\x02\u0212\u0213\x03" +
		"\x02\x02\x02\u0213\u0215\x03\x02\x02\x02\u0214\u0212\x03\x02\x02\x02\u0215" +
		"\u0216\x07\'\x02\x02\u0216c\x03\x02\x02\x02\u0217\u021B\x05l7\x02\u0218" +
		"\u021B\x05h5\x02\u0219\u021B\x05f4\x02\u021A\u0217\x03\x02\x02\x02\u021A" +
		"\u0218\x03\x02\x02\x02\u021A\u0219\x03\x02\x02\x02\u021Be\x03\x02\x02" +
		"\x02\u021C\u021E\x05&\x14\x02\u021D\u021C\x03\x02\x02\x02\u021E\u0221" +
		"\x03\x02\x02\x02\u021F\u021D\x03\x02\x02\x02\u021F\u0220\x03\x02\x02\x02" +
		"\u0220\u0222\x03\x02\x02\x02\u0221\u021F\x03\x02\x02\x02\u0222\u0223\x05" +
		"\x04\x03\x02\u0223\u0224\x07L\x02\x02\u0224\u0225\x05\x1A\x0E\x02\u0225" +
		"\u0226\x07)\x02\x02\u0226g\x03\x02\x02\x02\u0227\u0228\x05.\x18\x02\u0228" +
		"i\x03\x02\x02\x02\u0229\u022A\t\x07\x02\x02\u022Ak\x03\x02\x02\x02\u022B" +
		"\u022D\x05&\x14\x02\u022C\u022B\x03\x02\x02\x02\u022D\u0230\x03\x02\x02" +
		"\x02\u022E\u022C\x03\x02";
	private static readonly _serializedATNSegment1: string =
		"\x02\x02\u022E\u022F\x03\x02\x02\x02\u022F\u0231\x03\x02\x02\x02\u0230" +
		"\u022E\x03\x02\x02\x02\u0231\u0232\x05\x04\x03\x02\u0232\u0233\x05j6\x02" +
		"\u0233\u0234\x05 \x11\x02\u0234\u0235\x07/\x02\x02\u0235\u0236\x05\x18" +
		"\r\x02\u0236\u0237\x05,\x17\x02\u0237m\x03\x02\x02\x02\u0238\u0239\x07" +
		"$\x02\x02\u0239\u023A\x05V,\x02\u023A\u023B\x07%\x02\x02\u023Bo\x03\x02" +
		"\x02\x02\u023C\u023F\x05r:\x02\u023D\u023F\x058\x1D\x02\u023E\u023C\x03" +
		"\x02\x02\x02\u023E\u023D\x03\x02\x02\x02\u023F\u0244\x03\x02\x02\x02\u0240" +
		"\u0241\x070\x02\x02\u0241\u0243\x05r:\x02\u0242\u0240\x03\x02\x02\x02" +
		"\u0243\u0246\x03\x02\x02\x02\u0244\u0242\x03\x02\x02\x02\u0244\u0245\x03" +
		"\x02\x02\x02\u0245q\x03\x02\x02\x02\u0246\u0244\x03\x02\x02\x02\u0247" +
		"\u024B\x05*\x16\x02\u0248\u024B\x05\x12\n\x02\u0249\u024B\x05t;\x02\u024A" +
		"\u0247\x03\x02\x02\x02\u024A\u0248\x03\x02\x02\x02\u024A\u0249\x03\x02" +
		"\x02\x02\u024B\u024F\x03\x02\x02\x02\u024C\u024E\x05n8\x02\u024D\u024C" +
		"\x03\x02\x02\x02\u024E\u0251\x03\x02\x02\x02\u024F\u024D\x03\x02\x02\x02" +
		"\u024F\u0250\x03\x02\x02\x02\u0250s\x03\x02\x02\x02\u0251\u024F\x03\x02" +
		"\x02\x02\u0252\u0253\x07o\x02\x02\u0253u\x03\x02\x02\x02\u0254\u0255\x07" +
		"\x12\x02\x02\u0255\u025A\x05\x0E\b\x02\u0256\u0257\x07,\x02\x02\u0257" +
		"\u0259\x05\x0E\b\x02\u0258\u0256\x03\x02\x02\x02\u0259\u025C\x03\x02\x02" +
		"\x02\u025A\u0258\x03\x02\x02\x02\u025A\u025B\x03\x02\x02\x02\u025B\u025D" +
		"\x03\x02\x02\x02\u025C\u025A\x03\x02\x02\x02\u025D\u025E\x07\x13\x02\x02" +
		"\u025Ew\x03\x02\x02\x02\u025F\u026A\x07\x12\x02\x02\u0260\u0265\x05\x18" +
		"\r\x02\u0261\u0262\x07,\x02\x02\u0262\u0264\x05\x18\r\x02\u0263\u0261" +
		"\x03\x02\x02\x02\u0264\u0267\x03\x02\x02\x02\u0265\u0263\x03\x02\x02\x02" +
		"\u0265\u0266\x03\x02\x02\x02\u0266\u026B\x03\x02\x02\x02\u0267\u0265\x03" +
		"\x02\x02\x02\u0268\u026B\x07(\x02\x02\u0269\u026B\x05\x0E\b\x02\u026A" +
		"\u0260\x03\x02\x02\x02\u026A\u0268\x03\x02\x02\x02\u026A\u0269\x03\x02" +
		"\x02\x02\u026B\u026C\x03\x02\x02\x02\u026C\u026D\x07\x13\x02\x02\u026D" +
		"y\x03\x02\x02\x02\u026E\u0270\x05&\x14\x02\u026F\u026E\x03\x02\x02\x02" +
		"\u0270\u0273\x03\x02\x02\x02\u0271\u026F\x03\x02\x02\x02\u0271\u0272\x03" +
		"\x02\x02\x02\u0272\u0274\x03\x02\x02\x02\u0273\u0271\x03\x02\x02\x02\u0274" +
		"\u0275\x05\x04\x03\x02\u0275\u0276\x07p\x02\x02\u0276\u0277\x05\x0E\b" +
		"\x02\u0277\u0280\x07&\x02\x02\u0278\u027D\x05|?\x02\u0279\u027A\x07,\x02" +
		"\x02\u027A\u027C\x05|?\x02\u027B\u0279\x03\x02\x02\x02\u027C\u027F\x03" +
		"\x02\x02\x02\u027D\u027B\x03\x02\x02\x02\u027D\u027E\x03\x02\x02\x02\u027E" +
		"\u0281\x03\x02\x02\x02\u027F\u027D\x03\x02\x02\x02\u0280\u0278\x03\x02" +
		"\x02\x02\u0280\u0281\x03\x02\x02\x02\u0281\u0282\x03\x02\x02\x02\u0282" +
		"\u0283\x07\'\x02\x02\u0283{\x03\x02\x02\x02\u0284\u0286\x05&\x14\x02\u0285" +
		"\u0284\x03\x02\x02\x02\u0286\u0289\x03\x02\x02\x02\u0287\u0285\x03\x02" +
		"\x02\x02\u0287\u0288\x03\x02\x02\x02\u0288\u028A\x03\x02\x02\x02\u0289" +
		"\u0287\x03\x02\x02\x02\u028A\u028B\x05\x0E\b\x02\u028B}\x03\x02\x02\x02" +
		"\u028C\u028D\x05\x12\n\x02\u028D\u028E\x070\x02\x02\u028E\u028F\x05\x0E" +
		"\b\x02\u028F\x7F\x03\x02\x02\x026\x83\x92\xA4\xAD\xB0\xB5\xC4\xCA\xCF" +
		"\xD8\xDD\xE4\xE9\xF0\xF8\u0100\u010F\u011A\u011F\u012B\u0134\u0136\u013E" +
		"\u0143\u0150\u017D\u0182\u018A\u0192\u01DC\u01DE\u01EA\u01F2\u01F7\u01FE" +
		"\u0202\u020B\u0212\u021A\u021F\u022E\u023E\u0244\u024A\u024F\u025A\u0265" +
		"\u026A\u0271\u027D\u0280\u0287";
	public static readonly _serializedATN: string = Utils.join(
		[
			ArcSourceCodeParser._serializedATNSegment0,
			ArcSourceCodeParser._serializedATNSegment1,
		],
		"",
	);
	public static __ATN: ATN;
	public static get _ATN(): ATN {
		if (!ArcSourceCodeParser.__ATN) {
			ArcSourceCodeParser.__ATN = new ATNDeserializer().deserialize(Utils.toCharArray(ArcSourceCodeParser._serializedATN));
		}

		return ArcSourceCodeParser.__ATN;
	}

}

export class Arc_compilation_unitContext extends ParserRuleContext {
	public arc_namespace_block(): Arc_namespace_blockContext {
		return this.getRuleContext(0, Arc_namespace_blockContext);
	}
	public EOF(): TerminalNode { return this.getToken(ArcSourceCodeParser.EOF, 0); }
	public arc_stmt_link(): Arc_stmt_linkContext[];
	public arc_stmt_link(i: number): Arc_stmt_linkContext;
	public arc_stmt_link(i?: number): Arc_stmt_linkContext | Arc_stmt_linkContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_stmt_linkContext);
		} else {
			return this.getRuleContext(i, Arc_stmt_linkContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_compilation_unit; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_compilation_unit) {
			listener.enterArc_compilation_unit(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_compilation_unit) {
			listener.exitArc_compilation_unit(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_compilation_unit) {
			return visitor.visitArc_compilation_unit(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_accessibilityContext extends ParserRuleContext {
	public KW_PUBLIC(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_PUBLIC, 0); }
	public KW_INTERNAL(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_INTERNAL, 0); }
	public KW_PROTECTED(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_PROTECTED, 0); }
	public KW_PRIVATE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_PRIVATE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_accessibility; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_accessibility) {
			listener.enterArc_accessibility(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_accessibility) {
			listener.exitArc_accessibility(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_accessibility) {
			return visitor.visitArc_accessibility(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_mutabilityContext extends ParserRuleContext {
	public KW_CONSTANT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_CONSTANT, 0); }
	public KW_VARIABLE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_VARIABLE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_mutability; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_mutability) {
			listener.enterArc_mutability(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_mutability) {
			listener.exitArc_mutability(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_mutability) {
			return visitor.visitArc_mutability(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_namespace_identifierContext extends ParserRuleContext {
	public IDENTIFIER(): TerminalNode[];
	public IDENTIFIER(i: number): TerminalNode;
	public IDENTIFIER(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.IDENTIFIER);
		} else {
			return this.getToken(ArcSourceCodeParser.IDENTIFIER, i);
		}
	}
	public SCOPE(): TerminalNode[];
	public SCOPE(i: number): TerminalNode;
	public SCOPE(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.SCOPE);
		} else {
			return this.getToken(ArcSourceCodeParser.SCOPE, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_namespace_identifier; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_namespace_identifier) {
			listener.enterArc_namespace_identifier(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_namespace_identifier) {
			listener.exitArc_namespace_identifier(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_namespace_identifier) {
			return visitor.visitArc_namespace_identifier(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_namespace_declaratorContext extends ParserRuleContext {
	public KW_NAMESPACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_NAMESPACE, 0); }
	public arc_namespace_identifier(): Arc_namespace_identifierContext {
		return this.getRuleContext(0, Arc_namespace_identifierContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_namespace_declarator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_namespace_declarator) {
			listener.enterArc_namespace_declarator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_namespace_declarator) {
			listener.exitArc_namespace_declarator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_namespace_declarator) {
			return visitor.visitArc_namespace_declarator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_namespace_limiterContext extends ParserRuleContext {
	public LBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACKET, 0); }
	public arc_namespace_identifier(): Arc_namespace_identifierContext {
		return this.getRuleContext(0, Arc_namespace_identifierContext);
	}
	public RBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACKET, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_namespace_limiter; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_namespace_limiter) {
			listener.enterArc_namespace_limiter(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_namespace_limiter) {
			listener.exitArc_namespace_limiter(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_namespace_limiter) {
			return visitor.visitArc_namespace_limiter(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_single_identifierContext extends ParserRuleContext {
	public IDENTIFIER(): TerminalNode { return this.getToken(ArcSourceCodeParser.IDENTIFIER, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_single_identifier; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_single_identifier) {
			listener.enterArc_single_identifier(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_single_identifier) {
			listener.exitArc_single_identifier(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_single_identifier) {
			return visitor.visitArc_single_identifier(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_full_identifierContext extends ParserRuleContext {
	public arc_namespace_limiter(): Arc_namespace_limiterContext {
		return this.getRuleContext(0, Arc_namespace_limiterContext);
	}
	public DOT(): TerminalNode { return this.getToken(ArcSourceCodeParser.DOT, 0); }
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_full_identifier; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_full_identifier) {
			listener.enterArc_full_identifier(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_full_identifier) {
			listener.exitArc_full_identifier(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_full_identifier) {
			return visitor.visitArc_full_identifier(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_flexible_identifierContext extends ParserRuleContext {
	public arc_full_identifier(): Arc_full_identifierContext | undefined {
		return this.tryGetRuleContext(0, Arc_full_identifierContext);
	}
	public arc_single_identifier(): Arc_single_identifierContext | undefined {
		return this.tryGetRuleContext(0, Arc_single_identifierContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_flexible_identifier; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_flexible_identifier) {
			listener.enterArc_flexible_identifier(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_flexible_identifier) {
			listener.exitArc_flexible_identifier(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_flexible_identifier) {
			return visitor.visitArc_flexible_identifier(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_primitive_data_typeContext extends ParserRuleContext {
	public KW_INT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_INT, 0); }
	public KW_DECIMAL(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_DECIMAL, 0); }
	public KW_CHAR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_CHAR, 0); }
	public KW_STRING(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_STRING, 0); }
	public KW_BOOL(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_BOOL, 0); }
	public KW_BYTE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_BYTE, 0); }
	public KW_NONE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_NONE, 0); }
	public KW_ANY(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_ANY, 0); }
	public KW_INFER(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_INFER, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_primitive_data_type; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_primitive_data_type) {
			listener.enterArc_primitive_data_type(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_primitive_data_type) {
			listener.exitArc_primitive_data_type(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_primitive_data_type) {
			return visitor.visitArc_primitive_data_type(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_array_indicatorContext extends ParserRuleContext {
	public LBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACKET, 0); }
	public RBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACKET, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_array_indicator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_array_indicator) {
			listener.enterArc_array_indicator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_array_indicator) {
			listener.exitArc_array_indicator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_array_indicator) {
			return visitor.visitArc_array_indicator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_data_typeContext extends ParserRuleContext {
	public arc_primitive_data_type(): Arc_primitive_data_typeContext | undefined {
		return this.tryGetRuleContext(0, Arc_primitive_data_typeContext);
	}
	public arc_flexible_identifier(): Arc_flexible_identifierContext | undefined {
		return this.tryGetRuleContext(0, Arc_flexible_identifierContext);
	}
	public arc_generic_specialization_wrapper(): Arc_generic_specialization_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_generic_specialization_wrapperContext);
	}
	public arc_array_indicator(): Arc_array_indicatorContext[];
	public arc_array_indicator(i: number): Arc_array_indicatorContext;
	public arc_array_indicator(i?: number): Arc_array_indicatorContext | Arc_array_indicatorContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_array_indicatorContext);
		} else {
			return this.getRuleContext(i, Arc_array_indicatorContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_data_type; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_data_type) {
			listener.enterArc_data_type(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_data_type) {
			listener.exitArc_data_type(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_data_type) {
			return visitor.visitArc_data_type(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_data_declaratorContext extends ParserRuleContext {
	public arc_mutability(): Arc_mutabilityContext {
		return this.getRuleContext(0, Arc_mutabilityContext);
	}
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	public COLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.COLON, 0); }
	public arc_data_type(): Arc_data_typeContext {
		return this.getRuleContext(0, Arc_data_typeContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_data_declarator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_data_declarator) {
			listener.enterArc_data_declarator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_data_declarator) {
			listener.exitArc_data_declarator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_data_declarator) {
			return visitor.visitArc_data_declarator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_self_data_declaratorContext extends ParserRuleContext {
	public arc_mutability(): Arc_mutabilityContext {
		return this.getRuleContext(0, Arc_mutabilityContext);
	}
	public arc_self_wrapper(): Arc_self_wrapperContext {
		return this.getRuleContext(0, Arc_self_wrapperContext);
	}
	public COLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.COLON, 0); }
	public arc_data_type(): Arc_data_typeContext {
		return this.getRuleContext(0, Arc_data_typeContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_self_data_declarator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_self_data_declarator) {
			listener.enterArc_self_data_declarator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_self_data_declarator) {
			listener.exitArc_self_data_declarator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_self_data_declarator) {
			return visitor.visitArc_self_data_declarator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_arg_listContext extends ParserRuleContext {
	public arc_data_declarator(): Arc_data_declaratorContext[];
	public arc_data_declarator(i: number): Arc_data_declaratorContext;
	public arc_data_declarator(i?: number): Arc_data_declaratorContext | Arc_data_declaratorContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_data_declaratorContext);
		} else {
			return this.getRuleContext(i, Arc_data_declaratorContext);
		}
	}
	public arc_self_data_declarator(): Arc_self_data_declaratorContext | undefined {
		return this.tryGetRuleContext(0, Arc_self_data_declaratorContext);
	}
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_arg_list; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_arg_list) {
			listener.enterArc_arg_list(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_arg_list) {
			listener.exitArc_arg_list(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_arg_list) {
			return visitor.visitArc_arg_list(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_wrapped_arg_listContext extends ParserRuleContext {
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	public arc_arg_list(): Arc_arg_listContext | undefined {
		return this.tryGetRuleContext(0, Arc_arg_listContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_wrapped_arg_list; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_wrapped_arg_list) {
			listener.enterArc_wrapped_arg_list(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_wrapped_arg_list) {
			listener.exitArc_wrapped_arg_list(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_wrapped_arg_list) {
			return visitor.visitArc_wrapped_arg_list(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_param_listContext extends ParserRuleContext {
	public arc_expression(): Arc_expressionContext[];
	public arc_expression(i: number): Arc_expressionContext;
	public arc_expression(i?: number): Arc_expressionContext | Arc_expressionContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_expressionContext);
		} else {
			return this.getRuleContext(i, Arc_expressionContext);
		}
	}
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_param_list; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_param_list) {
			listener.enterArc_param_list(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_param_list) {
			listener.exitArc_param_list(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_param_list) {
			return visitor.visitArc_param_list(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_wrapped_param_listContext extends ParserRuleContext {
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	public arc_param_list(): Arc_param_listContext | undefined {
		return this.tryGetRuleContext(0, Arc_param_listContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_wrapped_param_list; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_wrapped_param_list) {
			listener.enterArc_wrapped_param_list(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_wrapped_param_list) {
			listener.exitArc_wrapped_param_list(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_wrapped_param_list) {
			return visitor.visitArc_wrapped_param_list(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_annotationContext extends ParserRuleContext {
	public AT(): TerminalNode { return this.getToken(ArcSourceCodeParser.AT, 0); }
	public arc_flexible_identifier(): Arc_flexible_identifierContext {
		return this.getRuleContext(0, Arc_flexible_identifierContext);
	}
	public arc_wrapped_param_list(): Arc_wrapped_param_listContext | undefined {
		return this.tryGetRuleContext(0, Arc_wrapped_param_listContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_annotation; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_annotation) {
			listener.enterArc_annotation(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_annotation) {
			listener.exitArc_annotation(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_annotation) {
			return visitor.visitArc_annotation(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_function_declaratorContext extends ParserRuleContext {
	public arc_accessibility(): Arc_accessibilityContext {
		return this.getRuleContext(0, Arc_accessibilityContext);
	}
	public KW_FUNCTION(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_FUNCTION, 0); }
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	public arc_wrapped_arg_list(): Arc_wrapped_arg_listContext {
		return this.getRuleContext(0, Arc_wrapped_arg_listContext);
	}
	public COLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.COLON, 0); }
	public arc_data_type(): Arc_data_typeContext {
		return this.getRuleContext(0, Arc_data_typeContext);
	}
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	public arc_generic_declaration_wrapper(): Arc_generic_declaration_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_generic_declaration_wrapperContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_function_declarator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_function_declarator) {
			listener.enterArc_function_declarator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_function_declarator) {
			listener.exitArc_function_declarator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_function_declarator) {
			return visitor.visitArc_function_declarator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_function_call_baseContext extends ParserRuleContext {
	public arc_flexible_identifier(): Arc_flexible_identifierContext {
		return this.getRuleContext(0, Arc_flexible_identifierContext);
	}
	public arc_wrapped_param_list(): Arc_wrapped_param_listContext {
		return this.getRuleContext(0, Arc_wrapped_param_listContext);
	}
	public arc_generic_specialization_wrapper(): Arc_generic_specialization_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_generic_specialization_wrapperContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_function_call_base; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_function_call_base) {
			listener.enterArc_function_call_base(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_function_call_base) {
			listener.exitArc_function_call_base(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_function_call_base) {
			return visitor.visitArc_function_call_base(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_wrapped_function_bodyContext extends ParserRuleContext {
	public LBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACE, 0); }
	public RBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACE, 0); }
	public arc_statement(): Arc_statementContext[];
	public arc_statement(i: number): Arc_statementContext;
	public arc_statement(i?: number): Arc_statementContext | Arc_statementContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_statementContext);
		} else {
			return this.getRuleContext(i, Arc_statementContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_wrapped_function_body; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_wrapped_function_body) {
			listener.enterArc_wrapped_function_body(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_wrapped_function_body) {
			listener.exitArc_wrapped_function_body(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_wrapped_function_body) {
			return visitor.visitArc_wrapped_function_body(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_function_blockContext extends ParserRuleContext {
	public arc_function_declarator(): Arc_function_declaratorContext {
		return this.getRuleContext(0, Arc_function_declaratorContext);
	}
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_function_block; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_function_block) {
			listener.enterArc_function_block(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_function_block) {
			listener.exitArc_function_block(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_function_block) {
			return visitor.visitArc_function_block(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_bool_valueContext extends ParserRuleContext {
	public KW_TRUE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_TRUE, 0); }
	public KW_FALSE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_FALSE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_bool_value; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_bool_value) {
			listener.enterArc_bool_value(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_bool_value) {
			listener.exitArc_bool_value(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_bool_value) {
			return visitor.visitArc_bool_value(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_instant_valueContext extends ParserRuleContext {
	public NUMBER(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.NUMBER, 0); }
	public LITERAL_STRING(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.LITERAL_STRING, 0); }
	public arc_bool_value(): Arc_bool_valueContext | undefined {
		return this.tryGetRuleContext(0, Arc_bool_valueContext);
	}
	public KW_NONE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_NONE, 0); }
	public KW_ANY(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_ANY, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_instant_value; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_instant_value) {
			listener.enterArc_instant_value(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_instant_value) {
			listener.exitArc_instant_value(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_instant_value) {
			return visitor.visitArc_instant_value(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_type_valueContext extends ParserRuleContext {
	public KW_TYPEOF(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_TYPEOF, 0); }
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public arc_data_type(): Arc_data_typeContext {
		return this.getRuleContext(0, Arc_data_typeContext);
	}
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_type_value; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_type_value) {
			listener.enterArc_type_value(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_type_value) {
			listener.exitArc_type_value(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_type_value) {
			return visitor.visitArc_type_value(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_data_valueContext extends ParserRuleContext {
	public arc_instant_value(): Arc_instant_valueContext | undefined {
		return this.tryGetRuleContext(0, Arc_instant_valueContext);
	}
	public arc_type_value(): Arc_type_valueContext | undefined {
		return this.tryGetRuleContext(0, Arc_type_valueContext);
	}
	public arc_call_chain(): Arc_call_chainContext | undefined {
		return this.tryGetRuleContext(0, Arc_call_chainContext);
	}
	public arc_enum_accessor(): Arc_enum_accessorContext | undefined {
		return this.tryGetRuleContext(0, Arc_enum_accessorContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_data_value; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_data_value) {
			listener.enterArc_data_value(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_data_value) {
			listener.exitArc_data_value(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_data_value) {
			return visitor.visitArc_data_value(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_constructor_callContext extends ParserRuleContext {
	public KW_NEW(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_NEW, 0); }
	public arc_flexible_identifier(): Arc_flexible_identifierContext {
		return this.getRuleContext(0, Arc_flexible_identifierContext);
	}
	public arc_wrapped_param_list(): Arc_wrapped_param_listContext {
		return this.getRuleContext(0, Arc_wrapped_param_listContext);
	}
	public arc_generic_specialization_wrapper(): Arc_generic_specialization_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_generic_specialization_wrapperContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_constructor_call; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_constructor_call) {
			listener.enterArc_constructor_call(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_constructor_call) {
			listener.exitArc_constructor_call(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_constructor_call) {
			return visitor.visitArc_constructor_call(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_statementContext extends ParserRuleContext {
	public SEMICOLON(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.SEMICOLON, 0); }
	public arc_stmt_decl(): Arc_stmt_declContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_declContext);
	}
	public arc_stmt_assign(): Arc_stmt_assignContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_assignContext);
	}
	public arc_stmt_return(): Arc_stmt_returnContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_returnContext);
	}
	public arc_stmt_break(): Arc_stmt_breakContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_breakContext);
	}
	public arc_stmt_continue(): Arc_stmt_continueContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_continueContext);
	}
	public arc_stmt_call(): Arc_stmt_callContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_callContext);
	}
	public arc_stmt_throw(): Arc_stmt_throwContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_throwContext);
	}
	public arc_stmt_while(): Arc_stmt_whileContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_whileContext);
	}
	public arc_stmt_loop(): Arc_stmt_loopContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_loopContext);
	}
	public arc_stmt_for(): Arc_stmt_forContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_forContext);
	}
	public arc_stmt_foreach(): Arc_stmt_foreachContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_foreachContext);
	}
	public arc_stmt_if(): Arc_stmt_ifContext | undefined {
		return this.tryGetRuleContext(0, Arc_stmt_ifContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_statement; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_statement) {
			listener.enterArc_statement(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_statement) {
			listener.exitArc_statement(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_statement) {
			return visitor.visitArc_statement(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_linkContext extends ParserRuleContext {
	public KW_LINK(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_LINK, 0); }
	public arc_namespace_identifier(): Arc_namespace_identifierContext {
		return this.getRuleContext(0, Arc_namespace_identifierContext);
	}
	public SEMICOLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.SEMICOLON, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_link; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_link) {
			listener.enterArc_stmt_link(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_link) {
			listener.exitArc_stmt_link(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_link) {
			return visitor.visitArc_stmt_link(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_returnContext extends ParserRuleContext {
	public KW_RETURN(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_RETURN, 0); }
	public arc_expression(): Arc_expressionContext | undefined {
		return this.tryGetRuleContext(0, Arc_expressionContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_return; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_return) {
			listener.enterArc_stmt_return(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_return) {
			listener.exitArc_stmt_return(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_return) {
			return visitor.visitArc_stmt_return(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_declContext extends ParserRuleContext {
	public arc_data_declarator(): Arc_data_declaratorContext {
		return this.getRuleContext(0, Arc_data_declaratorContext);
	}
	public ASSIGN(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.ASSIGN, 0); }
	public arc_expression(): Arc_expressionContext | undefined {
		return this.tryGetRuleContext(0, Arc_expressionContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_decl; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_decl) {
			listener.enterArc_stmt_decl(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_decl) {
			listener.exitArc_stmt_decl(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_decl) {
			return visitor.visitArc_stmt_decl(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_assignContext extends ParserRuleContext {
	public arc_call_chain(): Arc_call_chainContext {
		return this.getRuleContext(0, Arc_call_chainContext);
	}
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public ASSIGN(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.ASSIGN, 0); }
	public ASSIGN_IF_NULL(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.ASSIGN_IF_NULL, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_assign; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_assign) {
			listener.enterArc_stmt_assign(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_assign) {
			listener.exitArc_stmt_assign(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_assign) {
			return visitor.visitArc_stmt_assign(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_breakContext extends ParserRuleContext {
	public KW_BREAK(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_BREAK, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_break; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_break) {
			listener.enterArc_stmt_break(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_break) {
			listener.exitArc_stmt_break(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_break) {
			return visitor.visitArc_stmt_break(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_continueContext extends ParserRuleContext {
	public KW_CONTINUE(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_CONTINUE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_continue; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_continue) {
			listener.enterArc_stmt_continue(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_continue) {
			listener.exitArc_stmt_continue(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_continue) {
			return visitor.visitArc_stmt_continue(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_callContext extends ParserRuleContext {
	public KW_CALL(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_CALL, 0); }
	public arc_function_call_base(): Arc_function_call_baseContext | undefined {
		return this.tryGetRuleContext(0, Arc_function_call_baseContext);
	}
	public arc_call_chain(): Arc_call_chainContext | undefined {
		return this.tryGetRuleContext(0, Arc_call_chainContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_call; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_call) {
			listener.enterArc_stmt_call(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_call) {
			listener.exitArc_stmt_call(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_call) {
			return visitor.visitArc_stmt_call(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_whileContext extends ParserRuleContext {
	public KW_WHILE(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_WHILE, 0); }
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_while; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_while) {
			listener.enterArc_stmt_while(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_while) {
			listener.exitArc_stmt_while(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_while) {
			return visitor.visitArc_stmt_while(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_forContext extends ParserRuleContext {
	public KW_FOR(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_FOR, 0); }
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public arc_stmt_decl(): Arc_stmt_declContext {
		return this.getRuleContext(0, Arc_stmt_declContext);
	}
	public SEMICOLON(): TerminalNode[];
	public SEMICOLON(i: number): TerminalNode;
	public SEMICOLON(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.SEMICOLON);
		} else {
			return this.getToken(ArcSourceCodeParser.SEMICOLON, i);
		}
	}
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public arc_stmt_assign(): Arc_stmt_assignContext {
		return this.getRuleContext(0, Arc_stmt_assignContext);
	}
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_for; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_for) {
			listener.enterArc_stmt_for(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_for) {
			listener.exitArc_stmt_for(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_for) {
			return visitor.visitArc_stmt_for(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_loopContext extends ParserRuleContext {
	public KW_LOOP(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_LOOP, 0); }
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_loop; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_loop) {
			listener.enterArc_stmt_loop(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_loop) {
			listener.exitArc_stmt_loop(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_loop) {
			return visitor.visitArc_stmt_loop(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_foreachContext extends ParserRuleContext {
	public KW_FOREACH(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_FOREACH, 0); }
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public arc_data_declarator(): Arc_data_declaratorContext {
		return this.getRuleContext(0, Arc_data_declaratorContext);
	}
	public KW_IN(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_IN, 0); }
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_foreach; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_foreach) {
			listener.enterArc_stmt_foreach(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_foreach) {
			listener.exitArc_stmt_foreach(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_foreach) {
			return visitor.visitArc_stmt_foreach(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_throwContext extends ParserRuleContext {
	public KW_THROW(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_THROW, 0); }
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_throw; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_throw) {
			listener.enterArc_stmt_throw(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_throw) {
			listener.exitArc_stmt_throw(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_throw) {
			return visitor.visitArc_stmt_throw(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_stmt_ifContext extends ParserRuleContext {
	public KW_IF(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_IF, 0); }
	public LPAREN(): TerminalNode[];
	public LPAREN(i: number): TerminalNode;
	public LPAREN(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.LPAREN);
		} else {
			return this.getToken(ArcSourceCodeParser.LPAREN, i);
		}
	}
	public arc_expression(): Arc_expressionContext[];
	public arc_expression(i: number): Arc_expressionContext;
	public arc_expression(i?: number): Arc_expressionContext | Arc_expressionContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_expressionContext);
		} else {
			return this.getRuleContext(i, Arc_expressionContext);
		}
	}
	public RPAREN(): TerminalNode[];
	public RPAREN(i: number): TerminalNode;
	public RPAREN(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.RPAREN);
		} else {
			return this.getToken(ArcSourceCodeParser.RPAREN, i);
		}
	}
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext[];
	public arc_wrapped_function_body(i: number): Arc_wrapped_function_bodyContext;
	public arc_wrapped_function_body(i?: number): Arc_wrapped_function_bodyContext | Arc_wrapped_function_bodyContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_wrapped_function_bodyContext);
		} else {
			return this.getRuleContext(i, Arc_wrapped_function_bodyContext);
		}
	}
	public KW_ELIF(): TerminalNode[];
	public KW_ELIF(i: number): TerminalNode;
	public KW_ELIF(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.KW_ELIF);
		} else {
			return this.getToken(ArcSourceCodeParser.KW_ELIF, i);
		}
	}
	public KW_ELSE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_ELSE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_stmt_if; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_stmt_if) {
			listener.enterArc_stmt_if(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_stmt_if) {
			listener.exitArc_stmt_if(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_stmt_if) {
			return visitor.visitArc_stmt_if(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_expressionContext extends ParserRuleContext {
	public arc_data_value(): Arc_data_valueContext | undefined {
		return this.tryGetRuleContext(0, Arc_data_valueContext);
	}
	public arc_wrapped_expression(): Arc_wrapped_expressionContext | undefined {
		return this.tryGetRuleContext(0, Arc_wrapped_expressionContext);
	}
	public arc_expression(): Arc_expressionContext[];
	public arc_expression(i: number): Arc_expressionContext;
	public arc_expression(i?: number): Arc_expressionContext | Arc_expressionContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_expressionContext);
		} else {
			return this.getRuleContext(i, Arc_expressionContext);
		}
	}
	public MULTIPLY(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.MULTIPLY, 0); }
	public DIVIDE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.DIVIDE, 0); }
	public MODULO(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.MODULO, 0); }
	public PLUS(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.PLUS, 0); }
	public MINUS(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.MINUS, 0); }
	public BITWISE_LSHIFT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_LSHIFT, 0); }
	public BITWISE_RSHIFT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_RSHIFT, 0); }
	public BITWISE_AND(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_AND, 0); }
	public BITWISE_OR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_OR, 0); }
	public BITWISE_XOR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_XOR, 0); }
	public COMP_LT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_LT, 0); }
	public COMP_GT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_GT, 0); }
	public COMP_LTE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_LTE, 0); }
	public COMP_GTE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_GTE, 0); }
	public COMP_OBJ_EQ(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_OBJ_EQ, 0); }
	public COMP_REF_EQ(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_REF_EQ, 0); }
	public COMP_OBJ_NEQ(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_OBJ_NEQ, 0); }
	public COMP_REF_NEQ(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COMP_REF_NEQ, 0); }
	public LOGICAL_AND(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.LOGICAL_AND, 0); }
	public LOGICAL_OR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.LOGICAL_OR, 0); }
	public QUESTION(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.QUESTION, 0); }
	public COLON(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COLON, 0); }
	public RANGE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.RANGE, 0); }
	public ARROW(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.ARROW, 0); }
	public LOGICAL_NOT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.LOGICAL_NOT, 0); }
	public BITWISE_NOT(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.BITWISE_NOT, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_expression; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_expression) {
			listener.enterArc_expression(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_expression) {
			listener.exitArc_expression(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_expression) {
			return visitor.visitArc_expression(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_wrapped_expressionContext extends ParserRuleContext {
	public LPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.LPAREN, 0); }
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public RPAREN(): TerminalNode { return this.getToken(ArcSourceCodeParser.RPAREN, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_wrapped_expression; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_wrapped_expression) {
			listener.enterArc_wrapped_expression(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_wrapped_expression) {
			listener.exitArc_wrapped_expression(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_wrapped_expression) {
			return visitor.visitArc_wrapped_expression(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_namespace_blockContext extends ParserRuleContext {
	public arc_namespace_declarator(): Arc_namespace_declaratorContext {
		return this.getRuleContext(0, Arc_namespace_declaratorContext);
	}
	public LBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACE, 0); }
	public RBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACE, 0); }
	public arc_namespace_member(): Arc_namespace_memberContext[];
	public arc_namespace_member(i: number): Arc_namespace_memberContext;
	public arc_namespace_member(i?: number): Arc_namespace_memberContext | Arc_namespace_memberContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_namespace_memberContext);
		} else {
			return this.getRuleContext(i, Arc_namespace_memberContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_namespace_block; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_namespace_block) {
			listener.enterArc_namespace_block(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_namespace_block) {
			listener.exitArc_namespace_block(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_namespace_block) {
			return visitor.visitArc_namespace_block(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_namespace_memberContext extends ParserRuleContext {
	public arc_function_block(): Arc_function_blockContext | undefined {
		return this.tryGetRuleContext(0, Arc_function_blockContext);
	}
	public arc_group_block(): Arc_group_blockContext | undefined {
		return this.tryGetRuleContext(0, Arc_group_blockContext);
	}
	public arc_enum_declarator(): Arc_enum_declaratorContext | undefined {
		return this.tryGetRuleContext(0, Arc_enum_declaratorContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_namespace_member; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_namespace_member) {
			listener.enterArc_namespace_member(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_namespace_member) {
			listener.exitArc_namespace_member(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_namespace_member) {
			return visitor.visitArc_namespace_member(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_blockContext extends ParserRuleContext {
	public arc_accessibility(): Arc_accessibilityContext {
		return this.getRuleContext(0, Arc_accessibilityContext);
	}
	public KW_GROUP(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_GROUP, 0); }
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	public arc_wrapped_group_member(): Arc_wrapped_group_memberContext {
		return this.getRuleContext(0, Arc_wrapped_group_memberContext);
	}
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	public arc_generic_declaration_wrapper(): Arc_generic_declaration_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_generic_declaration_wrapperContext);
	}
	public COLON(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.COLON, 0); }
	public arc_group_derive_list(): Arc_group_derive_listContext | undefined {
		return this.tryGetRuleContext(0, Arc_group_derive_listContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_block; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_block) {
			listener.enterArc_group_block(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_block) {
			listener.exitArc_group_block(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_block) {
			return visitor.visitArc_group_block(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_derive_listContext extends ParserRuleContext {
	public arc_data_type(): Arc_data_typeContext[];
	public arc_data_type(i: number): Arc_data_typeContext;
	public arc_data_type(i?: number): Arc_data_typeContext | Arc_data_typeContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_data_typeContext);
		} else {
			return this.getRuleContext(i, Arc_data_typeContext);
		}
	}
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_derive_list; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_derive_list) {
			listener.enterArc_group_derive_list(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_derive_list) {
			listener.exitArc_group_derive_list(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_derive_list) {
			return visitor.visitArc_group_derive_list(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_wrapped_group_memberContext extends ParserRuleContext {
	public LBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACE, 0); }
	public RBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACE, 0); }
	public arc_group_member(): Arc_group_memberContext[];
	public arc_group_member(i: number): Arc_group_memberContext;
	public arc_group_member(i?: number): Arc_group_memberContext | Arc_group_memberContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_group_memberContext);
		} else {
			return this.getRuleContext(i, Arc_group_memberContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_wrapped_group_member; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_wrapped_group_member) {
			listener.enterArc_wrapped_group_member(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_wrapped_group_member) {
			listener.exitArc_wrapped_group_member(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_wrapped_group_member) {
			return visitor.visitArc_wrapped_group_member(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_memberContext extends ParserRuleContext {
	public arc_group_lifecycle_function(): Arc_group_lifecycle_functionContext | undefined {
		return this.tryGetRuleContext(0, Arc_group_lifecycle_functionContext);
	}
	public arc_group_function(): Arc_group_functionContext | undefined {
		return this.tryGetRuleContext(0, Arc_group_functionContext);
	}
	public arc_group_field(): Arc_group_fieldContext | undefined {
		return this.tryGetRuleContext(0, Arc_group_fieldContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_member; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_member) {
			listener.enterArc_group_member(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_member) {
			listener.exitArc_group_member(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_member) {
			return visitor.visitArc_group_member(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_fieldContext extends ParserRuleContext {
	public arc_accessibility(): Arc_accessibilityContext {
		return this.getRuleContext(0, Arc_accessibilityContext);
	}
	public KW_FIELD(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_FIELD, 0); }
	public arc_data_declarator(): Arc_data_declaratorContext {
		return this.getRuleContext(0, Arc_data_declaratorContext);
	}
	public SEMICOLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.SEMICOLON, 0); }
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_field; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_field) {
			listener.enterArc_group_field(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_field) {
			listener.exitArc_group_field(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_field) {
			return visitor.visitArc_group_field(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_functionContext extends ParserRuleContext {
	public arc_function_block(): Arc_function_blockContext {
		return this.getRuleContext(0, Arc_function_blockContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_function; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_function) {
			listener.enterArc_group_function(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_function) {
			listener.exitArc_group_function(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_function) {
			return visitor.visitArc_group_function(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_lifecycle_keywordContext extends ParserRuleContext {
	public KW_CONSTRUCTOR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_CONSTRUCTOR, 0); }
	public KW_DESTRUCTOR(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_DESTRUCTOR, 0); }
	public KW_CLONE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_CLONE, 0); }
	public KW_VALUE(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.KW_VALUE, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_lifecycle_keyword; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_lifecycle_keyword) {
			listener.enterArc_group_lifecycle_keyword(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_lifecycle_keyword) {
			listener.exitArc_group_lifecycle_keyword(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_lifecycle_keyword) {
			return visitor.visitArc_group_lifecycle_keyword(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_group_lifecycle_functionContext extends ParserRuleContext {
	public arc_accessibility(): Arc_accessibilityContext {
		return this.getRuleContext(0, Arc_accessibilityContext);
	}
	public arc_group_lifecycle_keyword(): Arc_group_lifecycle_keywordContext {
		return this.getRuleContext(0, Arc_group_lifecycle_keywordContext);
	}
	public arc_wrapped_arg_list(): Arc_wrapped_arg_listContext {
		return this.getRuleContext(0, Arc_wrapped_arg_listContext);
	}
	public COLON(): TerminalNode { return this.getToken(ArcSourceCodeParser.COLON, 0); }
	public arc_data_type(): Arc_data_typeContext {
		return this.getRuleContext(0, Arc_data_typeContext);
	}
	public arc_wrapped_function_body(): Arc_wrapped_function_bodyContext {
		return this.getRuleContext(0, Arc_wrapped_function_bodyContext);
	}
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_group_lifecycle_function; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_group_lifecycle_function) {
			listener.enterArc_group_lifecycle_function(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_group_lifecycle_function) {
			listener.exitArc_group_lifecycle_function(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_group_lifecycle_function) {
			return visitor.visitArc_group_lifecycle_function(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_indexContext extends ParserRuleContext {
	public LBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACKET, 0); }
	public arc_expression(): Arc_expressionContext {
		return this.getRuleContext(0, Arc_expressionContext);
	}
	public RBRACKET(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACKET, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_index; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_index) {
			listener.enterArc_index(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_index) {
			listener.exitArc_index(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_index) {
			return visitor.visitArc_index(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_call_chainContext extends ParserRuleContext {
	public arc_call_chain_term(): Arc_call_chain_termContext[];
	public arc_call_chain_term(i: number): Arc_call_chain_termContext;
	public arc_call_chain_term(i?: number): Arc_call_chain_termContext | Arc_call_chain_termContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_call_chain_termContext);
		} else {
			return this.getRuleContext(i, Arc_call_chain_termContext);
		}
	}
	public arc_constructor_call(): Arc_constructor_callContext | undefined {
		return this.tryGetRuleContext(0, Arc_constructor_callContext);
	}
	public DOT(): TerminalNode[];
	public DOT(i: number): TerminalNode;
	public DOT(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.DOT);
		} else {
			return this.getToken(ArcSourceCodeParser.DOT, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_call_chain; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_call_chain) {
			listener.enterArc_call_chain(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_call_chain) {
			listener.exitArc_call_chain(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_call_chain) {
			return visitor.visitArc_call_chain(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_call_chain_termContext extends ParserRuleContext {
	public arc_function_call_base(): Arc_function_call_baseContext | undefined {
		return this.tryGetRuleContext(0, Arc_function_call_baseContext);
	}
	public arc_flexible_identifier(): Arc_flexible_identifierContext | undefined {
		return this.tryGetRuleContext(0, Arc_flexible_identifierContext);
	}
	public arc_self_wrapper(): Arc_self_wrapperContext | undefined {
		return this.tryGetRuleContext(0, Arc_self_wrapperContext);
	}
	public arc_index(): Arc_indexContext[];
	public arc_index(i: number): Arc_indexContext;
	public arc_index(i?: number): Arc_indexContext | Arc_indexContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_indexContext);
		} else {
			return this.getRuleContext(i, Arc_indexContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_call_chain_term; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_call_chain_term) {
			listener.enterArc_call_chain_term(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_call_chain_term) {
			listener.exitArc_call_chain_term(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_call_chain_term) {
			return visitor.visitArc_call_chain_term(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_self_wrapperContext extends ParserRuleContext {
	public KW_SELF(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_SELF, 0); }
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_self_wrapper; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_self_wrapper) {
			listener.enterArc_self_wrapper(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_self_wrapper) {
			listener.exitArc_self_wrapper(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_self_wrapper) {
			return visitor.visitArc_self_wrapper(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_generic_declaration_wrapperContext extends ParserRuleContext {
	public COMP_LT(): TerminalNode { return this.getToken(ArcSourceCodeParser.COMP_LT, 0); }
	public arc_single_identifier(): Arc_single_identifierContext[];
	public arc_single_identifier(i: number): Arc_single_identifierContext;
	public arc_single_identifier(i?: number): Arc_single_identifierContext | Arc_single_identifierContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_single_identifierContext);
		} else {
			return this.getRuleContext(i, Arc_single_identifierContext);
		}
	}
	public COMP_GT(): TerminalNode { return this.getToken(ArcSourceCodeParser.COMP_GT, 0); }
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_generic_declaration_wrapper; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_generic_declaration_wrapper) {
			listener.enterArc_generic_declaration_wrapper(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_generic_declaration_wrapper) {
			listener.exitArc_generic_declaration_wrapper(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_generic_declaration_wrapper) {
			return visitor.visitArc_generic_declaration_wrapper(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_generic_specialization_wrapperContext extends ParserRuleContext {
	public COMP_LT(): TerminalNode { return this.getToken(ArcSourceCodeParser.COMP_LT, 0); }
	public COMP_GT(): TerminalNode { return this.getToken(ArcSourceCodeParser.COMP_GT, 0); }
	public QUESTION(): TerminalNode | undefined { return this.tryGetToken(ArcSourceCodeParser.QUESTION, 0); }
	public arc_single_identifier(): Arc_single_identifierContext | undefined {
		return this.tryGetRuleContext(0, Arc_single_identifierContext);
	}
	public arc_data_type(): Arc_data_typeContext[];
	public arc_data_type(i: number): Arc_data_typeContext;
	public arc_data_type(i?: number): Arc_data_typeContext | Arc_data_typeContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_data_typeContext);
		} else {
			return this.getRuleContext(i, Arc_data_typeContext);
		}
	}
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_generic_specialization_wrapper; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_generic_specialization_wrapper) {
			listener.enterArc_generic_specialization_wrapper(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_generic_specialization_wrapper) {
			listener.exitArc_generic_specialization_wrapper(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_generic_specialization_wrapper) {
			return visitor.visitArc_generic_specialization_wrapper(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_enum_declaratorContext extends ParserRuleContext {
	public arc_accessibility(): Arc_accessibilityContext {
		return this.getRuleContext(0, Arc_accessibilityContext);
	}
	public KW_ENUM(): TerminalNode { return this.getToken(ArcSourceCodeParser.KW_ENUM, 0); }
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	public LBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.LBRACE, 0); }
	public RBRACE(): TerminalNode { return this.getToken(ArcSourceCodeParser.RBRACE, 0); }
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	public arc_enum_member(): Arc_enum_memberContext[];
	public arc_enum_member(i: number): Arc_enum_memberContext;
	public arc_enum_member(i?: number): Arc_enum_memberContext | Arc_enum_memberContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_enum_memberContext);
		} else {
			return this.getRuleContext(i, Arc_enum_memberContext);
		}
	}
	public COMMA(): TerminalNode[];
	public COMMA(i: number): TerminalNode;
	public COMMA(i?: number): TerminalNode | TerminalNode[] {
		if (i === undefined) {
			return this.getTokens(ArcSourceCodeParser.COMMA);
		} else {
			return this.getToken(ArcSourceCodeParser.COMMA, i);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_enum_declarator; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_enum_declarator) {
			listener.enterArc_enum_declarator(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_enum_declarator) {
			listener.exitArc_enum_declarator(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_enum_declarator) {
			return visitor.visitArc_enum_declarator(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_enum_memberContext extends ParserRuleContext {
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	public arc_annotation(): Arc_annotationContext[];
	public arc_annotation(i: number): Arc_annotationContext;
	public arc_annotation(i?: number): Arc_annotationContext | Arc_annotationContext[] {
		if (i === undefined) {
			return this.getRuleContexts(Arc_annotationContext);
		} else {
			return this.getRuleContext(i, Arc_annotationContext);
		}
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_enum_member; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_enum_member) {
			listener.enterArc_enum_member(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_enum_member) {
			listener.exitArc_enum_member(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_enum_member) {
			return visitor.visitArc_enum_member(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


export class Arc_enum_accessorContext extends ParserRuleContext {
	public arc_flexible_identifier(): Arc_flexible_identifierContext {
		return this.getRuleContext(0, Arc_flexible_identifierContext);
	}
	public DOT(): TerminalNode { return this.getToken(ArcSourceCodeParser.DOT, 0); }
	public arc_single_identifier(): Arc_single_identifierContext {
		return this.getRuleContext(0, Arc_single_identifierContext);
	}
	constructor(parent: ParserRuleContext | undefined, invokingState: number) {
		super(parent, invokingState);
	}
	// @Override
	public get ruleIndex(): number { return ArcSourceCodeParser.RULE_arc_enum_accessor; }
	// @Override
	public enterRule(listener: ArcSourceCodeParserListener): void {
		if (listener.enterArc_enum_accessor) {
			listener.enterArc_enum_accessor(this);
		}
	}
	// @Override
	public exitRule(listener: ArcSourceCodeParserListener): void {
		if (listener.exitArc_enum_accessor) {
			listener.exitArc_enum_accessor(this);
		}
	}
	// @Override
	public accept<Result>(visitor: ArcSourceCodeParserVisitor<Result>): Result {
		if (visitor.visitArc_enum_accessor) {
			return visitor.visitArc_enum_accessor(this);
		} else {
			return visitor.visitChildren(this);
		}
	}
}


