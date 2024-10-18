namespace ScytheBuild.Platforms;

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.Win32;
using Common;

public class WindowsUtils
{
    private static string WindowsKitPath = "";
    private static string VisualStudioPath = "";

    // walk through the registry to find the Windows Kits path
    public static string GetWindowsKitPath()
    {
        if (WindowsKitPath != "")
            return WindowsKitPath;

        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows Kits\Installed Roots"))
        {
            if (key != null)
            {
                WindowsKitPath = key.GetValue("KitsRoot10") as string;
            }
        }

        if (WindowsKitPath == "")
            throw new Exception("Could not find Windows Kit Path");

        return WindowsKitPath;
    }

    private static string FindLatestKitVersion(string kitPath)
    {
        string latestVersion = "";
        Strings versions = new (Directory.GetDirectories(kitPath));
        // Sort from biggest number to smallest
        versions.Sort((a, b) => string.Compare(b, a));
        latestVersion = versions[0];

        return latestVersion;
    }
    
    public static Strings GetWindowsIncludePath()
    {
        var includeDirsParent = FindLatestKitVersion(Path.Combine(GetWindowsKitPath(), "Include"));
        // All subdirs should be an include path
        return new (Directory.GetDirectories(includeDirsParent, "*", SearchOption.AllDirectories));
    }

    public static Strings GetWindowsLibPath()
    {
        var baseFolder = FindLatestKitVersion(Path.Combine(GetWindowsKitPath(), "Lib"));
        
        string[] folders = {"um", "ucrt"};
        
        Strings result = new Strings();
        foreach (var folder in folders)
        {
            result.AddRange(GetArchFolder(Path.Combine(baseFolder, folder)));
        }

        return result;
    }

    public static string GetWindowsSdkPath()
    {
        return FindLatestKitVersion(Path.Combine(GetWindowsKitPath(), "SDK"));
    }

    // always returns the latest version of Visual Studio for now
    public static string GetVisualStudioPath()
    {
        var setupConfig = new SetupConfiguration();
        var query = (ISetupConfiguration2)setupConfig;
        var e = query.EnumAllInstances();

        int fetched;
        var instances = new ISetupInstance[1];
        e.Next(1, instances, out fetched);
        if (fetched > 0)
        {
            VisualStudioPath = instances[0].GetInstallationPath();
        }

        return VisualStudioPath;
    }

    public static Strings GetMSVCVersionPaths()
    {
        return new (Directory.GetDirectories(Path.Combine(GetVisualStudioPath(), "VC", "Tools", "MSVC")));
    }
    
    public static string GetLatestMSVCPath()
    {
        var paths = GetMSVCVersionPaths();
        // Sort from biggest number to smallest
        paths.Sort((a, b) => string.Compare(b, a));
        return paths[0];
    }

    public static string GetMSVCIncludePath(string toolChainPath)
    {
        return Path.Combine(toolChainPath, "include");
    }
    
    public static string GetMSVCBinPath(string toolChainPath)
    {
        var partialPath = Path.Combine(toolChainPath, "bin", "Hostx64");
        // Switch on the architecture
        partialPath = GetArchFolder(partialPath)[0];
        
        return partialPath;
    }

    public static Strings GetMSVCLibPath(string toolChainPath)
    {
        Strings result = new Strings();
        var basePath = Path.Combine(toolChainPath, "lib");
        result.AddRange(GetArchFolder(basePath));

        return result;
    }

    public static Strings GetArchFolder(string currentPath)
    {
        Strings result = new Strings();
        switch (RuntimeInformation.OSArchitecture)
        {
            case Architecture.X64:
                result.Add(Path.Combine(currentPath, "x64"));
                break;
            case Architecture.X86:
                result.Add(Path.Combine(currentPath, "x86"));
                break;
            case Architecture.Arm64:
                // use pattern to get all subdirs with arm*
                result.AddRange(Directory.GetDirectories(currentPath, "arm*"));
                break;
            default:
                throw new Exception("Unsupported architecture");
        }

        return result;
    }
    
    
    // LLVM
    public static string GetLLVMPath()
    {
        return Path.Combine(GetVisualStudioPath(), "VC", "Tools", "Llvm");
    }
    
    public static string GetLLVMBinPath(string toolchainPath)
    {
        return Path.Combine(GetArchFolder(GetLLVMPath())[0], "bin");
    }
    
    // public static string GetLLVMLibCxxPath()
    // {
    //     var partialPath = Path.Combine(GetArchFolder(GetLLVMPath())[0], "lib");
    //     return Path.Combine(partialPath, "libcxx");
    // }
}