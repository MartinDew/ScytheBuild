using System.Text.RegularExpressions;

namespace Barn.Files;
using System.IO;
// A compilation node that represents a file in the project
public class Node
{
    public string Name { get; private set; }
    // The file to compile
    public File File { get; private set;  }
    // the files that need to be compiled first 
    public List<Node> Dependencies { get; internal set; }
    // the files that depend on this file
    public List<Node> Dependents { get; internal set; }

    internal uint NodeType = 0; 
    
    // Constructor
    public Node(File file)
    {
        File = file;
        Dependencies = new();
        Dependents = new();
        Name = file.AbsPath;
    }
    
    // Add a dependency
    public void AddDependency(Node node)
    {
        Dependencies.Add(node);
        node.Dependents.Add(this);
    }
}