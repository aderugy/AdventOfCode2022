using IntXLib;

namespace Day11;

class Program
{
    public static void Main(string[] args)
    {
        Monkey[] monkeys = new Monkey[8];
        monkeys[0] = new Monkey(x => x * 11, (mks, worry) =>
            {
                if (worry % 7 == 0)
                    mks[6].Add(worry);
                else
                    mks[7].Add(worry);
                return worry;
            }
        ).Add(66).Add(79);
        
        monkeys[1] = new Monkey(x => x * 17, (mks, worry) =>
            {
                if (worry % 13 == 0)
                    mks[5].Add(worry);
                else
                    mks[2].Add(worry);
                return worry;
            }
        ).Add(84).Add(94).Add(94).Add(81).Add(98).Add(75);
        
        monkeys[2] = new Monkey(x => x + 8, (mks, worry) =>
            {
                if (worry % 5 == 0)
                    mks[4].Add(worry);
                else
                    mks[5].Add(worry);
                return worry;
            }
        ).Add(85).Add(79).Add(59).Add(64).Add(79).Add(95).Add(67);
        
        monkeys[3] = new Monkey(x => x + 3, (mks, worry) =>
            {
                if (worry % 19 == 0)
                    mks[6].Add(worry);
                else
                    mks[0].Add(worry);
                return worry;
            }
        ).Add(70);

        monkeys[4] = new Monkey(x => x + 4, (mks, worry) =>
            {
                if (worry % 2 == 0)
                    mks[0].Add(worry);
                else
                    mks[3].Add(worry);
                return worry;
            }
        ).Add(57).Add(69).Add(78).Add(78);
        
        monkeys[5] = new Monkey(x => x + 7, (mks, worry) =>
            {
                if (worry % 11 == 0)
                    mks[3].Add(worry);
                else
                    mks[4].Add(worry);
                return worry;
            }
        ).Add(65).Add(92).Add(60).Add(74).Add(72);
        
        monkeys[6] = new Monkey(x => x * x, (mks, worry) =>
            {
                if (worry % 17 == 0)
                    mks[1].Add(worry);
                else
                    mks[7].Add(worry);
                return worry;
            }
        ).Add(77).Add(91).Add(91);
        
        monkeys[7] = new Monkey(x => x + 6, (mks, worry) =>
            {
                if (worry % 3 == 0)
                    mks[2].Add(worry);
                else
                    mks[1].Add(worry);
                return worry;
            }
        ).Add(76).Add(58).Add(57).Add(55).Add(67).Add(77).Add(54).Add(99);

        int mod = 3 * 17 * 19 * 11 * 2 * 5 * 13 * 7;

        foreach (Monkey m in monkeys)
        {
            m.SetMod(mod);
        }

        for (int i = 0; i < 10_000; i++)
        {
            Round(monkeys);
        }

        PrintMonkeys(monkeys);
        Console.WriteLine("Result: " + new IntX(108357) * new IntX(108359));
    }

    public static void Round(Monkey[] monkeys)
    {
        foreach (Monkey m in monkeys)
        {
            while (!m.IsEmpty())
                m.Inspect(monkeys);
        }
    }

    public static void PrintMonkeys(Monkey[] mks)
    {
        for (int i = 0; i < mks.Length; i++)
        {
            Console.WriteLine($"{i}: {mks[i]}");
        }

        Console.WriteLine();
    }
}