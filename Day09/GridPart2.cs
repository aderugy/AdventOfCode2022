using System.Text;

namespace Day9;


public class GridPart2
{
    public CasePart2[,] Map;
    private int Width;
    private int Height;

    public CasePart2[] Tail;
    
    public GridPart2(int w, int h, int startX, int startY)
    {
        Map = new CasePart2[w, h];
        Width = w;
        Height = h;
        Tail = new CasePart2[10];

        Tail[0] = new CasePart2(startX, startY, true, false, -1);
        for (int i = 1; i < 10; i++)
        {
            Tail[i] = new CasePart2(startX, startY, false, true, i);
        }
        InitGrid(startX, startY);
    }

    public void Move(char direction, int index)
    {
        try
        {
            CasePart2 toMove = Tail[index];
            
            switch (direction)
            {
                case 'U':
                    toMove.X--;
                    break;

                case 'R':
                    toMove.Y++;
                    break;

                case 'L':
                    toMove.Y--;
                    break;

                default:
                    toMove.X++;
                    break;
            }
        }
        catch (Exception)
        {
            CasePart2 c = GetHead();
            Console.WriteLine($"x: {c.X}, y: {c.Y}");
        }
    }

    public void UpdateCases()
    {
        CasePart2 c = Tail[9];
        Map[c.X, c.Y].Visited = true;
    }

    public int CountVisitedCases()
    {
        int sum = 0;
        foreach (CasePart2 c in Map)
        {
            if (c.Visited) sum++;
        }

        return sum;
    }

    public void UpdateTail()
    {
        for (int i = 1; i < 10; i++)
        {
            Follow(i);
        }
    }

    public void Follow(int index)
    {
        CasePart2 tail = Tail[index];
        CasePart2 toFollow = Tail[index - 1];
        
        int dx = toFollow.X - tail.X;
        int dy = toFollow.Y - tail.Y;

        if (Math.Max(Math.Abs(dx), Math.Abs(dy)) <= 1) return;

        if (dx == 0)
        {
            Move(dy < 0 ? 'L' : 'R', index);
            UpdateCases();
            return;
        }

        if (dy == 0)
        {
            Move(dx < 0 ? 'U' : 'D', index);
            UpdateCases();
            return;
        }

        Move(dy < 0 ? 'L' : 'R', index);
        Move(dx < 0 ? 'U' : 'D', index);
        UpdateCases();
    }

    public CasePart2 GetHead()
    {
        return Tail[0];
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
            Move(direction, 0);
            UpdateTail();
        }
    }

    public void InitGrid(int posX, int posY)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Map[x, y] = new CasePart2(x, y);
            }
        }

        Map[posX, posY] = new CasePart2(posX, posY, true);
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

public class CasePart2
{
    public bool Visited;
    public bool StartingPoint;
    public bool Head;
    public bool Tail;
    public int X;
    public int Y;
    public int Index;

    public CasePart2(int x, int y)
    {
        Visited = false;
        StartingPoint = false;
        Head = false;
        Tail = false;
        X = x;
        Y = y;
        Index = -1;
    }
    
    public CasePart2(int x, int y, bool startingPoint)
    {
        Visited = true;
        StartingPoint = startingPoint;
        Head = false;
        Tail = false;
        X = x;
        Y = y;
        Index = -1;
    }
    
    public CasePart2(int x, int y, bool head, bool tail, int index)
    {
        Visited = false;
        StartingPoint = false;
        Head = head;
        Tail = tail;
        Index = index;
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        if (Head) return "H";
        if (Index != -1) return Index.ToString();
        if (Tail) return "T";
        if (StartingPoint) return "s";
        if (Visited) return "#";
        return ".";
    }
}
