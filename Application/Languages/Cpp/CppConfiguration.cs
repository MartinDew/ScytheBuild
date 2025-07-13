using Barn.Core;
using Barn.Utils;

namespace Barn.Languages.Cpp;

public class CppConfiguration : Configuration
{
    public Strings IncludeDirectories { get; set; } = new Strings();
    internal override CppOptions Options { get; } =   new CppOptions();
    
    public Strings Libraries { get; set; } = new();
    public Strings LibraryDirectories { get; set; } = new();
    public OutputType OutputType { get; set; } = OutputType.Executable;
    public Strings Defines { get; set; } = new Strings();
    
    public CppConfiguration(string name) : base(name)
    {
    }
    
    public override string ToString()
    {
        return $"CppConfiguration: {Name}, Includes: {IncludeDirectories.Count}, Options: {Options}";
    }
}