using System.Collections;

namespace Day20;

public class Part2
{
    
    public static void Main(string[] args)
    {
        ArrayList input = Parse();
        Console.WriteLine(Solve(input));
    }

    /**
     * Solving the puzzle
     */
    private static long Solve(ArrayList input)
    {
        // Copying our pair array
       ArrayList toModify = new ();
       foreach (object t in input)
       { toModify.Add(t); }

       // Modulo for our circular list
       int mod = input.Count;

       // Mixing 10 times
       for (int i = 0; i < 10; i++)
       {
           // Mixing list
           foreach (Pair p in input) 
           {
               if (p.Val == 0) continue;

               // Same as part 1
               int cur = Part1.IndexOf(toModify, p.Index);
               int next = Part1.GetNewIndex(cur, p.Val, mod);
               toModify.RemoveAt(cur);
               toModify.Insert(next, p);
           }
       }

       // Calculating answer
       int indexOf0 = Part1.IndexOf0(toModify);
       long a = ((Pair)toModify[Part1.Mod(indexOf0 + 1000, mod)]!).Val; 
       long b = ((Pair)toModify[Part1.Mod(indexOf0 + 2000, mod)]!).Val;
       long c = ((Pair)toModify[Part1.Mod(indexOf0 + 3000, mod)]!).Val;
       return a + b + c; 
    }

    /**
     * Parsing the input file to a list of Pairs (value, index)
     */
    private static ArrayList Parse()
    {
        // Getting input from input file
        string[] lines = File.ReadAllLines("../../../input_solution");
        //string[] lines = File.ReadAllLines("../../../input_example");
        ArrayList toReturn = new();
        for (int i = 0; i < lines.Length; i++)
        {
            // Casting to long as we would have an unsigned overflow otherwise
            // Multiplying by the decryption key
            toReturn.Add(new Pair(Convert.ToInt64(lines[i]) * 811_589_153, i));
        }

        return toReturn;
    }
}
