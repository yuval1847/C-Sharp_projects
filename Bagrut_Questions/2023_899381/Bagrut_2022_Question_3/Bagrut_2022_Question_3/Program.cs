class Pixel
{
    // A class which represent a computer one pixel unit
    private int red = 0;
    private int green = 0;
    private int blue = 0;

    public int GetRed()
    {
        // The function get nothing
        // The function return the value of the red attribute
        return red;
    }
    public int GetGreen()
    {
        // The function get nothing
        // The function return the value of the green attribute
        return green;
    }
    public int GetBlue()
    {
        // The function get nothing
        // The function return the value of the blue attribute
        return blue;
    }

    public void SetRed(int newRedValue)
    {
        // The function get an positive integer value
        // The function set the value of the red attribute to the 
        // given value
        this.red = newRedValue;
    }
    public void SetGreen(int newGreenValue)
    {
        // The function get an positive integer value
        // The function set the value of the Green attribute to the 
        // given value
        this.green = newGreenValue;
    }
    public void SetBlue(int newBlueValue)
    {
        // The function get an positive integer value
        // The function set the value of the Blue attribute to the 
        // given value
        this.blue = newBlueValue;
    }

    public bool IsRed()
    {
        // The function get nothing
        // The function return true if the pixel is red color, otherwise false
        return this.red > 0 && this.green == 0 && this.blue == 0;
    }
    public bool IsGreen()
    {
        // The function get nothing
        // The function return true if the pixel is green color, otherwise false
        return this.green > 0 && this.red == 0 && this.blue == 0;
    }
    public bool IsBlue()
    {
        // The function get nothing
        // The function return true if the pixel is blue color, otherwise false
        return this.blue > 0 && this.green == 0 && this.red == 0;
    }
}

class Structure
{
    private Pixel[] arr = new Pixel[9];

    public bool isBalanced()
    {
        // The function get nothing
        // The function return true if the all the pixel attributes are equals
        int redCounter = 0, greenCounter = 0, blueCounter = 0;
        for(int i = 0; i < arr.Length; i++)
        {
            if (arr[i].IsRed())
            {
                redCounter++;
            }
            if(arr[i].IsGreen())
            {
                greenCounter++;
            }
            if (arr[i].IsBlue())
            {
                blueCounter++;
            }
        }
        return redCounter == greenCounter && redCounter == blueCounter;
    }

    public bool IsBlackWhite()
    {
        // The function get nothing
        // The function return true if the arr array contain only balck and white pixels color,
        // otherwise false
        for(int i = 0; i < arr.Length; i++)
        {
            if (!((arr[i].GetRed() == 0 && arr[i].GetGreen() == 0 && arr[i].GetBlue() == 0) || (arr[i].GetRed() == 255 && arr[i].GetGreen() == 255 && arr[i].GetBlue() == 255)))
            {
                return false;
            }
        }
        return true;
    }
}



internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}