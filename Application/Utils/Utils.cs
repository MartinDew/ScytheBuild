using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ScytheBuild.Utils;

public class Utils
{
    public static string GetCurrentFileDirectory()
    {

        var location = System.Reflection.Assembly.GetCallingAssembly().Location;
        Path.GetDirectoryName(location);
        return location;
    }
}