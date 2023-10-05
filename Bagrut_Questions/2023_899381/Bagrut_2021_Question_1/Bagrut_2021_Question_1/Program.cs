using System.Runtime.InteropServices;

internal class Program
{
    public static int[] Filter(int[] arr, int num)
    {
        // The function get an array of integers and an integer
        // The function return a new integers array which contain only the values from 
        // the given array which aren't equal to num
        int lengthWithoutNum = 0, newArrayIndex = 0;
        for(int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != num)
            {
                lengthWithoutNum++;
            }
        }
        int[] newArray = new int[lengthWithoutNum];
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] != num)
            {
                newArray[newArrayIndex] = arr[i];
                newArrayIndex++;
            }
        }
        return newArray;
    }

    private static void Main(string[] args)
    {
        int[] array = { 1, 2, 3, 4, 3, 5, 6 };
        int[] newArray = Filter(array, 3);
        for(int i = 0; i < newArray.Length; i++)
        {
            Console.WriteLine(newArray[i]);
        }
    }
}