using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

public class FastAndFurious
{
    private static Dictionary<string, List<string>> cameras = 
        new Dictionary<string, List<string>>();
    private static Dictionary<string, Road> roads = 
        new Dictionary<string, Road>();
    private static Dictionary<string, SortedSet<CarInfo>> cars = 
        new Dictionary<string, SortedSet<CarInfo>>();
    private static Dictionary<string, TimeSpan> shortestTimes = 
        new Dictionary<string, TimeSpan>();
    private static SortedSet<string> speedingCars = 
        new SortedSet<string>();

    public static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        ReadInput();
        foreach (var car in cars)
        {
            var cameras = car.Value.ToList();
            for (int firstCamera = 0; firstCamera < cameras.Count; firstCamera++)
            {
                for (int secondCamera = firstCamera + 1; secondCamera < cameras.Count; secondCamera++)
                {
                    var time = cameras[secondCamera].RecordingTime - cameras[firstCamera].RecordingTime;
                    var shortestTime = GetShortestTimeBetweenCameras(cameras[secondCamera].CameraName, cameras[firstCamera].CameraName);
                    if (shortestTime < TimeSpan.MaxValue && !speedingCars.Contains(car.Key) && time < shortestTime)
                    {
                        speedingCars.Add(car.Key);
                    }
                }
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, speedingCars));
    }

    private static void ReadInput()
    {
        Console.ReadLine(); // Skip "Roads:"
        string line;
        while ((line = Console.ReadLine()) != "Records:")
        {
            string[] lineParts = line.Split(' ');
            if (!cameras.ContainsKey(lineParts[0]))
            {
                cameras.Add(lineParts[0], new List<string>());
            }

            if (!cameras.ContainsKey(lineParts[1]))
            {
                cameras.Add(lineParts[1], new List<string>());
            }

            cameras[lineParts[0]].Add(lineParts[1]);
            cameras[lineParts[1]].Add(lineParts[0]);

            var road = new Road(lineParts[0], lineParts[1], double.Parse(lineParts[2]), double.Parse(lineParts[3]));
            roads.Add(road.FirstCameraName + "-" + road.SecondCameraName, road);
            roads.Add(road.SecondCameraName + "-" + road.FirstCameraName, road);
        }

        while ((line = Console.ReadLine()) != "End")
        {
            string[] lineParts = line.Split(' ');
            if (!cars.ContainsKey(lineParts[1]))
            {
                cars.Add(lineParts[1], new SortedSet<CarInfo>());
            }

            var carInfo = new CarInfo(lineParts[0], DateTime.Parse(lineParts[2]));
            cars[lineParts[1]].Add(carInfo);
        }
    }

    private static TimeSpan GetShortestTimeBetweenCameras(string firstCamera, string secondCamera)
    {
        string key = firstCamera + "-" + secondCamera;
        if (shortestTimes.ContainsKey(key))
        {
            return shortestTimes[key];
        }

        // Get the shortest distance using Dijkstra's algorithm
        var start = cameras[firstCamera];
        var times = new Dictionary<string, TimeSpan>();
        var otherCameras = new List<string>();
        foreach (var camera in cameras)
        {
            if (camera.Key == firstCamera)
            {
                times[camera.Key] = TimeSpan.Zero;
            }
            else
            {
                times[camera.Key] = TimeSpan.MaxValue;
            }

            otherCameras.Add(camera.Key);
        }

        while (otherCameras.Count != 0)
        {
            string closestCamera = otherCameras[0];
            TimeSpan closestCameraDistance = times[otherCameras[0]];
            foreach (var camera in otherCameras)
            {
                if (times[camera] < closestCameraDistance)
                {
                    closestCamera = camera;
                    closestCameraDistance = times[camera];
                }
            }

            otherCameras.Remove(closestCamera);

            if (times[closestCamera] == TimeSpan.MaxValue)
            {
                break;
            }

            foreach (var neighbourCamera in cameras[closestCamera])
            {
                var alternativeTime = times[closestCamera] + roads[neighbourCamera + "-" + closestCamera].MinimumTime;
                if (alternativeTime < times[neighbourCamera])
                {
                    times[neighbourCamera] = alternativeTime;
                }
            }
        }

        // Update all calculated times
        foreach (var time in times)
        {
            string shortestTimeKey = firstCamera + "-" + time.Key;
            if (!shortestTimes.ContainsKey(shortestTimeKey) || time.Value < shortestTimes[shortestTimeKey])
            {
                shortestTimes[firstCamera + "-" + time.Key] = time.Value;
                shortestTimes[time.Key + "-" + firstCamera] = time.Value;
            }
        }

        return times[secondCamera];
    }
}

class Road
{
    public Road(string firstCameraName, string secondCameraName, double distance, double speedLimit)
    {
        this.FirstCameraName = firstCameraName;
        this.SecondCameraName = secondCameraName;
        this.Distance = distance;
        this.SpeedLimit = speedLimit;
        this.MinimumTime = TimeSpan.FromHours(distance / speedLimit);
    }

    public string FirstCameraName { get; private set; }

    public string SecondCameraName { get; private set; }

    public double Distance { get; private set; }

    public double SpeedLimit { get; private set; }

    public TimeSpan MinimumTime { get; private set; }
}

class CarInfo : IComparable<CarInfo>
{
    public CarInfo(string cameraName, DateTime recordingTime)
    {
        this.CameraName = cameraName;
        this.RecordingTime = recordingTime;
    }

    public string CameraName { get; private set; }

    public DateTime RecordingTime { get; private set; }

    public int CompareTo(CarInfo other)
    {
        return this.RecordingTime.CompareTo(other.RecordingTime);
    }
}
