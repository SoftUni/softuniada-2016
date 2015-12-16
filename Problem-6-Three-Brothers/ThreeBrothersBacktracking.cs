using System;
using System.Collections.Generic;
using System.Linq;

class ThreeBrothersBacktracking
{
    static int[] nums;
    static int targetSum;
    static List<int> firstNums;
    static List<int> secondNums;
    static List<int> thirdNums;
    static bool found;

    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        for (int i = 0; i < n; i++)
        {
            nums = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Console.WriteLine(Check3Partitions(nums) ? "Yes" : "No");
        }
    }

    static bool Check3Partitions(int[] nums)
    {
        int totalSum = nums.Sum();
        if (totalSum % 3 != 0)
        {
            // The total sum cannot be divided into 3 equal integer numbers
            return false;
        }

        targetSum = totalSum / 3;

        firstNums = new List<int>();
        secondNums = new List<int>();
        thirdNums = new List<int>();
        found = false;
        FindSum(0, 0, 0);
        return found;
    }

    static void FindSum(int firstSum, int secondSum, int index)
    {
        if (found || firstSum > targetSum || secondSum > targetSum || index >= nums.Length)
        {
            return;
        }

        if (firstSum == targetSum && secondSum == targetSum)
        {
            // Solution found --> print it
            found = true;
            //thirdNums.AddRange(nums.Skip(index));
            //Console.WriteLine("{0} = {1} = {2}",
            //    string.Join(" + ", firstNums),
            //    string.Join(" + ", secondNums),
            //    string.Join(" + ", thirdNums));
            return;
        }

        // Try to add the next number to the first sum
        //firstNums.Add(nums[index]);
        FindSum(firstSum + nums[index], secondSum, index + 1);
        //firstNums.RemoveAt(firstNums.Count - 1);

        // Try to add the next number to the second sum
        //secondNums.Add(nums[index]);
        FindSum(firstSum, secondSum + nums[index], index + 1);
        //secondNums.RemoveAt(secondNums.Count - 1);

        // Try to add the next number to the third sum
        //thirdNums.Add(nums[index]);
        FindSum(firstSum, secondSum, index + 1);
        //thirdNums.RemoveAt(thirdNums.Count - 1);
    }
}
