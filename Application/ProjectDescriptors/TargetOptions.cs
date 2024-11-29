namespace ScytheBuild.ProjectDescriptors;

[Flags]
enum Optimisation : uint
{
    Debug = 1 << 0,
    Release = 1 << 1,
}

[Flags]
enum Platform : uint
{
    Windows = 1 << 0,
    Linux = 1 << 1,
    // MacOS = 1 << 2,
}

[Flags]
public enum BinaryType
{
    Executable = 1,
    StaticLibrary = 2,
    DynamicLibrary = 4,
    // Test = 8,
    // Benchmark = 16,
    // Example = 32,
    All = 63
}