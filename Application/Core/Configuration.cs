using Barn.Files;

namespace Barn.Core;

public abstract class Configuration
{
    public string Name;
    internal abstract Options Options { get; }

    public Configuration(string name)
    {
        Name = name;
    }
}
