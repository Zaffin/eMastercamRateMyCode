namespace eMastercamRateMyCode
{
    internal class Point2D
    {
        public double X { get; set; } = 0.0;
        public double Y { get; set; } = 0.0;

        public Point2D()
        {
            X = 0.0;
            Y = 0.0;
        }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
