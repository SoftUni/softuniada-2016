using System;
using System.Linq;

public class Circle : Figure
{
    private const double Epsilon = 0.01;

    public Circle(string name, Point center, double radius)
        : base(name, new Point(center.X, center.Y + radius), new Point(center.X + radius, center.Y), new Point(center.X, center.Y - radius), new Point(center.X - radius, center.Y))
    {
        this.Center = center;
        this.Radius = radius;
    }

    public Point Center { get; private set; }

    public double Radius { get; set; }

    public static Circle Parse(string circleString)
    {
        string[] circleParts = circleString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        double[] circleCoords = circleParts.Skip(1).Select(double.Parse).ToArray();
        return new Circle(circleParts[0], new Point(circleCoords[0], circleCoords[1]), circleCoords[2]);
    }

    public override bool IsInside(Point point)
    {
        return Math.Pow(point.X - this.Center.X, 2) + Math.Pow(point.Y - this.Center.Y, 2) - Math.Pow(this.Radius, 2) <= Epsilon;
    }
}
