using System;

public class CarInfo : IComparable<CarInfo>
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
