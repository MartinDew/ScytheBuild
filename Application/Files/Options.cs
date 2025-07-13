// using System.Collections.Concurrent;
// using System.Reflection;
// using System.Runtime.InteropServices.JavaScript;
// using Barn.Common;
// using Barn.ProjectDescriptors;
//
// namespace Barn.Files;

namespace Barn.Files;

class Option <T>
{
    public string Name { get; }
    public T Value { get; set; }
    public string Description { get; }

    public Option(string name, T value, string description)
    {
        Name = name;
        Value = value;
        Description = description;
    }

    public override string ToString()
    {
        return $"{Name}: {Value} - {Description}";
    }
}


public class Options
{
    // Common options will be stored here
}