using System;
using System.Threading;
using System.Globalization;

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
