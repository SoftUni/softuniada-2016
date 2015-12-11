using System;
using System.Linq;

class Radars
{
    //const double resolution = 0.000000005;
    const double resolution = 0.000001;

    class Circle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }
        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }
        public double RadiusSquare { get; private set; }

        public Circle(double x, double y, double radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
            this.MinX = x - radius;
            this.MaxX = x + radius;
            this.MinY = y - radius;
            this.MaxY = y + radius;
            this.RadiusSquare = radius * radius;
        }
    }

    enum RectanglePosition
    {
        RectangleInside,
        RectangleOutside,
        RectangleIntersect
    }

    static Circle[] circles = new Circle[]
    {
        new Circle(0, 0, 1),
        new Circle(3, 1, 4),
        new Circle(4.5, -0.5, 0.5),
        new Circle(4, -3, 1),
        new Circle(-3, -1, 2),
        new Circle(-3, 4, 1.5),
        new Circle(-4.5, 0.5, 1.5),
        new Circle(-5.5, -0.5, 1),
    };

    static void Main()
    {
        var startTime = DateTime.Now;
        var areaScanPoints = CalcCirclesUnionAreaByScanPoints();
        var timeConsumed = DateTime.Now - startTime;
        Console.WriteLine("Scan points approximation: {0} (time = {1} ms)",
            areaScanPoints, timeConsumed.TotalMilliseconds);

        var startTime2 = DateTime.Now;
        var areaRectangleSplit = CalcCirclesUnionArea();
        var timeConsumed2 = DateTime.Now - startTime2;
        Console.WriteLine("Rectangle-split approximation: {0} (time = {1} ms)",
            areaRectangleSplit, timeConsumed2.TotalMilliseconds);
    }

    static double CalcCirclesUnionArea()
    {
        var minX = -100000;
        var maxX =  100000;
        var minY = -100000;
        var maxY =  100000;

        return CalcCirclesUnionArea(minX, maxX, minY, maxY);
    }

    static double CalcCirclesUnionArea(
        double minX, double maxX, double minY, double maxY)
    {
        var position = CalcRectanglePosition(minX, maxX, minY, maxY);

        if (position == RectanglePosition.RectangleOutside)
        {
            //Console.WriteLine("x:[{0},{1}], y:[{2},{3}] out", minX, maxX, minY, maxY);
            return 0;
        }

        var rectangleArea = (maxX - minX) * (maxY - minY);
        if (position == RectanglePosition.RectangleInside)
        {
            //Console.WriteLine("x:[{0},{1}], y:[{2},{3}] in", minX, maxX, minY, maxY);
            return rectangleArea;
        }

        if (rectangleArea < resolution)
        {
            // Rectangle is too small for further splitting
            return rectangleArea / 2;
        }

        // Split the rectangle into 4 sub-rectangles
        // and calculate their areas recursively
        double midX = (minX + maxX) / 2;
        double midY = (minY + maxY) / 2;
        double area =
            CalcCirclesUnionArea(minX, midX, minY, midY) + // up-left sub-rect
            CalcCirclesUnionArea(midX, maxX, minY, midY) + // up-right sub-rect
            CalcCirclesUnionArea(minX, midX, midY, maxY) + // down-left sub-rect
            CalcCirclesUnionArea(midX, maxX, midY, maxY);  // down-right sub-rect
        return area;
    }

    static RectanglePosition CalcRectanglePosition(
        double minX, double maxX, double minY, double maxY)
    {
        var result = RectanglePosition.RectangleOutside;
        foreach (var c in circles)
        {
            var position = CalcRectanglePosition(minX, maxX, minY, maxY, c);
            if (position == RectanglePosition.RectangleInside)
            {
                return RectanglePosition.RectangleInside;
            }
            if (position == RectanglePosition.RectangleIntersect)
            {
                result = RectanglePosition.RectangleIntersect;
            }
        }

        return result;
    }

    static RectanglePosition CalcRectanglePosition(
        double rectMinX, double rectMaxX, double rectMinY, double rectMaxY, Circle circle)
    {
        bool upLeftInsideCircle = IsPointInsideCircle(rectMinX, rectMinY, circle);
        bool upRightInsideCircle = IsPointInsideCircle(rectMinX, rectMaxY, circle);
        bool downLeftInsideCircle = IsPointInsideCircle(rectMaxX, rectMinY, circle);
        bool downRightInsideCircle = IsPointInsideCircle(rectMaxX, rectMaxY, circle);

        if (upLeftInsideCircle && upRightInsideCircle && downLeftInsideCircle && downRightInsideCircle)
        {
            return RectanglePosition.RectangleInside;
        }

        // Find the closest point to the circle within the rectangle
        double closestX = ClampValue(circle.X, rectMinX, rectMaxX);
        double closestY = ClampValue(circle.Y, rectMinY, rectMaxY);

        if (IsPointInsideCircle(closestX, closestY, circle))
        {
            return RectanglePosition.RectangleIntersect;
        }

        return RectanglePosition.RectangleOutside;
    }

    static bool IsPointInsideCircle(double x, double y, Circle circle)
    {
        var deltaX = (x - circle.X);
        var deltaY = (y - circle.Y);
        bool inside = (deltaX * deltaX + deltaY * deltaY) <= circle.RadiusSquare;
        return inside;
    }

    static double ClampValue(double value, double min, double max)
    {
        if (value < min)
        {
            value = min;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }

    static double CalcCirclesUnionAreaByScanPoints()
    {
        var minX = circles.Min(c => c.X - c.Radius);
        var minY = circles.Min(c => c.Y - c.Radius);
        var maxX = circles.Max(c => c.X + c.Radius);
        var maxY = circles.Max(c => c.Y + c.Radius);
        double totalAreaToScan = (maxX - minX) * (maxY - minY);
        double scanResolution = Math.Sqrt(totalAreaToScan / 5000000.0);
        double scanArea = scanResolution * scanResolution;

        double area = 0;
        for (double x = minX; x <= maxX; x += scanResolution)
        {
            for (double y = minY; y <= maxY; y += scanResolution)
            {
                if (IsPointInsideSomeCircle(x, y))
                {
                    area += scanArea;
                }
            }
        }

        return area;
    }

    static bool IsPointInsideSomeCircle(double x, double y)
    {
        foreach (var circle in circles)
        {
            if (IsPointInsideCircle(x, y, circle))
            {
                return true;
            }
        }

        return false;
    }
}
