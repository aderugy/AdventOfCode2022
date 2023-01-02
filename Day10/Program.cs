using System.Collections;
using System.Text;

namespace Day10;

class Program
{
    public static void Main(string[] args)
    {
        ArrayList lines = FormatInstructions(File.ReadAllLines("C:\\Users\\Arthur\\Desktop\\Arthur\\aderugy\\AdventOfCode\\Day10\\input"));
        
        char[,] screen = new char[6,40];
        int val = 1;
        
        for (int i = 0; i < 240; i++)
        {
            char[] sprite = GenerateSprite(val);
            screen[i / 40, i%40] = sprite[i%40] == '#' ? '#' : '.';
            val += GetNthValue(lines, i + 1);
        }

        StringBuilder printScreen = new StringBuilder();
        for (int x = 0; x < screen.GetLength(0); x++)
        {
            for (int y = 0; y < screen.GetLength(1); y++)
            {
                printScreen.Append(screen[x, y]).Append(' ');
            }

            printScreen.Append('\n');
        }

        Console.WriteLine(printScreen.ToString());
    }

    public static char[] GenerateSprite(int x)
    {
        char[] toReturn = new char[40];
        for (int i = 0; i < 40; i++)
        {
            toReturn[i] = x >= i - 1 && x <= i + 1 ? '#' : '.';
        }

        return toReturn;
    }
    
    public static int GetNthValue(ArrayList lines, int cycle)
    {
        return Convert.ToInt32(lines[cycle - 1]);
    }

    public static int GetValueAfterNthInstruction(ArrayList lines, int n)
    {
        int sum = 1;

        for (int i = 0; i < n - 1; i++)
        {
            sum += Convert.ToInt32(lines[i]);
        }

        return  n * sum;
    }

    public static ArrayList FormatInstructions(string[] lines)
    {
        ArrayList toReturn = new ArrayList();
        foreach (string l in lines)
        {
            if (l.Length < 4)
                continue;
            
            toReturn.Add("0");
            
            if (!l.Contains("noop"))
                toReturn.Add(l[4..].Replace("\n", "").Trim());
        }

        return toReturn;
    }
}