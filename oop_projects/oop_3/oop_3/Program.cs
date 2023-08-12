class Bicycle
{
    // A class which represent a bicycle

    private string company;
    private int gears;
    private string type;
    private double price;

    public Bicycle(string companyOfBike, int gearsOfBike, string typeOfBike, double priceOfBike)
    {
        // The function get a 4 variables (string, integer, string, double)
        // The function applies the given values in the class attributes
        this.company = companyOfBike;
        this.gears = gearsOfBike;
        this.type = typeOfBike;
        this.price = priceOfBike;
    }
    public Bicycle(Bicycle otherObject)
    {
        // The function get an object from this class(Bicycle class)
        // The function applies the current object's attributes to the 
        // given object values
        this.company = otherObject.company;
        this.gears = otherObject.gears;
        this.type = otherObject.type;
        this.price = otherObject.price;
    }
    public void SetCompany(string companyOfBike)
    {
        // The function get a string value which represent a company name
        // The function applies the given string to the company attribute
        // of the current object
        this.company = companyOfBike;
    }
    public string GetCompany()
    {
        // The function get nothing
        // The function return the company attribut of the current object
        return this.company;
    }
    public void SetGears(int gearsOfBike)
    {
        // The function get a integer value which represent a gears
        // The function applies the given integer to the gears attribute
        // of the current object
        this.gears = gearsOfBike;
    }
    public int GetGears()
    {
        // The function get nothing
        // The function return the gears attribut of the current object
        return this.gears;
    }
    public void SetType(string typeOfBike)
    {
        // The function get a string value which represent a type name
        // The function applies the given string to the type attribute
        // of the current object
        this.type = typeOfBike;
    }
    public string GetType()
    {
        // The function get nothing
        // The function return the type attribut of the current object
        return this.type;
    }
    public void SetPrice(double priceOfBike)
    {
        // The function get a double value which represent a price
        // The function applies the given double to the price attribute
        // of the current object
        this.price = priceOfBike;
    }
    public double GetPrice()
    {
        // The function get nothing
        // The function return the price attribut of the current object
        return this.price;
    }
    public string ToString()
    {
        // The fucntion get nothing
        // The function return a short description about the cuuernt
        // object using his attributs
        return $"company: {this.company}\ngears: {this.gears}\ntype: {this.type}\nprice: {this.price}$";
    }
    public bool IsEquals(Bicycle otherObject)
    {
        // The function get an object from the class Bicycle as a parameter
        // The function return true if the current object attributs are the same as the given
        // object attributes, otherwise false
        return otherObject.company == this.company && otherObject.gears == this.gears && otherObject.type == this.type;
    }
}


internal class Program
{
    public static bool BuyBike(Bicycle[] arrayOfBikes, Bicycle newBike)
    {
        // The function get an array of objects from Bicycle class and an object of Bicycle class
        // The function returns true if there is any null value index in the array and add the
        // given object in this index, otherwise it returns false
        for(int i = 0; i < arrayOfBikes.Length; i++)
        {
            if (arrayOfBikes[i] == null)
            {
                arrayOfBikes[i] = newBike;
                return true;
            }
        }
        return false;
    }

    public static void SuitableBike(Bicycle[] arrayOfBikes, double minPrice, double maxPrice)
    {
        // The function get an array of objects from Bicycle class and 2 double values as a parameters
        // The fucntion print all the objects from the given array which thier price attribute
        // is between the 2 given double values
        for(int i = 0; i < arrayOfBikes.Length; i++)
        {
            if (arrayOfBikes[i] != null && arrayOfBikes[i].GetPrice() > minPrice && arrayOfBikes[i].GetPrice() < maxPrice)
            {
                Console.WriteLine(arrayOfBikes[i].ToString());
            }
        }
    }

    public static Bicycle SellBike(Bicycle[] arrayOfBikes, Bicycle searchBike)
    {
        // The function get an array of objects from Bicycle class and an object of Bicycle class
        // The function check in the given array, if the are any object which his attributs same
        // to the given object, it applies the index as null and return the object, otherwise it returns null
        Bicycle foundBike = null;
        for(int i = 0; i < arrayOfBikes.Length; i++)
        {
            if (arrayOfBikes[i] != null && searchBike.IsEquals(arrayOfBikes[i]))
            {
                foundBike = arrayOfBikes[i];
                arrayOfBikes[i] = null;
                return foundBike;
            }
        }
        return null;
    }

    private static void Main(string[] args)
    {
        Bicycle[] arrayOfBikes = new Bicycle[20];
        Console.WriteLine("Hello seller, how many bikes do you have?: ");
        int amountOfBikes = int.Parse(Console.ReadLine());
        Bicycle newBike = null;
        for (int i = 0; i < amountOfBikes && BuyBike(arrayOfBikes, newBike); i++)
        {
            Console.WriteLine("Company: ");
            string companyName = Console.ReadLine();
            Console.WriteLine("Gears: ");
            int amountOfGears = int.Parse(Console.ReadLine());
            Console.WriteLine("Type: ");
            string typeOfBike = Console.ReadLine();
            Console.WriteLine("Price: ");
            double priceOfBike = double.Parse(Console.ReadLine());
            newBike = new Bicycle(companyName, amountOfGears, typeOfBike, priceOfBike);
            BuyBike(arrayOfBikes, newBike);
            newBike = null;
        }

        Console.WriteLine("Enter a minimum price: ");
        double minimumPrice = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter a maximum price: ");
        double maximumPrice = double.Parse(Console.ReadLine());
        SuitableBike(arrayOfBikes, minimumPrice, maximumPrice);

        Console.WriteLine("Enter you wanted bike attributs: ");
        Console.WriteLine("Company: ");
        string company = Console.ReadLine();
        Console.WriteLine("Gears: ");
        int gears = int.Parse(Console.ReadLine());
        Console.WriteLine("Type: ");
        string type = Console.ReadLine();
        Console.WriteLine("Price: ");
        double price = double.Parse(Console.ReadLine());
        Bicycle wantedBike = new Bicycle(company, gears, type, price);
        if (SellBike(arrayOfBikes, wantedBike) != null)
        {
            Console.WriteLine(wantedBike);
        }

        int countBikesInShop = 0;
        for(int i = 0; i < arrayOfBikes.Length; i++)
        {
            if (arrayOfBikes[i] != null)
            {
                countBikesInShop++;
            }
        }
        Console.WriteLine($"Right now, there are {countBikesInShop} bikes in the shop.");
    }
}