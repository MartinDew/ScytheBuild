namespace ScytheBuild.Common;

public class Strings : List<String>
{

    public Strings()
    { }
    
    public Strings(IEnumerable<string> strings)
    {
        AddRange(strings);
    }
}