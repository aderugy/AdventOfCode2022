
namespace Day21;

public class Part2
{
    private const string Unknown = "UNKNOWN";
    
    public static Dictionary<string, (string, char, string)> Pairs = new();
    public static LinkedList<string> Childrens = new();

    public static void Main(string[] args)
    {
        Part1.Parse();
        Part1.Solve();
        Parse();

        (string first, char ignored, string second) = Pairs["root"];
        long result = Childrens.Contains(first) ? Solve(first, Part1.Known[second]) : Solve(second, Part1.Known[first]);
        Console.WriteLine(result);
    }

    public static long Solve(string parent, long result)
    {
        while (true)
        {
            (string first, char operand, string second) = Pairs[parent];

            if (first.Equals("humn"))
                return SolveEq(result, second, operand);
            if (second.Equals("humn"))
                return SolveEq(result, first, operand);

            if (Childrens.Contains(first))
            {
                parent = first;
                result = SolveEq(result, second, operand);
                continue;
            }
            
            parent = second;
            
            if (operand == '-')
            {
                result = Calc(Convert.ToInt64(Part1.Known[first]), '-', result);
                continue;
            }

            if (operand == '/')
            {
                result = Calc(Convert.ToInt64(Part1.Known[first]), '/', result);
                continue;
            }
            
            result = SolveEq(result, first, operand);
        }
    }

    /**
     * Finds a in equation of type : a + b = r
     */
    public static long SolveEq(long r, string b, char operand)
    {
        long val = Part1.Known[b];

        long result = operand switch
        {
            '+' => Calc(r, '-', val),
            '-' => Calc(r, '+', val),
            '*' => Calc(r, '/', val),
            '/' => Calc(r, '*', val),
            _ => throw new ArgumentException()
        };

        return result;
    }
    
    public static long Calc(long first, char operand, long second)
    {
        long result = operand switch
            {
                '+' => first + second,
                '-' => first - second,
                '*' => first * second,
                '/' => first / second,
                _ => throw new InvalidDataException("No such operand " + operand)
            };

        return result;
    }
    
    public static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        foreach (string line in lines)
        {
            string[] monkey = line.Split(": ");
            if (monkey.Length != 2) throw new InvalidDataException();

            string name = monkey[0];

            if (!monkey[1].Contains(' ')) continue;

            string[] val = monkey[1].Split(' ');
            char operand = val[1][0];
                
            Pairs.Add(name, (val[0], operand, val[2]));
        }
        
        
        string first = (string) Part1.Relying["humn"][0]!;
        Childrens.AddFirst(first);
        while (true)
        {
            string next = (string)Part1.Relying[first][0];
            Childrens.AddLast(next);
            first = next;
            if (next.Equals("root")) break;
        }
    }
}