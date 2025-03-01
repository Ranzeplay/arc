using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public class ArcRelocationTarget
    {
        public ArcRelocationTargetType TargetType { get; set; }

        public long Location { get; set; }

        private object _destination = null!;

        public long Parameter { get; set; } = 0;

        public required Guid Layer { get; set; }

        public long TargetLocation
        {
            get
            {
                if (TargetType == ArcRelocationTargetType.Absolute)
                {
                    return (long)_destination;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                if (TargetType == ArcRelocationTargetType.Absolute)
                {
                    _destination = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public long Offset
        {
            get
            {
                if (TargetType == ArcRelocationTargetType.Relative)
                {
                    return (long)_destination;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                if (TargetType == ArcRelocationTargetType.Relative)
                {
                    _destination = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public ArcRelocationLabelType Label
        {
            get
            {
                if (TargetType == ArcRelocationTargetType.Label)
                {
                    return (ArcRelocationLabelType)_destination;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                if (TargetType == ArcRelocationTargetType.Label)
                {
                    _destination = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public ArcScopeTreeNodeBase Symbol
        {
            get
            {
                if (TargetType == ArcRelocationTargetType.Symbol)
                {
                    return (ArcScopeTreeNodeBase)_destination;
                }
                else
                {
                    throw new InvalidOperationException();

                }
            }
            set
            {
                if (TargetType == ArcRelocationTargetType.Symbol)
                {
                    _destination = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
