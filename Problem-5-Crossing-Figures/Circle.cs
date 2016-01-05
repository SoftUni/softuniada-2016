using System;
using System.Linq;

public class Circle
{
    private const double Epsilon = 0.01;

    public Circle(Point center, double radius)
    {
        this.Center = center;
        this.Radius = radius;
        this.Top = new Point(center.X, center.Y + radius);
        this.Right = new Point(center.X + radius, center.Y);
        this.Bottom = new Point(center.X, center.Y - radius);
        this.Left = new Point(center.X - radius, center.Y);
    }

    public Point Center { get; private set; }

    public double Radius { get; set; }

    public Point Top { get; private set; }

    public Point Right { get; private set; }

    public Point Bottom { get; private set; }

    public Point Left { get; private set; }

    public static Circle Parse(string circleString)
    {
        double[] circleCoords = circleString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
        return new Circle(new Point(circleCoords[0], circleCoords[1]), circleCoords[2]);
    }

    public bool IsInsideCircle(Point point)
    {
        return Math.Pow(point.X - this.Center.X, 2) + Math.Pow(point.Y - this.Center.Y, 2) - Math.Pow(this.Radius, 2) <= Epsilon;
    }
}
