using Mastercam.Curves;
using Mastercam.Database;
using System.Collections.Generic;
using System.Linq;

namespace eMastercamRateMyCode
{
    internal class UtilitiesFacade
    {
        private SelectionUtilities selection;
        private GeometryUtilities geometry;

        public UtilitiesFacade()
        {
            selection = new SelectionUtilities();
            geometry = new GeometryUtilities();
        }

        public void OffsetCutChain(int cutChainlevel, EntityData lower, EntityData upper)
        {
            selection.UnselectAll();
            selection.TurnOffVisibleLevels();

            selection.SetMainLevel(cutChainlevel);

            selection.CreateLevel(lower.Level.Number, lower.Level.Name);

            selection.CreateLevel(upper.Level.Number, upper.Level.Name);

            foreach (var chain in geometry.ChainAllByLevel(cutChainlevel))
            {
                geometry.OffsetChainRight(chain, lower.SmallOffsetRadius);
                geometry.OffsetChainLeft(chain, lower.LargeOffsetRadius);

                geometry.ProcessResultLines(lower.ColorID);
                selection.MoveSelectedToLevel(lower.Level.Number);

                geometry.OffsetChainLeft(chain, upper.SmallOffsetRadius);
                geometry.OffsetChainRight(chain, upper.LargeOffsetRadius);

                geometry.ProcessResultLines(upper.ColorID);
                selection.MoveSelectedToLevel(upper.Level.Number);
            }
        }

        public (bool wasGeometryCreated, List<LineGeometry> lowerLines, List<LineGeometry> upperLines) OffsetCreaseChain(int creaseChainlevel, EntityData lower, EntityData upper)
        {
            var lowerLines = new List<LineGeometry>();
            var upperLines = new List<LineGeometry>();

            selection.UnselectAll();
            selection.TurnOffVisibleLevels();

            selection.SetMainLevel(creaseChainlevel);

            selection.CreateLevel(lower.Level.Number, lower.Level.Name);

            selection.CreateLevel(upper.Level.Number, upper.Level.Name);

            foreach (var chain in geometry.ChainAllByLevel(creaseChainlevel))
            {
                geometry.OffsetChainLeft(chain, lower.SmallOffsetRadius);
                geometry.OffsetChainLeft(chain, lower.LargeOffsetRadius);

                geometry.OffsetChainRight(chain, lower.SmallOffsetRadius);
                geometry.OffsetChainRight(chain, lower.LargeOffsetRadius);

                lowerLines = geometry.ProcessResultLines(lower.ColorID);
                selection.MoveSelectedToLevel(lower.Level.Number);

                geometry.OffsetChainLeft(chain, upper.LargeOffsetRadius);
                geometry.OffsetChainRight(chain, upper.SmallOffsetRadius);

                upperLines = geometry.ProcessResultLines(upper.ColorID);
                selection.MoveSelectedToLevel(upper.Level.Number);
            }

            if (lowerLines.Any() && upperLines.Any())
                return (true, lowerLines, upperLines);
            else
                return (false, lowerLines, upperLines);
        }

        public void ConnectLines(EntityData data, List<LineGeometry> lines)
        {
            for (int index = 0; index < lines.Count(); index++)
            {
                var nextIndex = index + 1;

                var isPair = (nextIndex < lines.Count() - 1);

                var lineOne = lines[index];
                geometry.CreateLine(new Crease(lineOne));

                if (isPair)                
                {
                    var lineTwo = lines[nextIndex];
                    geometry.CreateLine(new Crease(lineTwo));
                    index++;
                }
            }

            geometry.ProcessResultLines(data.ColorID);
            selection.MoveSelectedToLevel(data.Level.Number);
        }
    }
}
