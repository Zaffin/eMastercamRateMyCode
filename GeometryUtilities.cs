using System.Linq;
using System.Collections.Generic;

using Mastercam.Math;
using Mastercam.Support;
using Mastercam.Curves;
using Mastercam.Database;
using Mastercam.Database.Types;


namespace eMastercamRateMyCode
{
    internal class GeometryUtilities
    {
        public bool CreateLine(LineGeometry line, bool isSelected = true)
        {
            line.Selected = isSelected;
            return line.Commit();         
        }

        public List<LineGeometry> ProcessChain(Chain chain, int level, int colorID, bool isSelected = true)
        {
            var lines = new List<LineGeometry>();
     
            foreach (var geometryEntity in ChainManager.GetGeometryInChain(chain))
            {
                if (geometryEntity is LineGeometry line)
                {
                    line.Level = level;
                    line.Color = colorID;
                    line.Selected = isSelected;

                    if (line.Commit())
                        lines.Add(line);
                }
            }

            return lines;
        }

        public List<LineGeometry> ProcessResultLines(int colorID, bool isSelected = true)
        {
            var lines = new List<LineGeometry>();

            var resultGeometry = SearchManager.GetResultGeometry();
            foreach (var geometryEntity in resultGeometry)
            {
                if (geometryEntity is LineGeometry line)
                {
                    line.Color = colorID;
                    line.Selected = isSelected;

                    if (line.Commit())
                        lines.Add(line);
                }
            }

            return lines;
        }

        public List<Chain> ChainAllByLevel(int levelToChain) 
            => ChainManager.ChainAll(levelToChain).ToList();

        public Chain OffsetChainLeft(Chain chainToOffset, double radius)
        {
            return OffsetChain(chainToOffset, OffsetSideType.Left, radius);
        }

        public Chain OffsetChainRight(Chain chainToOffset, double radius)
        {
            return OffsetChain(chainToOffset, OffsetSideType.Right, radius);
        }

        private Chain OffsetChain(Chain chainToOffset, OffsetSideType offsetSide, double radius)
        {
            return chainToOffset.OffsetChain2D(offsetSide, radius, OffsetRollCornerType.None, .5, false, .005, false);
        }
    }
}
