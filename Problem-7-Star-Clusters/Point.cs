using System;

public class Point
{
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    public double X { get; set; }

    public double Y { get; set; }

    public double ComputeDistanceTo(Point other)
    {
        // The square of the Eucledian distance can also be used, there is no need to take
        // the square root of this expression (and it is faster)
        return Math.Pow(other.X - this.X, 2) + Math.Pow(other.Y - this.Y, 2);
    }
}