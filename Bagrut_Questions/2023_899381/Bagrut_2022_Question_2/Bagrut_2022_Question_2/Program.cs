internal class Program
{
    public static bool IsPerfect(int[] arr)
    {
        // The function get an integer array as a parameter
        // The function return true if the array defined as a 'perfect'
        // (When the function scan each index and finish in 0)
        // otherwiase false
        int indexInArray = 0;
        int[] copyArray = new int[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            copyArray[indexInArray] = arr[indexInArray];
            indexInArray = arr[indexInArray];
        }
        if (arr[indexInArray] != arr[0])
        {
            return false;
        }
        for (int i = 0; i < arr.Length; i++)
        {
            if (!(arr[i] == copyArray[i]))
            {
                return false;
            }
        }
        return true;
    }

    private static void Main(string[] args)
    {
        // Test the function
        int[] testArray = {1, 2, 3, 4, 0 };
        Console.WriteLine(IsPerfect(testArray));
    }
}