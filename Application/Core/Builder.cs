using Barn.Files;

namespace Barn.Core;

public interface IBuildStep
{
    /// <summary>
    ///     Executes the build step.
    /// </summary>
    /// <param name="target">The target to build.</param>
    void Execute(Target target)
    {
        // foreach (var VARIABLE in target.Node)
        // {
        //     
        // }
    }

    void ExecuteOnNode(Target target, Node node)
    {
    }

    /// <summary>
    ///     Cleans the build step.
    /// </summary>
    /// <param name="target">The target to clean.</param>
    void Clean(Target target);

    /// <summary>
    ///    Cleans the build step on a specific node.
    /// </summary>
    /// <param name="target">The target to clean.</param>
    /// <param name="node">The node to clean.</param>
    /// void CleanOnNode(Target target, Node node);
    void CleanOnNode(Target target, Node node)
    {
    }
}

[Flags]
public enum BuildStepFlags
{
    None = 0,
    PreBuild = 1 << 0,
    Build = 1 << 1,
    PostBuild = 1 << 2
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BuildStepAttribute : Attribute
{
    public BuildStepFlags Flags { get; }

    public BuildStepAttribute(BuildStepFlags flags)
    {
        Flags = flags;
    }
}

[AttributeUsage(AttributeTargets.Class)]
class BuildStepOrdering : Attribute
{
    public int Order { get; }

    public BuildStepOrdering(int order)
    {
        Order = order;
    }
}

public abstract class Builder
{
    private List<IBuildStep> _preBuildSteps = new List<IBuildStep>();
    private List<IBuildStep> _buildSteps = new List<IBuildStep>();
    private List<IBuildStep> _postBuildSteps = new List<IBuildStep>();

    /// <summary>
    ///     Builds the project.
    /// </summary>
    /// <param name="configuration">The configuration to build.</param>
    /// <param name="options">The options to use for building.</param>
    public abstract void Build(Target target);

    /// <summary>
    ///     Cleans the project.
    /// </summary>
    /// <param name="configuration">The configuration to clean.</param>
    public abstract void Clean(Target target);
    
    public void AddBuildStep(IBuildStep step)
    {
        if (step is null)
            throw new ArgumentNullException(nameof(step), "Build step cannot be null.");
        var attributes = step.GetType().GetCustomAttributes(typeof(BuildStepAttribute), false);
        if (attributes.Length == 0)
        {
            throw new InvalidOperationException($"Build step {step.GetType().Name} does not have a BuildStepAttribute.");
        }
        var buildStepAttribute = (BuildStepAttribute)attributes[0];
        switch (buildStepAttribute.Flags)
        {
            case BuildStepFlags.PreBuild:
                _preBuildSteps.Add(step);
                // Sort pre-build steps by order if specified
                var orderingAttribute = step.GetType().GetCustomAttributes(typeof(BuildStepOrdering), false);
                break;
            case BuildStepFlags.Build:
                _buildSteps.Add(step);
                // Sort build steps by order if specified
                orderingAttribute = step.GetType().GetCustomAttributes(typeof(BuildStepOrdering), false);
                break;
            case BuildStepFlags.PostBuild:
                _postBuildSteps.Add(step);
                // Sort post-build steps by order if specified
                orderingAttribute = step.GetType().GetCustomAttributes(typeof(BuildStepOrdering), false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buildStepAttribute.Flags), "Invalid build step flag.");
        }
    }
}