using System;
using System.Linq;

public class Rectangle : Figure
{
    public Rectangle(string name, Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        : base(name, topLeft, topRight, bottomLeft, bottomRight)
    {
    }

    public static Rectangle Parse(string rectangleString)
    {
        string[] rectangleParts = rectangleString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        double[] rectangleCoords = rectangleParts.Skip(1).Select(double.Parse).ToArray();
        return new Rectangle(
            rectangleParts[0],
            new Point(rectangleCoords[0], rectangleCoords[1]),
            new Point(rectangleCoords[2], rectangleCoords[1]),
            new Point(rectangleCoords[0], rectangleCoords[3]),
            new Point(rectangleCoords[2], rectangleCoords[3]));
    }

    public override bool IsInside(Point point)
    {
        return this.PointA.X <= point.X && point.X <= this.PointB.X &&
            this.PointC.Y <= point.Y && point.Y <= PointB.Y;
    }
}