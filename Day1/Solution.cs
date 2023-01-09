namespace Day1;

public class Solution
{
    public static void Main(string[] args)
    {
        SolvePartII();
    }
    
    private static void SolvePartI()
    {
        string[] fileContent = File.ReadAllLines("../../../input");

        int greatestTotalAmountCarried = 0;

        int currentAmount = 0;
        foreach (string line in fileContent)
        {
            try
            {
                currentAmount += Convert.ToInt32(line);
            }
            catch (Exception)
            {
                if (currentAmount > greatestTotalAmountCarried) greatestTotalAmountCarried = currentAmount;
                currentAmount = 0;
            }
        }

        if (currentAmount > greatestTotalAmountCarried) greatestTotalAmountCarried = currentAmount;

        Console.WriteLine($"Max: {greatestTotalAmountCarried}");
    }

    private static void SolvePartII()
    {
        string[] fileContent = File.ReadAllLines("../../../input");

        PriorityQueue<int, int> threeGreatestAmountCarried = new();
        threeGreatestAmountCarried.Enqueue(0, 0);
        threeGreatestAmountCarried.Enqueue(0, 0);
        threeGreatestAmountCarried.Enqueue(0, 0);

        int current = 0;
        foreach (string line in fileContent)
        {
            try
            {
                current += Convert.ToInt32(line);
            }
            catch (Exception)
            {
                if (current > threeGreatestAmountCarried.Peek())
                {
                    threeGreatestAmountCarried.Dequeue();
                    threeGreatestAmountCarried.Enqueue(current, current);
                }

                current = 0;
            }
        }

        int totalValue = 0;
        while (threeGreatestAmountCarried.Count > 0)
            totalValue += threeGreatestAmountCarried.Dequeue();
        Console.WriteLine($"Total value: {totalValue}");
    }
}