using System;

namespace Recursion_1
{
    internal class Program
    {
        public static int SumFromOne(int n)
        {
            // The function get a positive integer as a parameter
            // The function return the sum of all the numbers between 1 to the given integer
            if (n == 1)
            {
                return 1;
            }
            return SumFromOne(n - 1) + n;
        }
        public static int FactorialFromOne(int n)
        {
            // The function get a positive integer as a parameter
            // The function return the mult of all the numbers between 1 to the given integer
            if (n == 1)
            {
                return 1;
            }
            return FactorialFromOne(n - 1)*n;
        }
        
        public static int FactorialOnlyOdds(int n)
        {
            // The function get a positive integer as a parameter
            // The function return the mult of all the odd numbers between 1 to the given integer
            if (n == 1)
            {
                return 1;
            }
            if (n % 2 == 0)
            {
                n--;
            }
            return FactorialOnlyOdds(n - 2) * n;
        }

        public static int AmountOfDigits(int number)
        {
            // The function get a positive integer as a parameter
            // The function return the amount of digits which appears in the number
            if (number <= 9)
            {
                return 1;
            }
            return AmountOfDigits(number/10)+1;
        }

        public static int divisionQuotient(int firstNumber, int secondNumber)
        {
            // The function get 2 integers as a parameters
            // The function return the division quotient of the first number divided by the second number
            if (secondNumber == firstNumber)
            {
                return 1;
            }
            if (secondNumber > firstNumber)
            {
                return 0;
            }
            firstNumber -= secondNumber;
            return divisionQuotient(firstNumber, secondNumber) + 1;
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"Sum 1 to 5: {SumFromOne(5)}");
            Console.WriteLine($"Fuctorial of 5: {FactorialFromOne(5)}");
            Console.WriteLine($"Only odds fuctorial of 5: {FactorialOnlyOdds(5)}");
            Console.WriteLine($"Amount of digits in the number 1234 is: {AmountOfDigits(1234)}");
            Console.WriteLine($"The round division of 21 / 5: {divisionQuotient(21, 5)}");

        }
    }
}