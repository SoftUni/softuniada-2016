using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TennisTestGenerator
{
	private static int NameSuffix = 1;

	private const string Name = "Pesho";

	private static Random random = new Random();

	static void Main()
	{
		int blossoms = 5;
		int blossomSizeMax = 15;
		int extraConnections = 0;
		int links = 0;
		int linksLength = 4;
		int edges = 0;
		int edgesLenght = 3;
		int oneVertexEdges = 7;
		int oneVertexSize = 3;
		List<string> people = new List<string>();
		HashSet<string> connections = new HashSet<string>();

		// Create blossoms
		for (int i = 0; i < blossoms; i++)
		{
			int size = random.Next(1, blossomSizeMax) * 2;
			string curName = GetName();
			string startName = curName;
			for (int j = 0; j < size; j++)
			{
				people.Add(curName);
				string connection = curName;
				curName = GetName();
				connection = connection + " - " + curName;
				connections.Add(connection);
			}
			connections.Add(curName + " - " + startName);
			people.Add(curName);
		}

		// Create direct connections
		for (int i = 0; i < extraConnections; i++)
		{
			string firstName = Name + random.Next(1, NameSuffix);
			string secondName = Name + random.Next(1, NameSuffix);
			if (firstName != secondName)
			{
				connections.Add(firstName + " - " + secondName);
			}
		}

		// Create linked connections
		for (int i = 0; i < links; i++)
		{
			int currentMax = people.Count;
			int startIndex = random.Next(0, people.Count);
			string startName = people[startIndex];
			int size = random.Next(0, linksLength);

			string curName = GetName();
			connections.Add(startName + " - " + curName);

			for (int j = 0; j < size; j++)
			{
				people.Add(curName);
				string connection = curName;
				curName = GetName();
				connection = connection + " - " + curName;
				connections.Add(connection);
			}
			people.Add(curName);
			int endIndex = random.Next(0, currentMax);
			string endName = people[endIndex];
			connections.Add(curName + " - " + endName);   
		}

		// Create leaf connections
		for (int i = 0; i < edges; i++)
		{
			int size = random.Next(1, edgesLenght + 1);
			string curName = people[random.Next(0, people.Count)];
			for (int j = 0; j < size; j++)
			{
				string connection = curName;
				curName = GetName();
				people.Add(curName);
				connection = connection + " - " + curName;
				connections.Add(connection);
			}
		}

		// Create points where multiple leaf edges intersect one vertex 
		for (int i = 0; i < oneVertexEdges; i++)
		{
			string centerVertex = people[random.Next(0, people.Count)];
			int size = random.Next(1, oneVertexSize + 1);
			for (int j = 0; j < size; j++)
			{
				string curName = GetName();
				people.Add(curName);
				connections.Add(centerVertex + " - " + curName);
			}
		}

		string file = "../../Test.txt";
		File.Delete(file);
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("People:");
		foreach (var name in people)
		{
			sb.AppendLine(name);
		}
		File.AppendAllText(file, sb.ToString());
		sb = new StringBuilder();
		sb.AppendLine("Connections:");
		foreach (var connection in connections)
		{
			sb.AppendLine(connection);
		}
		sb.AppendLine("END");
		File.AppendAllText(file, sb.ToString());
	}

	private static string GetName()
	{
		return Name + NameSuffix++;
	}
}
