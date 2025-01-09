using System.Collections.Generic;

namespace CppParser;

using Models;

class BuildLayer
{
    public HashSet<ModuleUnit> Units { get; } = new();
    public HashSet<ModuleUnit> CompletedDependencies { get; } = new();
}

class DependencyGraph
{
    private Dictionary<ModuleUnit, HashSet<ModuleUnit>> _graph = new();
    private Dictionary<ModuleUnit, HashSet<ModuleUnit>> _reverseDependencies = new();

    public void AddNode(ModuleUnit unit, IEnumerable<ModuleUnit> dependencies)
    {
        _graph[unit] = new HashSet<ModuleUnit>(dependencies);
        foreach (var dep in dependencies)
        {
            if (!_reverseDependencies.ContainsKey(dep))
                _reverseDependencies[dep] = new HashSet<ModuleUnit>();
            _reverseDependencies[dep].Add(unit);
        }
    }

    public List<BuildLayer> GetBuildLayers()
    {
        var layers = new List<BuildLayer>();
        var completed = new HashSet<ModuleUnit>();
        
        while (_graph.Count > 0)
        {
            var currentLayer = new BuildLayer();
            var availableNodes = _graph.Where(kvp => 
                !kvp.Value.Except(completed).Any()).Select(kvp => kvp.Key).ToList();

            if (!availableNodes.Any())
                throw new Exception("Circular dependency detected");

            currentLayer.Units.UnionWith(availableNodes);
            currentLayer.CompletedDependencies.UnionWith(completed);
            
            foreach (var node in availableNodes)
            {
                _graph.Remove(node);
                completed.Add(node);
            }
            
            layers.Add(currentLayer);
        }
        
        return layers;
    }
}