namespace Day13;

public class Part1
{
    /*
    public static void Main(string[] args)
    {
        // Getting the input from the input file
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");
        
        // Result
        int sum = 0;
        for (int i = 0, pairIndex = 1; i < lines.Length; i+=3, pairIndex++)
        {
            // Retrieving left and right strings of the pair
            string left = lines[i];
            string right = lines[i + 1];
            
            // Result of the comparison
            bool result = Process(left, right) == 1;
            
            // If successful, adding the value of the index
            if (result) sum += pairIndex;
        }

        Console.WriteLine($"Total: {sum}");
    }
    */
    
    public static int Process(string left, string right)
    {
        while (true)
        {
            // Stopping criteria (sign of comparison between int)
            if (left.Equals(right)) return 0;
            if (left.Equals("")) return 1;
            if (right.Equals("")) return -1;

            // Popping next sequence to compare from both left and right
            string x = GetNextSeq(left);
            left = left[x.Length..];
            string y = GetNextSeq(right);
            right = right[y.Length..];

            // Case where both are ints (and different)
            if (IsInt(x) && IsInt(y) && !x.Equals(y))
            {
                int valX = Convert.ToInt32(x);
                int valY = Convert.ToInt32(y);

                return valX < valY ? 1 : -1;
            }

            // If only one is an int, put it back in a list and starting the operation again
            if (IsInt(y) && !IsInt(x))
            {
                left = "[" + left;
                right = $"[{y}]{right}";
                continue;
            }
            if (IsInt(x) && !IsInt(y))
            {
                right = "[" + right;
                left = $"[{x}]{left}";
                continue;
            }

            // Trickiest case: both are lists
            if (x.Equals("[") && y.Equals("["))
            {
                // Keeping track of the depth of the list
                // We want to retrieve the content between the first [ and the last ] of the list
                int stack = 1;

                // Size of the content to retrieve at the end of the operation
                int lenL = 0;
                int lenR = 0;

                foreach (char t in left)
                {
                    if (t == '[') stack++;
                    if (t == ']')
                        if (--stack == 0)
                            break;

                    lenL++;
                }

                stack = 1;
                foreach (char t in right)
                {
                    if (t == '[') stack++;
                    if (t == ']' && --stack == 0) break;

                    lenR++;
                }

                // Comparing the content of the lists we are comparing by recursion
                int result = Process(left[..lenL], right[..lenR]);

                // If the comparison is not null, we keep searching with the rest of the packet
                if (result != 0) return result;
                left = left[(1 + lenL)..];
                right = right[(1 + lenR)..];
            }
        }
    }

    /**
     * Returns the next substring to compare
     */
    private static string GetNextSeq(string s)
    {
        return s.StartsWith("10") ? "10" : s[0].ToString();
    }

    /**
     * Returns true if the string in parameter is an int
     */
    private static bool IsInt(string c)
    {
        
        return "1023456789".Contains(c);
    }
}