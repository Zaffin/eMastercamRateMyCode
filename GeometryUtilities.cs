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

        /// <summary>
        /// Extracts the lines from a chain.
        /// </summary>
        /// <param name="chain"> The chain to process.</param>
        /// <param name="level"> The level to place the extracted lines on.</param>
        /// <param name="colorID"> The color to apply to the extracted lines.</param>
        /// <param name="commit"> Optional. True to commit the extracted lines to the database.</param>
        /// <returns>A list containing the extracted lines. </returns>
        public List<LineGeometry> ProcessLinesInChain(Chain chain, int level, int colorID, bool commit = false)
        {
            var lines = new List<LineGeometry>();
     
            foreach (var geometryEntity in ChainManager.GetGeometryInChain(chain))
            {
                if (geometryEntity is LineGeometry line)
                {
                    line.Level = level;
                    line.Color = colorID;

                    if (commit)
                        line.Commit();

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
