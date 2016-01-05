using System;

class DrawHouse
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        // Print the triangle roof of stars
        for (int line = 1; line < n; line++)
        {
            Console.Write(new string('.', n - line));
            Console.Write('*');
            if (line > 1)
            {
                Console.Write(new string(' ', 2 * line - 3));
                Console.Write('*');
            }
            Console.WriteLine(new string('.', n - line));
        }
        Console.Write('*');
        for (int i = 1; i < n; i++)
        {
            Console.Write(" *");
        }
        Console.WriteLine();

        // Print the house body
        Console.Write('+');
        Console.Write(new string('-', 2*n-3));
        Console.Write('+');
        Console.WriteLine();
        for (int i = 0; i < n-2; i++)
        {
            Console.Write('|');
            Console.Write(new string(' ', 2 * n - 3));
            Console.Write('|');
            Console.WriteLine();
        }
        Console.Write('+');
        Console.Write(new string('-', 2 * n - 3));
        Console.Write('+');
        Console.WriteLine();
    }
}
