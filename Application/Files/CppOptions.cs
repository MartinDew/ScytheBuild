// using ScytheBuild.Common;
// using ScytheBuild.ToolChains;
//
// namespace ScytheBuild.Files;
//
// public static partial class Options
// {
//     [Flags]
//     public enum DefaultTarget
//     {
//         Debug = 0x01,
//         Release = 0x02,
//         All = Debug | Release
//     }
//
//     [System.AttributeUsage(AttributeTargets.Field)]
//     public class Default : Attribute
//     {
//         public DefaultTarget DefaultTarget { get; set; }
//
//         public Default()
//         {
//             DefaultTarget = DefaultTarget.All;
//         }
//
//         public Default(DefaultTarget defaultValue)
//         {
//             DefaultTarget = defaultValue;
//         }
//     }
//
//     // Default values
//     public static DefaultTarget DefaultTargets = DefaultTarget.All;
//     
//     // Settings for C++ files
//     public record CxxSettings
//     {
//         public record ExtensionsSettings
//         {
//             private Strings ModuleInterface = new Strings([".ixx", ".cppm"]);
//             private Strings ImplementationUnit = new Strings([".cpp", ".cxx"]);
//             private Strings HeaderUnits = new Strings([".h", ".hpp", ".hxx"]);
//         }
//         
//         public enum Version
//         {
//             Cpp98,
//             Cpp11,
//             Cpp14,
//             Cpp17,
//             Cpp20,
//             [Default]
//             Cpp23,
//             Latest = Cpp23
//         }
//     }
//
//     // Settings for C files
//     public record CSettings
//     {
//         record Extensions
//         {
//             private Strings ImplementationUnit = new Strings([".c", ".cc"]);
//         }
//         
//         public enum Version
//         {
//             C89,
//             C99,
//             C11,
//             [Default]
//             C18,
//         }
//     }
//
//     // Some general cpp settings
//
//     public enum ConformanceMode : byte
//     {
//         Enabled,
//         [Default]
//         Disabled
//     }
//
//     public enum Exceptions 
//     {
//         [Default]
//         Enabled,
//         Disabled
//     }
//
//     public enum RTTI
//     {
//         [Default]
//         Enabled,
//         Disabled
//     }
//
//     public enum WarningLevels
//     {
//         Level0,
//         Level1,
//         Level2,
//         Level3,
//         [Default]
//         Level4,
//         All
//     }
//
//     public enum StandardLibraryModules 
//     {
//         [Default]
//         Enabled,
//         Disabled
//     }
//     
//     public enum DebugSymbols
//     {
//         [Default]
//         Enabled,
//         Disabled
//     }
// }