using IntXLib;

namespace Day11;

public class Monkey
{
    public int InspectedCount;
    private Queue<IntX> _inventory;
    public Func<IntX, IntX> Operation;
    public Func<Monkey[], IntX, IntX> Throw;
    public int Mod;

    public Monkey(Func<IntX, IntX> op, Func<Monkey[], IntX, IntX> thr)
    {
        Operation = op;
        InspectedCount = 0;
        _inventory = new Queue<IntX>();
        Throw = thr;
    }

    public void SetMod(int mod)
    {
        Mod = mod;
    }

    public void Inspect(Monkey[] monkeys)
    {
        IntX element = Get();
        IntX worryLevel = Operation(element);
        Throw(monkeys, worryLevel % Mod);
        IncreaseCount();
    }
    
    public void IncreaseCount()
    {
        InspectedCount++;
    }

    public bool IsEmpty()
    {
        return _inventory.Count == 0;
    }

    public Monkey Add(IntX worryLevel)
    {
        _inventory.Enqueue(worryLevel);
        return this;
    }

    public IntX Get()
    {
        return _inventory.Dequeue();
    }

    public override string ToString()
    {
        string tor = "";
        if (!IsEmpty())
            tor += _inventory.Peek() + "  ";
        return tor + $"Inspected: {InspectedCount}";
    }
}