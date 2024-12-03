using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Solvers;

public class Day02 : Solver
{
    private readonly List<int[]> reports = new();

    public override int Day => 2;

    public override void Parse(TextReader input)
    {
        Regex regex = new(@"(?<level>[0-9]+)");
        string? line = input.ReadLine();
        while (line != null)
        {
            MatchCollection matches = regex.Matches(line);
            int[] report = new int[matches.Count];
            for (int index = 0; index < matches.Count; index++)
            {
                report[index] = int.Parse(matches[index].Groups["level"].Value);
            }

            this.reports.Add(report);
            line = input.ReadLine();
        }
    }

    public override string Part1()
    {
        int safeReport = 0;
        foreach (int[] report in this.reports)
        {
            if (this.IsReportSafe(report))
            {
                safeReport++;
            }
        }

        return safeReport.ToString();
    }

    public override string Part2()
    {
        int safeReport = 0;
        foreach (int[] report in this.reports)
        {
            // Try to remove each level...
            for (int index = -1; index < report.Length; index++)
            {
                if (this.IsReportSafe(report, indexToIgnore: index))
                {
                    safeReport++;
                    break;
                }
            }
        }

        return safeReport.ToString();
    }

    private bool IsReportSafe(int[] report, int indexToIgnore = -1)
    {
        int startIndex = indexToIgnore == 0 ? 1 : 0;
        int previous = report[startIndex];
        int sign = 0;
        for (int index = startIndex + 1; index < report.Length; index++)
        {
            if (index == indexToIgnore)
            {
                continue;
            }

            int current = report[index];
            int diff = current - previous;
            if (sign != 0 && sign != Math.Sign(diff))
            {
                return false;
            }

            sign = Math.Sign(diff);

            if (Math.Abs(diff) == 0 || Math.Abs(diff) > 3)
            {
                return false;
            }

            previous = current;
        }

        return true;
    }
}
