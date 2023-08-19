internal class Program
{
    public static int[] Multiply(int[] arr1, int[] arr2)
    {
        // The function get 2 integers array(arr1, arr2).
        // The function return a new integers array which any index equals to the
        // multiply of the same index value from the 2 given array.
        int[] newMultiplyArray = new int[Math.Max(arr1.Length, arr2.Length)];
        int[] longestArray;
        if (arr1.Length > arr2.Length)
        {
            longestArray = arr1;
        }
        else
        {
            longestArray = arr2;
        }
        for(int i = 0; i < Math.Min(arr1.Length, arr2.Length); i++)
        {
            newMultiplyArray[i] = arr1[i] * arr2[i];
        }
        for(int i = 0; i < newMultiplyArray.Length; i++)
        {
            if (newMultiplyArray[i] == 0)
            {
                newMultiplyArray[i] = longestArray[i];
            }
        }
        return newMultiplyArray;
    }

    private static void Main(string[] args)
    {
        // Testing the function
        int[] array1 = { 1, 2, 3, 4 };
        int[] array2 = { 1, 2, 3, 4, 5, 6, 7, 8 };
        for(int i = 0; i < Multiply(array1, array2).Length; i++)
        {
            Console.Write(Multiply(array1, array2)[i]+"|");
        }
    }
}