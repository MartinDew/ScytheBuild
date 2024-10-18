using ScytheBuild.Common;

namespace ScytheBuild;

public interface ITarget
{ 
    string Name { get; set; }
    Strings SourceFiles { get; set; }
    Strings SourceDirectories { get; set; }
    Strings ExcludedDirectories { get; set; }
    Strings ExcludedFiles { get; set; } 
    Strings IncludeDirectories { get; set; }
    Strings Libraries { get; set; }
    Strings LibraryDirectories { get; set; }
    Strings Flags { get; set; }
    // define with their value
    Strings Defines { get; set; }
}