using ScytheBuild.Common;

namespace ScytheBuild.Files;

public record Settings
{
    private static Settings _instance = new Settings();
    public static Settings Instance => _instance;

    private Settings(){}

    public record CppSettings
    {
        record Extensions
        {
            private Strings ModuleInterface = new Strings([".ixx", ".cppm"]);
            private Strings ImplementationUnit = new Strings([".cpp", ".cxx"]);
            private Strings HeaderUnits = new Strings([".h", ".hpp", ".hxx"]);
        }
    }
    public record CSettings
    {
        record Extensions
        {
            private Strings ImplementationUnit = new Strings([".c", ".cc"]);
        }
    }

    public CppSettings Cpp { get; set; }
    public CSettings C { get; set; }
}