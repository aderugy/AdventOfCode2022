using System.Collections;

namespace Day23;

public class Part1
{
    public static HashSet<(int, int)> Elves = new();

    private const char North = 'N';
    private const char South = 'S';
    private const char West = 'W';
    private const char East = 'E';

    private static ArrayList _directions = new ()
    {
        North,
        South,
        West,
        East
    };

    /**
     * Tackles the problem
     */
    private static void Solve()
    {
        // Initializing the fields
        Parse();
        // Simulating the 10 rounds
        for (int i = 0; i < 10; i++)
        {
            SimulateProposition();
        }

        // Getting the width and height of the square
        int minX = -1;
        int maxX = -1;
        int minY = -1;
        int maxY = -1;

        foreach ((int x, int y) in Elves)
        {
            if (minX == -1 && maxX == -1 && minY == -1 && maxY == -1)
            {
                minX = x;
                maxX = x;
                minY = y;
                maxY = y;
                continue;
            }
            
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }

        // We calculate the number of cases in the square, and substract the number of elves
        int result = (Math.Abs(maxX - minX) + 1) * (Math.Abs(maxY - minY) + 1) - Elves.Count;
        Console.WriteLine($"Result: {result}");
    }

    /**
     * Simulates a round
     */
    private static void SimulateProposition()
    {
        // <Destination coords, Initial coords and updated dir>
        Dictionary<(int, int), (int, int)> toUpdate = new();
        // Banned coordinates (intersection of propositions are stored here)
        HashSet<(int, int)> banned = new();

        foreach ((int x, int y) in Elves)
        {
            // Retrieving the destination coordinates
            (int destX, int destY) = GetDestination(x, y);

            // If banned, we continue
            if (banned.Contains((destX, destY))) continue;
            
            // If there is an intersection, we ban the coordinates
            if (toUpdate.ContainsKey((destX, destY)))
            {
                banned.Add((destX, destY));
                toUpdate.Remove((destX, destY));
                continue;
            }
            
            // Adding the coordinates to the list of elves to update
            toUpdate.Add((destX, destY), (x, y));
        }

        // Updating the elves that have to move
        foreach ((int newX, int newY) in toUpdate.Keys)
        {
            (int x, int y) = toUpdate[(newX, newY)];
            Elves.Remove((x, y));
            Elves.Add((newX, newY));
        }
        
        // Rotating the direction
        UpdateDirections();
    }

    /**
     * The first element is put at the last position
     * I maybe should've used a Queue, would have made more sense I guess
     */
    public static void UpdateDirections()
    {
        char dir = (char) _directions[0]!;
        _directions.RemoveAt(0);
        _directions.Add(dir);
    }

    /**
     * Returns the coordinates of where the elf would move
     */
    public static (int, int) GetDestination(int x, int y)
    {
        // If it has no neighbours, it doesnt move
        if (!HasNeighbours(x, y)) return (x, y);

        // Updating position
        foreach (char dir in _directions)
        {
            if (!CanMove(x, y, dir)) continue;
            (int posX, int posY) = dir switch
            {
                North => (x, y - 1),
                South => (x, y + 1),
                West => (x - 1, y),
                East => (x + 1, y),
                _ => throw new ArgumentException()
            };

            return (posX, posY);
        }

        // Case where there are no directions possible
        return (x, y);
    }
    
    /**
     * Checks if there is an elf nearby (the 8 cases around position (x,y))
     */
    private static bool HasNeighbours(int x, int y)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y) continue;
                if (Elves.Contains((i, j))) return true;
            }
        }

        return false;
    }
    
    /**
     * Checks if an elf can move towards a certain direction
     */
    private static bool CanMove(int x, int y, char direction)
    {
        int posX;
        int posY;

        switch (direction)
        {
            case North:
                posY = y - 1;
                return !(Elves.Contains((x, posY)) ||
                         Elves.Contains((x - 1, posY)) ||
                         Elves.Contains((x + 1, posY)));
            case South:
                posY = y + 1;
                return !(Elves.Contains((x, posY)) ||
                         Elves.Contains((x - 1, posY)) ||
                         Elves.Contains((x + 1, posY)));
            case East:
                posX = x + 1;
                return !(Elves.Contains((posX, y)) ||
                         Elves.Contains((posX, y - 1)) ||
                         Elves.Contains((posX, y + 1)));
           case West:
                posX = x - 1;
                return !(Elves.Contains((posX, y)) ||
                         Elves.Contains((posX, y - 1)) ||
                         Elves.Contains((posX, y + 1)));
           default:
               throw new ArgumentException($"No such direction '{direction}'");
        }
    }

    /**
     * Parsing the input (just retrieving the coordinates)
     */
    public static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#') Elves.Add((x, y));
            }
        }
    }
}