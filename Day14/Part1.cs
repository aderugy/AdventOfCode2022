using System.Text;

namespace Day14;

public class Part1
{
    private static (int, int)[][] coordinates;
    private static char[,] map;
    
    // Index of maximum x value
    private static int maxX;
    
    // X coordinate of spawnPoint
    private static int spawnX = 500;
    
    // Index of maximum y value
    private static int maxY;

    // Number of sand units at rest
    private static int count;

    /*
    public static void Main(string[] args)
    {
        Parse();
        DrawMap();
        while (UnitFall(spawnX, 1) != -1) {}
        Console.WriteLine($"Count: {count}");
    }
    */

    /**
     * Simulating the sand falling
     * Returns -1 if the sand fell outside of the map
     */
    public static int UnitFall(int x, int y)
    {
        while (true)
        {
            // Moving one step down if possible
            if (y + 1 > maxY) return -1;

            if (map[x, y + 1] == '.')
            {
                y += 1;
                continue;
            }

            // Moving one step down and to the left if possible
            if (x - 1 < 0) return -1;
            
            if (map[x - 1, y + 1] == '.')
            {
                x -= 1;
                y += 1;
                continue;
            }

            // sMoving one step down and to the right if possible
            if (x + 1 > maxX) return -1;

            if (map[x + 1, y + 1] == '.')
            {
                x += 1;
                y += 1;
                continue;
            }

            map[x, y] = 'o';
            count++;
            return 0;
        }
    }

    /**
     * Prints the map (for debug)
     */
    public static void PrintMap()
    {
        StringBuilder sb = new();
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                sb.Append(map[x, y]).Append(' ');
            }

            sb.Append('\n');
        }

        Console.WriteLine(sb.ToString());
    }

    
    /**
     * Processes all of the instructions in the input file and places the rocks where they should be places.
     * Also places for visual comfort the spawn point of the sand (spawnX, 0)
     */
    public static void DrawMap()
    {
        // Filling map with air
        map = new char[maxX + 1, maxY + 1];
        for (int x = 0; x < maxX + 1; x++)
        {
            for (int y = 0; y < maxY + 1; y++)
            {
                map[x, y] = '.';
            }
        }

        foreach ((int, int)[] instructions in coordinates)
        {
            // Storing last position
            int posX = -1;
            int posY = -1;
            
            for (int j = 0; j < instructions.Length; j++)
            {
                int x = instructions[j].Item1;
                int y = instructions[j].Item2;

                if (posX == -1) posX = x;
                if (posY == -1) posY = y;

                for (int cursorX = Math.Min(x, posX); cursorX <= Math.Max(x, posX); cursorX++)
                {
                    map[cursorX, y] = '#';
                }
                for (int cursorY = Math.Min(y, posY); cursorY <= Math.Max(y, posY); cursorY++)
                {
                    map[x, cursorY] = '#';
                }

                posX = x;
                posY = y;
            }
        }

        map[spawnX, 0] = '+';
    }

    /**
     * Converts the content of the file into a 2D array
     * Each line of the 2D array contains a list of tuples of coordinates as ints
     * It also stores the highest values of X and Y
     * It then retrieves the lowest value of X to all of the X coordinates
     */
    private static void Parse()
    {
        int minX = int.MaxValue;
        
        // Getting input from input file
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");
        int height = lines.Length;
        
        // Initialisation of coordinates field
        coordinates = new (int,int)[height][];

        // Initializing coordinates, minX, maxX, maxY fields
        for (int i = 0; i < height; i++)
        {
            // Splitting the instruction in string tuples of coordinates (format: x,y)
            string[] instruction = lines[i].Split(" -> ");
            (int, int)[] coordinatesList = new (int, int)[instruction.Length];

            for (int j = 0; j < instruction.Length; j++)
            {
                // Splitting tuples of coordinates
                string[] coordinateSplit = instruction[j].Split(',');
                
                // Checking if the tuple is valid
                if (coordinateSplit.Length != 2)
                    throw new ArgumentException($"Length: {instruction[j].Length}");

                // Conversion to int
                int x = Convert.ToInt32(coordinateSplit[0]);
                int y = Convert.ToInt32(coordinateSplit[1]);

                // Updating fields
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;

                coordinatesList[j] = (x, y);
            }

            coordinates[i] = coordinatesList;
        }

        // Retrieving minX to all of the X coordinates
        // That way, we can get the width and height of the matrix in which we will draw the map with the instructions
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < coordinates[y].Length; x++)
            {
                coordinates[y][x].Item1 -= minX;
            }
        }

        maxX -= minX;
        spawnX -= minX;
    }
}