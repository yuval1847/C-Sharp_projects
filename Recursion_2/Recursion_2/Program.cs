using System;

namespace Recursion_2
{
    internal class Program
    {
        public static bool IsDivisible(int x, int y){
            // The function get 2 integers as a parameters
            // The function return true if y/x has no rest, otherwise false
            if (x < y && x > -y){
                return x == 0;
            }
            return IsDivisible(x-y, y);
        }

        public static bool IsPrime(int checkedNumber, int divisor = 2)
        {
            // The function get a positive integer as a parameter
            // The function return true if the number is a prime number, otherwise false
            if (checkedNumber == divisor) 
            {
                return true;
            }
            if (checkedNumber % divisor == 0)
            {
                return false;
            }
            return IsPrime(checkedNumber, divisor + 1);
        }

        public static bool IsAllOddsOrEvens(int checked_number)
        {
            // The function get a positive integer
            // The function return true if all the given number digits are odd or evens, otherwise false
            if (checked_number < 10)
            {
                return true;
            }
            int lastDigit = checked_number % 10;
            int secondLastDigit = (checked_number / 10) % 10;
            if (lastDigit % 2 == 0 != (secondLastDigit % 2 == 0))
            {
                return false;
            }
            return IsAllOddsOrEvens(checked_number / 10);
        }


        static void Main(string[] args)
        {
            Console.WriteLine(IsDivisible(18, 6));
            Console.WriteLine(IsPrime(17));
            Console.WriteLine(IsAllOddsOrEvens(24648));
        }
    }
}
