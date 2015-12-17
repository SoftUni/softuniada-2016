using System;

public class StarsTestGenerator
{
    static void Main()
    {
        int n = 75;
        int starsCount = 5;

        // Generate random cube
        Random rnd = new Random();
        var cube = new char[n, n, n];
        for (int h = 0; h < n; h++)
        {
            for (int d = 0; d < n; d++)
            {
                for (int w = 0; w < n; w++)
                {
                    char letter = 'z';
                    if (rnd.Next(3) == 0)
                        letter = 'y';
                    if (rnd.Next(5) == 0)
                        letter = (char)('a' + rnd.Next(3));
                    if (rnd.Next(20) == 0)
                        letter = (char)('a' + rnd.Next(25));
                    cube[w, h, d] = letter;
                }
            }
        }

        // Generate random stars
        for (int i = 0; i < starsCount; i++)
        {
            char letter = (char)('a' + rnd.Next(26));
            int w = rnd.Next(n-2) + 1;
            int d = rnd.Next(n-2) + 1;
            int h = rnd.Next(n-2) + 1;
            cube[w, h, d] = letter;
            cube[w + 1, h, d] = letter;
            cube[w - 1, h, d] = letter;
            cube[w, h + 1, d] = letter;
            cube[w, h - 1, d] = letter;
            cube[w, h, d + 1] = letter;
            cube[w, h, d - 1] = letter;
        }

        // Print the cube
        Console.WriteLine(n);
        for (int h = 0; h < n; h++)
        {
            for (int d = 0; d < n; d++)
            {
                for (int w = 0; w < n; w++)
                {
                    if (w > 0)
                    {
                        Console.Write(' ');
                    }
                    Console.Write(cube[w, h, d]);
                }
                if (d < n - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
    }
}
