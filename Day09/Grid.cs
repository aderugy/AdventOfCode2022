using System.Text;

namespace Day9;

public class Grid
{
    public Case[,] Map;
        private int Width;
        private int Height;

        public Grid(int w, int h)
        {
            Map = new Case[w, h];
            Width = w;
            Height = h;
        }

        public void Move(char direction, bool moveHead)
        {
            try
            {
                Case toMove = moveHead ? GetHead() : GetTail();
                toMove.Head = !moveHead && toMove.Head;
                toMove.Tail = moveHead && toMove.Tail;

                Case newCase;
                switch (direction)
                {
                    case 'U':
                        newCase = Map[toMove.X - 1, toMove.Y];
                        break;

                    case 'R':
                        newCase = Map[toMove.X, toMove.Y + 1];
                        break;

                    case 'L':
                        newCase = Map[toMove.X, toMove.Y - 1];
                        break;

                    default:
                        newCase = Map[toMove.X + 1, toMove.Y];
                        break;
                }

                newCase.Head = moveHead || newCase.Head;
                newCase.Tail = !moveHead || newCase.Tail;
            }
            catch (Exception)
            {
                Case c = GetHead();
                Console.WriteLine($"x: {c.X}, y: {c.Y}");
            }
        }

        public void UpdateCase()
        {
            GetTail().Visited = true;
        }

        public int CountVisitedCases()
        {
            int sum = 0;
            foreach (Case c in Map)
            {
                if (c.Visited) sum++;
            }

            return sum;
        }

        public void UpdateTail()
        {
            Case head = GetHead();
            Case tail = GetTail();
            int dx = head.X - tail.X;
            int dy = head.Y - tail.Y;

            if (Math.Max(Math.Abs(dx), Math.Abs(dy)) <= 1) return;

            if (dx == 0)
            {
                Move(dy < 0 ? 'L' : 'R', false);
                UpdateCase();
                return;
            }

            if (dy == 0)
            {
                Move(dx < 0 ? 'U' : 'D', false);
                UpdateCase();
                return;
            }

            Move(dy < 0 ? 'L' : 'R', false);
            Move(dx < 0 ? 'U' : 'D', false);
            UpdateCase();
        }

        public Case GetHead()
        {
            foreach (Case c in Map)
            {
                if (c.Head) return c;
            }

            return new Case(-1, -1, false);
        }

        public Case GetTail()
        {
            foreach (Case c in Map)
            {
                if (c.Tail) return c;
            }

            return new Case(-1, -1, false);
        }

        public void Execute(string command)
        {
            if (command.Length < 3) return;
            char direction = command[0];
            int amount = Convert.ToInt32(command.Replace("\n", "").Substring(2));

            for (int i = 0; i < amount; i++)
            {
                Print();
                Console.ReadLine();
                Move(direction, true);
                UpdateTail();
            }
        }

        public void InitGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Map[x, y] = new Case(x, y, false);
                }
            }

            Map[342, 38] = new Case(342, 38, true);
            //Map[5, 0] = new Case(5, 0, true);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    sb.Append(Map[x, y]).Append(' ');
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
}


public class Case
{
    public bool Visited;
    public bool StartingPoint;
    public bool Head;
    public bool Tail;
    public int X;
    public int Y;

    public Case(int x, int y, bool starter)
    {
        Visited = starter;
        StartingPoint = starter;
        Head = starter;
        Tail = starter;
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        if (Head) return "H";
        if (Tail) return "T";
        if (StartingPoint) return "s";
        if (Visited) return "#";
        return ".";
    }
}