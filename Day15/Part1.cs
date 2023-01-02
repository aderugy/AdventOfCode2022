namespace Day15;

public class Part1
{
    private static Sensor[] _sensors;

    /**
     * To optimize this, I could have done added the card of the set determined by the union of all the bounds.
     * It would have been more work and kind of overkill.
     * This version of the program calculated the solution in around half a second, which was OK in this case.
     *
     * Edit: just read the subject of part 2 and I already regret not optimizing.
     */

    /*
    public static void Main(string[] args)
    {
        Parse();
        Solve(2000000); 
    }
    */

    public static void Solve(int y)
    {
        // Storing the bounds of each sensor
        (int, int)[] bounds = new (int, int)[_sensors.Length];

        // Getting the lowest and the highest bound that will be our search bounds
        int minX = 0;
        int maxX = 0;
        
        for (int i = 0; i < _sensors.Length; i++)
        {
            // Initialisation of our bonds array
            (int, int) b = _sensors[i].GetBounds(y);

            if (b.Item1 < minX) minX = b.Item1;
            if (b.Item2 > maxX) maxX = b.Item2;

            bounds[i] = b;
        }

        int count = 0;
        for (int x = minX; x <= maxX; x++)
        {
            // If x is in the searched area of one of the sensors, we increment count
            if (CheckX(bounds, x)) count++;
        }

        Console.WriteLine(count);
    }

    public static bool CheckX((int, int)[] bounds, int x)
    {
        foreach ((int, int) bound in bounds)
        {
            if (x >= bound.Item1 && x <= bound.Item2)
                return true;
        }
        
        return false;
    }

    /**
     * Parses the content of the input file into a list of sensors
     * Each sensor has a position x, a position y and a range.
     * With these parameters, we will be able to determine the number of cases that a sensor will be able to reach on a row
     */
    public static void Parse()
    {
        // Getting input from input file
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");
        
        int height = lines.Length;
        _sensors = new Sensor[height];

        for (int i = 0; i < height; i++)
        {
            _sensors[i] = new Sensor(Process(lines[i]));
        }
    }

    /**
     * Parsing function
     * Extracts the coordinates of the sensor and of the beacon for each line
     */
    private static int[] Process(string toProcess)
    {
        string[] stringCoords = toProcess.Replace("\n", "")
            .Replace("Sensor at x=", "")
            .Replace(", y=", ";")
            .Replace(": closest beacon is at x=", ";")
            .Split(";");

        if (stringCoords.Length != 4)
            throw new InvalidDataException("Error while parsing data: " + toProcess);
        
        int[] coordinates = new int[stringCoords.Length];

        for (int i = 0; i < stringCoords.Length; i++)
        {
            coordinates[i] = Convert.ToInt32(stringCoords[i]);
        }

        return coordinates;
    }
}