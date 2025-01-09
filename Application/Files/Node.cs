using System.Text.RegularExpressions;

namespace ScytheBuild.Files;
using System.IO;
// A c++ node to compile. Will establish dependencies based on  
public class Node
{
    public string Name { get; private set; }
    // The file to compile
    public File File { get; private set;  }
    // the files that need to be compiled first 
    public List<Node> Dependencies { get; internal set; }
    // the files that depend on this file
    public List<Node> Dependents { get; internal set; }

    public uint FileType { get; internal set; }
    
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