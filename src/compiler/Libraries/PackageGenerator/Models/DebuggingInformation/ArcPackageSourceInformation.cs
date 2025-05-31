namespace Arc.Compiler.PackageGenerator.Models.DebuggingInformation
{
    public class ArcPackageSourceInformation
    {
        public long Uid { get; set; }

        public Dictionary<ulong, string> SymbolMapping { get; set; } = [];

        public Dictionary<ulong, Dictionary<ulong, string>> FunctionDataSlotMapping { get; set; } = [];

        public void MergeMappings(ArcPackageSourceInformation packageSourceInformation)
        {
            foreach (var functionDataSlotMapping in packageSourceInformation.FunctionDataSlotMapping)
            {
                if (!FunctionDataSlotMapping.ContainsKey(functionDataSlotMapping.Key))
                {
                    FunctionDataSlotMapping[functionDataSlotMapping.Key] = functionDataSlotMapping.Value;
                }
                else
                {
                    foreach (var dataSlotMapping in functionDataSlotMapping.Value)
                    {
                        if (!FunctionDataSlotMapping[functionDataSlotMapping.Key].ContainsKey(dataSlotMapping.Key))
                        {
                            FunctionDataSlotMapping[functionDataSlotMapping.Key][dataSlotMapping.Key] = dataSlotMapping.Value;
                        }
                    }
                }
            }

            foreach (var symbolMapping in packageSourceInformation.SymbolMapping)
            {
                if (!SymbolMapping.ContainsKey(symbolMapping.Key))
                {
                    SymbolMapping[symbolMapping.Key] = symbolMapping.Value;
                }
            }
        }
    }
}
