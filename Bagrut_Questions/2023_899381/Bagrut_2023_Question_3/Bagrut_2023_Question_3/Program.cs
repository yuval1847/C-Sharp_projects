class CarInfo
{
    // A class which represent a Car

    private string id;
    private bool privateCar;
    private int speed;
    
    public CarInfo(string newId, bool IsPrivateCar, int newSpeed)
    {
        // The function get values to the new object attributes
        // The function set the attributes values to the given values
        this.id = newId;
        this.privateCar = IsPrivateCar;
        this.speed = newSpeed;
    }

    public string GetId()
    {
        // The function get nothing
        // The function return the current object id attribute value
        return this.id;
    }

    public void SetId(string newId)
    {
        // The function get a string value which represent an Id
        // The function set the current object id attribute value to the
        // given value
        this.id = newId;
    }
    public bool GetPrivateCar()
    {
        // The function get nothing
        // The function return the current object PriaveCar attribute value
        return this.privateCar;
    }

    public void SetPrivateCar(bool IsPrivateCar)
    {
        // The function get a bool value which represent an Id
        // The function set the current object privateCar attribute value to the
        // given value
        this.privateCar = IsPrivateCar;
    }
    public int GetSpeed()
    {
        // The function get nothing
        // The function return the current object speed attribute value
        return this.speed;
    }

    public void SetSpeed(int newSpeed)
    {
        // The function get an int value which represent speed
        // The function set the current object speed attribute value to the
        // given value
        this.speed = newSpeed;
    }

    public bool Illegal(int maxSpeed)
    {
        // The fucntion get an integer value which reoresent the speed max limitation
        // The fucntion return true if the current car is a private or if this car pass
        // the limitation
        return this.privateCar || this.speed > maxSpeed;
    }
}

class CameraInfo
{
    // A class which represent a traffic camera
    private int city;
    private int maxSpeed;
    private CarInfo[] cars;

    public CameraInfo(int newCity, int newMaxSpeed, CarInfo[] newCars)
    {
        // The function get values to the new object attributes
        // The function set the attributes values to the given values
        this.city = newCity;
        this.maxSpeed = newMaxSpeed;
        this.cars = newCars;
    }

    public int GetCity()
    {
        // The function get nothing
        // The function return the current object's city attribute value
        return this.city;
    }

    public void SetCity(int newCityCode)
    {
        // The function get a integer value which represent a city code value
        // The fucntion set the current city attribute value to the given one
        this.city = newCityCode;
    }

    public int GetMaxSpeed()
    {
        // The function get nothing
        // The function return the current object's maxSpeed attribute value
        return this.maxSpeed;
    }

    public void SetMaxSpeed(int newMaxSpeed)
    {
        // The function get a integer value which represent a new maxSpeed value
        // The fucntion set the current maxSpeed attribute value to the given one
        this.maxSpeed = newMaxSpeed;
    }
    public CarInfo[] GetCars()
    {
        // The function get nothing
        // The function return the current object's cars attribute value
        return this.cars;
    }

    public void SetCars(CarInfo[] newCarArray)
    {
        // The function get a CarInfo array value which represent an array of cars
        // The fucntion set the current cars attribute value to the given value
        this.cars = newCarArray;
    }

    public bool AllGood()
    {
        // The function get nothing
        // The function return true if all the car objects from car's attribute don't have
        // a traffic violation (Illegal function from car info), otherwise false
        for(int i = 0; i < this.cars.Length; i++)
        {
            if (!this.cars[i].Illegal(this.maxSpeed))
            {
                return false;
            }
        }
        return true;
    }
}

internal class Program
{
    public static int LegalCities(CameraInfo[] citiesCamerasArray)
    {
        // The function get a CameraInfo array of 100 CameraInfo objects
        // The function return the amount of cities(CameraInfo objects) where
        // There isn't any traffic violation
        int noViolationTrafficSum = 0;
        for (int i = 0; i < citiesCamerasArray.Length; i++)
        {
            if (citiesCamerasArray[i].AllGood())
            {
                noViolationTrafficSum++;
            }
        }
        return noViolationTrafficSum;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}