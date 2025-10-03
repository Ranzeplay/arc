// Generated from src/parser/ArcSourceCodeParser.g4 by ANTLR 4.9.0-SNAPSHOT


import { ParseTreeListener } from "antlr4ts/tree/ParseTreeListener";

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
 * This interface defines a complete listener for a parse tree produced by
 * `ArcSourceCodeParser`.
 */
export interface ArcSourceCodeParserListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_compilation_unit`.
	 * @param ctx the parse tree
	 */
	enterArc_compilation_unit?: (ctx: Arc_compilation_unitContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_compilation_unit`.
	 * @param ctx the parse tree
	 */
	exitArc_compilation_unit?: (ctx: Arc_compilation_unitContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_accessibility`.
	 * @param ctx the parse tree
	 */
	enterArc_accessibility?: (ctx: Arc_accessibilityContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_accessibility`.
	 * @param ctx the parse tree
	 */
	exitArc_accessibility?: (ctx: Arc_accessibilityContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_mutability`.
	 * @param ctx the parse tree
	 */
	enterArc_mutability?: (ctx: Arc_mutabilityContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_mutability`.
	 * @param ctx the parse tree
	 */
	exitArc_mutability?: (ctx: Arc_mutabilityContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_namespace_identifier`.
	 * @param ctx the parse tree
	 */
	enterArc_namespace_identifier?: (ctx: Arc_namespace_identifierContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_namespace_identifier`.
	 * @param ctx the parse tree
	 */
	exitArc_namespace_identifier?: (ctx: Arc_namespace_identifierContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_namespace_declarator`.
	 * @param ctx the parse tree
	 */
	enterArc_namespace_declarator?: (ctx: Arc_namespace_declaratorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_namespace_declarator`.
	 * @param ctx the parse tree
	 */
	exitArc_namespace_declarator?: (ctx: Arc_namespace_declaratorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_namespace_limiter`.
	 * @param ctx the parse tree
	 */
	enterArc_namespace_limiter?: (ctx: Arc_namespace_limiterContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_namespace_limiter`.
	 * @param ctx the parse tree
	 */
	exitArc_namespace_limiter?: (ctx: Arc_namespace_limiterContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_single_identifier`.
	 * @param ctx the parse tree
	 */
	enterArc_single_identifier?: (ctx: Arc_single_identifierContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_single_identifier`.
	 * @param ctx the parse tree
	 */
	exitArc_single_identifier?: (ctx: Arc_single_identifierContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_full_identifier`.
	 * @param ctx the parse tree
	 */
	enterArc_full_identifier?: (ctx: Arc_full_identifierContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_full_identifier`.
	 * @param ctx the parse tree
	 */
	exitArc_full_identifier?: (ctx: Arc_full_identifierContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_flexible_identifier`.
	 * @param ctx the parse tree
	 */
	enterArc_flexible_identifier?: (ctx: Arc_flexible_identifierContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_flexible_identifier`.
	 * @param ctx the parse tree
	 */
	exitArc_flexible_identifier?: (ctx: Arc_flexible_identifierContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_primitive_data_type`.
	 * @param ctx the parse tree
	 */
	enterArc_primitive_data_type?: (ctx: Arc_primitive_data_typeContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_primitive_data_type`.
	 * @param ctx the parse tree
	 */
	exitArc_primitive_data_type?: (ctx: Arc_primitive_data_typeContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_array_indicator`.
	 * @param ctx the parse tree
	 */
	enterArc_array_indicator?: (ctx: Arc_array_indicatorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_array_indicator`.
	 * @param ctx the parse tree
	 */
	exitArc_array_indicator?: (ctx: Arc_array_indicatorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_data_type`.
	 * @param ctx the parse tree
	 */
	enterArc_data_type?: (ctx: Arc_data_typeContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_data_type`.
	 * @param ctx the parse tree
	 */
	exitArc_data_type?: (ctx: Arc_data_typeContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_data_declarator`.
	 * @param ctx the parse tree
	 */
	enterArc_data_declarator?: (ctx: Arc_data_declaratorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_data_declarator`.
	 * @param ctx the parse tree
	 */
	exitArc_data_declarator?: (ctx: Arc_data_declaratorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_self_data_declarator`.
	 * @param ctx the parse tree
	 */
	enterArc_self_data_declarator?: (ctx: Arc_self_data_declaratorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_self_data_declarator`.
	 * @param ctx the parse tree
	 */
	exitArc_self_data_declarator?: (ctx: Arc_self_data_declaratorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_arg_list`.
	 * @param ctx the parse tree
	 */
	enterArc_arg_list?: (ctx: Arc_arg_listContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_arg_list`.
	 * @param ctx the parse tree
	 */
	exitArc_arg_list?: (ctx: Arc_arg_listContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_wrapped_arg_list`.
	 * @param ctx the parse tree
	 */
	enterArc_wrapped_arg_list?: (ctx: Arc_wrapped_arg_listContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_arg_list`.
	 * @param ctx the parse tree
	 */
	exitArc_wrapped_arg_list?: (ctx: Arc_wrapped_arg_listContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_param_list`.
	 * @param ctx the parse tree
	 */
	enterArc_param_list?: (ctx: Arc_param_listContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_param_list`.
	 * @param ctx the parse tree
	 */
	exitArc_param_list?: (ctx: Arc_param_listContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_wrapped_param_list`.
	 * @param ctx the parse tree
	 */
	enterArc_wrapped_param_list?: (ctx: Arc_wrapped_param_listContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_param_list`.
	 * @param ctx the parse tree
	 */
	exitArc_wrapped_param_list?: (ctx: Arc_wrapped_param_listContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_annotation`.
	 * @param ctx the parse tree
	 */
	enterArc_annotation?: (ctx: Arc_annotationContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_annotation`.
	 * @param ctx the parse tree
	 */
	exitArc_annotation?: (ctx: Arc_annotationContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_function_declarator`.
	 * @param ctx the parse tree
	 */
	enterArc_function_declarator?: (ctx: Arc_function_declaratorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_function_declarator`.
	 * @param ctx the parse tree
	 */
	exitArc_function_declarator?: (ctx: Arc_function_declaratorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_function_call_base`.
	 * @param ctx the parse tree
	 */
	enterArc_function_call_base?: (ctx: Arc_function_call_baseContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_function_call_base`.
	 * @param ctx the parse tree
	 */
	exitArc_function_call_base?: (ctx: Arc_function_call_baseContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_wrapped_function_body`.
	 * @param ctx the parse tree
	 */
	enterArc_wrapped_function_body?: (ctx: Arc_wrapped_function_bodyContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_function_body`.
	 * @param ctx the parse tree
	 */
	exitArc_wrapped_function_body?: (ctx: Arc_wrapped_function_bodyContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_function_block`.
	 * @param ctx the parse tree
	 */
	enterArc_function_block?: (ctx: Arc_function_blockContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_function_block`.
	 * @param ctx the parse tree
	 */
	exitArc_function_block?: (ctx: Arc_function_blockContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_bool_value`.
	 * @param ctx the parse tree
	 */
	enterArc_bool_value?: (ctx: Arc_bool_valueContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_bool_value`.
	 * @param ctx the parse tree
	 */
	exitArc_bool_value?: (ctx: Arc_bool_valueContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_instant_value`.
	 * @param ctx the parse tree
	 */
	enterArc_instant_value?: (ctx: Arc_instant_valueContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_instant_value`.
	 * @param ctx the parse tree
	 */
	exitArc_instant_value?: (ctx: Arc_instant_valueContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_type_value`.
	 * @param ctx the parse tree
	 */
	enterArc_type_value?: (ctx: Arc_type_valueContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_type_value`.
	 * @param ctx the parse tree
	 */
	exitArc_type_value?: (ctx: Arc_type_valueContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_data_value`.
	 * @param ctx the parse tree
	 */
	enterArc_data_value?: (ctx: Arc_data_valueContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_data_value`.
	 * @param ctx the parse tree
	 */
	exitArc_data_value?: (ctx: Arc_data_valueContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_constructor_call`.
	 * @param ctx the parse tree
	 */
	enterArc_constructor_call?: (ctx: Arc_constructor_callContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_constructor_call`.
	 * @param ctx the parse tree
	 */
	exitArc_constructor_call?: (ctx: Arc_constructor_callContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_statement`.
	 * @param ctx the parse tree
	 */
	enterArc_statement?: (ctx: Arc_statementContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_statement`.
	 * @param ctx the parse tree
	 */
	exitArc_statement?: (ctx: Arc_statementContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_link`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_link?: (ctx: Arc_stmt_linkContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_link`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_link?: (ctx: Arc_stmt_linkContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_return`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_return?: (ctx: Arc_stmt_returnContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_return`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_return?: (ctx: Arc_stmt_returnContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_decl`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_decl?: (ctx: Arc_stmt_declContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_decl`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_decl?: (ctx: Arc_stmt_declContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_assign`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_assign?: (ctx: Arc_stmt_assignContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_assign`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_assign?: (ctx: Arc_stmt_assignContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_break`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_break?: (ctx: Arc_stmt_breakContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_break`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_break?: (ctx: Arc_stmt_breakContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_continue`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_continue?: (ctx: Arc_stmt_continueContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_continue`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_continue?: (ctx: Arc_stmt_continueContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_call`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_call?: (ctx: Arc_stmt_callContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_call`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_call?: (ctx: Arc_stmt_callContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_while`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_while?: (ctx: Arc_stmt_whileContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_while`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_while?: (ctx: Arc_stmt_whileContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_for`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_for?: (ctx: Arc_stmt_forContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_for`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_for?: (ctx: Arc_stmt_forContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_loop`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_loop?: (ctx: Arc_stmt_loopContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_loop`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_loop?: (ctx: Arc_stmt_loopContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_foreach`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_foreach?: (ctx: Arc_stmt_foreachContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_foreach`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_foreach?: (ctx: Arc_stmt_foreachContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_throw`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_throw?: (ctx: Arc_stmt_throwContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_throw`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_throw?: (ctx: Arc_stmt_throwContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_stmt_if`.
	 * @param ctx the parse tree
	 */
	enterArc_stmt_if?: (ctx: Arc_stmt_ifContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_stmt_if`.
	 * @param ctx the parse tree
	 */
	exitArc_stmt_if?: (ctx: Arc_stmt_ifContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_expression`.
	 * @param ctx the parse tree
	 */
	enterArc_expression?: (ctx: Arc_expressionContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_expression`.
	 * @param ctx the parse tree
	 */
	exitArc_expression?: (ctx: Arc_expressionContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_wrapped_expression`.
	 * @param ctx the parse tree
	 */
	enterArc_wrapped_expression?: (ctx: Arc_wrapped_expressionContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_expression`.
	 * @param ctx the parse tree
	 */
	exitArc_wrapped_expression?: (ctx: Arc_wrapped_expressionContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_namespace_block`.
	 * @param ctx the parse tree
	 */
	enterArc_namespace_block?: (ctx: Arc_namespace_blockContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_namespace_block`.
	 * @param ctx the parse tree
	 */
	exitArc_namespace_block?: (ctx: Arc_namespace_blockContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_namespace_member`.
	 * @param ctx the parse tree
	 */
	enterArc_namespace_member?: (ctx: Arc_namespace_memberContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_namespace_member`.
	 * @param ctx the parse tree
	 */
	exitArc_namespace_member?: (ctx: Arc_namespace_memberContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_block`.
	 * @param ctx the parse tree
	 */
	enterArc_group_block?: (ctx: Arc_group_blockContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_block`.
	 * @param ctx the parse tree
	 */
	exitArc_group_block?: (ctx: Arc_group_blockContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_derive_list`.
	 * @param ctx the parse tree
	 */
	enterArc_group_derive_list?: (ctx: Arc_group_derive_listContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_derive_list`.
	 * @param ctx the parse tree
	 */
	exitArc_group_derive_list?: (ctx: Arc_group_derive_listContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_wrapped_group_member`.
	 * @param ctx the parse tree
	 */
	enterArc_wrapped_group_member?: (ctx: Arc_wrapped_group_memberContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_wrapped_group_member`.
	 * @param ctx the parse tree
	 */
	exitArc_wrapped_group_member?: (ctx: Arc_wrapped_group_memberContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_member`.
	 * @param ctx the parse tree
	 */
	enterArc_group_member?: (ctx: Arc_group_memberContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_member`.
	 * @param ctx the parse tree
	 */
	exitArc_group_member?: (ctx: Arc_group_memberContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_field`.
	 * @param ctx the parse tree
	 */
	enterArc_group_field?: (ctx: Arc_group_fieldContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_field`.
	 * @param ctx the parse tree
	 */
	exitArc_group_field?: (ctx: Arc_group_fieldContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_function`.
	 * @param ctx the parse tree
	 */
	enterArc_group_function?: (ctx: Arc_group_functionContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_function`.
	 * @param ctx the parse tree
	 */
	exitArc_group_function?: (ctx: Arc_group_functionContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_keyword`.
	 * @param ctx the parse tree
	 */
	enterArc_group_lifecycle_keyword?: (ctx: Arc_group_lifecycle_keywordContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_keyword`.
	 * @param ctx the parse tree
	 */
	exitArc_group_lifecycle_keyword?: (ctx: Arc_group_lifecycle_keywordContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_function`.
	 * @param ctx the parse tree
	 */
	enterArc_group_lifecycle_function?: (ctx: Arc_group_lifecycle_functionContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_group_lifecycle_function`.
	 * @param ctx the parse tree
	 */
	exitArc_group_lifecycle_function?: (ctx: Arc_group_lifecycle_functionContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_index`.
	 * @param ctx the parse tree
	 */
	enterArc_index?: (ctx: Arc_indexContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_index`.
	 * @param ctx the parse tree
	 */
	exitArc_index?: (ctx: Arc_indexContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_call_chain`.
	 * @param ctx the parse tree
	 */
	enterArc_call_chain?: (ctx: Arc_call_chainContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_call_chain`.
	 * @param ctx the parse tree
	 */
	exitArc_call_chain?: (ctx: Arc_call_chainContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_call_chain_term`.
	 * @param ctx the parse tree
	 */
	enterArc_call_chain_term?: (ctx: Arc_call_chain_termContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_call_chain_term`.
	 * @param ctx the parse tree
	 */
	exitArc_call_chain_term?: (ctx: Arc_call_chain_termContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_self_wrapper`.
	 * @param ctx the parse tree
	 */
	enterArc_self_wrapper?: (ctx: Arc_self_wrapperContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_self_wrapper`.
	 * @param ctx the parse tree
	 */
	exitArc_self_wrapper?: (ctx: Arc_self_wrapperContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_generic_declaration_wrapper`.
	 * @param ctx the parse tree
	 */
	enterArc_generic_declaration_wrapper?: (ctx: Arc_generic_declaration_wrapperContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_generic_declaration_wrapper`.
	 * @param ctx the parse tree
	 */
	exitArc_generic_declaration_wrapper?: (ctx: Arc_generic_declaration_wrapperContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_generic_specialization_wrapper`.
	 * @param ctx the parse tree
	 */
	enterArc_generic_specialization_wrapper?: (ctx: Arc_generic_specialization_wrapperContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_generic_specialization_wrapper`.
	 * @param ctx the parse tree
	 */
	exitArc_generic_specialization_wrapper?: (ctx: Arc_generic_specialization_wrapperContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_enum_declarator`.
	 * @param ctx the parse tree
	 */
	enterArc_enum_declarator?: (ctx: Arc_enum_declaratorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_enum_declarator`.
	 * @param ctx the parse tree
	 */
	exitArc_enum_declarator?: (ctx: Arc_enum_declaratorContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_enum_member`.
	 * @param ctx the parse tree
	 */
	enterArc_enum_member?: (ctx: Arc_enum_memberContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_enum_member`.
	 * @param ctx the parse tree
	 */
	exitArc_enum_member?: (ctx: Arc_enum_memberContext) => void;

	/**
	 * Enter a parse tree produced by `ArcSourceCodeParser.arc_enum_accessor`.
	 * @param ctx the parse tree
	 */
	enterArc_enum_accessor?: (ctx: Arc_enum_accessorContext) => void;
	/**
	 * Exit a parse tree produced by `ArcSourceCodeParser.arc_enum_accessor`.
	 * @param ctx the parse tree
	 */
	exitArc_enum_accessor?: (ctx: Arc_enum_accessorContext) => void;
}

