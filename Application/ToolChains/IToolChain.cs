namespace ScytheBuild.ToolChains;

using Common;

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