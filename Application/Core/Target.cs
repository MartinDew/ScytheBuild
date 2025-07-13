using System.Diagnostics.CodeAnalysis;
using Barn.Files;
using Barn.Utils;

namespace Barn.Core;

// The target defines a build target. It holds the different configuration flags and should be able to resolve a proper Configuration depending on  
public abstract class Target
{
    string Name { get; set; }
    private Strings DependsOnTarget { get; set; } = new();
    private string Root { get; set; }
    
    public List<Target> SubTargets { get; protected set; }
    
    Dictionary<string, uint> TargetFlags { get; set; } = new();
    
    internal Configuration currentConfiguration;
    
    internal List<Node> Nodes { get; private set; }
    
    public Target(Configuration configuration)
    {
        currentConfiguration = configuration;
    }

    // [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
    // private void WalkAndFindConfigurations()
    // {
    //     // Walk the directory and find all configuration files destined to this
    //     // Add them to Configurations
    //     // Start from root
    //     
    //     var files = Directory.GetFiles(Root, "*.conf.cs", SearchOption.AllDirectories);
    //     var codeLoader = new DynamicCodeLoader();
    //     foreach (var file in files)
    //     {
    //         var confAssembly = codeLoader.LoadFromFile(file);
    //         var configurations = codeLoader.GetSubclassesInAssembly<Configuration>(confAssembly);
    //         foreach (var configuration in configurations)
    //         {
    //             Configurations.Add(configuration.GetType());
    //         }
    //     }
    // }
}