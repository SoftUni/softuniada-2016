namespace _07StarClusters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }

    public class StarClusters
    {
        public static void Main(string[] args)
        {
            int clusters = int.Parse(Console.ReadLine());
            Dictionary<string, Point> clusterCenters = new Dictionary<string, Point>();
            SortedDictionary<string, List<Point>> stars = new SortedDictionary<string, List<Point>>();
            for (int i = 0; i < clusters; i++)
            {
                string[] parameters = Console.ReadLine()
                    .Split(new char[] { ' ', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

                string name = parameters[0];
                int x = int.Parse(parameters[1]);
                int y = int.Parse(parameters[2]);
                Point point = new Point(x, y);
                clusterCenters.Add(name, point);
                stars.Add(name, new List<Point>());
                stars[name].Add(point);
            }

            string line = Console.ReadLine();
            while (line != "end")
            {
                string[] parameters = line.Split(new char[] { ' ', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < parameters.Length; i = i + 2)
                {
                    int x = int.Parse(parameters[i]);
                    int y = int.Parse(parameters[i + 1]);
                    Point point = new Point(x, y);

                    int minDistance = int.MaxValue;
                    string cluster = string.Empty;
  
                    foreach (var clusterCenter in clusterCenters)
                    {
                        int distanceX = clusterCenter.Value.X - x;
                        int distanceY = clusterCenter.Value.Y - y;
                        int distance = (distanceX * distanceX) + (distanceY * distanceY);

                        if (distance < minDistance)
                        {
                            cluster = clusterCenter.Key;
                            minDistance = distance;

                        }
                    }


                    stars[cluster].Add(point);

                }

                line = Console.ReadLine();
            }

            foreach (var cluster in stars)
            {
                int centerX = (int)Math.Round(cluster.Value.Average(star => star.X));
                int centerY = (int)Math.Round(cluster.Value.Average(star => star.Y));
                Console.WriteLine("{0} ({1}, {2}) -> {3} stars", cluster.Key, centerX, centerY, cluster.Value.Count);
            }
        }
    }
}
