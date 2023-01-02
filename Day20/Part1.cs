using System.Collections;

namespace Day20;

public class Part1
{
    /*
    public static void Main(string[] args)
    {
        ArrayList input = Parse();
        Console.WriteLine(Solve(input));
    }
    */
    

    private static long Solve(ArrayList input)
    {
        // Cloning the input so we can safely modify it
        ArrayList toModify = new ();
        foreach (object t in input)
        { toModify.Add(t);}

        // Initializing our modulo (for circular array)
        int mod = input.Count;

        // Mixing the pairs
        foreach (Pair p in input)
        {
            // If there is no offset, go to next pair
            if (p.Val == 0) continue;
            
            // Getting the index of the current pair in the list to modify
            int cur = IndexOf(toModify, p.Index);
            
            // Calculating the new pos of our Pair
            int next = GetNewIndex(cur, p.Val, mod);
            
            // Moving the element to it's new position
            toModify.RemoveAt(cur);
            toModify.Insert(next, p);
        }

        // Determining the solution
        int indexOf0 = IndexOf0(toModify);
        long a = ((Pair)toModify[Mod(indexOf0 + 1000, mod)]!).Val;
        long b = ((Pair)toModify[Mod(indexOf0 + 2000, mod)]!).Val;
        long c = ((Pair)toModify[Mod(indexOf0 + 3000, mod)]!).Val;
        return a + b + c;
    }

    /**
     * Returns the position where the element should be after mixing
     */
    public static int GetNewIndex(long current, long val, int mod)
    {
        long index = current;

        // We consider that if the value is negative, the we will just add its modulo
        // Therefore we don't have to go backwards, which is more work for nothing
        // We use modulo - 1 as everytime it reaches the end, it skips a position
        if (val < 0) val = Mod(val, mod - 1);
        index = Mod(index + val, mod - 1);

        return Convert.ToInt32(index);
    }

    /**
     * Return the position of the 0 element
     */
    public static int IndexOf0(ArrayList array)
    {
        int indexOf0 = -1;
        for (int i = 0; i < array.Count; i++)
        {
            if (((Pair)array[i]!).Val != 0) continue;
            indexOf0 = i;
            break;
        }

        return indexOf0;
    }

    /**
     * Return the position of the element which had the given index in the original ordered list of pairs
     */
    public static int IndexOf(ArrayList toModify, int index)
    {
        for (int j = 0; j < toModify.Count; j++)
        {
            if (((Pair) toModify[j]!).Index == index)
                return j;
        }

        return -1;
    }

    /**
     * Modulo but from arithmetics
     */
    public static int Mod(long n, int m)
    {
        int mod = Convert.ToInt32(n % m);
        return mod < 0 ? m + mod : mod;
    }

    /**
     * Parsing the data from the list
     */
    private static ArrayList Parse()
    {
        string[] lines = File.ReadAllLines("../../../input_solution");
        //string[] lines = File.ReadAllLines("../../../input_example");
        ArrayList toReturn = new();
        for (int i = 0; i < lines.Length; i++)
        {
            toReturn.Add(new Pair(Convert.ToInt32(lines[i]), i));
        }

        return toReturn;
    }
}
