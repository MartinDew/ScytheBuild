using ScytheBuild.Common;

namespace ScytheBuild.ToolChains;

public class Clang : IToolChain
{
    public override Dictionary<string, string> Compilers { get; set; } = new Dictionary<string, string>
    {
        { "c", "clang" },
        { "cxx", "clang++" }
    };
    
    public override string ToolchainBinPath { get; } = "/usr/bin";
    public override Strings DefaultIncludes { get; } 
    public override Strings DefaultLibraries { get; }

    public Clang(string toolchainPath) : base(toolchainPath)
    {
        
    }
}