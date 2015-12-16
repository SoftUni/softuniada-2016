using System;
using System.Linq;
using System.Collections.Generic;

class ThreeBrothers
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        for (int i = 0; i < n; i++)
        {
            int[] nums = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
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

        int targetSum = totalSum / 3;
        var sumReached = new bool[targetSum + 1, targetSum + 1];
        //var prev1 = new short[targetSum + 1, targetSum + 1];
        //var prev2 = new short[targetSum + 1, targetSum + 1];

        // Dynamic programming: fill sumReached[0...targetSum, 0...targetSum]
        sumReached[0, 0] = true;
        foreach (short num in nums)
        {
            for (int s1 = targetSum; s1 >= 0; s1--)
            {
                for (int s2 = targetSum; s2 >= 0; s2--)
                {
                    if (sumReached[s1, s2])
                    {
                        if (s1 + num <= targetSum && !sumReached[s1 + num, s2])
                        {
                            sumReached[s1 + num, s2] = true;
                            //prev1[s1 + num, s2] = num;
                        }
                        if (s2 + num <= targetSum && !sumReached[s1, s2 + num])
                        {
                            sumReached[s1, s2 + num] = true;
                            //prev2[s1, s2 + num] = num;
                        }
                    }
                }
            }
        }

        bool possible = sumReached[targetSum, targetSum];
        //if (possible)
        //{
        //    PrintSolution(nums, sumReached, prev1, prev2, targetSum);
        //}
        return possible;
    }

    //static void PrintSolution(int[] nums, bool[,] sumReached, 
    //    short[,] prev1, short[,] prev2, int targetSum)
    //{
    //    int sum1 = targetSum;
    //    int sum2 = targetSum;
    //    var nums1 = new List<int>();
    //    var nums2 = new List<int>();
    //    var nums3 = new List<int>(nums);
    //    while (sum1 > 0 || sum2 > 0)
    //    {
    //        if (prev1[sum1, sum2] > 0)
    //        {
    //            nums1.Add(prev1[sum1, sum2]);
    //            nums3.Remove(prev1[sum1, sum2]);
    //            sum1 -= prev1[sum1, sum2];
    //        }
    //        else if (prev2[sum1, sum2] > 0)
    //        {
    //            nums2.Add(prev2[sum1, sum2]);
    //            nums3.Remove(prev2[sum1, sum2]);
    //            sum2 -= prev2[sum1, sum2];
    //        }
    //    }

    //    Console.WriteLine("Yes");
    //    Console.WriteLine("{0} = {1} = {2}",
    //        string.Join(" + ", nums1),
    //        string.Join(" + ", nums2),
    //        string.Join(" + ", nums3));
    //}
}
