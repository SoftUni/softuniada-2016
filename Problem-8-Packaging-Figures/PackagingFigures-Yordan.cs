using System;
using System.Collections.Generic;
using System.Linq;

public class PackagingFigures
{
    public static void Main()
    {
        var figures = ReadFigures();
        BuildGraph(figures);
        var longestSequence = BuildLongestSequence(figures);
        Console.WriteLine(string.Join(" < ", longestSequence));
    }

    private static List<Figure> ReadFigures()
    {
        var figures = new List<Figure>();
        string line;
        while ((line = Console.ReadLine()) != "End")
        {
            string[] lineParts = line.Split(new[] { ' ' }, 2);
            Figure figure = null;
            switch (lineParts[0])
            {
                case "rectangle":
                    figure = Rectangle.Parse(lineParts[1]);
                    break;
                case "square":
                    figure = Square.Parse(lineParts[1]);
                    break;
                case "circle":
                    figure = Circle.Parse(lineParts[1]);
                    break;
            }

            figures.Add(figure);
        }

        return figures;
    }

    private static void BuildGraph(List<Figure> figures)
    {
        for (int u = 0; u < figures.Count; u++)
        {
            for (int v = 0; v < figures.Count; v++)
            {
                if (u != v && IsFigureInsideOther(figures[v], figures[u]))
                {
                    figures[v].ParentFigures.Add(u);
                    figures[u].ChildFiguresCount++;
                }
            }
        }
    }

    private static bool IsFigureInsideOther(Figure inner, Figure outer)
    {
        if (inner is Circle && outer is Circle)
        {
            var innerCircle = inner as Circle;
            var outerCircle = outer as Circle;
            double deltaX = outerCircle.Center.X - innerCircle.Center.X;
            double deltaY = outerCircle.Center.Y - innerCircle.Center.Y;
            double distanceBetweenCentersSquared = deltaX * deltaX + deltaY * deltaY;
            if (innerCircle.Radius > outerCircle.Radius || distanceBetweenCentersSquared > outerCircle.Radius * outerCircle.Radius)
            {
                return false;
            }

            double deltaR = outerCircle.Radius - innerCircle.Radius;
            return distanceBetweenCentersSquared <= deltaR * deltaR;
        }

        return outer.IsInside(inner.PointA) &&
            outer.IsInside(inner.PointB) &&
            outer.IsInside(inner.PointC) &&
            outer.IsInside(inner.PointD);
    }

    private static List<string> BuildLongestSequence(List<Figure> figures)
    {
        //Initialize childrenCount[] and maxSeqLen[] for each rectangle
        const int NotCalculated = -1;
        const int NoNextFigure = -1;
        int[] maxLengths = new int[figures.Count];
        int[] childrenCounts = new int[figures.Count];
        int[] nextFigures = new int[figures.Count];
        for (int i = 0; i < maxLengths.Length; i++)
        {
            childrenCounts[i] = figures[i].ChildFiguresCount;
            maxLengths[i] = (childrenCounts[i] == 0) ? 1 : NotCalculated;
            nextFigures[i] = NoNextFigure;
        }

        // Calculate the maximal sequence for all nodes, using a
        // combination of topological sorting + Dijkstra's algorithm
        bool[] used = new bool[figures.Count];
        while (true)
        {
            var figuresWithNoChildren = Enumerable.Range(0, figures.Count)
                .Where(v => !used[v] && childrenCounts[v] == 0);
            if (!figuresWithNoChildren.Any())
            {
                // No figure without children -> algorithm finished
                break;
            }

            var currentFigure = figuresWithNoChildren.First();
            used[currentFigure] = true;

            // Improve maxSeqLen[] and remove the currentNode from the graph
            foreach (var parent in figures[currentFigure].ParentFigures)
            {
                childrenCounts[parent]--;
                if ((maxLengths[currentFigure] + 1 > maxLengths[parent]) ||
                    (maxLengths[currentFigure] + 1 == maxLengths[parent] &&
                     figures[currentFigure].Name.CompareTo(figures[nextFigures[parent]].Name) < 0))
                {
                    // A better path to parent is found -> improve maxSeqLen[]
                    maxLengths[parent] = maxLengths[currentFigure] + 1;
                    nextFigures[parent] = currentFigure;
                }
            }
        }

        // Find the starting figure that has the longest sequence
        int startFigure = 0;
        for (int i = 0; i < maxLengths.Length; i++)
        {
            if ((maxLengths[i] > maxLengths[startFigure]) ||
                (maxLengths[i] == maxLengths[startFigure] &&
                figures[i].Name.CompareTo(figures[startFigure].Name) < 0))
            {
                startFigure = i;
            }
        }

        // Reconstruct the longest sequence of nested figures
        List<string> sequence = new List<string>();
        while (startFigure != NoNextFigure)
        {
            sequence.Add(figures[startFigure].Name);
            startFigure = nextFigures[startFigure];
        }

        return sequence;
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

public abstract class Figure
{
    public Figure(string name, Point pointA, Point pointB, Point pointC, Point pointD)
    {
        this.Name = name;
        this.PointA = pointA;
        this.PointB = pointB;
        this.PointC = pointC;
        this.PointD = pointD;
        this.ParentFigures = new List<int>();
    }

    public string Name { get; private set; }

    public Point PointA { get; private set; }

    public Point PointB { get; private set; }

    public Point PointC { get; private set; }

    public Point PointD { get; private set; }

    public List<int> ParentFigures { get; set; }

    public int ChildFiguresCount { get; set; }

    public abstract bool IsInside(Point point);
}

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

public class Circle : Figure
{
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
        return (point.X - this.Center.X) * (point.X - this.Center.X) + (point.Y - this.Center.Y) * (point.Y - this.Center.Y) <= this.Radius * this.Radius;
    }
}
