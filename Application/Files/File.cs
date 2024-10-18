
using System.Runtime.InteropServices;
using ScytheBuild.Common;

namespace ScytheBuild.Files;

public interface File
{
    public string AbsPath { get; set; }
    public string Extension => Path.GetExtension(AbsPath);

    public string? CompiledFilePath { get; set; }
}