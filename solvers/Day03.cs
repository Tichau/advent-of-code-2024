using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Solvers;

public class Day03 : Solver
{
    private readonly List<(Operation, int, int)> instructions = [];

    public override int Day => 3;

    public enum Operation
    {
        Mult,
        Do,
        Dont,
    }

    public override void Parse(TextReader input)
    {
        Regex regex = new(@"((?<operation>mul|do|don't)\(((?<left>[0-9]+),(?<right>[0-9]+))?\))");
        string? line = input.ReadLine();
        while (line != null)
        {
            Debug.Assert(line != null);
            foreach (Match match in regex.Matches(line))
            {
                Operation operation = match.Groups["operation"].Value switch
                {
                    "mul" => Operation.Mult,
                    "do" => Operation.Do,
                    "don't" => Operation.Dont,
                    _ => throw new Exception(),
                };

                if (operation == Operation.Mult)
                {
                    this.instructions.Add((operation, int.Parse(match.Groups["left"].Value), int.Parse(match.Groups["right"].Value)));
                }
                else
                {
                    this.instructions.Add((operation, 0, 0));
                }
            }

            line = input.ReadLine();
        }
    }

    public override string Part1()
    {
        int sum = 0;
        foreach ((Operation operation, int leftOperand, int rightOperand) in this.instructions)
        {
            if (operation == Operation.Mult)
            {
                sum += leftOperand * rightOperand;
            }
        }

        return sum.ToString();
    }

    public override string Part2()
    {
        int sum = 0;
        bool ignore = false;
        foreach ((Operation operation, int leftOperand, int rightOperand) in this.instructions)
        {
            if (operation == Operation.Mult && !ignore)
            {
                sum += leftOperand * rightOperand;
            }
            else if (operation == Operation.Do)
            {
                ignore = false;
            }
            else if (operation == Operation.Dont)
            {
                ignore = true;
            }
        }

        return sum.ToString();
    }
}
