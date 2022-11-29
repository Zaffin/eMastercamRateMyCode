using System.Linq;
using System.Collections.Generic;

using Mastercam.Curves;


namespace eMastercamRateMyCode
{
    internal class UtilitiesFacade
    {
        private readonly SelectionUtilities selection;
        private readonly GeometryUtilities geometry;

        public UtilitiesFacade()
        {
            selection = new SelectionUtilities();
            geometry = new GeometryUtilities();
        }

        public void OffsetCutChain(int cutChainlevel, EntityData lower, EntityData upper)
        {

            selection.CreateLevel(lower.Level.Number, lower.Level.Name);

            selection.CreateLevel(upper.Level.Number, upper.Level.Name);

            foreach (var chain in geometry.ChainAllByLevel(cutChainlevel))
            {
                var lowerChainRight = geometry.OffsetChainRight(chain, lower.SmallOffsetRadius);
                geometry.ProcessLinesInChain(lowerChainRight, lower.Level.Number, lower.ColorID);

                var lowerChainLeft = geometry.OffsetChainLeft(chain, lower.LargeOffsetRadius);         
                geometry.ProcessLinesInChain(lowerChainLeft, lower.Level.Number, lower.ColorID);
            
                var upperChainRight = geometry.OffsetChainRight(chain, upper.LargeOffsetRadius);
                geometry.ProcessLinesInChain(upperChainRight, upper.Level.Number, upper.ColorID);

                var upperChainLeft = geometry.OffsetChainLeft(chain, upper.SmallOffsetRadius);
                geometry.ProcessLinesInChain(upperChainLeft, upper.Level.Number, upper.ColorID);
            }

            selection.ClearGroupColors();
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
                var lowerChainLeftSmall = geometry.OffsetChainLeft(chain, lower.SmallOffsetRadius);
                lowerLines.AddRange(geometry.ProcessLinesInChain(lowerChainLeftSmall, lower.Level.Number, lower.ColorID));

                var lowerChainLeftLarge = geometry.OffsetChainLeft(chain, lower.LargeOffsetRadius);
                lowerLines.AddRange(geometry.ProcessLinesInChain(lowerChainLeftLarge, lower.Level.Number, lower.ColorID));

                var lowerChainRightSmall = geometry.OffsetChainRight(chain, lower.SmallOffsetRadius);
                lowerLines.AddRange(geometry.ProcessLinesInChain(lowerChainRightSmall, lower.Level.Number, lower.ColorID));

                var lowerChainRightLarge = geometry.OffsetChainRight(chain, lower.LargeOffsetRadius);
                lowerLines.AddRange(geometry.ProcessLinesInChain(lowerChainRightSmall, lower.Level.Number, lower.ColorID));


                var upperChainRightLarge = geometry.OffsetChainLeft(chain, upper.LargeOffsetRadius);
                upperLines.AddRange(geometry.ProcessLinesInChain(upperChainRightLarge, lower.Level.Number, lower.ColorID));

                var upperChainRightSmall = geometry.OffsetChainRight(chain, upper.SmallOffsetRadius);
                upperLines.AddRange(geometry.ProcessLinesInChain(upperChainRightSmall, lower.Level.Number, lower.ColorID));
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
                geometry.CreateLine(lineOne);

                if (isPair)                
                {
                    var lineTwo = lines[nextIndex];
                    geometry.CreateLine(lineTwo);
                    index++;
                }
            }

            selection.MoveSelectedToLevel(data.Level.Number);
        }
    }
}
