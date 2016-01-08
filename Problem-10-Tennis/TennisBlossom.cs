using System;
using System.Collections.Generic;

// Implements the "Blossom" algorithm - https://en.wikipedia.org/wiki/Blossom_algorithm

public class TennisBlossom
{
	public static void Main()
	{
		List<string> people = new List<string>();
		Console.ReadLine();
		string line = Console.ReadLine();
		while (line != "Connections:")
		{
			people.Add(line);
			line = Console.ReadLine();
		}

		Dictionary<string, int> mask = new Dictionary<string, int>();

		for (int i = 0; i < people.Count; i++)
		{
			mask.Add(people[i], i);
		}

		List<int>[] graph = new List<int>[people.Count];
		for (int i = 0; i < people.Count; i++)
		{
			graph[i] = new List<int>();
		}

		line = Console.ReadLine();
		while (line != "END")
		{
			string[] pair = line.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
			int parent = mask[pair[0]];
			int child = mask[pair[1]];
			graph[parent].Add(child);
			graph[child].Add(parent);
			line = Console.ReadLine();
		}

		Console.WriteLine(MaxMatching(graph));
	}

	private static int Lca(int[] match, int[] index, int[] path, int vertex, int child)
	{
		bool[] used = new bool[match.Length];
		while (true)
		{
			vertex = index[vertex];
			used[vertex] = true;
			if (match[vertex] == -1)
			{
				break;
			}

			vertex = path[match[vertex]];
		}

		while (true)
		{
			child = index[child];
			if (used[child])
			{
				return child;
			}

			child = path[match[child]];
		}
	}

	private static void MarkPath(int[] match, int[] index, bool[] blossom, int[] path, int vertex, int curbase, int children)
	{
		for (; index[vertex] != curbase; vertex = path[match[vertex]])
		{
			blossom[index[vertex]] = blossom[index[match[vertex]]] = true;
			path[vertex] = children;
			children = match[vertex];
		}
	}

	private static int FindPath(List<int>[] graph, int[] match, int[] path, int root)
	{
		int n = graph.Length;
		bool[] used = new bool[n];

		// reset path array
		for (int i = 0; i < n; i++)
		{
			path[i] = -1;
		}

		int[] index = new int[n];
		for (int i = 0; i < n; ++i)
		{
			index[i] = i;
		}

		used[root] = true;
		int qh = 0;
		int qt = 0;
		int[] q = new int[n];
		q[qt++] = root;
		while (qh < qt)
		{
			int vertex = q[qh++];

			for (int j = 0; j < graph[vertex].Count; j++)
			{
				int child = graph[vertex][j];
				if (index[vertex] == index[child] || match[vertex] == child)
				{
					continue;
				}

				if ((child == root || match[child] != -1) && path[match[child]] != -1)
				{
					int curbase = Lca(match, index, path, vertex, child);
					bool[] blossom = new bool[n];
					MarkPath(match, index, blossom, path, vertex, curbase, child);
					MarkPath(match, index, blossom, path, child, curbase, vertex);
					for (int i = 0; i < n; ++i)
					{
						if (blossom[index[i]])
						{
							index[i] = curbase;
							if (!used[i])
							{
								used[i] = true;
								q[qt++] = i;
							}
						}
					}
				}
				else if (path[child] == -1)
				{
					path[child] = vertex;
					if (match[child] == -1)
					{
						return child;
					}

					child = match[child];
					used[child] = true;
					q[qt++] = child;
				}
			}
		}

		return -1;
	}

	private static int MaxMatching(List<int>[] graph)
	{
		int n = graph.Length;
		int[] match = new int[n];

		// Initialize the match array
		for (int i = 0; i < n; i++)
		{
			match[i] = -1;
		}

		int[] path = new int[n];
		for (int i = 0; i < n; ++i)
		{
			if (match[i] == -1)
			{
				int vertex = FindPath(graph, match, path, i);
				while (vertex != -1)
				{
					int prevVertex = path[vertex];
					int prevVertexPath = match[prevVertex];
					match[vertex] = prevVertex;
					match[prevVertex] = vertex;
					vertex = prevVertexPath;
				}
			}
		}

		int matches = 0;
		for (int i = 0; i < n; ++i)
		{
			if (match[i] != -1)
			{
				++matches;
			}
		}

		return matches / 2;
	}
}
