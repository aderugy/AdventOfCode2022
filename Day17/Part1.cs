namespace Day17;

public class Part1
{
    public static void Main(string[] args)
    {
        Game game = new (3000);

        while (game.Count <= 2023)
        {
            game.NextMove();
            //game.Print();
            //Console.ReadLine();
        }

        Console.WriteLine(game.GetHeight());
    }
}
