using System;
using System.Collections.Generic;
using System.Linq;

class ThreeBrothersGreedy
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        for (int i = 0; i < n; i++)
        {
            List<int> nums = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
            Console.WriteLine(Check3Partitions(nums) ? "Yes" : "No");
        }
    }

    static bool Check3Partitions(List<int> nums)
    {
        int totalSum = nums.Sum();
        if (totalSum % 3 != 0)
        {
            // The total sum cannot be divided into 3 equal integer numbers
            return false;
        }

        int targetSum = totalSum / 3;

        for (int i = 0; i < 2; i++)
        {
            int sum = targetSum;
            while (sum > 0)
            {
                var possibleNums = nums.Where(x => x <= sum);
                if (!possibleNums.Any())
                {
                    return false;
                }
                int maxNum = possibleNums.Max();
                sum -= maxNum;
                nums.Remove(maxNum);
            }
        }

        return true;
    }
}
