namespace Day18;

public static class Part1
{
    /*
    public static void Main(string[] args)
    {
        (int, int, int)[] coords = Parse();
        (int xMax, int yMax, int zMax) = GetDimensions(coords);

        int[,,] map = new int[xMax + 1, yMax + 1, zMax + 1];
        FillMap(map, coords);

         Console.WriteLine(CountExposedSurface(map));
    }
    */

    /**
     * Sum of the number of Air neighbour of each Lava cube
     */
    public static int CountExposedSurface(int[,,] map)
    {
        int sum = 0;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int z = 0; z < map.GetLength(2); z++)
                {
                    sum += CountFacesExposed(map, x, y, z);
                }
            }
        }

        return sum;
    }

    /**
     * Counts the number of neighbours of the cube that are air if it is Lava
     */
    private static int CountFacesExposed(int[,,] map, int x, int y, int z)
    {
        // If not lava, return 0
        if (map[x, y, z] != 1) return 0;
        
        int number = 0;

        // Checking each neighbour of the cube if it is Lava
        if (IsCaseExposed(map, x + 1, y, z)) number++;
        if (IsCaseExposed(map, x - 1, y, z)) number++;
        if (IsCaseExposed(map, x, y + 1, z)) number++;
        if (IsCaseExposed(map, x, y - 1, z)) number++;
        if (IsCaseExposed(map, x, y, z + 1)) number++;
        if (IsCaseExposed(map, x, y, z - 1)) number++;

        return number;
    }

    /**
     * Checking if the case at (x, y, z) is Air
     */
    private static bool IsCaseExposed(int[,,] map, int x, int y, int z)
    {
        // We consider that when the position is out of the map, it is air
        if (x < 0 || y < 0 || z < 0 ||
            x >= map.GetLength(0) ||
            y >= map.GetLength(1) ||
            z >= map.GetLength(2)) return true;

        return map[x, y, z] != 1;
    }
    
    /**
     * Places the lava cube at the coordinates given in the input file
     */
    private static void FillMap(int[,,] map, (int, int, int)[] coords)
    {
        foreach ((int, int, int) coord in coords)
        {
            (int x, int y, int z) = coord;
            map[x, y, z] = 1;
        }
    }

    /**
     * Returns the dimensions of our future matrix (max coordinate on each axis)
     */
    private static (int, int, int) GetDimensions((int, int, int)[] coords)
    {
        int xMax = 0;
        int yMax = 0;
        int zMax = 0;

        foreach ((int x, int y, int z) in coords)
        {
            if (x > xMax) xMax = x;
            if (y > yMax) yMax = y;
            if (z > zMax) zMax = z;
        }
        
        return (xMax, yMax, zMax);
    }

    /**
     * Parsing our input data in an array of 3-element int tuple (x, y, z)
     */
    private static (int, int, int)[] Parse()
    {
        // Getting the input from the input file
        // string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        (int, int, int)[] toReturn = new (int, int, int)[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] splitLine = lines[i].Split(',');

            if (splitLine.Length != 3)
                throw new InvalidCastException("Error while parsing");

            int x = Convert.ToInt32(splitLine[0]);
            int y = Convert.ToInt32(splitLine[1]);
            int z = Convert.ToInt32(splitLine[2]);

            toReturn[i] = (x, y, z);
        }

        return toReturn;
    }
}