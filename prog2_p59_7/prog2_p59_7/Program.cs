using System;

namespace prog2_p59_7
{
    class Program
    {
        public static void Main(string[] args)
        {
            int min;
            Console.WriteLine("Enter minutes:");
            min = int.Parse(Console.ReadLine());
            Console.WriteLine("The time is: "+(min/60)+" H "+(min%60)+" min");
        }
    }
}
