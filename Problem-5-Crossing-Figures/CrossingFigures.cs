using System;
using System.Threading;
using System.Globalization;
using System.Linq;

public class CrossingFigures
{
    private static Rectangle rectangle;
    private static Circle circle;

    public static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        int testCases = int.Parse(Console.ReadLine());
        for (int testCase = 0; testCase < testCases; testCase++)
        {
            ReadFigures();
            bool topLeftInsideCircle = circle.IsInsideCircle(rectangle.TopLeft);
            bool topRightInsideCircle = circle.IsInsideCircle(rectangle.TopRight);
            bool bottomLeftInsideCircle = circle.IsInsideCircle(rectangle.BottomLeft);
            bool bottomRightInsideCircle = circle.IsInsideCircle(rectangle.BottomRight);

            bool topInsideRectangle = rectangle.IsInsideRectangle(circle.Top);
            bool rightInsideRectangle = rectangle.IsInsideRectangle(circle.Right);
            bool bottomInsideRectangle = rectangle.IsInsideRectangle(circle.Bottom);
            bool leftInsideRectangle = rectangle.IsInsideRectangle(circle.Left);
            if (topLeftInsideCircle && topRightInsideCircle && bottomLeftInsideCircle && bottomRightInsideCircle)
            {
                Console.WriteLine("Rectangle inside circle");
            }
            else if (topLeftInsideCircle || topRightInsideCircle || bottomLeftInsideCircle || bottomRightInsideCircle)
            {
                Console.WriteLine("Rectangle and circle cross");
            }
            else if (topInsideRectangle && rightInsideRectangle && bottomInsideRectangle && leftInsideRectangle)
            {
                Console.WriteLine("Circle inside rectangle");
            }
            else if (topInsideRectangle || rightInsideRectangle || bottomInsideRectangle || leftInsideRectangle)
            {
                Console.WriteLine("Rectangle and circle cross");
            }
            else
            {
                Console.WriteLine("Rectangle and circle do not cross");
            }
        }
    }

    private static void ReadFigures()
    {
        for (int i = 0; i < 2; i++)
        {
            string[] figureParts = Console.ReadLine().Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            switch (figureParts[0])
            {
                case "rectangle":
                    rectangle = Rectangle.Parse(figureParts[1]);
                    break;
                case "circle":
                    circle = Circle.Parse(figureParts[1]);
                    break;
            }
        }
    }
}

public class Point
{
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    public double X { get; private set; }

    public double Y { get; private set; }
}

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
