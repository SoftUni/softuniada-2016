using System;

public class TennisNaiveGreedy
{
	public static void Main(string[] args)
	{
		int count = 0;
		Console.ReadLine();
		string line = Console.ReadLine();
		while (line != "Connections:")
		{
			count++;
			line = Console.ReadLine();
		}

		Console.WriteLine(count / 2);
		//Console.WriteLine((count / 2) - 1);
		//Console.WriteLine((count / 2) - 2);
		//Console.WriteLine((count / 2) - 3);
		//Console.WriteLine((count / 2) - 4);
		//Console.WriteLine((count / 2) - 5);
	}
}
