public class Star : Point
{
    public Star(double x, double y)
        : base(x, y)
    {
    }

    public int Cluster { get; set; }
}