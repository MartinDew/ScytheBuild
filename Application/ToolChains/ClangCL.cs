using ScytheBuild.Common;
using ScytheBuild.Platforms;

namespace ScytheBuild.ToolChains;

public class ClangCL : IToolChain
{
    // Clang-cl depends on msvc includes
    public string MSVCToolchainPath { get; set; } = WindowsUtils.GetLatestMSVCPath();

    public override Dictionary<string, string> Compilers { get; set; } = new Dictionary<string, string>
    {
        { "c", "clang-cl.exe" },
        { "cxx", "clang-cl.exe" }
    };

    public override string ToolchainBinPath
    {
        get { return WindowsUtils.GetLLVMBinPath(ToolchainPath); }
    }

    public override Strings DefaultIncludes
    {
        get
        {
            Strings includes = new Strings();
            includes.AddRange(WindowsUtils.GetWindowsIncludePath());
            includes.Add(WindowsUtils.GetMSVCIncludePath(MSVCToolchainPath));
            return includes;
        }
    }

    public override Strings DefaultLibraries
    {
        get
        {
            Strings result = new Strings();
            result.AddRange(WindowsUtils.GetMSVCLibPath(MSVCToolchainPath)
                .Concat(WindowsUtils.GetWindowsLibPath()));
            return result;
        }
    }

    public ClangCL()
        : base(WindowsUtils.GetLLVMPath())
    {
    }
}