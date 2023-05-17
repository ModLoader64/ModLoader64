using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Core; 

public static class Kernel32 {
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string DllToLoad);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr HModule, string ProcedureName);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr HModule);
}
