namespace _09FastAndFurious
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BinaryHeap<T> where T : IComparable<T>
    {
        private List<T> heap;

        public BinaryHeap()
        {
            this.heap = new List<T>();
        }

        public BinaryHeap(T[] elements)
        {
            this.heap = new List<T>(elements);
            for (int i = this.heap.Count / 2; i >= 0; i--)
            {
                this.HeapifyDown(i);
            }
        }

        public BinaryHeap(List<T> elements)
        {
            this.heap = new List<T>(elements);
            for (int i = this.heap.Count / 2; i >= 0; i--)
            {
                this.HeapifyDown(i);
            }
        }

        public int Count
        {
            get
            {
                return this.heap.Count;
            }
        }

        public T ExtractMin()
        {
            var min = this.heap[0];
            this.heap[0] = this.heap[this.heap.Count - 1];
            this.heap.RemoveAt(this.heap.Count - 1);
            if (this.heap.Count > 0)
            {
                this.HeapifyDown(0);
            }

            return min;
        }

        public T PeekMin()
        {
            return this.heap[0];
        }

        public void Insert(T node)
        {
            this.heap.Add(node);
            this.HeapifyUp(this.heap.Count - 1);
        }

        public void Reorder(T element)
        {
            int index = this.heap.IndexOf(element);
            this.HeapifyUp(index);
        }

        private void HeapifyDown(int i)
        {
            var left = (2 * i) + 1;
            var right = (2 * i) + 2;
            var smallest = i;

            if (left < this.heap.Count && this.heap[left].CompareTo(this.heap[smallest]) < 0)
            {
                smallest = left;
            }

            if (right < this.heap.Count && this.heap[right].CompareTo(this.heap[smallest]) < 0)
            {
                smallest = right;
            }

            if (smallest != i)
            {
                T old = this.heap[i];
                this.heap[i] = this.heap[smallest];
                this.heap[smallest] = old;
                this.HeapifyDown(smallest);
            }
        }

        private void HeapifyUp(int i)
        {
            var parent = (i - 1) / 2;
            while (i > 0 && this.heap[i].CompareTo(this.heap[parent]) < 0)
            {
                T old = this.heap[i];
                this.heap[i] = this.heap[parent];
                this.heap[parent] = old;

                i = parent;
                parent = (i - 1) / 2;
            }
        }
    }

    public class Edge
    {
        public Edge(string parent, string child, decimal travelTime)
        {
            this.Parent = parent;
            this.Child = child;
            this.TravelTime = travelTime;
        }

        public decimal TravelTime { get; set; }

        public string Parent { get; set; }

        public string Child { get; set; }
    }

    public class Node : IComparable<Node>
    {
        public Node(string name, decimal travelTime)
        {
            this.Name = name;
            this.Edges = new List<Edge>();
            this.TravelTime = travelTime;
        }

        public string Name { get; set; }

        public List<Edge> Edges { get; private set; }

        public decimal TravelTime { get; set; }

        public int CompareTo(Node other)
        {
            return this.TravelTime.CompareTo(other.TravelTime);
        }
    }

    public class FastAndFurious
    {
        public static void Main()
        {
            Dictionary<string, Node> graph = new Dictionary<string, Node>();
            Console.ReadLine();
            string line = Console.ReadLine();
            while (line != "Records:")
            {
                string[] parameters = line.Split();
                string parent = parameters[0];
                string child = parameters[1];
                decimal distance = decimal.Parse(parameters[2]);
                decimal maxSpeed = decimal.Parse(parameters[3]);
                decimal travelTime = distance / maxSpeed;
                Edge edge = new Edge(parent, child, travelTime);
                Edge reverse = new Edge(child, parent, travelTime);
                if (!graph.ContainsKey(parent))
                {
                    graph.Add(parent, new Node(parent, decimal.MaxValue));
                }

                if (!graph.ContainsKey(child))
                {
                    graph.Add(child, new Node(child, decimal.MaxValue));
                }

                graph[parent].Edges.Add(edge);
                graph[child].Edges.Add(reverse);
                line = Console.ReadLine();
            }

            var cars = new SortedDictionary<string, List<KeyValuePair<DateTime, string>>>();
            line = Console.ReadLine();
            while (line != "End")
            {
                string[] parameters = line.Split();
                string city = parameters[0];
                string licensePlate = parameters[1];
                DateTime time = DateTime.Parse(parameters[2]);
                KeyValuePair<DateTime, string> sighting = new KeyValuePair<DateTime, string>(time, city);
                if (!cars.ContainsKey(licensePlate))
                {
                    cars.Add(licensePlate, new List<KeyValuePair<DateTime, string>>());
                }

                cars[licensePlate].Add(sighting);
                line = Console.ReadLine();
            }

            foreach (var car in cars)
            {
                if (IsSpeeding(car, graph))
                {
                    Console.WriteLine(car.Key);
                }
            }
        }


        private static bool IsSpeeding(KeyValuePair<string, List<KeyValuePair<DateTime,string>>> car, Dictionary<string,Node> graph)
        {
            List<KeyValuePair<DateTime, string>> sightings = car.Value.OrderByDescending(x => x.Key).ToList();
            for (int i = 0; i < sightings.Count; i++)
            {
                for (int j = i + 1; j < sightings.Count; j++)
                {
                    TimeSpan travelTime = sightings[i].Key.Subtract(sightings[j].Key);
                    TimeSpan allowedTime = Dijkstra(sightings[j].Value, sightings[i].Value, graph);
                    TimeSpan difference = travelTime.Subtract(allowedTime);
                    if (difference.TotalSeconds < 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static TimeSpan Dijkstra(string start, string end, Dictionary<string, Node> graph)
        {
            // reset distances
            foreach (var city in graph)
            {
                city.Value.TravelTime = decimal.MaxValue;
            }
            HashSet<string> added = new HashSet<string>();
            HashSet<string> visited = new HashSet<string>();
            graph[start].TravelTime = 0;
            BinaryHeap<Node> priorityQueue = new BinaryHeap<Node>();
            priorityQueue.Insert(graph[start]);
            visited.Add(start);

            while (priorityQueue.Count > 0)
            {
                Node currentNode = priorityQueue.ExtractMin();
                visited.Add(currentNode.Name);

                if (currentNode.Name == end)
                {
                    break;
                }

                foreach (var edge in currentNode.Edges)
                {
                    if (!visited.Contains(edge.Child))
                    {
                        if (!added.Contains(edge.Child))
                        {
                            priorityQueue.Insert(graph[edge.Child]);
                            added.Add(edge.Child);
                        }

                        decimal currentTravelTime = currentNode.TravelTime + edge.TravelTime;
                        if (graph[edge.Child].TravelTime > currentTravelTime)
                        {
                            graph[edge.Child].TravelTime = currentTravelTime;
                            priorityQueue.Reorder(graph[edge.Child]);
                        }
                    }
                }
            }

            if(graph[end].TravelTime == decimal.MaxValue)
            {
                return new TimeSpan(0, 0, 0);
            }

            int hours = (int)graph[end].TravelTime;
            decimal rest = (graph[end].TravelTime - hours) * 60;
            int minutes = (int)rest;
            rest = rest - minutes;
            int seconds = (int)Math.Round(rest * 60);
            TimeSpan timeTraveled = new TimeSpan(hours,minutes,seconds);
            return timeTraveled;
        }
    }
}
