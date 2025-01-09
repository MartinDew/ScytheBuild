using System.Collections;

namespace ScytheBuild.Files;

// a node tree is a tree data structure that consists of nodes in a parent/child relationship
public class NodeGraph
{
    private List<Node> AllNodes { get; set; }
    private List<Node> Roots { get; set; }
    private Dictionary<string, Node> NodeMap {get; set; }

    // Finds a node 
    public Node FindNode(string nodeName)
    {
        return NodeMap[nodeName];
    }
    
    // Adds a node to the graph
    public virtual void AddNode(Node node)
    {
        NodeMap.Add(node.Name, node);
        AllNodes.Add(node);
        
        if (node.Dependencies.Count == 0)
        {
            Roots.Add(node);
        }
    }

    public virtual  void CreateNode(File nodeFile)
    {
        Node node = new Node(nodeFile);
        AddNode(node);
    }
    
    public virtual void ReorderGraph()
    {
        Roots = new List<Node>();
        foreach (Node node in AllNodes)
        {
            if (node.Dependencies.Count == 0)
            {
                Roots.Add(node);
            }
        }
        
        List<Node> orderedNodes = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        
        foreach (Node root in Roots)
        {
            queue.Enqueue(root);
        }
        
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            orderedNodes.Add(node);
            
            foreach (Node child in node.Dependents)
            {
                child.Dependencies.Remove(node);
                if (child.Dependencies.Count == 0)
                {
                    queue.Enqueue(child);
                }
            }
        }
        
        AllNodes = orderedNodes;
    }
    
    public virtual void CreateGraph()
    {
        Roots.Clear();
        // Parse all nodes and set the dependencies
        foreach (Node node in AllNodes)
        {
            node.Dependents.Clear();
            node.Dependencies.Clear();
        }
    }
}