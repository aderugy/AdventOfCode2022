using System.Text;

namespace Day24;

public class Part1
{
    private static HashSet<(int, int)>[][] _blizzards;
    private static int _minute;

    private const int Wait = 4;
    private const int Right = 0;
    private const int Left = 1;
    private const int Up = 2;
    private const int Down = 3;

    private static (int, int) _start;
    private static (int, int) _end;

    private static readonly int[] Directions =
    {
        Right,
        Left,
        Up,
        Down,
        Wait
    };

    private static int _width;
    private static int _height;

    public static void Main(string[] args)
    {
        Parse();

        // Starting coordinates
        _start = (1, 0);
        _end = (_width - 2, _height - 1);

        // Solution to Part I
        Console.WriteLine("Part I: " + Solve(_start, _end));
        
        // Solution to Part II
        Console.WriteLine("Part II: " + Solve(_start, _end) + Solve(_end, _start) + Solve(_start, _end));
    }

    /**
     * Solving the problem
     * Returns the minimal path length from start to end
     */
    private static int Solve((int, int) start, (int, int) end)
    {
        // Initializing a queue with the node at pos start as our starting node
        Queue<(int, int)> queue = new();
        queue.Enqueue(start);
        
        // Length of the current path
        int n = 0;
        
        while (queue.Count > 0)
        {
            Queue<(int, int)> next = new();
            // By increasing the time BEFORE checking where we can move, we ensure that we will check the moves at the right time.
            _minute++;

            // We make sure to empty the queue containing possible moves at the instant _minute before going to the next frame
            while (queue.Count > 0)
            {
                // Popping next element
                (int x, int y) = queue.Dequeue();
                
                // Stopping criteria
                if ((x, y) == end)
                {
                    _minute--;
                    return n;
                }

                // Adding the neighbour to the queue of the next frame
                foreach (int direction in Directions)
                {
                    // Checking if it is possible to move
                    (int posX, int posY) = MoveIfPossible(x, y, direction);
                    if (posX == -1 || posY == -1) continue;
                    
                    // HUGE optimization: there are not that many cases possible (worst case scenario: (width - 1)(height - 1) possible moves)
                    // However, multiple moves can have the same neighbours, but those duplicates don't change the outtput
                    // Time complexity goes from (n- 1)^(n - 1)² to (n - 1)²
                    if (next.Contains((posX, posY))) continue;
                    next.Enqueue((posX, posY));
                }
            }

            n++;
            
            // Updating our queue
            queue = next;
        }

        throw new Exception("Path not found.");
    }

    /**
     * Returns new position of player moving towards the direction 'dir' if possible
     * Else: returns (-1, -1)
     */
    private static (int, int) MoveIfPossible(int x, int y, int dir)
    {
        switch (dir)
        {
            case Wait:
                break;
            case Right:
                x++;
                break;
            case Left:
                x--;
                break;
            case Up:
                y--;
                break;
            case Down:
                y++;
                break;
            default:
                throw new Exception();
            
        }

        // Allow moves on start case
        if ((x, y) == _start || (x, y) == _end) return (x, y);
        
        // Moving outside of map / on blizzard
        if (x <= 0 || y <= 0 || x >= _width - 1 || y >= _height - 1 || Contains(x, y)) return (-1, -1);
        return (x, y);
    }

    /**
     * Returns true if there is a blizzard at the given case
     */
    private static bool Contains(int x, int y)
    {
        return Directions.Where(dir => dir != Wait).Any(dir => GetCurrent(dir).Contains((x, y)));
    }

    /**
     * Returns the Set of blizzards that corresponds to the direction dir at the current state of the game
     */
    private static HashSet<(int x, int y)> GetCurrent(int dir)
    {
        int length = _blizzards[dir].Length;
        return _blizzards[dir][_minute % length];
    }

    /**
     * Simulates all the possible movements of the blizzards.
     * It initializes all the sets in _blizzards, because the movements are a cycle
     * Therefore, we can just calculate once the position of the blizzards and use modulos to retrieve the current state of the map
     */
    private static void InitBlizzards()
    {
        // For each direction, IE _blizzards[i] for every i: 0 <= i < 4
        foreach (int dir in Directions)
        {
            // Not a blizzard direction
            if (dir == Wait) continue;
            
            // For every not initialized HashSet
            for (int i = 1; i < _blizzards[dir].Length; i++)
            {
                HashSet<(int, int)> next = new();

                // For every blizzard in the previous HashSet (at time = minute - 1)
                // We simulate the evolution from the last 'state' of the map
                foreach ((int x, int y) in _blizzards[dir][i - 1])
                {
                    int posX = x;
                    int posY = y;
                    
                    // Updating the position of the blizzard
                    switch (dir)
                    {
                        case Right:
                            posX++;
                            break;
                        case Left:
                            posX--;
                            break;
                        case Up:
                            posY--;
                            break;
                        case Down:
                            posY++;
                            break;
                        default:
                            throw new Exception();
                    }

                    // Checking if we reach an edge, and wrap around the map if needed
                    if (posX <= 0) posX = _width - 2;
                    if (posX >= _width - 1) posX = 1;
                    if (posY <= 0) posY = _height - 2;
                    if (posY >= _height - 1) posY = 1;
                    
                    next.Add((posX, posY));
                }

                // Initializing the set at time i
                _blizzards[dir][i] = next;
            }
        }
    }

    /**
     * Parsing the input file
     * We retrieve the coordinates of the blizzards at the initial state and store it in the _blizzards 2D array
     * This 2D array will contain all the possible positions of the blizzards
     */
    private static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        _height = lines.Length;
        _width = lines[0].Length;

        _blizzards = new HashSet<(int, int)>[4][];
        _blizzards[Right] = new HashSet<(int, int)>[_width - 2];
        _blizzards[Left] = new HashSet<(int, int)>[_width - 2];
        _blizzards[Up] = new HashSet<(int, int)>[_height - 2];
        _blizzards[Down] = new HashSet<(int, int)>[_height - 2];

        HashSet<(int, int)> right = new();
        HashSet<(int, int)> left = new();
        HashSet<(int, int)> up = new();
        HashSet<(int, int)> down = new();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                int val = lines[y][x];

                switch (val)
                {
                    case '>':
                        right.Add((x, y));
                        break;
                    case '<':
                        left.Add((x, y));
                        break;
                    case '^':
                        up.Add((x, y));
                        break;
                    case 'v':
                        down.Add((x, y));
                        break;
                }
            }
        }

        _blizzards[Right][0] = right;
        _blizzards[Left][0] = left;
        _blizzards[Up][0] = up;
        _blizzards[Down][0] = down;
        
        InitBlizzards();
    }
}