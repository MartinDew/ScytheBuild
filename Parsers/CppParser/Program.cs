using CppParser;
using System.Diagnostics;

var stopwatch = new Stopwatch();
stopwatch.Start();

if (args.Length == 0)
{
    Console.WriteLine("Please provide a path to a project");
    return;
}

// get path from args
var path = args[0];

if (string.IsNullOrEmpty(path))
{
    Console.WriteLine("Please provide a path to a project");
    return;
}

var parser = new Parser();

var moduleUnits = parser.ParseProject(path);
var buildOrder = parser.GetCompilationOrder();

// foreach (var file in buildOrder)
// {
//     Console.WriteLine(file);   
// }

parser.PrintBuildTree();

stopwatch.Stop();

Console.WriteLine($"Created dep graph in {stopwatch.ElapsedMilliseconds} ms");