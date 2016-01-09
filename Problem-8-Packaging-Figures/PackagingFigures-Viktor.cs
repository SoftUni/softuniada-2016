using System;
using System.Collections.Generic;
using System.Linq;

namespace _08PackagingFigures
{
    public class PackagingFigures
    {
        public abstract class Figure
        {
            protected Figure(string name)
            {
                this.Name = name;
                this.Children = new List<Figure>();
            }

            public string Name { get; set; }
            public List<Figure> Children { get; private set; }
            public int Depth { get; set; }
            public Figure Successor { get; set; }

        }
        public class Rectangle : Figure
        {
            public Rectangle(string name, int x1, int y1, int x2, int y2)
               : base(name)
            {
                this.X1 = x1;
                this.Y1 = y1;
                this.X2 = x2;
                this.Y2 = y2;
            }

            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
        }

        public class Circle : Figure
        {
            public Circle(string name, int centerX, int centerY, int radius)
               : base(name)
            {
                this.CenterX = centerX;
                this.CenterY = centerY;
                this.Radius = radius;
            }

            public int CenterX { get; set; }
            public int CenterY { get; set; }
            public int Radius { get; set; }
        }
        public static void Main()
        {
            List<Figure> figures = new List<Figure>();
            string line = Console.ReadLine();
            while (line != "End")
            {
                string[] parameters = line.Split();
                string type = parameters[0];
                string name = parameters[1];

                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                Figure figure = null;

                switch (type)
                {
                    case "rectangle":
                        x1 = int.Parse(parameters[2]);
                        y1 = int.Parse(parameters[3]);
                        x2 = int.Parse(parameters[4]);
                        y2 = int.Parse(parameters[5]);
                        figure = new Rectangle(name, x1, y1, x2, y2);
                        break;
                    case "square":
                        x1 = int.Parse(parameters[2]);
                        y1 = int.Parse(parameters[3]);
                        int side = int.Parse(parameters[4]);
                        x2 = x1 + side;
                        y2 = y1 - side;
                        figure = new Rectangle(name, x1, y1, x2, y2);
                        break;
                    case "circle":
                        int centerX = int.Parse(parameters[2]);
                        int centerY = int.Parse(parameters[3]);
                        int radius = int.Parse(parameters[4]);
                        figure = new Circle(name, centerX, centerY, radius);
                        break;
                }
                figures.Add(figure);
                line = Console.ReadLine();
            }
            for (int i = 0; i < figures.Count; i++)
            {
                Figure firstFigure = figures[i];
                for (int j = 0; j < figures.Count; j++)
                {
                    if (i != j)
                    {
                        if (firstFigure is Circle)
                        {
                            Circle first = (Circle)firstFigure;
                            Figure secondFigure = figures[j];
                            if (secondFigure is Circle)
                            {
                                Circle second = (Circle)secondFigure;
                                if (second.Radius > first.Radius)
                                {
                                    continue;
                                }
                                int distanceX = Math.Abs(first.CenterX - second.CenterX);
                                int distanceY = Math.Abs(first.CenterY - second.CenterY);
                                if (distanceX > first.Radius || distanceY > first.Radius)
                                {
                                    continue;
                                }
                                long distanceSquare = (long)distanceY*distanceY + (long)distanceX*distanceX;
                                long radiusSquared = (long)(first.Radius - second.Radius)*(first.Radius - second.Radius);
                                if (distanceSquare <= radiusSquared)
                                {
                                    firstFigure.Children.Add(secondFigure);
                                }
                            }
                            else
                            {
                                Rectangle second = (Rectangle)secondFigure;
                                int distanceX = Math.Max(first.CenterX - second.X1, second.X2 - first.CenterX);
                                int distanceY = Math.Max(second.Y1 - first.CenterY, first.CenterY - second.Y2);
                                bool isInside = ((long)first.Radius * first.Radius >= ((long)distanceX * distanceX + (long)distanceY * distanceY));
                                if (isInside)
                                {
                                    firstFigure.Children.Add(secondFigure);
                                }
                            }
                        }
                        else
                        {
                            Rectangle first = (Rectangle)firstFigure;
                            Figure secondFigure = figures[j];
                            if (secondFigure is Circle)
                            {
                                Circle second = (Circle)secondFigure;
                                bool inLeft = (second.CenterX - first.X1) >= second.Radius;
                                bool inRight = (first.X2 - second.CenterX) >= second.Radius;
                                bool inTop = (first.Y1 - second.CenterY) >= second.Radius;
                                bool inBottom = (second.CenterY - first.Y2) >= second.Radius;
                                if (inLeft && inRight && inTop && inBottom)
                                {
                                    firstFigure.Children.Add(secondFigure);
                                }              
                            }
                            else
                            {
                                Rectangle second = (Rectangle)secondFigure;
                                if (second.X2 > first.X2 || second.Y1 > first.Y1 || second.Y2 < first.Y2 || second.X1 < first.X1)
                                {
                                    continue;
                                }
                                firstFigure.Children.Add(secondFigure);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < figures.Count; i++)
            {
                Dfs(figures[i]);
            }
            Figure top = figures.OrderByDescending(x => x.Depth).ThenBy(x => x.Name).First();
            int depth = top.Depth;
            while (true)
            {
                Console.Write(top.Name);
                depth -= 1;
                top = top.Successor;
                if (top == null || depth == 0)
                {
                    Console.WriteLine();
                    break;
                }
                Console.Write(" < ");
            }
        }

        private static int Dfs(Figure element)
        {
            if (element.Depth > 0)
            {
                return element.Depth;
            }

            element.Depth = 1;
            element.Successor = null;
            foreach (var child in element.Children)
            {
                int currentDepth = Dfs(child) + 1;
                if (currentDepth > element.Depth || (currentDepth == element.Depth && child.Name.CompareTo(element.Successor.Name) < 0))
                {
                    element.Depth = currentDepth;
                    element.Successor = child;
                }

            }
            return element.Depth;
        }
    }
}
