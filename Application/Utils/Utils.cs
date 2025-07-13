using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Barn.Utils;

public class Utils
{
    public static string GetCurrentFileDirectory()
    {
        var location = System.Reflection.Assembly.GetCallingAssembly().Location;
        Path.GetDirectoryName(location);
        return location;
    }
}