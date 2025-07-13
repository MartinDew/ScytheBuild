
using System.Runtime.InteropServices;

namespace Barn.Files;

public interface File
{
    public string AbsPath { get; set; }
    public string Extension => Path.GetExtension(AbsPath);

    public string? CompiledFilePath { get; set; }
}