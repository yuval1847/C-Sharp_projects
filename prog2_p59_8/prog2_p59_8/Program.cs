using System;

namespace prog2_p59_8
{
    class Program
    {
        static void Main(string[] args)
        {
            int x;
            Console.WriteLine("Enter the childern number:");
            x = int.Parse(Console.ReadLine());
            Console.WriteLine("there are:"+(x/2)+" couples");
            Console.WriteLine("there are:" + (x % 2) + "children that don't have couple");
            Console.WriteLine("there are:" + (x % 3) + "children that don't have triple");
        }
    }
}
