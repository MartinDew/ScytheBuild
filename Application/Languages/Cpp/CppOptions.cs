using Barn.Core;
using Barn.Files;

namespace Barn.Languages.Cpp;

public class CppOptions : Options
{
    
    // Compiler optimization level
    public OptimizationLevel Optimization { get; set; } = OptimizationLevel.None;
    
    // C++ standard version (e.g., "c++11", "c++17", "c++20")
    public string Standard { get; set; } = "c++20"; 
    
    // Enable all common warnings (-Wall)
    public bool EnableAllWarnings { get; set; } = true;
    
    // Treat warnings as errors (-Werror)
    public bool WarningsAsErrors { get; set; } = false;
    
    // Enable extra warnings (-Wextra)
    public bool EnableExtraWarnings { get; set; } = false;
    
    // Enable debug symbols (e.g., -g for GCC/Clang)
    public bool DebuggingEnabled { get; set; } = false;
    
    // Path for header files (-I)
    public string IncludePath { get; set; } = "";
    
    // Path for libraries (-L)
    public string LibraryPath { get; set; } = "";
    
    public CppOptions()
    {
    }

    public enum OptimizationLevel
    {
        None, 
        Level1,
        Level2,
        Level3,
        Full,
    }
    
    public enum StandardVersion : uint
    {
        Cpp11 = 0,
        Cpp14,
        Cpp17,
        Cpp20,
        [Default]
        Cpp23,
        Latest = Cpp23,
        Preview
    }
}