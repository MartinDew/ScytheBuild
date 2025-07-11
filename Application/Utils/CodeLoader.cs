using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ScytheBuild.Utils;

public class DynamicCodeLoader
{
    private readonly List<string> defaultNamespaces;
    private readonly List<Assembly> referencedAssemblies;

    public DynamicCodeLoader()
    {
        // Common namespaces that might be needed
        defaultNamespaces = new List<string>
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "System.Threading.Tasks"
        };

        // Common assembly references
        referencedAssemblies = new List<Assembly>
        {
            typeof(object).Assembly,
            typeof(Console).Assembly,
            typeof(Enumerable).Assembly
        };
    }

    public Assembly LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Source file not found: {filePath}");

        string sourceCode = File.ReadAllText(filePath);
        return CompileAndLoad(sourceCode);
    }

    public Assembly LoadFromString(string sourceCode)
    {
        return CompileAndLoad(sourceCode);
    }

    private Assembly CompileAndLoad(string sourceCode)
    {
        // Parse the source code
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Create compilation
        string assemblyName = Path.GetRandomFileName();
        var references = referencedAssemblies
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
            .ToList();

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithUsings(defaultNamespaces));

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics
                    .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                string errors = string.Join("\n", failures.Select(diagnostic =>
                    $"{diagnostic.Id}: {diagnostic.GetMessage()}, Location: {diagnostic.Location}"));
                throw new Exception($"Compilation failed:\n{errors}");
            }

            ms.Seek(0, SeekOrigin.Begin);
            return AssemblyLoadContext.Default.LoadFromStream(ms);
        }
    }

    public void AddNamespace(string namespaceName)
    {
        if (!defaultNamespaces.Contains(namespaceName))
            defaultNamespaces.Add(namespaceName);
    }

    public void AddAssemblyReference(Assembly assembly)
    {
        if (!referencedAssemblies.Contains(assembly))
            referencedAssemblies.Add(assembly);
    }

    public T CreateInstance<T>(Assembly assembly, string typeName, params object[] args)
    {
        Type type = assembly.GetType(typeName) 
            ?? throw new ArgumentException($"Type {typeName} not found in assembly");
        
        return (T)Activator.CreateInstance(type, args)
            ?? throw new Exception($"Failed to create instance of {typeName}");
    }

    [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
    public List<Type> GetSubclassesInAssembly<T>(Assembly assembly)
    {
        return assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(T))).ToList();
    }
}