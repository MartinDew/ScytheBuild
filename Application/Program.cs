// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using ScytheBuild.Platforms;
using ScytheBuild.ToolChains;

Console.WriteLine("Hello, World!");

var toolchain = new ClangCL();

var compileCommand = toolchain.GetCompileCommand("cxx");
// Console.WriteLine(toolchain.GetIncludeString());

// Call compiler on Samples/First/main.cpp
var filePath =  "main.cpp";
 
// Add default includes to INCLUDE environment variable
var includes = string.Join(";", toolchain.DefaultIncludes);
Environment.SetEnvironmentVariable("INCLUDE", includes);
Environment.SetEnvironmentVariable("LIB", string.Join(";", toolchain.DefaultLibraries));

Console.WriteLine(includes);

// print default libs
Console.WriteLine(string.Join(";", toolchain.DefaultLibraries));

var compileArgs = new [] {filePath, "/EHsc", "/Fe:main.exe", "/MDd"};
// Console.WriteLine(toolchain.GetIncludeString());
var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = compileCommand,
        RedirectStandardOutput = false,
        UseShellExecute = false,
        CreateNoWindow = false,
        Arguments = string.Join(" ", compileArgs),
        WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Samples", "First"),
    }
};

process.Start();
process.WaitForExit();
// Console.WriteLine(process.StandardOutput.ReadToEnd());