namespace Day23;

public class Part2
{
    public static void Main(string[] args)
    {
        Solve();
    }
    
    /**
     * Solving the problem
     */
    private static void Solve()
    {
        Part1.Parse();
        
        // Counting the number if rounds
        int count = 1;
        // While there are moves left, we increase the count by 1 per round
        while (SimulateProposition()) {count++;}

        Console.WriteLine($"Result: {count}");
    }

    /**
     * Same as part 1, only difference is that it returns false when there are no moves left
     */
    private static bool SimulateProposition()
    {
        // <Destination coords, Initial coords and updated dir>
        Dictionary<(int, int), (int, int)> toUpdate = new();
        Dictionary<(int, int), (int, int)> banned = new();

        foreach ((int x, int y) in Part1.Elves)
        {
            (int destX, int destY) = Part1.GetDestination(x, y);
            
            if (destX == x && destY == y) continue;
            
            if (banned.ContainsKey((destX, destY))) continue;
            if (toUpdate.ContainsKey((destX, destY)))
            {
                banned.Add((destX, destY), (x, y));
                toUpdate.Remove((destX, destY));
                continue;
            }
            toUpdate.Add((destX, destY), (x, y));
        }

        if (toUpdate.Count == 0) return false;

        foreach ((int newX, int newY) in toUpdate.Keys)
        {
            (int x, int y) = toUpdate[(newX, newY)];
            Part1.Elves.Remove((x, y));
            Part1.Elves.Add((newX, newY));
        }
        
        Part1.UpdateDirections();
        return true;
    } 
}