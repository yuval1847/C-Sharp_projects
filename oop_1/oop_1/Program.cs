using System.Runtime.InteropServices;

class Musical
{
    private string name;
    private string material;
    private string type;
    private string company;
    private int price;

    public Musical(string name_of_instrument, string material_of_instrument, string type_of_instrument, string company_of_instrument, int price_of_instrument)
    {
        // The function get the variables values of the Musical class new object
        // The function set all the values to the right attributes variables
        this.name = name_of_instrument;
        this.material = material_of_instrument;
        this.type = type_of_instrument;
        this.company = company_of_instrument;
        this.price = price_of_instrument;
    }

    public Musical(Musical other_object)
    {
        // The function get an object of the current class
        // The function set the given object's attributes variable to the new object
        this.name = other_object.name;
        this.material = other_object.material;
        this.type = other_object.type;
        this.company = other_object.company;
        this.price = other_object.price;
    }
    public void SetMaterial(string newMaterial)
    {
        // The function get a new value of string
        // The function set the current object material attribute to the given string
        this.material = newMaterial;
    }
    public void SetType(string newType)
    {
        // The function get a new value of string
        // The function set the current object type attribute to the given string
        this.type = newType;
    }
    public void SetCompany(string newCompany)
    {
        // The function get a new value of string
        // The function set the current object company attribute to the given string
        this.company = newCompany;
    }
    public void SetPrice(string newPrice)
    {
        // The function get a new value of full number as a parameter
        // The function set the current object price attribute to the given integer
        this.material = newPrice;
    }
    public string GetName()
    {
        // The function get nothing
        // The function return the name attribute of the object
        return this.name;
    }
    public string GetMaterial()
    {
        // The function get nothing
        // The function return the material attribute of the object
        return this.material;
    }
    public string GetType()
    {
        // The function get nothing
        // The function return the type attribute of the object
        return this.type;
    }
    public string GetCompany()
    {
        // The function get nothing
        // The function return the company attribute of the object
        return this.company;
    }
    public int GetPrice()
    {
        // The function get nothing
        // The function return the name attribute of the object
        return this.price;
    }
    public string ToString()
    {
        // The function get nothing
        // The function return a shirt description about the object using the object's attributes
        return $"name: {this.name}\nmaterial: {this.material}\ntype: {this.type}\ncompany: {this.company}\nprice: {this.price}$";
    }
    public int Calculate(int precentegesOfDiscount)
    {
        // The function get an integer as a parameter which represent the presenteges of discount
        // The function update the price and return it
        this.price -= this.price / 100 * precentegesOfDiscount;
        return this.price;
    }
    public bool isEquals(Musical otherObject)
    {
        // The function get another object from the Musical class as a paramter
        // The function return true if the given object's attributes(not inclued price) are equals
        // otherwaise, false
        return this.name == otherObject.name && this.material == otherObject.material && this.type == otherObject.type && this.company == otherObject.company;
    }
    public string CompareTo(Musical otherObject)
    {
        // The function get another object from the Musical class as a paramter
        // The function compare between the current object and the given object and 
        // return if the current object if more expensive/less expensive or identical price
        if (this.price > otherObject.price)
        {
            return "The current price is higher than the given price";
        }
        else if(this.price < otherObject.price)
        {
            return "The current price is lower than the given price";
        }
        return "They both have the same price";
    }
}


internal class Program
{
    private static void Main(string[] args)
    {
        // Testing the class Musical
        Musical Guitar_1 = new Musical("z20", "wood", "guitar", "asus", 2000);
        Musical Guitar_2 = new Musical("z20", "wood", "guitar", "asus", 2000);
        Console.WriteLine(Guitar_2.ToString());
    }
}