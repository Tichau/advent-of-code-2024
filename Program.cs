using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CommandLine;

Options? options = default;
Parser.Default.ParseArguments<Options>(args)
    .WithParsed(option => options = option);

if (options == null)
{
    Console.WriteLine("Failed to parse arguments.");
    return -1;
}

// Search for solver in assembly.
Dictionary<int, Solver> solverByDay = new();
{
    var assembly = Assembly.GetExecutingAssembly();
    foreach (Type type in assembly.GetTypes())
    {
        if (type.IsAssignableTo(typeof(Solver)))
        {
            if (type.IsAbstract)
            {
                continue;
            }

            Solver? solver = Activator.CreateInstance(type) as Solver;
            Debug.Assert(solver != null);

            Debug.Assert(!solverByDay.ContainsKey(solver.Day));
            solverByDay.Add(solver.Day, solver);
        }
    }
}

// Get day.
int day = options.Day;
if (day == 0)
{
    // Fallback on today's date if no day has been given as arguments.
    day = DateTime.Now.Day;
}

Console.WriteLine($"# Day {day}");

if (!solverByDay.TryGetValue(day, out Solver? todaySolver))
{
    Console.WriteLine($"Error: Can't find solver for day {todaySolver:00}.");
    return -1;
}

string? dataPath = options.DataPath;
if (dataPath == null)
{
    dataPath = $"day{day:00}.txt";
}

Stopwatch stopwatch = new();

// Parse input.
stopwatch.Start();
using (FileStream fileStream = File.OpenRead(Path.Combine("data", dataPath)))
using (var textReader = new StreamReader(fileStream))
{
    todaySolver.Parse(textReader);
}

stopwatch.Stop();
TimeSpan inputDuration = stopwatch.Elapsed;
Console.WriteLine($"Input parsed in {inputDuration.TotalMilliseconds}ms\n");

// Run solver.
stopwatch.Restart();
string result = todaySolver.Part1();
stopwatch.Stop();
TimeSpan part1Duration = stopwatch.Elapsed;
Console.WriteLine($"## Part 1\nresult: {result}\ncompute time: {part1Duration.TotalMilliseconds}ms\n");

stopwatch.Restart();
result = todaySolver.Part2();
stopwatch.Stop();
TimeSpan part2Duration = stopwatch.Elapsed;
Console.WriteLine($"## Part 2\nresult: {result}\ncompute time: {part2Duration.TotalMilliseconds}ms\n");

TimeSpan totalDuration = inputDuration + part1Duration + part2Duration;
Console.WriteLine($"total compute time: {totalDuration.TotalMilliseconds}ms");

return 0;

public class Options
{
    [Value(0, MetaName = "day", HelpText = "the solver day to run")]
    public int Day { get; set; }
    
    [Value(1, MetaName = "data", HelpText = "the solver data to run")]
    public string? DataPath { get; set; }
}
