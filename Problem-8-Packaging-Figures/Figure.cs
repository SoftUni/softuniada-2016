using System.Collections.Generic;

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
