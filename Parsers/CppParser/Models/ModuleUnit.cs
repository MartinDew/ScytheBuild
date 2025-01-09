namespace CppParser.Models;

class ModuleUnit
{
    public string FilePath { get; set; } = "";
    public string ModuleName { get; set; } = "";
    public string? PartitionName { get; set; } = null;
    public ModuleType Type { get; set; }
    public List<string> Dependencies { get; set; } = new();
    public bool HasInterfaceUnit { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not ModuleUnit other)
            return false;

        return FilePath == other.FilePath &&
               ModuleName == other.ModuleName &&
               PartitionName == other.PartitionName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FilePath, ModuleName, PartitionName);
    }
}