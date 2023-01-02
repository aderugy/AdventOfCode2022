using System.Text;

namespace Day22;

public class Part2
{
    public static char[][,] Map;

    public static Dictionary<(int, int), (int, int, Func<int, int, (int, int)>)> Rules = new ();
    public static Dictionary<int, (int, int)> AbsolutePos = new ();

    public static int Direction;
    public static int CurrentFace = 1;

    public static int Width;
    
    public const int Right = 0;
    public const int Down = 1;
    public const int Left = 2;
    public const int Up = 3;

    public const char Wall = '#';
    public const char Void = ' ';

    public static int X;
    public static int Y;

    public static string Instructions;
    public static int InstructionPos;

    public static void Main(string[] args)
    {
        Solve();
    }
    
    /**
     * Solving the puzzle
     */
    private static void Solve()
    {
        Parse();
        while (Move()) { }

        (int x, int y) = AbsolutePos[CurrentFace];
        Console.WriteLine(1000 * (Y + 1 + y * Width) + 4 * (X + 1 + x * Width) + CurrentFace);
    }

    /**
     * Processes the instruction field
     * Returns the next instruction and increments the InstructionPos (caret)
     */
    private static string GetNextInstruction()
    {
        string instruction = "";

        if (InstructionPos >= Instructions.Length)
            return instruction;

        char current;
        do
        {
            current = Instructions[InstructionPos];
            if (!char.IsDigit(current) && instruction.Length != 0) break;
            instruction += current;
            InstructionPos++;
        } while (char.IsDigit(current) && InstructionPos < Instructions.Length);

        return instruction;
    }

    
    
    /**
     * Processes the instruction, moves the player
     * Returns false if the simulations is over
     */
    private static bool Move()
    {
        // Retrieving next instruction
        string instruction = GetNextInstruction();
        // If empty (end of instructions) return false (end of simulation)
        if (instruction.Length == 0) return false;
        
        // Processing instruction in function of the direction
        switch (instruction[0])
        {
            case 'R':
                Turn(Right);
                break;
            case 'L':
                Turn(Left);
                break;
            default:
                Walk(Convert.ToInt32(instruction));
                break;
        }

        return true;
    }

    /**
     * Makes the Player move 'distance' cases in the direction 'Direction'
     */
    private static void Walk(int distance)
    {
        // We will move case by case
        for (int i = 0; i < distance; i++)
        {
            // Calculate the next position
            int posX = X + Direction switch
            {
                Right => 1,
                Left => -1,
                _ => 0
            };
            int posY = Y + Direction switch
            {
                Up => -1,
                Down => 1,
                _ => 0
            };

            // If we walk out of the map, we wrap around it
            if (posX < 0 || posY < 0 ||
                posX >= Map[CurrentFace - 1].GetLength(0) ||
                posY >= Map[CurrentFace - 1].GetLength(1) ||
                Map[CurrentFace - 1][posX, posY] == Void)
            {
                // If it is not possible, we end the motion here
                if (!WrapAround()) return;
                // We finish the motion
                continue;
            }
            
            // If we hit wall, the motion stops without updating the coordinates
            if (Map[CurrentFace - 1][posX, posY] == Wall) return;

            // If the motion is valid, we update the coordinates
            X = posX;
            Y = posY;
            Map[CurrentFace - 1][posX, posY] = Direction switch
            {
                Right => '>',
                Left => '<',
                Down => 'v',
                Up => '^',
                _ => throw new Exception()
            };
        }
    }

    /**
     * Processes the motion when moving out of the map
     * It moves the player to the case it should be when wrapping around
     * It uses the data in the Rules field to move the caret to the next position
     */
    private static bool WrapAround()
    {
        // Retrieving the new data
        (int, int, Func<int, int, (int, int)>) next = Rules[(CurrentFace, Direction)];
        int newFace = next.Item1;
        int newDirection = next.Item2;
        (int posX, int posY) = next.Item3(X, Y);

        // If we would land on a wall, cancel
        if (Map[newFace - 1][posX, posY] == Wall) return false;
        
        // Else updating data
        CurrentFace = newFace;
        Direction = newDirection;
        X = posX;
        Y = posY;
        
        return true;
    }

    /**
     * Rotates the direction of the player by 90/-90 degrees
     */
    private static void Turn(int direction)
    {
        switch (direction)
        {
            case Right:
                Direction = (Direction + 1) % 4;
                break;
            case Left:
                Direction--;
                if (Direction < 0)
                    Direction = 3;
                break;
            default:
                throw new ArgumentException();
        }

        if (Direction is < 0 or >= 4)
            throw new ApplicationException();
    }

    /**
     * Parsing the input files in data
     * -> Fills the Map field. Undefined cases are marked as Void (' '), walls as Walls ('#'), others as Walkable ('.')
     * -> Splits the map between 6 independent faces
     * -> Fills the Instructions field
     * -> Initialization of the coordinates field
     * -> Stores the coordinates of each face in the AbsolutePos field
     */
    private static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        AbsolutePos = new Dictionary<int, (int, int)>();
        
        // Calculating the position of the end of the map and the width of a face
        int indexOfInstructions = -1;
        Width = lines[0].Length;

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains('.'))
            {
                indexOfInstructions = i;
                break;
            }

            int count = lines[i].Count(c => !c.Equals(Void));

            if (count < Width) Width = count;
        }

        if (indexOfInstructions <= 0) throw new InvalidDataException();

        // Initialization of the map
        Map = new char[6][,];

        int faceCount = 0;

        // Splitting the map between 6 faces
        for (int absolutePosY = 0; absolutePosY < 4; absolutePosY++)
        {
            if (faceCount == 6) break;
            for (int absolutePosX = 0; absolutePosX < 4; absolutePosX++)
            {
                if (faceCount == 6) break;
                
                int startX = absolutePosX * Width;
                int startY = absolutePosY * Width;
                
                if (startY >= lines.Length) continue;
                if (startX >= lines[startY].Length) continue;
                
                if (lines[startY][startX] == Void) continue;

                AbsolutePos.Add(faceCount + 1, (absolutePosX, absolutePosY));
                
                char[,] face = new char[Width, Width];
                
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        face[x, y] = lines[startY + y][startX + x];
                    }
                }

                Map[faceCount] = face;
                faceCount++;
            }
        }
        
        // Processing the data about the instructions
        StringBuilder sb = new();
        for (int i = indexOfInstructions; i < lines.Length; i++)
        {
            sb.Append(lines[i]);
        }
        Instructions = sb.ToString();
        
        // Initializing our data
        InstructionPos = 0;
        InitRules(Width - 1);
        CurrentFace = 1;
        X = 0;
        Y = 0;
        Direction = Right;
    }

    /**
     * In this function, I calculated the output for every single motion scenario
     * Pure hardcode
     */
    private static void InitRules(int max)
    {
        (int, int) XAndMax(int x, int y) => (x, max);
        (int, int) MaxAndY(int x, int y) => (max, y);
        (int, int) XAnd0(int x, int y) => (x, 0);
        (int, int) ZeroAndY(int x, int y) => (0, y);
        
        Rules = new Dictionary<(int, int), (int, int, Func<int, int, (int, int)>)>
        {
            // TOP FACE
            { (5, Up), (3, Up, XAndMax) }, // CORRECT
            { (5, Left), (4, Left, MaxAndY) }, // CORRECT
            { (5, Down), (6, Left, (x, y) => (max, x)) }, // CORRECT
            { (5, Right), (2, Left, (x, y) => (max, max - y)) }, // HARD TO SAY
            
            // UP FACE
            { (3, Up), (1, Up, XAndMax)}, // CORRECT
            { (3, Left), (4, Down, (x, y) => (y, 0))}, // CORRECT
            { (3, Down), (5, Down, (x, y) => (x, 0))}, // CORRECT
            { (3, Right), (2, Up, (x, y) => (y, max))}, // CORRECT
            
            // BOTTOM FACE
            { (1, Up), (6, Right, (x, y) => (0, x))}, // HARD TO SAY
            { (1, Left), (4, Right, (x, y) => (0, max - y))}, // HARD TO SAY
            { (1, Down), (3, Down, XAnd0)}, // CORRECT
            { (1, Right), (2, Right, (x, y) => (0, y))}, // CORRECT
            
            // RIGHT FACE
            { (2, Right), (5, Left, (x, y) => (max, max - y))}, // CORRECT
            { (2, Up), (6, Up, XAndMax)}, // CORRECT
            { (2, Left), (1, Left, (x, y) => (max, y))}, // CORRECT
            { (2, Down), (3, Left, (x, y) => (max, x))}, // CORRECT
            
            // DOWN FACE
            { (6, Up), (4, Up, (x, y) => (x, max))}, // CORRECT
            { (6, Left), (1, Down, (x, y) => (y, 0))}, // CORRECT
            { (6, Down), (2, Down, (x, y) => (x, 0))}, // CORRECT
            { (6, Right), (5, Up, (x, y) => (y, max))}, // CORRECT
            
            // LEFT FACE
            { (4, Up), (3, Right, (x, y) => (0, x))}, // CORRECT
            { (4, Left), (1, Right, (x, y) => (0, max - y))}, // CORRECT
            { (4, Down), (6, Down, (x, y) => (x, 0))}, // CORRECT
            { (4, Right), (5, Right, ZeroAndY)} // CORRECT
        };
    }
}
