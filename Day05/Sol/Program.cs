namespace Sol
{
    class Program
    {
        public static void Main(string[] args)
        {
            Stack<char>[] stacks = new Stack<char>[9];
            InitStacks(stacks);
            
            string[] lines = File.ReadAllLines("/home/arthur.goullet-de-rugy/afs/aderugy/AdventOfCode/Day5/Sol/input");
            foreach (string line in lines)
            {
                Execute(stacks, line);
            }
            
            PrintStacks(stacks);
        }

        private static void Execute(Stack<char>[] stacks, string cmd)
        {
            cmd = cmd.Replace("move ", "")
                .Replace(" from ", " ")
                .Replace(" to ", " ")
                .Replace("\n", "");
            string[] lines = cmd.Split(' ');

            Console.WriteLine($"Amount: '{lines[0]}' From: '{lines[1]}' To: '{lines[2]}'");
            
            MultiMove9001(stacks, Convert.ToInt32(lines[0]), 
                Convert.ToInt32(lines[1]), 
                Convert.ToInt32(lines[2]));
        }

        private static void MultiMove9001(Stack<char>[] stacks, int amount, int from, int to)
        {
            from--;
            to--;

            Stack<char> temporaryStack = new Stack<char>();

            for (int i = 0; i < amount; i++)
            {
                temporaryStack.Push(stacks[from].Pop());
            }

            foreach (char c in temporaryStack)
            {
                stacks[to].Push(c);
            }
        }

        private static void MultiMove(Stack<char>[] stacks, int amount, int from, int to)
        {
            for (int i = 0; i < amount; i++)
            {
                Move(stacks, from, to);
            }
        }

        private static void Move(Stack<char>[] stacks, int from, int to)
        {
            from--;
            to--;

            char c = stacks[from].Pop();
            stacks[to].Push(c);
        }

        private static void PrintStacks(Stack<char>[] stacks)
        {
            for (var index = 0; index < stacks.Length; index++)
            {
                Stack<char> stack = stacks[index];
                string sb = "";
                foreach (char c in stack)
                {
                    sb = $"[{c}]" + sb;
                }

                Console.WriteLine($"{index + 1}: {sb}");
            }

            Console.WriteLine();
        }

        private static void InitStacks(Stack<char>[] stacks)
        {
            for (int i = 0; i < 9; i++)
                stacks[i] = new Stack<char>();
            InitStack(stacks[0], "SMRNWJVT");
            InitStack(stacks[1], "BWDJQPCV");
            InitStack(stacks[2], "BJFHDRP");
            InitStack(stacks[3], "FRPBMND");
            InitStack(stacks[4], "HVRPTB");
            InitStack(stacks[5], "CBPT");
            InitStack(stacks[6], "BJRPL");
            InitStack(stacks[7], "NCSLTZBW");
            InitStack(stacks[8], "LSG");
        }

        private static void InitStack(Stack<char> stack, string content)
        {
            foreach (char c in content)
                stack.Push(c);
        }
    }
}