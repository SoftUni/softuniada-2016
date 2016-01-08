using System;

public class Road
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