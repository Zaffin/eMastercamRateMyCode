using Mastercam.Curves;

namespace eMastercamRateMyCode
{
    internal class Crease
    {
        public Point2D StartPoint { get; set; } = new Point2D();
        public Point2D Endpoint { get; set; } = new Point2D();

        public Crease(LineGeometry line)
        {
            StartPoint = new Point2D(line.EndPoint1.x, line.EndPoint1.y);
            Endpoint = new Point2D(line.EndPoint2.x, line.EndPoint2.y);
        }
    }
}
