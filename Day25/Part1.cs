using System.Text;

namespace Day25;

public class Part1
{
    // <weight, coefficient>
    private static Dictionary<int, int> _values = new();
    
    public static void Main(string[] args)
    {
        Parse();
        Console.WriteLine(Calc());
    }

    /**
     * Parses the _values dictionary into a valid base 5 representation
     */
    private static string Calc()
    {
        // Cleaning our values
        int i = 0;
        while (i <= _values.Keys.Max())
        {
            int current = _values[i];
            int toAdd = 0;
            
            while (current is < -2 or > 2)
            {
                if (current < -2)
                {
                    current += 5;
                    toAdd--;
                }

                if (current <= 2) continue;
                current -= 5;
                toAdd++;
            }
            _values[i] = current;
            i++;
            
            if (toAdd == 0) continue;
            
            if (_values.ContainsKey(i))
                _values[i] += toAdd;
            else
                _values[i] = toAdd;
        }

        // Formatting the output
        string toReturn = "";
        for (int j = 0; j <= _values.Keys.Max(); j++)
        {
            toReturn += ToBase5(_values[j]);
        }
        return Reverse(toReturn);
    }

    /**
     * Returns the reversed string
     */
    private static string Reverse(string s)
    {
        StringBuilder sb = new();

        foreach (char c in s)
        {
            sb.Insert(0, c);
        }

        return sb.ToString();
    }

    /**
     * Converts an int to the corresponding base 5 character
     */
    private static char ToBase5(int i)
    {
        if (i is < -2 or > 2) throw new Exception();

        return i switch
        {
            2 => '2',
            1 => '1',
            0 => '0',
            -1 => '-',
            -2 => '=',
            _ => throw new Exception()
        };
    }

    /**
     * Parsing the input file into a base 5 dictionnary
     */
    private static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        foreach (string line in lines)
        {
            string number = line.Replace("\n", "");
            
            for (int i = 0; i < number.Length; i++)
            {
                char current = number[^(i + 1)];
                int value = current switch
                {
                    '2' => 2,
                    '1' => 1,
                    '0' => 0,
                    '-' => -1,
                    '=' => -2,
                    _ => throw new Exception()
                };

                if (_values.ContainsKey(i)) _values[i] += value;
                else _values.Add(i, value);
            }
        }
    }
}