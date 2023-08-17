internal class Program
{

    public static bool PosOrder(int[] arr)
    {
        // The function get an integer array
        // The function return true if the positive values in the array
        // are ordered in increasing order, otherwise false
        int previousPositiveInteger = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > 0 && arr[i] < previousPositiveInteger)
            {
                return false;
            }
            else if (arr[i] > previousPositiveInteger)
            {
                previousPositiveInteger = arr[i];
            }
        }
        return true;
    }

    private static void Main(string[] args)
    {
        // Testing the function
        int[] array = {1, 3, -4, 5, 2};
        Console.WriteLine(PosOrder(array));
    }
}