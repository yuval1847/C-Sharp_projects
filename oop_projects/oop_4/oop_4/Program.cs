public class Musical
{
    private string name;//שם הכלי
    private string material;//סוג החומר
    private string type;//סוג הכלי - נשיפה/הקשה/קלידים/מיתרים
    private string company;//שם החברה
    private int price;//מחיר הכלי בשקלים שלמים
    //constructors
    public Musical(string name, string material, string type, string company, int price){
        this.name = name;
        this.material = material;
        this.type = type;
        this.company = company;
        this.price = price;
    }

    public Musical(Musical other)
    {
        this.name = other.name;
        this.material = other.material;
        this.type = other.type;
        this.company = other.company;
        this.price = other.price;
    }
    //Setters
    public void SetMaterial(string material)
    {
        this.material = material;
    }
    public void SetType(string type)
    {
        this.type = type;
    }
    public void SetCompany(string company)
    {
        this.company = company;
    }
    public void SetPrice(int price)
    {
        this.price = price;
    }
    //Getters
    public string GetName()
    {
        return this.name;
    }
    public string GetMaterial()
    {
        return this.material;
    }
    public string GetType()
    {
        return this.type;
    }
    public string GetCompany()
    {
        return this.company;
    }
    public int GetPrice()
    {
        return this.price;
    }
    public override string ToString()
    {
        return "Musical name: " + this.name +
            " material: " + this.material +
            " type: " + this.type +
            " company: " + this.company +
            " price: " + this.price;
    }
    //טענת כניסה: הפעולה מקבלת אחוז הנחה - במספר שלם
    //טענת יציאה: הפעולה מעדכנת ומחזירה את הסכום לתשלום עבור הכלי הנוכחי, לאחר ההנחה
    public int Calculate(int discountPercent)
    {
        this.price = (int)(this.price * (100 - discountPercent) / 100);
        return this.price;
    }
    //הפעולה מקבלת כפרמטר כלי נגינה נוסף
    //טענת יציאה: הפעולה משווה את הכלי שהתקבל לכלי הנוכחי ומחזירה אמת
    //אם התכונות: שם הכלי, החומר ממנו מיוצר הכלי, סוג הכלי והחברה
    //לא כולל השוואת המחיר - זהות. אחרת, הפעולה מחזירה שקר
    public bool Equals(Musical other)
    {
        return this.name == other.name &&
            this.material == other.material &&
            this.type == other.type &&
            this.company == other.company;
    }
    //טענת כניסה: הפעולה מקבלת כפרמטר כלי נגינה נוסף
    //טענת יציאה: הפעולה משווה בין מחירו למחיר של הנוכחי. הפעולה מחזירה
    //אם מחיר הכלי הנוכחי גבוה ממחיר הכלי שהתקבל כפרמטר
    //אם מחיר הכלי הנוכחי נמוך ממחיר הכלי שהתקבל כפרמטר
    //אם מחירי שני כלי הנגינה זהים
    public int CompareTo(Musical other)
    {
        if (this.price > other.price)
            return 1;
        if (this.price < other.price)
            return -1;
        return 0;
    }
}

class Concert{
    // A class which is represent a concert

    private string recital;
    private Musical[] musicals;
    private int current;

    public Concert(string recitalName)
    {
        this.recital = recitalName;
        this.musicals = new Musical[40];
        this.current = 0;
    }
    
    public string GetRecital()
    {
        // The function get nothing
        // The function return the recital attribute string value
        // of the current object
        return this.recital;
    }

    public void SetRecital(string recitalName)
    {
        // The function get nothing
        // The function set the string attribute recital
        // of the current object to the given string
        this.recital = recitalName;
    }

    public Musical[] GetMusicals()
    {
        // The fucntion get nothing
        // The fucntion return a copy of the given Musical array attribute musicals
        Musical[] copyOfArray = new Musical[this.musicals.Length];
        copyOfArray = this.musicals;
        return copyOfArray;
    }

    public void SetMusicals(Musical[] arrayOfMusicals)
    {
        // The function get an array of musical object 
        // The function set the values of a copy of the
        // Musical array attribute musicals to the given array
        Musical[] copyOfArray = new Musical[arrayOfMusicals.Length];
        copyOfArray = arrayOfMusicals;
    }

    public int GetCurrent()
    {
        // The fucntion get nothing
        // The fucntion return the current attribute integer value
        return this.current;
    }

    public bool AddMusical(Musical musicalInstrument)
    {
        // The function get a musical object
        // The function check, if it can be added to the musical array, so it add
        // it and return true, otherwise false
        if (this.musicals.Length < 40)
        {
            for(int i = 0; i > this.musicals.Length; i++)
            {
                if (this.musicals[i] == null)
                {
                    this.musicals[i] = musicalInstrument;
                    this.current++;
                }
            }
            return true;
        }
        return false;
    }

    public Musical RemoveMusical()
    {
        // The function get nothing
        // The function remove the last object from the musicals array
        Musical lastMusicalInstrument = musicals[this.current - 1];
        musicals[this.current - 1] = null;
        return lastMusicalInstrument;
    }

    public void PrintMusical(string nameOfInstrument)
    {
        // The function get a string
        // The function return a list of all the instruments which thier name are similar to
        // the given string
        for(int i = 0; i < musicals.Length; i++)
        {
            if (musicals[i].GetName() == nameOfInstrument)
            {
                Console.WriteLine(musicals[i]);
            }
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {

    }
}