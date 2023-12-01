using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Core;

public static class NativeMethods {
    private const string WindowsDll = "kernel32.dll";
    private const string LinuxDll = "libdl.so";
    private const string OsxDll = "libdl.dylib";

    [DllImport(WindowsDll, EntryPoint = "LoadLibrary")]
    private static extern IntPtr WindowsLoadLibrary(string dllToLoad);

    [DllImport(LinuxDll, EntryPoint = "dlopen")]
    private static extern IntPtr LinuxLoadLibrary(string dllToLoad, int flags);

    public static IntPtr LoadLibrary(string dllToLoad) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsLoadLibrary(dllToLoad);
        }
        else {
            return LinuxLoadLibrary(dllToLoad, 2);
        }
    }

    [DllImport(WindowsDll, EntryPoint = "GetProcAddress")]
    private static extern IntPtr WindowsGetProcAddress(IntPtr hModule, string procedureName);

    [DllImport(LinuxDll, EntryPoint = "dlsym")]
    private static extern IntPtr LinuxGetProcAddress(IntPtr hModule, string procedureName);

    public static IntPtr GetProcAddress(IntPtr hModule, string procedureName) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsGetProcAddress(hModule, procedureName);
        }
        else {
            return LinuxGetProcAddress(hModule, procedureName);
        }
    }

    [DllImport(WindowsDll, EntryPoint = "FreeLibrary")]
    private static extern bool WindowsFreeLibrary(IntPtr hModule);

    [DllImport(LinuxDll, EntryPoint = "dlclose")]
    private static extern int LinuxFreeLibrary(IntPtr hModule);

    public static bool FreeLibrary(IntPtr hModule) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsFreeLibrary(hModule);
        }
        else {
            return LinuxFreeLibrary(hModule) == 0;
        }
    }
}


