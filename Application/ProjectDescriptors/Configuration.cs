using ScytheBuild.Common;
using ScytheBuild.Files;
using File = System.IO.File;

namespace ScytheBuild.ProjectDescriptors;

public abstract class Configuration
{
    public Strings Sources { get; set; } = new();
    public Strings ExcludedSources { get; set; } = new();
    public Strings Subdir { get; set; } = new();
    public Strings Includes { get; set; } = new();
    public Strings ModuleIncludeFolders { get; set; }
    public Strings Libraries { get; set; } = new();
    public Strings LibraryDirectories { get; set; } = new();
    // flags 
    public Dictionary<Type, object> Flags;
    public Strings Defines { get; set; } = new();
    // public OptionDatabase Options;
    
    // Direct compiler options. Setting these need to differ depending on your different settings and aren't managed by the build system.
    public Dictionary<string, List<string>> CompilerOptions { get; set; } = new();
    protected Configuration(Target target)
    {
        // Set default values
        CompilerOptions.Add("c", new());
        CompilerOptions.Add("cxx", new());
        CompilerOptions.Add("ld", new());
    }

    internal Strings ResolvedFiles { get; set; } = new();
    
    // Resolve files that should be compiled by the configuration
    void ResolveFiles()
    {
        // Add files to ResolvedFiles

        foreach (var source in Sources)
        {
            // Check if source is a directory
            FileAttributes attr = File.GetAttributes(source);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                // Add all files in the directory
                ResolvedFiles.AddRange(Directory.GetFiles(source, "*.*", SearchOption.AllDirectories));
            }
            else
            {
                // Add the file
                ResolvedFiles.Add(source);
            }
        }
        
        // Remove excluded files
            
        foreach (var excluded in ExcludedSources)
        {
            // Check if source is a directory
            FileAttributes attr = File.GetAttributes(excluded);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                // Remove all files in the directory
                ResolvedFiles.RemoveAll(x => Directory.GetFiles(excluded, "*.*", SearchOption.AllDirectories).Contains(x));
            }
            else
            {
                // Remove the file
                ResolvedFiles.RemoveAll(x => x == excluded);
            }
        }
    }
}

#if DEBUG
public class ExempleConfiguration : Configuration
{
    [ConfigurationType(Optimisation.Debug)]
    ExempleConfiguration(Target target) : base(target)
    {
    }
}
#endif