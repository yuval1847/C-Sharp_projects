using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

public class Point
{
    private double x;
    private double y;
    //default constructors
    public Point()
    {
        this.x = 0;
        this.y = 0;
    }
    //constructor
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    //copy constructor
    public Point(Point p)
    {
        this.x = p.x;
        this.y = p.y;
    }
    //setters
    public void SetX(double x)
    {
        this.x = x;
    }
    public void SetY(double y)
    {
        this.y = y;
    }
    //getters
    public double GetX()
    {
        return this.x;
    }
    public double GetY()
    {
        return this.y;
    }
    //ToString
    public override string ToString()
    {
        return "(" + this.x + "," + this.y + ")";
    }
    //טענת כניסה: הפעולה מקבלת כפרמטר ערך של נקודה נוספת
    //טענת יציאה: הפעולה מעדכנת את ערכי הנקודה הנוכחית, כך שערך ה-x
    //שלה מתקבל מסכום ערכי ה-x-ים
    //של שתי הנקודות (של הנוכחית ושל הפרמטר),
    //וערך ה-y שלה מתקבל מסכום ערכי
    //ה-y-ים של שתי הנקודות
    public void Add(Point p)
    {
        this.x += p.x;
        this.y += p.y;
    }
    //טענת כניסה: הפעולה משתמשת בתכונות המחלקה
    //טענת יציאה: הפעולה מחזירה את מרחק הנקודה מראשית הצירים ע"פ משפט פיתגורס
    public double Distance()
    {
        return Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2));
    }
    //טענת כניסה: הפעולה מקבלת כפרמטר נקודה נוספת
    //טענת יציאה: הפעולה מחזירה את המרחק בין שתי הנקודות ע"פ משפט פיתגורס
    public double Distance(Point p)
    {
        return Math.Sqrt(Math.Pow(this.x - p.x, 2) + Math.Pow(this.y - p.y, 2));
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
       Point[] ArrayOfPoints = new Point[10];
       for(int i = 0; i < ArrayOfPoints.Length; i++)
       {
            ArrayOfPoints[i] = new Point();
            Console.Write("X: ");
            ArrayOfPoints[i].SetX(double.Parse(Console.ReadLine()));
            Console.Write("Y: ");
            ArrayOfPoints[i].SetY(double.Parse(Console.ReadLine()));
       }

       Point farthestPointFromOrigin = new Point();
       Point OriginPoint = new Point();
       for (int i = 0; i < ArrayOfPoints.Length; i++)
       {
            if (OriginPoint.Distance(ArrayOfPoints[i]) > OriginPoint.Distance(farthestPointFromOrigin))
            {
                farthestPointFromOrigin = ArrayOfPoints[i];
            }
       }
        // Printing the farthest point from origin point
        Console.WriteLine($"The farthest point from the origin point is: {farthestPointFromOrigin.ToString()}");

        // Creating a new point
        Point newPoint = new Point();
        Console.Write("X: ");
        newPoint.SetX(int.Parse(Console.ReadLine()));
        Console.Write("Y: ");
        newPoint.SetY(int.Parse(Console.ReadLine()));
        
        // Calculate the average distance of the point in the array from the new point
        double sumOfDistances = 0;
        for(int i = 0; i < ArrayOfPoints.Length; i++)
        {
            sumOfDistances += ArrayOfPoints[i].Distance(newPoint);
        }
        Console.WriteLine($"The average distance from the new point is {sumOfDistances/10}");
    }
}