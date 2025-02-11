using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcRelocationHelper
    {
        public static int LocateLabelRelativeLocation(IEnumerable<ArcRelocationLabel> labels, bool forwardSearch, ArcRelocationTarget target, long count)
        {
            var label = target.Label;
            var antiLabel = label.GetAntiLabel();
            var sourceLocation = target.Location;

            var list = labels.ToList();
            var directionList = forwardSearch switch
            {
                true => list.Where(l => l.Location >= sourceLocation).OrderBy(l => l.Location),
                false => list.Where(l => l.Location <= sourceLocation).OrderByDescending(l => l.Location)
            };

            var layer = 0;
            for (int i = 0; i < directionList.Count(); i++)
            {
                var l = directionList.ElementAt(i);

                if (l.Type == antiLabel)
                {
                    layer++;
                }
                else if (l.Type == label)
                {
                    layer--;
                    if (layer <= 0)
                    {
                        count--;
                        if (count == 0)
                        {
                            return (forwardSearch ? 1 : -1) * (i + 1);
                        }
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(labels), "Cannot find the corresponding label");
        }
    }
}
