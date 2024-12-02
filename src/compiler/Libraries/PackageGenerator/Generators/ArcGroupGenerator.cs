using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcGroupGenerator
    {
        public static ArcGroupDescriptor GenerateGroupDescriptorSkelecton(ArcGenerationSource source, ArcGroup group)
        {
            source.ParentSignature.Locators = source.ParentSignature.Locators.Append(group);
            var result = new ArcGroupDescriptor() { Name = source.ParentSignature.GetSignature() };

            foreach (var fn in group.Functions)
            {
                result.Functions = result.Functions.Append(ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator));
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count() - 1);
            }

            foreach (var field in group.Fields)
            {
                var fieldDescriptor = GenerateFieldDescriptor(source, field);
                result.Fields = result.Fields.Append(fieldDescriptor);
            }

            return result;
        }

        public static ArcGroupFieldDescriptor GenerateFieldDescriptor(ArcGenerationSource source, ArcGroupField field)
        {
            source.ParentSignature.Locators = source.ParentSignature.Locators.Append(field);
            var result = new ArcGroupFieldDescriptor() { Name = source.ParentSignature.GetSignature() };

            return result;
        }
    }
}
