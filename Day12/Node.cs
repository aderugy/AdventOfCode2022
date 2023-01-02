/**
 * The node class will store coordinates and the cost of the path from the start node to itself
 */
public class Node
{
    public int X;
    public int Y;
    public int Cost;

    private const int Infinity = int.MaxValue;

    public Node(int x, int y)
    {
        Cost = Infinity;
        X = x;
        Y = y;
    }

    public override bool Equals(Object? o)
    {
        if (o is null) return false;
        
        Node other = (Node) o;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
