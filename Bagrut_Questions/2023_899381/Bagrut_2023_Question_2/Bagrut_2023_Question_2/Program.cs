internal class Program
{
    public static int MissingNum(int[] arr)
    {
        // The function get an integers array.
        // The function return a number which suppose to be somewhere between 2 nearby cells
        // according to the arr's constant diffrence between each 2 cells.
        int diffrenceInArray = 0;
        if (arr[1] - arr[0] == arr[2] - arr[1])
        {
            diffrenceInArray = arr[1] - arr[0];
        }
        else if (arr[1] - arr[0] == arr[arr.Length - 1] - arr[arr.Length - 2])
        {
            diffrenceInArray = arr[1] - arr[0];
        }
        else
        {
            diffrenceInArray = arr[2] - arr[1];
        }
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i+1] - arr[i] != diffrenceInArray)
            {
                return arr[i] + diffrenceInArray;
            }
        }
        return 0;
    }
    private static void Main(string[] args)
    {
        int[] arrayOfNubers = { 1, 2, 3, 5, 6 };
        Console.WriteLine(MissingNum(arrayOfNubers));
    }
}