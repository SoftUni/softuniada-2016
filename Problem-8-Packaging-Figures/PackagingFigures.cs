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