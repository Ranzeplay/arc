using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcGroupGenerator
    {
        public static ArcGroupDescriptor GenerateGroupDescriptorSkelecton(ArcGenerationSource source, ArcGroup group)
        {
            source.ParentSignature.Locators.Add(group);
            var result = new ArcGroupDescriptor() { Name = source.ParentSignature.GetSignature() };

            foreach (var fn in group.Functions)
            {
                result.Functions.Add(ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator));
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            foreach (var field in group.Fields)
            {
                var fieldDescriptor = GenerateFieldDescriptor(source, field);
                result.Fields.Add(fieldDescriptor);
            }

            return result;
        }

        public static ArcGroupFieldDescriptor GenerateFieldDescriptor(ArcGenerationSource source, ArcGroupField field)
        {
            source.ParentSignature.Locators.Add(field);
            var result = new ArcGroupFieldDescriptor() { Name = source.ParentSignature.GetSignature() };

            return result;
        }
    }
}
