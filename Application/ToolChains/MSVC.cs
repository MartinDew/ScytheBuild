using Barn.Utils;

namespace Barn.ToolChains;

public class MSVC : ICxxToolchain
{
    public override Dictionary<string, string> Compilers { get; set; } = new Dictionary<string, string>
    {
        { "c", "cl.exe" },
        { "cxx", "cl.exe" }
    };

    public override string ToolchainBinPath { get => WindowsUtils.GetMSVCBinPath(ToolchainPath); }

    public override Strings DefaultIncludes
    {
        get
        {
            Strings includes = new Strings();
            includes.AddRange(WindowsUtils.GetWindowsIncludePath());
            includes.Add(WindowsUtils.GetMSVCIncludePath(ToolchainPath));
            return includes;
        }
    }

    public override Strings DefaultLibraries
    {
        get
        { 
            Strings result = new Strings();
            result.AddRange(WindowsUtils.GetMSVCLibPath(ToolchainPath)
                .Concat(WindowsUtils.GetWindowsLibPath()));
            return result;
        }
    }

    public MSVC() : base(WindowsUtils.GetLatestMSVCPath())
    {
    }
}