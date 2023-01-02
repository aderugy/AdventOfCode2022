using System.Runtime.CompilerServices;
using System.Text;

namespace Day9;

class Program
{
    public static void Main(string[] args)
    {
        //GridPart2 grid = new GridPart2(405, 300, 342, 38);
        GridPart2 grid = new GridPart2(6, 6, 5, 0);
        Console.WriteLine("Generated Grid.");

        string[] lines = File.ReadAllLines("C:\\Users\\Arthur\\Desktop\\Arthur\\aderugy\\AdventOfCode\\Day9\\input");
        
        
        foreach (string line in lines)
        {
            grid.Execute(line);
        }

        Console.WriteLine(grid.CountVisitedCases());
    }
}