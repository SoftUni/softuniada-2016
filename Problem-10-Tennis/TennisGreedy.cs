using System;
using System.Collections.Generic;
using System.Linq;

public class TennisGreedy
{
	static void Main()
	{
		string line = Console.ReadLine();
		while (line != "Connections:")
		{
			line = Console.ReadLine();
		}
		Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();

		line = Console.ReadLine();
		
		// List with the graph edges for testing
		List<string> graphTest = new List<string>();
		while(line != "END")
		{
			string[] pair = line.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
			string parent = pair[0];
			string child = pair[1];
			if (!graph.ContainsKey(parent))
			{
				graph.Add(parent, new List<string>());
			}
			if (!graph.ContainsKey(child))
			{
				graph.Add(child, new List<string>());
			}
			graph[parent].Add(child);
			graph[child].Add(parent);
			line = Console.ReadLine();
		}

		int pairs = 0;
		HashSet<string> paired = new HashSet<string>();
		 
		// Greedy: connect the vertices with lowest children count
		foreach (var entry in graph.OrderBy(x => x.Value.Count))
		{
			foreach (var child in entry.Value.OrderBy(x=>graph[x].Count))
			{
				if (!paired.Contains(entry.Key) && !paired.Contains(child))
				{
					pairs++;
					paired.Add(entry.Key);
					paired.Add(child);
					break;
				}            
			}
		}
		Console.WriteLine(pairs);
	}
}
