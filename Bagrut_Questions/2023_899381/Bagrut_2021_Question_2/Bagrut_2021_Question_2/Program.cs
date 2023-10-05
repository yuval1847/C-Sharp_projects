public class Subject
{
    private string subName;
    private int grade;

    // The Get functions:
    public string GetSubName()
    {
        // The function get noting and return the subName attribute
        return this.subName;
    }
    public int GetGrade()
    {
        // The function get noting and return the subName attribute
        return this.grade;
    }

    // The Set functions:
    public void SetSubName(string subNameNewValue)
    {
        // The function get a string and set the subName attribute to the given string
        this.subName = subNameNewValue;
    }
    public void SetGrade(int gradeNewValue)
    {
        // The function get a integer and set the grade attribute to the given integer
        this.grade = gradeNewValue;
    }


}
public class ReportCard
{
    private string stuName;
    private Subject[] subArray; 

    public ReportCard(string stuNameValue, Subject[] subArrayValue)
    {
        // The function get the class attributes values
        // The current object's attributes are set to the given values
        this.stuName = stuNameValue;
        this.subArray = subArrayValue;
    }
    // The Get functions:
    public string GetStuName()
    {
        // The function get noting and return the stuName attribute
        return this.stuName;
    }
    public Subject[] GetSubArray()
    {
        // The function get noting and return the subArray attribute
        return this.subArray;
    }

    // The Set functions:
    public void SetSubName(string stuNameNewValue)
    {
        // The function get a string and set the stuName attribute to the given string
        this.stuName = stuNameNewValue;
    }
    public void SetGrade(Subject[] subArrayNewValue)
    {
        // The function get a Subject array
        // and set the grade attribute to the given integer
        this.subArray = subArrayNewValue;
    }
    public bool IsExcellent()
    {
        // The function get nothing
        // The function return true if the current subArray attribute avarage is equal
        // or up to 85, all the values are up to 54 and at least one of the values is 100
        bool is100 = false;
        int sum = 0;
        for(int i = 0; i < this.subArray.Length; i++)
        {
            sum += this.subArray[i].GetGrade();
            if(this.subArray[i].GetGrade() < 54)
            {
                return false;
            }
            if (this.subArray[i].GetGrade() == 100)
            {
                is100 = true;
            }
        }
        if((double)sum / this.subArray.Length < 84)
        {
            return false;
        }
        return is100;
    }

}

internal class Program
{
    public static void PrintExcellent(ReportCard[] array)
    {
        // The function get a ReportCard array
        // The function print the names of all the excellent students
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i].IsExcellent())
            {
                Console.WriteLine(array[i].GetStuName()+"is Excellent");
            }
        }
    }
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}