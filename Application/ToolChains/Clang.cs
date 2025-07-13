using Barn.Languages.Cpp;
using Barn.Utils;
using CppParser.Models;

namespace Barn.ToolChains;

public class Clang : ICxxToolchain
{
    public override Dictionary<string, string> Compilers { get; set; } = new Dictionary<string, string>
    {
        { "c", "clang" },
        { "cxx", "clang++" }
    };
    
    public override string ToolchainBinPath { get; } = "/usr/bin";
    public override Strings DefaultIncludes { get; } 
    public override Strings DefaultLibraries { get; }
    public override string CreateCompileCommand(ModuleUnit unit, CppConfiguration config)
    {
        var command = new List<string>();
        var options = config.Options;
        
        // Set optimization level
        switch (options.Optimization)
        {
            case CppOptions.OptimizationLevel.None:
                command.Add("-O0");
                break;
            case CppOptions.OptimizationLevel.Level1:
                command.Add("-O1");
                break;
            case CppOptions.OptimizationLevel.Level2:
                command.Add("-O2");
                break;
            case CppOptions.OptimizationLevel.Level3:
                command.Add("-O3");
                break;
            case CppOptions.OptimizationLevel.Full:
                command.Add("-Ofast");
                break;
        }

        // Set C++ standard
        if (!string.IsNullOrEmpty(options.Standard))
        {
            command.Add($"-std={options.Standard}");
        }

        // Set warning flags
        if (options.EnableAllWarnings)
        {
            command.Add("-Wall");
        }
        if (options.EnableExtraWarnings)
        {
            command.Add("-Wextra");
        }
        if (options.WarningsAsErrors)
        {
            command.Add("-Werror");
        }

        // Debugging symbols
        if (options.DebuggingEnabled)
        {
            command.Add("-g");
        }
        
        // Includes and Libraries (simplified representation)
        if (!string.IsNullOrEmpty(options.IncludePath))
        {
            command.Add($"-I{options.IncludePath}");
        }
        if (!string.IsNullOrEmpty(options.LibraryPath))
        {
            // For simplicity, we assume -L for path and -l for linking in this example.
            command.Add($"-L{options.LibraryPath}");
        }

        command.Add("-c");
        
        // Add source file and output file
        command.Add(unit.FilePath);
        command.Add("-o");
        
        command.Add();

        return $"{ExecutablePath} {string.Join(" ", command)}";
    }
    }

    public Clang(string toolchainPath) : base(toolchainPath)
    {
        
    }
}