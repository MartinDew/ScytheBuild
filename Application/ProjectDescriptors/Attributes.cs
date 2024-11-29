namespace ScytheBuild.ProjectDescriptors;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ConfigurationType : Attribute
{
    public object[] Flags { get; }

    public ConfigurationType()
    {
        Flags = null;
    }

    public ConfigurationType(params object[] flags)
    {
        Flags = flags;
    }

    public bool HasSameFlags(ConfigurationType other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Flags == null && other.Flags == null)
            return true;

        var groupedFlags = Flags.ToLookup(f => f.GetType(), f => f);
        var groupedOtherFlags = other.Flags.ToLookup(f => f.GetType(), f => f);

        // don't even bother to accumulate the values in case a flag type is not present in the other array
        foreach (var groupedFlag in groupedFlags)
        {
            if (!groupedOtherFlags.Contains(groupedFlag.Key))
                return false;
        }

        // if we're here, we know all of the types in the first array are in the second, so iterate and merge the values
        foreach (var groupedFlag in groupedFlags)
        {
            int accumulate = 0;
            foreach (var flag in groupedFlag)
                accumulate |= (int)flag;

            int otherAccumulate = 0;
            foreach (var flag in groupedOtherFlags[groupedFlag.Key])
                otherAccumulate |= (int)flag;

            if (accumulate != otherAccumulate)
                return false;
        }

        return true;
    }

    public override string ToString()
    {
        if (Flags == null)
            return "null";

        return string.Join(" ", Flags);
    }
}