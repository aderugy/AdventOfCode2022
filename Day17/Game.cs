using System.Text;

namespace Day17;

public class Game
{
    private const int Width = 7;
    private const int SpawnHeightGap = 3;
    private const char Down = 'D';
    private const char Right = '>';
    private const char Left = '<';

    private Pattern[] patterns = new Pattern[5];

    private string _jetPattern = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    public char[,] Map;
    public int Count;
    public int JetCount;
    public int SpawningHeight;

    public Game(int height)
    {
        Map = new char[Width, height];
        SpawningHeight = SpawnHeightGap;
        Count = 1;
        JetCount = 0;
        InitPatterns();
        InitMap();
        Spawn();
    }


    public int GetHeight()
    {
        for (int i = 0; i < Map.GetLength(1); i++)
        {
            bool found = false;
            for (int x = 0; x < Width && !found; x++)
            {
                if (Map[x, i] == '#') found = true;
            }

            if (!found) return i;
        }

        return -1;
    }

    private void Clear()
    {
        Pattern current = GetPattern();

        foreach ((int rockX, int rockY) in current.Rocks)
        {
            Map[current.X + rockX, current.Y + rockY] = '.';
        }
    }

    private void Draw()
    {
       Pattern current = GetPattern();

       foreach ((int rockX, int rockY) in current.Rocks)
       { 
           Map[current.X + rockX, current.Y + rockY] = '@';
       } 
    }

    public void NextMove()
    {
        Clear();
        Pattern current = GetPattern();
        
        char direction = GetJetPattern();
        JetCount++;
        if (CheckMove(direction))
        {
            switch (direction)
            {
                case Left:
                    current.X--;
                    break;
                case Right:
                    current.X++;
                    break;
                default:
                    throw new ArgumentException("Unknown direction");
            }
        }

        if (CheckMove(Down))
            current.Y--;
        else
            Attach();
        Draw();
    }
    
    public bool CheckMove(char direction)
    {
        Pattern pattern = GetPattern();
        switch (direction)
        {
            case Left:
            case Right:
                foreach ((int x, int unused) in pattern.Rocks)
                {
                    int newX = pattern.X + x + (direction == Right ? 1 : -1);
                    if (newX is < 0 or >= Width) return false;
                    if (Map[newX, pattern.Y] == '#') return false;
                }

                return true;
            
            case Down:
                foreach ((int x, int y) in pattern.Rocks)
                {
                    int newX = pattern.X + x;
                    int newY = pattern.Y + y - 1;
                    
                    if (newY >= Map.GetLength(1))
                        throw new IndexOutOfRangeException("Map is too short.");

                    if (newY < 0) return false;
                    if (newX >= Width) return false;

                    if (Map[newX, newY] == '#') return false;
                }

                return true;
        }

        throw new ArgumentException($"No such direction '{direction}'");
    }

    private void Spawn()
    {
        Count++;
        GetPattern().Spawn(SpawningHeight);
    }

    /**
     * Fixing the rock to the map
     */
    public void Attach()
    {
        // Getting the current pattern
        Pattern pattern = GetPattern();

        // Storing the highest point of the rock to calculate the new height to span next pattern
        int maxY = 0;
        foreach ((int rockX, int rockY) in pattern.Rocks)
        {
            if (rockY > maxY) maxY = rockY;
            
            // Replacing '@' by '#' for solid rock
            Map[pattern.X + rockX, pattern.Y + rockY] = '#';
        }

        // Updating tracking variables
        SpawningHeight = GetHeight() + SpawnHeightGap;
        Spawn();
    }

    /**
     * Returns the current pattern
     */
    private Pattern GetPattern()
    {
        return patterns[(Count - 1) % patterns.Length];
    }

    /**
     * Returns the next direction of jet gas
     */
    private char GetJetPattern()
    {
        return _jetPattern[JetCount % _jetPattern.Length];
    }

    public void Print()
    {
        StringBuilder sb = new ();
        for (int y = Map.GetLength(1) - 1; y >= 0; y--)
        {
            sb.Append('|');
            for (int x = 0; x < Width; x++)
            {
                sb.Append(Map[x, y]);
            }
            
            sb.Append('|');
            sb.Append('\n');
        }

        sb.Append("+-------+");
        Console.WriteLine(sb.ToString());
    }

    /**
     * Fills the map with '.'
    */
    private void InitMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                Map[x, y] = '.';
            }
        }
    }
    
    /**
     * Initialization of all of the patterns
     */
    private void InitPatterns()
    {
        Pattern minus = new ( new []
        {
            (0, 0),
            (1, 0),
            (2, 0),
            (3, 0)
        });
        
        Pattern cross = new (new []
        {
            (1, 0),
            (0, 1),
            (1, 1),
            (2, 1),
            (1, 2)
        });

        Pattern l = new (new[]
        {
            (2, 0),
            (2, 1),
            (0, 0),
            (1, 0),
            (2, 2)
        });

        Pattern i = new (new[]
        {
            (0, 0),
            (0, 1),
            (0, 2),
            (0, 3)
        });

        Pattern square = new (new[]
        {
            (0, 0),
            (1, 0),
            (0, 1),
            (1, 1),
        });

        patterns[0] = minus;
        patterns[1] = cross;
        patterns[2] = l;
        patterns[3] = i;
        patterns[4] = square;
    }
}
    