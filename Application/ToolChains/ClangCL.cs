using CppParser.Models;
using Barn.Core;
using Barn.Languages.Cpp;
using Barn.Utils;

namespace Barn.ToolChains;

public class ClangCL : ICxxToolchain
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

    public override string CreateCompileCommand(ModuleUnit unit, CppConfiguration config)
    {
        if (config == null)
        {
            throw new ArgumentException("Configuration must be of type CppConfiguration", nameof(config));
        }
        
        string command = GetCompileCommand("cxx");
        command += " /c";
        command += " /I" + string.Join(" /I", DefaultIncludes);
        command += " /I" + string.Join(" /I", config.IncludeDirectories);
        command += " /D" + string.Join(" /D", config.Defines);
        
        command += " " + unit.FilePath;
        return command;
    }

    public ClangCL()
        : base(WindowsUtils.GetLLVMPath())
    {
    }
}