using System.Collections;

namespace Day15;

public class Part2
{
    private static Sensor[] _sensors;
    
    public static void Main(string[] args)
    {
        Parse();
        Solve();
    }

    /**
     * To solve the problem, we will find the unique situation in which 4 squares are overlapping in a way that only one cell of the matrix is uncovered by sensors.
     * To do that, we will calculate the equation of the line of the bounds of the area covered by the sensors (squares rotated by 45°)
     * We will then find the ones with a distance of 2, the coordinate will be the average of those two.
     */
    public static void Solve()
    {
        int height = _sensors.Length;

        // Values of the positive lines of each sensor (even index for lowest, odd for highest)
        int[] positiveLines = new int[2 * height];
        for (int i = 0; i < height; i++)
        {
            var t = _sensors[i].PositiveLines();
            positiveLines[2 * i] = t.Item1;
            positiveLines[2 * i + 1] = t.Item2;
        }
        
        // Values of the negative lines of each sensor (even index for lowest, odd for highest)
        int[] negativeLines = new int[2 * height];
        for (int i = 0; i < height; i++)
        {
            var t = _sensors[i].NegativeLines();
            negativeLines[2 * i] = t.Item1;
            negativeLines[2 * i + 1] = t.Item2;
        }
        
        // Those fields will contain the results
        int pos = int.MaxValue;
        int neg = int.MaxValue;
        
        for (int i = 0; i < 2 * height; i++)
        {
            for (int j = i + 1; j < 2 * height; j++)
            {
                int a = positiveLines[i];
                int b = positiveLines[j];

                // Checking if negative lines are matching
                if (Math.Abs(a - b) == 2)
                    pos = Math.Min(a, b) + 1;

                a = negativeLines[i];
                b = negativeLines[j];

                // Checking if negative lines are matching
                if (Math.Abs(a - b) == 2)
                    neg = Math.Min(a, b) + 1;
            }
        }

        // Calculating the coordinates of the beacon
        long x = (pos + neg) / 2;
        long y = (neg - pos) / 2;

        // Printing solution
        Console.WriteLine($"{4000000*x + y}");
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

