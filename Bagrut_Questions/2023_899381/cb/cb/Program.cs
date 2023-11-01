
using System.Drawing;

class ColoredBox
{
    private double length, width, height;
    private string color;
    public ColoredBox(double length, double width, double height, string color)
    {
        this.length = length;
        this.width = width;
        this.height = height;
        this.color = color;
    }

    public double GetLength() { 
        return length;
    }
    public double GetWidth() { 
        return width;
    }
    public double GetHeight() { 
        return height;
    }
    public string GetColor() {
        return color;
    }
    public void SetLenght(double length) {
        this.length = length; 
    }
    public void SetWidth(double width) { 
        this.width = width;
    }
    public void SetHeight(double height)
    {
        this.height = height;
    }
    public void SetColor(string color)
    {
        this.color = color;
    }
    public double GetVolume() {
        return this.height * this.width * this.height; 
    }
    public string ToString()
    {
        return "The box attributes: \nLength:"+this.length+"\nWidth:"+this.width+"\nHeight"+this.height;
    }
    internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
