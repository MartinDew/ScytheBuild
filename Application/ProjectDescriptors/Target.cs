namespace ScytheBuild.ProjectDescriptors;

public abstract class Target
{ 
    string ProjectName { get; set; }
    private string Root { get; set; }
    public List<Configuration> Configurations { get; protected set; }
    public List<Target> SubTargets { get; protected set; }
    
    // enum bit masks
    uint Platforms { get; set; }
    uint OutputType { get; set; }
    uint Optimizations { get; set; }
}