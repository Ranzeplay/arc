using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupNode(ArcGroupDescriptor group) : ArcScopeTreeNodeBase
    {
        public override long Id { get => Descriptor.Id; set => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

        public ArcGroupDescriptor Descriptor { get; set; } = group;

        public ArcGroup SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.GetSignature();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];

        public void ExpandSubDescriptors(ArcGenerationSource source)
        {
            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in SyntaxTree.Functions)
            {
                var fnDescriptor = ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator);
                Descriptor.Functions.Add(fnDescriptor);
                functionNodes.Add(new ArcScopeTreeGroupFunctionNode(fnDescriptor) { SyntaxTree = fn });
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in SyntaxTree.Fields)
            {
                var fieldDescriptor = ArcGroupGenerator.GenerateFieldDescriptor(source, field);
                Descriptor.Fields.Add(fieldDescriptor);
                fieldNodes.Add(new ArcScopeTreeGroupFieldNode(fieldDescriptor));
            }

            AddChildren(functionNodes);
            AddChildren(fieldNodes);
        }
    }
}
