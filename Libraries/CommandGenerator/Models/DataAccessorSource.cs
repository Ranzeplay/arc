using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.CompilerCommandGenerator.Models
{
    internal class DataAccessorSource
    {
        public DataAccessor DataAccessor { get; }

        public enum AccessorOrigin
        {
            Local,
            Global
        };

        public AccessorOrigin Origin { get; }

        public long Slot { get; }

        public DataAccessorSource(DataAccessor dataAccessor, AccessorOrigin origin, long slot)
        {
            DataAccessor = dataAccessor;
            Origin = origin;
            Slot = slot;
        }

        public DataAccessorSource(GenerationContext<DataAccessor> source)
        {
            DataAccessor = source.Component;

            // Local data has more priority than global data
            var localIndex = source.LocalData.FindIndex(d => d.Equals(source.Component.DataDeclarator));
            if (localIndex != -1)
            {
                Origin = AccessorOrigin.Local;
                Slot = localIndex;

                return;
            }

            var globalIndex = source.GlobalData.FindIndex(d => d.Equals(source.Component.DataDeclarator));
            if (globalIndex != -1)
            {
                Origin = AccessorOrigin.Global;
                Slot = globalIndex;

                return;
            }
        }
    }
}
