using System;
using System.Collections.Generic;
using System.Linq;

public class TennisBacktracking
{
	private static int MaxMatch = 0;

	static void Main()
	{
		var people = new List<string>();
		Console.ReadLine();
		string line = Console.ReadLine();
		while (line != "Connections:")
		{
			people.Add(line);
			line = Console.ReadLine();
		}
		Dictionary<int, List<Edge>> adjacencyList = new Dictionary<int, List<Edge>>();
		Dictionary<string, int> mask = new Dictionary<string, int>();

		for (int i = 0; i < people.Count; i++)
		{
			mask.Add(people[i], i);
			adjacencyList.Add(i, new List<Edge>());
		}


		List<Edge> edges = new List<Edge>();
		line = Console.ReadLine();
		while (line != "END")
		{
			string[] pair = line.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
			int parent = mask[pair[0]];
			int child = mask[pair[1]];
			Edge edge = new Edge(parent, child);
			adjacencyList[parent].Add(edge);
			adjacencyList[child].Add(edge);
			edges.Add(edge);
			line = Console.ReadLine();
		}
		bool[] used = new bool[edges.Count];
		Traverse(used, edges, adjacencyList, 0);
		Console.WriteLine(MaxMatch);
	}

	private static void Traverse(bool[] used, List<Edge> edges, Dictionary<int, List<Edge>> graph, int count)
	{
		if (used.All(x => x))
		{
			if (MaxMatch < count)
			{
				MaxMatch = count;
			}
		}
		else
		{
			for (int i = 0; i < edges.Count; i++)
			{
				if (!used[i])
				{
					count++;
					List<int> blacklisted = new List<int>();
					used[i] = true;
					blacklisted.Add(i);
					foreach (var edge in graph[edges[i].Parent])
					{
						int index = edges.IndexOf(edge);
						if (!used[index])
						{
							used[index] = true;
							blacklisted.Add(index);
						}
					}
					foreach (var edge in graph[edges[i].Child])
					{
						int index = edges.IndexOf(edge);
						if (!used[index])
						{
							used[index] = true;
							blacklisted.Add(index);
						}
					}

					Traverse(used, edges, graph, count);
					foreach (var removed in blacklisted)
					{
						used[removed] = false;
					}

					count--;
				}
			}
		}
	}
}
	
class Edge
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
