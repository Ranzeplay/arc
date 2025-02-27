using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Generation;
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

        public IEnumerable<byte> Encode(ArcScopeTree tree)
        {
            var iterResult = new List<byte>();
            iterResult.Add((byte)ArcSymbolType.Group);
            iterResult.AddRange(new ArcStringEncoder().Encode(Signature));
            iterResult.AddRange(ArcGroupSymbolEncoder.EncodeGroupSymbol(this));

            return iterResult;
        }

        public void ExpandSubDescriptors(ArcGenerationSource source)
        {
            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in SyntaxTree.Functions)
            {
                var fnDescriptor = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeGroupFunctionNode>(source, fn.Declarator);
                fnDescriptor.SyntaxTree = fn;
                Functions.Add(fnDescriptor);
                functionNodes.Add(fnDescriptor);
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in SyntaxTree.Fields)
            {
                var fieldDescriptor = ArcGroupGenerator.GenerateFieldDescriptor(source, field);
                Fields.Add(fieldDescriptor);
                fieldNodes.Add(fieldDescriptor);
            }

            AddChildren(functionNodes);
            AddChildren(fieldNodes);
        }
    }
}
