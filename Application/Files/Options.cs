// using System.Collections.Concurrent;
// using System.Reflection;
// using System.Runtime.InteropServices.JavaScript;
// using ScytheBuild.Common;
// using ScytheBuild.ProjectDescriptors;
//
// namespace ScytheBuild.Files;
//
// public static partial class Options
// {
//     
// }
//
// public class OptionDatabase
// {
//     public static ConcurrentDictionary<string, object> Options { get; set; } = new ConcurrentDictionary<string, object>();
//     
//     public static void AddOption<T>(object value)
//     {
//         Options.TryAdd(typeof(T).FullName, value);
//     }
//     
//     public static T GetOption<T>()
//     {
//         Options.TryGetValue(typeof(T).FullName, out object value);
//         return (T)value;
//     }
//
//     public OptionDatabase()
//     {
//         AddAllOptionsAndDefault(typeof(Options));
//     }
//
//     void AddAllOptionsAndDefault(Type type)
//     {
//         foreach (var t in type.GetNestedTypes())
//         {
//             AddAllOptionsAndDefault(t);
//         }
//
//         foreach (var field in type.GetFields())
//         {
//             // if it has Default attribute, add it to the database
//             if (field.GetCustomAttribute<Options.Default>() != null)
//             {
//                 Options.TryAdd(type.FullName, field.GetValue(null));
//             }
//         }
//     }
//     
// #if DEBUG
//     public void PrintDatabase()
//     {
//         foreach (var option in Options)
//         {
//             Console.WriteLine($"{option.Key}: {option.Value}");
//         }
//     }
// #endif
// }