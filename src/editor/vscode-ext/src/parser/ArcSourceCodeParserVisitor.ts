// Generated from src/parser/ArcSourceCodeParser.g4 by ANTLR 4.9.0-SNAPSHOT


import { ParseTreeVisitor } from "antlr4ts/tree/ParseTreeVisitor";

import { Arc_compilation_unitContext } from "./ArcSourceCodeParser";
import { Arc_accessibilityContext } from "./ArcSourceCodeParser";
import { Arc_mutabilityContext } from "./ArcSourceCodeParser";
import { Arc_namespace_identifierContext } from "./ArcSourceCodeParser";
import { Arc_namespace_declaratorContext } from "./ArcSourceCodeParser";
import { Arc_namespace_limiterContext } from "./ArcSourceCodeParser";
import { Arc_single_identifierContext } from "./ArcSourceCodeParser";
import { Arc_full_identifierContext } from "./ArcSourceCodeParser";
import { Arc_flexible_identifierContext } from "./ArcSourceCodeParser";
import { Arc_primitive_data_typeContext } from "./ArcSourceCodeParser";
import { Arc_array_indicatorContext } from "./ArcSourceCodeParser";
import { Arc_data_typeContext } from "./ArcSourceCodeParser";
import { Arc_data_declaratorContext } from "./ArcSourceCodeParser";
import { Arc_self_data_declaratorContext } from "./ArcSourceCodeParser";
import { Arc_arg_listContext } from "./ArcSourceCodeParser";
import { Arc_wrapped_arg_listContext } from "./ArcSourceCodeParser";
import { Arc_param_listContext } from "./ArcSourceCodeParser";
import { Arc_wrapped_param_listContext } from "./ArcSourceCodeParser";
import { Arc_annotationContext } from "./ArcSourceCodeParser";
import { Arc_function_declaratorContext } from "./ArcSourceCodeParser";
import { Arc_function_call_baseContext } from "./ArcSourceCodeParser";
import { Arc_wrapped_function_bodyContext } from "./ArcSourceCodeParser";
import { Arc_function_blockContext } from "./ArcSourceCodeParser";
import { Arc_bool_valueContext } from "./ArcSourceCodeParser";
import { Arc_instant_valueContext } from "./ArcSourceCodeParser";
import { Arc_type_valueContext } from "./ArcSourceCodeParser";
import { Arc_data_valueContext } from "./ArcSourceCodeParser";
import { Arc_constructor_callContext } from "./ArcSourceCodeParser";
import { Arc_statementContext } from "./ArcSourceCodeParser";
import { Arc_stmt_linkContext } from "./ArcSourceCodeParser";
import { Arc_stmt_returnContext } from "./ArcSourceCodeParser";
import { Arc_stmt_declContext } from "./ArcSourceCodeParser";
import { Arc_stmt_assignContext } from "./ArcSourceCodeParser";
import { Arc_stmt_breakContext } from "./ArcSourceCodeParser";
import { Arc_stmt_continueContext } from "./ArcSourceCodeParser";
import { Arc_stmt_callContext } from "./ArcSourceCodeParser";
import { Arc_stmt_whileContext } from "./ArcSourceCodeParser";
import { Arc_stmt_forContext } from "./ArcSourceCodeParser";
import { Arc_stmt_loopContext } from "./ArcSourceCodeParser";
import { Arc_stmt_foreachContext } from "./ArcSourceCodeParser";
import { Arc_stmt_throwContext } from "./ArcSourceCodeParser";
import { Arc_stmt_ifContext } from "./ArcSourceCodeParser";
import { Arc_expressionContext } from "./ArcSourceCodeParser";
import { Arc_wrapped_expressionContext } from "./ArcSourceCodeParser";
import { Arc_namespace_blockContext } from "./ArcSourceCodeParser";
import { Arc_namespace_memberContext } from "./ArcSourceCodeParser";
import { Arc_group_blockContext } from "./ArcSourceCodeParser";
import { Arc_group_derive_listContext } from "./ArcSourceCodeParser";
import { Arc_wrapped_group_memberContext } from "./ArcSourceCodeParser";
import { Arc_group_memberContext } from "./ArcSourceCodeParser";
import { Arc_group_fieldContext } from "./ArcSourceCodeParser";
import { Arc_group_functionContext } from "./ArcSourceCodeParser";
import { Arc_group_lifecycle_keywordContext } from "./ArcSourceCodeParser";
import { Arc_group_lifecycle_functionContext } from "./ArcSourceCodeParser";
import { Arc_indexContext } from "./ArcSourceCodeParser";
import { Arc_call_chainContext } from "./ArcSourceCodeParser";
import { Arc_call_chain_termContext } from "./ArcSourceCodeParser";
import { Arc_self_wrapperContext } from "./ArcSourceCodeParser";
import { Arc_generic_declaration_wrapperContext } from "./ArcSourceCodeParser";
import { Arc_generic_specialization_wrapperContext } from "./ArcSourceCodeParser";
import { Arc_enum_declaratorContext } from "./ArcSourceCodeParser";
import { Arc_enum_memberContext } from "./ArcSourceCodeParser";
import { Arc_enum_accessorContext } from "./ArcSourceCodeParser";


/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by `ArcSourceCodeParser`.
 *
 * @param <Result> The return type of the visit operation. Use `void` for
 * operations with no return type.
 */
export interface ArcSourceCodeParserVisitor<Result> extends ParseTreeVisitor<Result> {
	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_compilation_unit`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_compilation_unit?: (ctx: Arc_compilation_unitContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_accessibility`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_accessibility?: (ctx: Arc_accessibilityContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_mutability`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_mutability?: (ctx: Arc_mutabilityContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_namespace_identifier`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_namespace_identifier?: (ctx: Arc_namespace_identifierContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_namespace_declarator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_namespace_declarator?: (ctx: Arc_namespace_declaratorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_namespace_limiter`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_namespace_limiter?: (ctx: Arc_namespace_limiterContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_single_identifier`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_single_identifier?: (ctx: Arc_single_identifierContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_full_identifier`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_full_identifier?: (ctx: Arc_full_identifierContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_flexible_identifier`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_flexible_identifier?: (ctx: Arc_flexible_identifierContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_primitive_data_type`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_primitive_data_type?: (ctx: Arc_primitive_data_typeContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_array_indicator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_array_indicator?: (ctx: Arc_array_indicatorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_data_type`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_data_type?: (ctx: Arc_data_typeContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_data_declarator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_data_declarator?: (ctx: Arc_data_declaratorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_self_data_declarator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_self_data_declarator?: (ctx: Arc_self_data_declaratorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_arg_list`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_arg_list?: (ctx: Arc_arg_listContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_arg_list`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_wrapped_arg_list?: (ctx: Arc_wrapped_arg_listContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_param_list`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_param_list?: (ctx: Arc_param_listContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_param_list`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_wrapped_param_list?: (ctx: Arc_wrapped_param_listContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_annotation`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_annotation?: (ctx: Arc_annotationContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_function_declarator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_function_declarator?: (ctx: Arc_function_declaratorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_function_call_base`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_function_call_base?: (ctx: Arc_function_call_baseContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_function_body`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_wrapped_function_body?: (ctx: Arc_wrapped_function_bodyContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_function_block`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_function_block?: (ctx: Arc_function_blockContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_bool_value`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_bool_value?: (ctx: Arc_bool_valueContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_instant_value`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_instant_value?: (ctx: Arc_instant_valueContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_type_value`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_type_value?: (ctx: Arc_type_valueContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_data_value`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_data_value?: (ctx: Arc_data_valueContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_constructor_call`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_constructor_call?: (ctx: Arc_constructor_callContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_statement`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_statement?: (ctx: Arc_statementContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_link`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_link?: (ctx: Arc_stmt_linkContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_return`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_return?: (ctx: Arc_stmt_returnContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_decl`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_decl?: (ctx: Arc_stmt_declContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_assign`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_assign?: (ctx: Arc_stmt_assignContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_break`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_break?: (ctx: Arc_stmt_breakContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_continue`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_continue?: (ctx: Arc_stmt_continueContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_call`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_call?: (ctx: Arc_stmt_callContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_while`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_while?: (ctx: Arc_stmt_whileContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_for`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_for?: (ctx: Arc_stmt_forContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_loop`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_loop?: (ctx: Arc_stmt_loopContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_foreach`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_foreach?: (ctx: Arc_stmt_foreachContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_throw`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_throw?: (ctx: Arc_stmt_throwContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_stmt_if`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_stmt_if?: (ctx: Arc_stmt_ifContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_expression`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_expression?: (ctx: Arc_expressionContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_expression`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_wrapped_expression?: (ctx: Arc_wrapped_expressionContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_namespace_block`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_namespace_block?: (ctx: Arc_namespace_blockContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_namespace_member`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_namespace_member?: (ctx: Arc_namespace_memberContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_block`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_block?: (ctx: Arc_group_blockContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_derive_list`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_derive_list?: (ctx: Arc_group_derive_listContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_group_member`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_wrapped_group_member?: (ctx: Arc_wrapped_group_memberContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_member`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_member?: (ctx: Arc_group_memberContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_field`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_field?: (ctx: Arc_group_fieldContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_function`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_function?: (ctx: Arc_group_functionContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_keyword`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_lifecycle_keyword?: (ctx: Arc_group_lifecycle_keywordContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_function`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_group_lifecycle_function?: (ctx: Arc_group_lifecycle_functionContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_index`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_index?: (ctx: Arc_indexContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_call_chain`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_call_chain?: (ctx: Arc_call_chainContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_call_chain_term`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_call_chain_term?: (ctx: Arc_call_chain_termContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_self_wrapper`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_self_wrapper?: (ctx: Arc_self_wrapperContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_generic_declaration_wrapper`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_generic_declaration_wrapper?: (ctx: Arc_generic_declaration_wrapperContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_generic_specialization_wrapper`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_generic_specialization_wrapper?: (ctx: Arc_generic_specialization_wrapperContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_enum_declarator`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_enum_declarator?: (ctx: Arc_enum_declaratorContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_enum_member`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_enum_member?: (ctx: Arc_enum_memberContext) => Result;

	/**
	 * Visit a parse tree produced by `ArcSourceCodeParser.arc_enum_accessor`.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	visitArc_enum_accessor?: (ctx: Arc_enum_accessorContext) => Result;
}

