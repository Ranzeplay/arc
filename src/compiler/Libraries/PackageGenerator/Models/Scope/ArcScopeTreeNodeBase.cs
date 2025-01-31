using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public abstract class ArcScopeTreeNodeBase
    {
        public virtual long Id { get; set; } = new Random().NextInt64();

        public abstract string Name { get; }

        public abstract ArcScopeTreeNodeType NodeType { get; }

        public List<ArcScopeTreeNodeBase> Children { get; set; } = [];

        public virtual ArcScopeTreeNodeBase Parent { get; set; }

        public ArcScopeTreeNodeBase AddChild(ArcScopeTreeNodeBase child)
        {
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public ArcScopeTreeNodeBase AddChildren(IEnumerable<ArcScopeTreeNodeBase> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
            return this;
        }

        public IEnumerable<string> ParentSignatureStack
        {
            get
            {
                var stack = new List<string>();
                var current = this;
                while (current is not ArcRootScopeNode)
                {
                    stack.Add(current.SignatureAddend);
                    current = current.Parent;
                }
                return stack;
            }
        }

        public abstract string SignatureAddend { get; }

        public string Signature { get => string.Join("+", ParentSignatureStack.Reverse()); }

        public ArcRootScopeNode Root
        {
            get
            {
                var current = this;
                while (current is not ArcRootScopeNode)
                {
                    current = current.Parent;
                }
                return (ArcRootScopeNode)current;
            }
        }

        public IEnumerable<ArcScopeTreeNodeBase> GetAncestors()
        {
            var current = this;
            while (current is not ArcRootScopeNode)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public T? GetSpecificChild<T>(Func<T, bool> predicate, bool recursive = false) where T : ArcScopeTreeNodeBase
        {
            return GetSpecificChild(n => n is T t && predicate(t), recursive) as T;
        }

        public ArcScopeTreeNodeBase? GetSpecificChild(Func<ArcScopeTreeNodeBase, bool> predicate, bool recursive = false)
        {
            return GetChildren(predicate, recursive).FirstOrDefault();
        }

        public IEnumerable<T> GetSpecificChildren<T>(Func<T, bool> predicate, bool recursive = false) where T : ArcScopeTreeNodeBase
        {
            return GetChildren(n => n is T t && predicate(t), recursive).Cast<T>();
        }

        public IEnumerable<ArcScopeTreeNodeBase> GetChildren(Func<ArcScopeTreeNodeBase, bool> predicate, bool recursive = false)
        {
            foreach (var child in Children)
            {
                if (predicate(child))
                {
                    yield return child;
                }
                if (recursive)
                {
                    foreach (var result in child.GetChildren(predicate, true))
                    {
                        yield return result;
                    }
                }
            }
        }

        public IEnumerable<T> GetChildren<T>(Func<ArcScopeTreeNodeBase, bool> predicate, bool recursive = false) where T : ArcScopeTreeNodeBase
        {
            return GetChildren(n => n is T, recursive).Cast<T>();
        }

        public IEnumerable<T> GetChildren<T>(bool recursive = false) where T : ArcScopeTreeNodeBase
        {
            return GetChildren<T>(n => true, recursive);
        }

        public abstract IEnumerable<ArcSymbolBase> GetSymbols();
    }
}
