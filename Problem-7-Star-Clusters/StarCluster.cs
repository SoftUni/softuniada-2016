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
