using System;
using System.Linq;
using System.Collections.Generic;

class ThreeBrothers
{
    static void Main()
    {
        int n = 100;
        Console.WriteLine(n);

        Random rnd = new Random();
        for (int counter = 0; counter < n; counter++)
        {
            int[] nums = new int[rnd.Next(50, 100)];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = rnd.Next(16, 18);
            }
            while (nums.Sum() % 3 != 0)
            {
                nums[0] = rnd.Next(1, 21);
            }
            Console.WriteLine(string.Join(" ", nums));
        }
    }
}
