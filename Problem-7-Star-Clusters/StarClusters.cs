using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

public class StarClusters
{
    public static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        int clustersCount = int.Parse(Console.ReadLine());

        // Read clusters
        var clusters = new List<StarCluster>();
        var stars = new List<Star>();
        for (int i = 0; i < clustersCount; i++)
        {
            string[] lineParts = Console.ReadLine().Split(new[] { ' ', '(', ',', ')' },
                StringSplitOptions.RemoveEmptyEntries);
            int[] starCoords = new[] { int.Parse(lineParts[1]), int.Parse(lineParts[2]) };
            clusters.Add(new StarCluster(lineParts[0], starCoords[0], starCoords[1]));
            stars.Add(new Star(starCoords[0], starCoords[1]));
        }

        // Read stars
        string line;
        while ((line = Console.ReadLine()) != "end")
        {
            string[] lineParts = line.Split(new[] { ' ', '(', ',', ')' },
                StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lineParts.Length; i += 2)
            {
                stars.Add(new Star(int.Parse(lineParts[i]), int.Parse(lineParts[i + 1])));
            }
        }

        var centers = FindStarClusterCenters(stars, clusters);
        PrintStarClusterCenters(centers);
    }

    private static List<StarCluster> FindStarClusterCenters(
        List<Star> stars, List<StarCluster> clusters)
    {
        var distances = new double[stars.Count, clusters.Count];
        bool foundFinalClusters = false;
        while (!foundFinalClusters)
        {
            for (int star = 0; star < stars.Count; star++)
            {
                for (int cluster = 0; cluster < clusters.Count; cluster++)
                {
                    distances[star, cluster] = clusters[cluster].ComputeDistanceTo(stars[star]);
                }
            }

            foundFinalClusters = true;
            for (int star = 0; star < stars.Count; star++)
            {
                double minDistance = double.MaxValue;
                int minCluster = 0;
                for (int cluster = 0; cluster < clusters.Count; cluster++)
                {
                    if (distances[star, cluster] < minDistance)
                    {
                        minCluster = cluster;
                        minDistance = distances[star, cluster];
                    }
                }

                if (stars[star].Cluster != minCluster)
                {
                    stars[star].Cluster = minCluster;
                    foundFinalClusters = false;
                }
            }

            for (int cluster = 0; cluster < clusters.Count; cluster++)
            {
                var starsInCluster = stars.Where(s => s.Cluster == cluster).ToList();
                clusters[cluster].X = starsInCluster.Average(s => s.X);
                clusters[cluster].Y = starsInCluster.Average(s => s.Y);
                clusters[cluster].StarsCount = starsInCluster.Count;
            }
        }

        return clusters;
    }

    private static void PrintStarClusterCenters(List<StarCluster> clusters)
    {
        var sortedClusters = clusters
            .OrderBy(c => c.Name);
        foreach (var cluster in sortedClusters)
        {
            Console.WriteLine("{0} ({1}, {2}) -> {3} stars", 
                cluster.Name, Math.Round(cluster.X), Math.Round(cluster.Y), cluster.StarsCount);
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

    public double X { get; set; }

    public double Y { get; set; }

    public double ComputeDistanceTo(Point other)
    {
        // The square of the Eucledian distance can also be used, there is no need
        // to take the square root of this expression (and it is faster)
        double deltaX = other.X - this.X;
        double deltaY = other.Y - this.Y;
        return deltaX * deltaX + deltaY * deltaY;
    }
}

public class Star : Point
{
    public Star(double x, double y)
        : base(x, y)
    {
    }

    public int Cluster { get; set; }
}

public class StarCluster : Point
{
    public StarCluster(string name, double x, double y)
        : base(x, y)
    {
        this.Name = name;
    }

    public string Name { get; set; }

    public int StarsCount { get; set; }
}
