using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solvers;

public class Day01 : Solver
{
    private readonly List<int> leftList = new();
    private readonly List<int> rightList = new();

    public override int Day => 1;

    private readonly Approach approach = Approach.Fastest;

    private enum Approach
    {
        Naive,
        Fast,
        Fastest,
    }

    public override void Parse(TextReader input)
    {
        Regex regex = new(@"(?<first>[0-9]+)\s+(?<second>[0-9]+)");
        string? line = input.ReadLine();
        while (line != null)
        {
            Match match = regex.Match(line);
            Debug.Assert(match.Success);
            int first = int.Parse(match.Groups["first"].Value);
            leftList.Add(first);
            int second = int.Parse(match.Groups["second"].Value);
            rightList.Add(second);

            line = input.ReadLine();
        }
    }

    public override string Part1()
    {
        this.leftList.Sort();
        this.rightList.Sort();

        int totalDistance = 0;
        for (int index = 0; index < this.leftList.Count; index++)
        {
            int distance = Math.Abs(this.leftList[index] - this.rightList[index]);
            totalDistance += distance;
        }

        return totalDistance.ToString();
    }

    public override string Part2()
    {
        int totalSimilarity = 0;
        for (int index = 0; index < this.leftList.Count; index++)
        {
            int leftValue = this.leftList[index];

            int rightCount = 0;
            switch (this.approach)
            {
                case Approach.Naive: // ~91ms
                    rightCount = this.rightList.Sum(right => right == leftValue ? 1 : 0);
                    break;

                case Approach.Fast: // ~6ms
                    for (int rightIndex = 0; rightIndex < this.rightList.Count; rightIndex++)
                    {
                        int rightValue = this.rightList[rightIndex];
                        if (rightValue == leftValue)
                        {
                            rightCount++;
                        }
                        else if (rightValue > leftValue)
                        {
                            // Since the right list is sorted, we can stop when right value is greater than target.
                            break;
                        }
                    }
                    break;

                case Approach.Fastest: // ~0.3ms
                    // Since right list is sorted, we can locale value in log(n) using binary search...
                    int validIndex = rightList.BinarySearch(leftValue);
                    if (validIndex >= 0)
                    {
                        // Then look backward and forward to count the occurrences.
                        for (int rightIndex = validIndex - 1; rightIndex >= 0; rightIndex--)
                        {
                            if (this.rightList[rightIndex] == leftValue)
                            {
                                rightCount++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        for (int rightIndex = validIndex; rightIndex < this.rightList.Count; rightIndex++)
                        {
                            if (this.rightList[rightIndex] == leftValue)
                            {
                                rightCount++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }

            totalSimilarity += leftValue * rightCount;
        }

        return totalSimilarity.ToString();
    }
}
