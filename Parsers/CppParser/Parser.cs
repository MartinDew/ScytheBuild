using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CppParser;

enum ModuleType
{
    InterfaceUnit, // Primary modules
    PartitionInterfaceUnit, // Module partitions
    ImplementationUnit, //  Cpp files
    HeaderUnit, // header files that are imported. We do not need to parse them.
    InternalPartition, // header files that are imported. We do not need to parse them.
    Unknown,
}

class ModuleUnit
{
    public string FilePath { get; set; } = "";
    public string ModuleName { get; set; } = "";
    public string? PartitionName { get; set; } = null;
    public ModuleType Type { get; set; }
    public List<string> Dependencies { get; set; } = new();
    public bool HasInterfaceUnit { get; set; }
}

class Parser
{
    public Dictionary<ModuleType, List<string>> ExtensionsByModuleType { get; set; } = new()
    {
        { ModuleType.HeaderUnit, [".hpp", ".h", ".hxx"] },
        { ModuleType.InterfaceUnit, [".ixx", ".cppm"] },
        { ModuleType.PartitionInterfaceUnit, [".ixx", ".cppm"] },
        { ModuleType.ImplementationUnit, [".cpp", ".cxx"] },
    };

    private readonly Dictionary<string, ModuleUnit> _moduleUnits = new();
    private readonly HashSet<string> _processedFiles = new();
    private string _currentModuleName = "";

    IEnumerable<string> GetFilesInFolder(string folder, List<string> extensions,
        SearchOption searchOption = SearchOption.AllDirectories)
    {
        if (string.IsNullOrEmpty(folder) || extensions == null || !extensions.Any())
            return Enumerable.Empty<string>();

        var uniqueExtensions = extensions
            .Select(ext => ext.TrimStart('.').ToLowerInvariant())
            .Distinct()
            .Select(ext => $".{ext}");

        return Directory
            .EnumerateFiles(folder, "*.*", searchOption)
            .Where(file => uniqueExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()));
    }

    public IReadOnlyDictionary<string, ModuleUnit> ParseProject(string projectPath)
    {
        List<string> me = new ();

        foreach (var extensions in ExtensionsByModuleType.Values)
        {
            me.AddRange(extensions);
        }
        
        var moduleFiles = GetFilesInFolder(projectPath, me.ToList());

        foreach (var file in moduleFiles)
        {
            ParseFile(file);
        }

        // Second pass to identify internal partitions vs partition implementation units
        IdentifyPartitionTypes();

        return _moduleUnits;
    }

    private void ParseFile(string filePath)
    {
        if (_processedFiles.Contains(filePath))
            return;

        _processedFiles.Add(filePath);
        var content = File.ReadAllText(filePath);
        content = RemoveComments(content);

        var moduleUnit = new ModuleUnit
        {
            FilePath = filePath,
            Type = ModuleType.Unknown
        };

        // Look for module declaration first to handle local partition imports
        // var moduleMatch = Regex.Match(content, @"(?:export\s+)?module\s+(?:([a-zA-Z_.]\w*)(?:\s*:\s*([\w\.]+))?)\s*;");
        var moduleMatch = Regex.Match(content, @"(?:export\s+)?module\s+(?:([a-zA-Z_.]+)(?:\s*:\s*([\w\.]+))?)\s*;");
        if (moduleMatch.Success)
        {
            moduleUnit.ModuleName = moduleMatch.Groups[1].Value;
            moduleUnit.PartitionName = moduleMatch.Groups[2].Success ? moduleMatch.Groups[2].Value : null;
            _currentModuleName = moduleUnit.ModuleName; // Store for local partition imports

            // Determine module unit type
            if (content.Contains("export module"))
            {
                moduleUnit.Type = (moduleUnit.PartitionName == null)
                    ? ModuleType.InterfaceUnit
                    : ModuleType.PartitionInterfaceUnit;
            }
            else
            {
                moduleUnit.Type = ModuleType.ImplementationUnit;
            }

            // Parse imports
            ParseImports(content, moduleUnit);

            var key = moduleUnit.PartitionName != null
                ? $"{moduleUnit.ModuleName}:{moduleUnit.PartitionName}"
                : moduleUnit.ModuleName;

            if (!content.Contains("export module"))
            {
                key += "_impl";
            }

            _moduleUnits[key] = moduleUnit;
        }
        // else
        // {
        //     // Check if it's an implementation unit
        //     var implModuleMatch = Regex.Match(content, @"?module\s+(?:([a-zA-Z_]\w*)(?:\s*:\s*([\w\.]+))?)\s*;");
        //     if (implModuleMatch.Success)
        //     {
        //         moduleUnit.ModuleName = implModuleMatch.Groups[0].Value;
        //         moduleUnit.PartitionName = moduleMatch.Groups[1].Success ? moduleMatch.Groups[1].Value : null;
        //         moduleUnit.Type = ModuleType.ImplementationUnit;
        //         ParseImports(content, moduleUnit);
        //
        //         _moduleUnits[moduleUnit.ModuleName] = moduleUnit;
        //     }
        // }
    }

    private void ParseImports(string content, ModuleUnit moduleUnit)
    {
        // Match standard module imports: import modulename;
        var moduleImports = Regex.Matches(content, @"import\s+([a-zA-Z_]\w*)(?:\s*:\s*([\w\.]+))?\s*;");
        foreach (Match match in moduleImports)
        {
            var importedModule = match.Groups[1].Value;
            var partitionName = match.Groups[2].Success ? match.Groups[2].Value : null;

            if (partitionName != null)
            {
                moduleUnit.Dependencies.Add($"{importedModule}:{partitionName}");
            }
            else
            {
                moduleUnit.Dependencies.Add(importedModule);
            }
        }

        // Match local partition imports: import :partitionname;
        var localImports = Regex.Matches(content, @"import\s+:(\w+)\s*;");
        foreach (Match match in localImports)
        {
            var partitionName = match.Groups[1].Value;
            if (_currentModuleName != null)
            {
                moduleUnit.Dependencies.Add($"{_currentModuleName}:{partitionName}");
            }
        }
    }

    private string RemoveComments(string content)
    {
        // Remove single-line comments
        content = Regex.Replace(content, @"//.*$", "", RegexOptions.Multiline);

        // Remove multi-line comments
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

        return content;
    }

    private void IdentifyPartitionTypes()
    {
        List<string> toRemove = new List<string>();
        var toAdd = new List<KeyValuePair<string, ModuleUnit>>();
        foreach (var unitPair in _moduleUnits.Where(u => u.Value.Type == ModuleType.ImplementationUnit))
        {
            var unit = unitPair.Value;
            var partitionInterfaceKey = $"{unit.ModuleName}:{unit.PartitionName}";
            var hasInterface = _moduleUnits.Values.Any(u =>
                u.Type == ModuleType.PartitionInterfaceUnit &&
                u.ModuleName == unit.ModuleName &&
                u.PartitionName == unit.PartitionName);

            if (!hasInterface)
            {
                unit.Type = ModuleType.InternalPartition;
                if (unitPair.Key.Contains("_impl"))
                {
                    toAdd.Add(new KeyValuePair<string, ModuleUnit>(partitionInterfaceKey, unit));
                    // _moduleUnits.Remove();
                    toRemove.Add($"{unit.ModuleName}:{unit.PartitionName}_impl");
                }
            }
        }

        foreach (var r in toAdd)
        {
            _moduleUnits[r.Key] = r.Value;
        }
        
        foreach (var r in toRemove)
        {
            _moduleUnits.Remove(r);
        }
    }

    HashSet<string> visited = new HashSet<string>();
    private Stack<string> circularDependencyStack = new Stack<string>();

    public List<string> GetCompilationOrder()
    {
        var layers = GetBuildLayers();
        var order = new List<string>();
    
        // Flatten the layers into a linear order while maintaining dependencies
        foreach (var layer in layers)
        {
            // Add all files from current layer in any order since they can be built in parallel
            order.AddRange(layer.Files);
        }
    
        return order;
    }

    public void PrintModuleInfo()
    {
        foreach (var unit in _moduleUnits.Values)
        {
            Console.WriteLine($"\nFile: {unit.FilePath}");
            Console.WriteLine($"Type: {unit.Type}");
            Console.WriteLine(
                $"Module: {unit.ModuleName}{(unit.PartitionName != null ? $":{unit.PartitionName}" : "")}");
            if (unit.Dependencies.Any())
            {
                Console.WriteLine("Dependencies:");
                foreach (var dep in unit.Dependencies)
                {
                    Console.WriteLine($"  - {dep}");
                }
            }
        }
    }
    
    public List<BuildLayer> GetBuildLayers()
    {
        var graph = new DependencyGraph();
    
        foreach (var unit in _moduleUnits)
        {
            graph.AddNode(unit.Value.FilePath, unit.Value.Dependencies
                .Where(dep => _moduleUnits.ContainsKey(dep))
                .Select(dep => _moduleUnits[dep].FilePath));
        }
    
        return graph.GetBuildLayers();
    }
}