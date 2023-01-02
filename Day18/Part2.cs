namespace Day18;

public class Part2
{
    private const int Lava = 1;
    private const int Air = 0;
    private const int FreeAir = 2;
    
    public static void Main(string[] args)
    {
        (int, int, int)[] coords = Parse();
        (int xMax, int yMax, int zMax) = GetDimensions(coords);

        int[,,] map = new int[xMax + 1, yMax + 1, zMax + 1];
        FillMap(map, coords);
        ReplaceFreeAir(map);

        Console.WriteLine(CountExposedSurface(map));
    }

    /**
     * Iterates through every cell of every faces of the cube
     * If the encountered block is Air (meaning it has not yet been replaced by free air)
     * Then replace it by Free Air and call the function recursively on all of it's neighbour.
     * That way, it is impossible to miss an Air case hidden from the outside, unlike the previous tested method.
     */
    private static void ReplaceFreeAir(int[,,] map)
    {
        // Checking bottom then top face
        int zMax = map.GetLength(2) - 1;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                // Checking bottom face
                ReplaceFreeAirRecursion(map, x, y, 0);
                // Checking top face
                ReplaceFreeAirRecursion(map, x, y, zMax);
            }
        }

        // Same on Y axis
        int yMax = map.GetLength(1) - 1;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int z = 0; z < map.GetLength(1); z++)
            {
                ReplaceFreeAirRecursion(map, x, 0, z);
                ReplaceFreeAirRecursion(map, x, yMax, z);
            }
        }
        
        // Same on X axis
        int xMax = map.GetLength(0) - 1;
        for (int z = 0; z < map.GetLength(2); z++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                ReplaceFreeAirRecursion(map, 0, y, z);
                ReplaceFreeAirRecursion(map, xMax, y, z);
            }
        }

    }
    
    /**
     * Checks if there is air at the current position.
     * If true, it replaces it by Free Air and recursively calls itself on each of the neighbours.
     */
    private static void ReplaceFreeAirRecursion(int[,,] map, int x, int y, int z)
    {
        // Checking if index is out of bounds, ie. neighbour doesnt exist
        if (x < 0 || y < 0 || z < 0 ||
            x >= map.GetLength(0) ||
            y >= map.GetLength(1) ||
            z >= map.GetLength(2)) return;
        
        // Stopping criteria : no Air at position
        if (map[x, y, z] != Air) return;

        map[x, y, z] = FreeAir;
        
        // Recursive call on the neighbours
        ReplaceFreeAirRecursion(map, x - 1, y, z);
        ReplaceFreeAirRecursion(map, x + 1, y, z);
        ReplaceFreeAirRecursion(map, x, y + 1, z);
        ReplaceFreeAirRecursion(map, x, y - 1, z);
        ReplaceFreeAirRecursion(map, x, y, z + 1);
        ReplaceFreeAirRecursion(map, x, y, z - 1);
    }
    
    /**
     * Returns the total number of faces that can be seen from the outside
     */
    private static int CountExposedSurface(int[,,] map)
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
     * Counting the number of faces of the cube at position (x, y, z) exposed to the outside
     */
    private static int CountFacesExposed(int[,,] map, int x, int y, int z)
    {
        if (map[x, y, z] != Lava) return 0;
        
        int number = 0;

        // Checking each face one by one
        if (IsCaseExposed(map, x + 1, y, z)) number++;
        if (IsCaseExposed(map, x - 1, y, z)) number++;
        if (IsCaseExposed(map, x, y + 1, z)) number++;
        if (IsCaseExposed(map, x, y - 1, z)) number++;
        if (IsCaseExposed(map, x, y, z + 1)) number++;
        if (IsCaseExposed(map, x, y, z - 1)) number++;

        return number;
    }

    /**
     * Checking if the case exposed to the outside or not
     */
    private static bool IsCaseExposed(int[,,] map, int x, int y, int z)
    {
        // Index out of bounds -> false
        if (x < 0 || y < 0 || z < 0 ||
            x >= map.GetLength(0) ||
            y >= map.GetLength(1) ||
            z >= map.GetLength(2)) return true;

        // A case is only exposed to the outside if it's neighbour is free air 
        return map[x, y, z] == FreeAir;
    }
    
    /**
     * Placing lava att the coordinates in the input file
     */
    private static void FillMap(int[,,] map, (int, int, int)[] coords)
    {
        foreach ((int, int, int) coord in coords)
        {
            (int x, int y, int z) = coord;
            map[x, y, z] = Lava;
        }
    }

    /**
     * Returning the bounds of our future matrix
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
     * Parsing the input file into a list of 3-elements int tuple (x,y,z)
     */
    private static (int, int, int)[] Parse()
    {
        // Getting the input from the input file
        //string[] lines = File.ReadAllLines("../../../input_example");
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
