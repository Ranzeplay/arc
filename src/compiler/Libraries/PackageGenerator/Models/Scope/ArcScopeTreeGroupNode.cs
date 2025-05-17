using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGroupNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

        public ArcGroup SyntaxTree { get; set; }

        public override string SignatureAddend => ShortName;

        public override string Name => ShortName;

        public string ShortName { get; set; }

        public List<ArcScopeTreeGroupFunctionNode> Functions { get; set; } = [];

        public List<ArcScopeTreeGroupFunctionNode> Constructors { get; set; } = [];

        public List<ArcScopeTreeGroupFunctionNode> Destructors { get; set; } = [];

        public List<ArcScopeTreeGroupFieldNode> Fields { get; set; } = [];

        public List<ArcScopeTreeGroupNode> Groups { get; set; } = [];

        public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; } = ArcAccessibility.Private;

        public IEnumerable<ArcScopeTreeGenericTypeNode> GenericTypes => Children.OfType<ArcScopeTreeGenericTypeNode>();

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.Group,

                ..BitConverter.GetBytes(GenericTypes.LongCount()),
                ..GenericTypes.SelectMany(g => BitConverter.GetBytes(g.Id)),

                ..ArcGroupSymbolEncoder.EncodeGroupSymbol(this)
            ];

        public IEnumerable<ArcCompilationLogBase> ExpandSubDescriptors(ArcGenerationSource source)
        {
            var logs = new List<ArcCompilationLogBase>();

            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in SyntaxTree.Functions)
            {
                var (fnDescriptor, iterLogs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeGroupFunctionNode>(source, fn.Declarator);

                logs.AddRange(iterLogs);

                if (fnDescriptor == null)
                {
                    continue;
                }

                fnDescriptor.SyntaxTree = fn;
                Functions.Add(fnDescriptor);
                functionNodes.Add(fnDescriptor);
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in SyntaxTree.Fields)
            {
                var (fieldDescriptor, iterLogs) = ArcGroupGenerator.GenerateFieldDescriptor(source, field);

                logs.AddRange(iterLogs);
                if (fieldDescriptor == null)
                {
                    continue;
                }

                Fields.Add(fieldDescriptor);
                fieldNodes.Add(fieldDescriptor);
            }

            AddChildren(functionNodes);
            AddChildren(fieldNodes);

            return logs;
        }

        public void MoveToConstructor(ArcScopeTreeGroupFunctionNode constructor)
        {
            Constructors.Add(constructor);
            Functions.Remove(constructor);
        }
    }
}
