using System.IO;

public abstract class Solver
{
    public abstract int Day
    {
        get;
    }

    public abstract void Parse(TextReader input);

    public abstract string Part1();

    public abstract string Part2();
}

