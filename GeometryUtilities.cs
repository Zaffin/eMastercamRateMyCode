using Mastercam.Curves;
using Mastercam.Database;
using Mastercam.Database.Types;
using Mastercam.Math;
using Mastercam.Support;
using System.Collections.Generic;
using System.Linq;

namespace eMastercamRateMyCode
{
    internal class GeometryUtilities
    {
        public bool CreateLine(Crease crease, bool isSelected = true)
        {
            var lowerPoint = new Point3D(crease.StartPoint.X, crease.StartPoint.Y, 0.0);
            var upperPoint = new Point3D(crease.Endpoint.X, crease.Endpoint.Y, 0.0);
            var line = new LineGeometry(lowerPoint, upperPoint)
            {
                Selected = isSelected
            };

            return line.Commit();         
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
