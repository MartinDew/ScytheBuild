using System.Diagnostics.CodeAnalysis;
using ScytheBuild.Common;
using ScytheBuild.Utils;

namespace ScytheBuild.ProjectDescriptors;

// The target defines a build target. It holds the different configuration flags and should be able to resolve a proper Configuration depending on  
public abstract class Target
{
    string ProjectName { get; set; }
    private Strings DependsOnTarget { get; set; } = new();
    private string Root { get; set; }
    public List<Type> Configurations { get; protected set; }
    public List<Target> SubTargets { get; protected set; }
    
    Dictionary<string, uint> TargetFlags { get; set; } = new();
    
    public Target()
    {
        Configurations = new();
        SubTargets = new();
        Root = Utils.Utils.GetCurrentFileDirectory();
        
    }

    [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
    private void WalkAndFindConfigurations()
    {
        // Walk the directory and find all configuration files destined to this
        // Add them to Configurations
        // Start from root
        
        var files = Directory.GetFiles(Root, "*.conf.cs", SearchOption.AllDirectories);
        var codeLoader = new DynamicCodeLoader();
        foreach (var file in files)
        {
            var confAssembly = codeLoader.LoadFromFile(file);
            var configurations = codeLoader.GetSubclassesInAssembly<Configuration>(confAssembly);
            foreach (var configuration in configurations)
            {
                Configurations.Add(configuration.GetType());
            }
        }
    }
}