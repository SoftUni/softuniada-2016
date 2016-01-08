namespace TennisBacktrack
{
    public class Edge
    {
        public Edge(int parent, int child)
        {
            this.Parent = parent;
            this.Child = child;
        }

        public int Parent { get; set; }
        public int Child { get; set; }

        public override bool Equals(object obj)
        {
            Edge other = (Edge)obj;
            return this.Parent == other.Parent && this.Child == other.Child;
        }

        public override int GetHashCode()
        {
            return this.Parent + this.Child;
        }
    }

}
