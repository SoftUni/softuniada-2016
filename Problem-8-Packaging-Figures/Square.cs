using System;
using System.Linq;

public class Square : Rectangle
{
    public Square(string name, Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        : base(name, topLeft, topRight, bottomLeft, bottomRight)
    {
    }

    public static new Square Parse(string squareString)
    {
        string[] squareParts = squareString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        double[] squareCoords = squareParts.Skip(1).Select(double.Parse).ToArray();
        return new Square(
            squareParts[0],
            new Point(squareCoords[0], squareCoords[1]),
            new Point(squareCoords[0] + squareCoords[2], squareCoords[1]),
            new Point(squareCoords[0], squareCoords[1] - squareCoords[2]),
            new Point(squareCoords[0] + squareCoords[2], squareCoords[1] - squareCoords[2]));
    }

    public override bool IsInside(Point point)
    {
        return base.IsInside(point);
    }
}