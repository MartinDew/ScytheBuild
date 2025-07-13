using System.Diagnostics;

namespace Barn.Core;

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
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
    
    // With a flag value, matches weather it's there and return weather the two values are equal. If not there, return true.
    public bool HasFlag<T>(T flag) where T : Enum
    {
        if (Flags == null)
            return true;

        foreach (var f in Flags)
        {
            if (f is T t && t.Equals(flag))
                return true;
        }

        return false;
    }

    public override string ToString()
    {
        if (Flags == null)
            return "null";

        return string.Join(" ", Flags);
    }
}

// Marks the default configuration for a project
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class DefaultConfiguration : Attribute
{
    private string DefaultConfigName; 
    public DefaultConfiguration(string value)
    {
        DefaultConfigName = value;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class LangType : Attribute
{
    public Type _internalType;

    // Only applies if langType inherits from Language
    public LangType(Type langType)
    {
        _internalType = langType;
    }

    public bool Matches<T>()
    {
        return _internalType == typeof(T);
    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class DefaultAttribute : Attribute
{
    static uint GetDefaultValue<TEnum>()
    {
        if (!typeof(TEnum).IsEnum)
            throw new ArgumentException("TEnum must be an enumerated type");

        var enumType = typeof(TEnum);
        var fields = enumType.GetFields();
        uint defaultValue = 0;
        
        foreach (var field in fields)
        {
            var attribute = GetCustomAttribute(field, typeof(DefaultAttribute));
            if (attribute != null)
            {
                if (field.IsLiteral)
                {
                    return Convert.ToUInt32(field.GetRawConstantValue());
                }
            }
        }
        return defaultValue;
    }
}