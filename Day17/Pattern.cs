namespace Day17;

public class Pattern
{
    private const int SpawnX = 2;
    
    public readonly (int, int)[] Rocks;
    public int X;
    public int Y;

    public Pattern((int, int)[] rocks)
    {
        Rocks = rocks;
        X = SpawnX;
        Y = 0;
    }

    public Pattern Spawn(int y)
    {
        Y = y;
        X = SpawnX;
        return this;
    }
}