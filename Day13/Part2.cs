using System.Collections;

namespace Day13;

public class Part2
{
    public static void Main(string[] args)
    {
        // Parsing the input before processing it
        string[] packets = Parse();

        int index2 = 0;
        int index6 = 0;
        
        for (int i = 0; i < packets.Length; i++)
        {
            string packet = packets[i];

            // Retrieving the index of the two tracked packets
            switch (packet)
            {
                case "[[2]]": 
                    index2 = i + 1;
                    break;
                case "[[6]]":
                    index6 = i + 1;
                    break;
            }
        }

        // Calculating the answer
        Console.WriteLine($"Solution {index2 * index6}");
    }

    private static string[] Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        // Adding each packet to an Array List
        ArrayList p = new ();
        foreach (string t in lines)
        {
            // Hard checking if the packet is valid
            if (t.Length >= 2)
                p.Add(t);
        }

        // Adding the two packets to track
        p.Add("[[2]]");
        p.Add("[[6]]");

        // Initializing the array to return
        string[] packets = new string[p.Count];
        for (int i = 0; i < p.Count; i++)
        {
            packets[i] = (string)p[i]!;
        }

        // Sorting our array in place
        Sort(packets);
        return packets;
    }

    // Not optimised sorting, but it works
    private static void Sort(string[] packets)
    {
        for (int i = 0; i < packets.Length - 1; i++)
        {
            if (Process(packets[i], packets[i + 1]) != -1) continue;
            (packets[i], packets[i + 1]) = (packets[i + 1], packets[i]);
            i = -1;
        }
    }

    /**
     * Same function as in Part I
     * Comparison between two packets
     * returns 0 when equal
     * returns 1 when right > left
     * returns -1 when left > right
     */
    private static int Process(string left, string right)
    {
        while (true)
        {
            if (left.Equals(right)) return 0;
            if (left.Equals("")) return 1;
            if (right.Equals("")) return -1;

            string x = GetNextSeq(left);
            left = left[x.Length..];
            
            string y = GetNextSeq(right);
            right = right[y.Length..];

            if (IsInt(x) && IsInt(y) && !x.Equals(y))
            {
                int valX = Convert.ToInt32(x);
                int valY = Convert.ToInt32(y);

                return valX < valY ? 1 : -1;
            }

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

            if (x.Equals("[") && y.Equals("["))
            {
                int stack = 1;

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

                int result = Process(left[..lenL], right[..lenR]);

                if (result != 0) return result;
                left = left[(1 + lenL)..];
                right = right[(1 + lenR)..];
            }
        }
    }
    private static string GetNextSeq(string s)
    {
        return s.StartsWith("10") ? "10" : s[0].ToString();
    }

    private static bool IsInt(string c)
    {
        
        return "1023456789".Contains(c);
    }
}
