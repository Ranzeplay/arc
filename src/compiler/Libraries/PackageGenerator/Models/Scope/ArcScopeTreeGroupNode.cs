using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGroupNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

        public ArcGroup SyntaxTree { get; set; }

        public override string SignatureAddend => ShortName;

        public override string Name => ShortName;

        public string ShortName { get; set; }

        public IEnumerable<ArcScopeTreeGroupFunctionNode> Functions => GetChildren<ArcScopeTreeGroupFunctionNode>();

        public IEnumerable<ArcScopeTreeLifecycleFunctionNode> LifecycleFunctions => GetChildren<ArcScopeTreeLifecycleFunctionNode>();

        public IEnumerable<ArcScopeTreeGroupFieldNode> Fields => GetChildren<ArcScopeTreeGroupFieldNode>();

        public IEnumerable<ArcScopeTreeGroupNode> Groups => GetChildren<ArcScopeTreeGroupNode>();

        public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
        
        public IEnumerable<ArcGroupDerivationLink> Derivations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; } = ArcAccessibility.Private;

        public IEnumerable<ArcScopeTreeGenericTypeNode> GenericTypes => GetChildren<ArcScopeTreeGenericTypeNode>();

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.Group,
                ..ArcGroupSymbolEncoder.EncodeGroupSymbol(this)
            ];

        public IEnumerable<ArcCompilationLogBase> ExpandSubDescriptors(ArcGenerationSource source)
        {
            var logs = new List<ArcCompilationLogBase>();
            
            Derivations = SyntaxTree.Derivations
                .Select(derivation => ArcDataTypeHelper.GetDataType(source, derivation))
                .Select(typeProxy => new ArcGroupDerivationLink { Target = typeProxy, GenericTypeMap = [] })
                .ToArray();

            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in SyntaxTree.Functions)
            {
                var (fnDescriptor, iterLogs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeGroupFunctionNode>(source, fn.Declarator);

                logs.AddRange(iterLogs);

                fnDescriptor.SyntaxTree = fn;
                functionNodes.Add(fnDescriptor);
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }
            
            var lifecycleFunctionNodes = new List<ArcScopeTreeLifecycleFunctionNode>();
            foreach (var lifecycleFn in SyntaxTree.LifecycleFunctions)
            {
                var (fnDesc, iterLogs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeLifecycleFunctionNode>(source, lifecycleFn.Declarator);

                logs.AddRange(iterLogs);

                fnDesc.SyntaxTree = lifecycleFn;
                lifecycleFunctionNodes.Add(fnDesc);
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in SyntaxTree.Fields)
            {
                var (fieldDescriptor, iterLogs) = ArcGroupGenerator.GenerateFieldDescriptor(source, field);

                logs.AddRange(iterLogs);

                fieldNodes.Add(fieldDescriptor);
            }

            AddChildren(functionNodes);
            AddChildren(lifecycleFunctionNodes);
            AddChildren(fieldNodes);

            return logs;
        }
    }
}
