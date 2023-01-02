namespace Day20;
public class Pair
{
    public long Val;
    public int Index;

    public Pair(long val, int index)
    {
        Val = val;
        Index = index;
    }

    public override string ToString()
    {
        return $" {Val} ";
    }
}
