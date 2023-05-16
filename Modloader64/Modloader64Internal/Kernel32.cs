using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Modloader64.Modloader64Internal; 
public static class Kernel32 {
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string DllToLoad);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr HModule, string ProcedureName);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr HModule);
}
