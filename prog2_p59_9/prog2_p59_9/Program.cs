using System;

namespace prog2_p59_9
{
    class Program
    {
        static void Main(string[] args)
        {
            int x, y;
            Console.WriteLine("Enter 2 digits:");
            x = int.Parse(Console.ReadLine());
            y = int.Parse(Console.ReadLine());
            Console.WriteLine("The two posible numbers:"+x+""+y+", "+y+""+x);
            Console.WriteLine("The sum of the digits:"+ (x+y));
            Console.WriteLine("The division of the digits:"+ (x/y));
            Console.WriteLine("The rest of the digits division:" + (x % y));

        }
    }
}
