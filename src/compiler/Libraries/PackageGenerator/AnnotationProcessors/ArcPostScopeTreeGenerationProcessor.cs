using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.AnnotationProcessors
{
    class ArcPostScopeTreeGenerationProcessor
    {
        private static readonly string _dataTypeRemovalSignature = "NArc+NStd+NCompilation+ANoType";
        private static readonly string _symbolIdSignature = "NArc+NStd+NCompilation+ASymbolId";
        private static readonly string _withAnnotationIdSignature = "NArc+NStd+NCompilation+AWithAnnotationId";
        private static readonly string _withAnnotationSignature = "NArc+NStd+NCompilation+AWithAnnotation";
        private static readonly string _constructorSignature = "NArc+NStd+NCompilation+AConstructor";

        public static void All(ref ArcScopeTree scopeTree)
        {
            DataTypeRemoval(ref scopeTree);
            ManualSymbolId(ref scopeTree);
            ManualAnnotatonId(ref scopeTree);
            PreserveAnnotations(ref scopeTree);
            // BindConstructors(ref scopeTree);
        }

        public static void DataTypeRemoval(ref ArcScopeTree scopeTree)
        {
            scopeTree.FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .Where(x => x.Annotations.Keys.Any(k => k.Signature == _dataTypeRemovalSignature))
                .ToList()
                .ForEach(n =>
                {
                    var dataTypeNode = n.Parent.Children.OfType<ArcScopeTreeDataTypeNode>().First(dt => dt.ComplexTypeGroup?.Id == n.Id);
                    n.Parent.RemoveChild(dataTypeNode.Id);
                });
        }

        public static void ManualSymbolId(ref ArcScopeTree scopeTree)
        {
            scopeTree.FlattenedNodes
                .OfType<ArcScopeTreeFunctionNodeBase>()
                .Where(x => x.Annotations.Keys.Any(k => k.Signature == _symbolIdSignature))
                .ToList()
                .ForEach(n =>
                {
                    var newId = n.Annotations
                        .First(n => n.Key.Signature == _symbolIdSignature)
                        .Value.First()
                        .Terms.First()
                        .DataValue!
                        .InstantValue!
                        .IntegerValue!
                        .Value;
                    n.Id = (ulong)newId;
                });

            scopeTree.FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .Where(x => x.Annotations.Keys.Any(k => k.Signature == _symbolIdSignature))
                .ToList()
                .ForEach(n =>
                {
                    var newId = n.Annotations
                        .First(n => n.Key.Signature == _symbolIdSignature)
                        .Value.First()
                        .Terms.First()
                        .DataValue!
                        .InstantValue!
                        .IntegerValue!
                        .Value;
                    n.Id = (ulong)newId;
                });
        }

        public static void ManualAnnotatonId(ref ArcScopeTree scopeTree)
        {
            scopeTree.FlattenedNodes
                .OfType<ArcScopeTreeAnnotationNode>()
                .Where(x => x.TargetGroup.Annotations.Keys.Any(k => k.Signature == _withAnnotationIdSignature))
                .ToList()
                .ForEach(n =>
                {
                    var newId = n.TargetGroup.Annotations
                        .First(n => n.Key.Signature == _withAnnotationIdSignature)
                        .Value.First()
                        .Terms.First()
                        .DataValue!
                        .InstantValue!
                        .IntegerValue!
                        .Value;
                    n.Id = (ulong)newId;
                });
        }

        public static void PreserveAnnotations(ref ArcScopeTree scopeTree)
        {
            scopeTree.FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .Where(x => !x.Annotations.Keys.Any(k => k.Signature == _withAnnotationSignature)
                         && !x.Annotations.Keys.Any(k => k.Signature == _withAnnotationIdSignature)
                )
                .ToList()
                .ForEach(n =>
                {
                    var annotation = n.Parent.Children
                        .OfType<ArcScopeTreeAnnotationNode>()
                        .First(a => a.TargetGroup.Id == n.Id);

                    n.Parent.RemoveChild(annotation.Id);
                });
        }

        public static void BindConstructors(ref ArcScopeTree scopeTree)
        {
            /*
              scopeTree.FlattenedNodes
                  .OfType<ArcScopeTreeGroupFunctionNode>()
                  .Where(x => x.Annotations.Keys.Any(k => k.Signature == _constructorSignature))
                  .ToList()
                  .ForEach(n =>
                  {
                      var parent = n.Parent as ArcScopeTreeGroupNode;
                      parent!.MoveToConstructor(n);
                  });
             */
            
            throw new InvalidOperationException();
        }
    }
}
