using System.Collections.Generic;

namespace CppParser;

class BuildLayer
{
    public HashSet<string> Files { get; } = new();
    public HashSet<string> CompletedDependencies { get; } = new();
}

class DependencyGraph
{
    private Dictionary<string, HashSet<string>> _graph = new();
    private Dictionary<string, HashSet<string>> _reverseDependencies = new();

    public void AddNode(string file, IEnumerable<string> dependencies)
    {
        _graph[file] = new HashSet<string>(dependencies);
        foreach (var dep in dependencies)
        {
            if (!_reverseDependencies.ContainsKey(dep))
                _reverseDependencies[dep] = new HashSet<string>();
            _reverseDependencies[dep].Add(file);
        }
    }

    public List<BuildLayer> GetBuildLayers()
    {
        var layers = new List<BuildLayer>();
        var completed = new HashSet<string>();
        
        while (_graph.Count > 0)
        {
            var currentLayer = new BuildLayer();
            
            // Find all nodes with no remaining dependencies
            var availableNodes = _graph.Where(kvp => 
                !kvp.Value.Except(completed).Any()).Select(kvp => kvp.Key).ToList();

            if (!availableNodes.Any())
                throw new Exception("Circular dependency detected");

            currentLayer.Files.UnionWith(availableNodes);
            currentLayer.CompletedDependencies.UnionWith(completed);
            
            // Remove processed nodes from graph
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