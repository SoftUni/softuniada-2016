using System;
using System.Linq;

public class Rectangle
{
    public Rectangle(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
    {
        this.TopLeft = topLeft;
        this.TopRight = topRight;
        this.BottomLeft = bottomLeft;
        this.BottomRight = bottomRight;
    }

    public Point TopLeft { get; private set; }

    public Point TopRight { get; private set; }

    public Point BottomLeft { get; private set; }

    public Point BottomRight { get; private set; }

    public static Rectangle Parse(string rectangleString)
    {
        double[] rectangleCoords = rectangleString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
        return new Rectangle(
            new Point(rectangleCoords[0], rectangleCoords[1]),
            new Point(rectangleCoords[2], rectangleCoords[1]),
            new Point(rectangleCoords[0], rectangleCoords[3]),
            new Point(rectangleCoords[2], rectangleCoords[3]));
    }

    public bool IsInsideRectangle(Point point)
    {
        return this.TopLeft.X <= point.X && point.X <= this.TopRight.X &&
            this.BottomRight.Y <= point.Y && point.Y <= TopRight.Y;
    }
}