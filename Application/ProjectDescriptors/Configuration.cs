using ScytheBuild.Common;

namespace ScytheBuild.ProjectDescriptors;

public abstract class Configuration
{
    Strings Sources { get; set; } = new();
    Strings ExcludedFiles { get; set; } = new();
    Strings Subdir { get; set; } = new();
    Strings Includes { get; set; } = new();
    Strings Libraries { get; set; } = new();
    Strings LibraryDirectories { get; set; } = new();
    Strings Flags { get; set; } = new();
    Strings Defines { get; set; } = new();
    
    private Dictionary<string, List<string>> CompilerOptions { get; set; } = new();
    // Dictionary for compiler options

    Configuration(Target target)
    {
        
    }
}