using System.Text;

namespace Day22;

public class Part1
{
    public static char[,] Map;

    public static int Direction = 0;
    
    public const int Right = 0;
    public const int Down = 1;
    public const int Left = 2;
    public const int Up = 3;

    public const char Wall = '#';
    public const char Void = ' ';
    public const char Walkable = '.';

    public static int X;
    public static int Y;

    public static string Instructions = "";
    public static int InstructionPos;
    
    
    /**
     * Solving the puzzle
     */
    public static void Solve()
    {
        Parse();
        while (Move()) { }
        Console.WriteLine($"Solution: {1000 * (Y + 1) + 4 * (X + 1) + Direction}");
    }

    /**
     * Processes the instruction field
     * Returns the next instruction and increments the InstructionPos (caret)
     */
    public static string GetNextInstruction()
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
    public static bool Move()
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
    public static void Walk(int distance)
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
                posX >= Map.GetLength(0) ||
                posY >= Map.GetLength(1) ||
                Map[posX, posY] == Void)
            {
                // If it is not possible, we end the motion here
                if (!WrapAround()) return;
                // We finish the motion
                continue;
            }
            
            // If we hit wall, the motion stops without updating the coordinates
            if (Map[posX, posY] == Wall) return;

            // If the motion is valid, we update the coordinates
            X = posX;
            Y = posY;
        }
    }

    /**
     * Processes the motion when moving out of the map
     * It moves the player to the case it should be when wrapping around
     * We walk towards the direction opposite to the current 'Direction' until we encounter Void or the bounds of the map
     * If the case before is a wall, then return false
     * Else move the player to this case and return true
     */
    public static bool WrapAround()
    {
        int posX = X;
        int posY = Y;

        do
        {
            int newPosX = posX + Direction switch
            {
                Right => -1,
                Left => 1,
                _ => 0
            };
            
            int newPosY = posY + Direction switch
            {
                Up => 1,
                Down => -1,
                _ => 0
            };
            
            if (newPosX >= 0 && newPosY >= 0 &&
                newPosX < Map.GetLength(0) &&
                newPosY < Map.GetLength(1) &&
                Map[newPosX, newPosY] != Void)
            {
                posX = newPosX;
                posY = newPosY;
                continue;
            }
            
            if (Map[posX, posY] == Wall) return false;
            X = posX;
            Y = posY;
            break;

        } while (true);

        return true;
    }

    /**
     * Rotates the direction of the player by 90/-90 degrees
     */
    public static void Turn(int direction)
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
     * Calculates the starting coordinates of the player
     */
    public static void InitCoords()
    {
        Y = 0;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            if (Map[i, 0] != Walkable) continue;
            X = i;
            break;
        }
    }

    /**
     * Parsing the input files in data
     * -> Fills the Map field. Undefined cases are marked as Void (' '), walls as Walls ('#'), others as Walkable ('.')
     * -> Fills the Instructions field
     * -> Initialization of the coordinates field
     */
    public static void Parse()
    {
        //string[] lines = File.ReadAllLines("../../../input_example");
        string[] lines = File.ReadAllLines("../../../input_solution");

        int indexOfInstructions = -1;
        int maxWidth = 0;
        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(Walkable))
            {
                if (lines[i].Length > maxWidth)
                    maxWidth = lines[i].Length;
            }
            else
            {
                indexOfInstructions = i;
                break;
            }
        }

        if (indexOfInstructions <= 0) throw new InvalidDataException();

        Map = new char[maxWidth, indexOfInstructions];

        for (int i = 0; i < indexOfInstructions; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                if (j < lines[i].Length)
                    Map[j, i] = lines[i][j];
                else
                    Map[j, i] = ' ';
            }
        }

        StringBuilder sb = new();
        for (int i = indexOfInstructions; i < lines.Length; i++)
        {
            sb.Append(lines[i]);
        }

        Instructions = sb.ToString();
        InstructionPos = 0;
        InitCoords();
    }
}