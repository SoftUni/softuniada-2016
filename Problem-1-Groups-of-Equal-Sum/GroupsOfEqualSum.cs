using System;

class GroupsОfEqualSum
{
    static void Main()
    {
        int num1 = int.Parse(Console.ReadLine());
        int num2 = int.Parse(Console.ReadLine());
        int num3 = int.Parse(Console.ReadLine());
        int num4 = int.Parse(Console.ReadLine());

        double halfSum = (num1 + num2 + num3 + num4) / 2.0;
        
        if (num1 == halfSum ||
            num2 == halfSum ||
            num3 == halfSum ||
            num4 == halfSum ||
            num1 + num2 == halfSum ||
            num1 + num3 == halfSum ||
            num1 + num4 == halfSum)
        {
            Console.WriteLine("Yes");
            Console.WriteLine(halfSum);
        }
        else
        {
            Console.WriteLine("No");
        }
    }
}
