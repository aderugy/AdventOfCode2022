using System.Collections;

namespace Day21;

public class Part1
{
    public static Dictionary<string, long> Known = new ();
    public static Dictionary<string, ArrayList> Relying = new();
    public static Dictionary<string, (string, char, string)> Unknown = new();

    /**
    public static void Main(string[] args)
    {
        Parse();
        Console.WriteLine(Relying["humn"].Count);
        Solve();
        Console.WriteLine($"Value of root: {Known["root"]}");
    }
    */

    public static void Solve()
    {
        // Setting up a queue with all known values
        // As we decrypt the values of other monkeys, we will keep adding them to the queue
        Queue<string> knownVal = new();
        foreach (string name in Known.Keys)
        { knownVal.Enqueue(name); }
        
        while (knownVal.Count > 0)
        {
            // Next known value
            string current = knownVal.Dequeue();
            // Checking if any other value relies on it
            if (!Relying.ContainsKey(current)) continue;

            // For each call that rely on a known one
            foreach (string rely in Relying[current])
            {
                // If known, we continue
                if (!Unknown.ContainsKey(rely)) continue;

                // Retrieving the monkey to update
                (string first, char operand, string second) = Unknown[rely];
                
                // If a monkey's call relies on a known value, we replace the name by the value
                if (first.Equals(current)) first = Known[current].ToString();
                if (second.Equals(current)) second = Known[current].ToString();

                // If the monkey relies on two known values, we calculate the new val, and update our collections
                if (CanCalc(first, second))
                {
                    Unknown.Remove(rely);
                    Known.Add(rely, Calc(first, operand, second));
                    knownVal.Enqueue(rely);
                    continue;
                }

                // If not, we update it's value in the 'Unknown' map
                Unknown[rely] = (first, operand, second);
            }
        }
    }

    /**
     * Computes an operation
     */
    public static long Calc(string first, char operand, string second)
    {
        long valFirst = Convert.ToInt64(first);
        long valSecond = Convert.ToInt64(second);
        
        
        long result = operand switch
            {
                '+' => valFirst + valSecond,
                '-' => valFirst - valSecond,
                '*' => valFirst * valSecond,
                '/' => valFirst / valSecond,
                _ => throw new InvalidDataException("No such operand " + operand)
            };

        return result;
    }
    
    /**
     * Checks if it is possible to compute an operation
     * We could make some optimisations here as I convert each string to long twice
     * I'll maybe get back to it (maybe not..)
     */
    private static bool CanCalc(string first, string second)
    {
        try
        {
            long valFirst = Convert.ToInt64(first);
            long valSecond = Convert.ToInt64(second);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /**
     * Initializing our collections
     * If the value is already defined -> 'Known'
     * Else -> 'Unknown'
     * We also keep track of who relies on who
     */
    public static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        foreach (string line in lines)
        {
            string[] monkey = line.Split(": ");
            if (monkey.Length != 2) throw new InvalidDataException();

            string name = monkey[0];

            try
            {
                long val = Convert.ToInt32(monkey[1]);
                Known.Add(name, val);
            }
            catch (Exception e)
            {
                string[] val = monkey[1].Split(' ');
                char operand = val[1][0];
                
                Unknown.Add(name, (val[0], operand, val[2]));

                if (Relying.ContainsKey(val[0]))
                    Relying[val[0]].Add(name);
                else
                    Relying.Add(val[0], new ArrayList {name});
                
                if (Relying.ContainsKey(val[2]))
                    Relying[val[2]].Add(name);
                else
                    Relying.Add(val[2], new ArrayList {name});
            }
        }
    }
}