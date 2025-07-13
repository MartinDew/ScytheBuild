using Barn.Core;
using Barn.Languages.Cpp;
using Barn.Utils;

namespace Barn.ToolChains;

using CppParser.Models;

public abstract class IToolChain
{
    // contains compiler names used by language ex : c, cxx, rust(not implemented), etc
    public abstract Dictionary<string, string> Compilers { get; set; }

    private string _toolchainPath = "";
    public string ToolchainPath
    {
        get
        {
            return _toolchainPath;
        }
        set
        {
            if (value == "" || !Path.Exists(value))
                throw new Exception($"Toolchain path {value} does not exist");
            _toolchainPath = value;
        }
    }

    public abstract string ToolchainBinPath { get; }
    public abstract Strings DefaultIncludes { get; }
    public abstract Strings DefaultLibraries { get; }

    public virtual string GetCompileCommand(string lang)
    {
        return Path.Combine(ToolchainBinPath, Compilers[lang]);
    }

    protected IToolChain(string toolchainPath)
    {
        if (toolchainPath == "")
            throw new Exception("Toolchain path cannot be empty");
        ToolchainPath = toolchainPath;
    }
}

public abstract class ICxxToolchain : IToolChain
{
    protected ICxxToolchain(string toolchainPath) : base(toolchainPath)
    {
    }

    public abstract string CreateCompileCommand(ModuleUnit unit, CppConfiguration config);
}