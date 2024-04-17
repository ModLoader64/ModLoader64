using System.Reflection;
using System.Runtime.InteropServices;

namespace ModLoader64.Core;

public static class Natives {
    public const string WindowsExtension = ".dll";
    public const string NixExtension = ".so";

    private const string WindowsLibrary = "kernel32.dll";
    private const string LinuxLibrary = "libdl.so";
    private const string OsxLibrary = "libdl.dylib"; // TODO: OSX

    #region mupen64plus
    /// <summary>
    /// The name of the library to load the functions from
    /// </summary>
    public static string MUPEN_LIBRARY;

    /// <summary>
    /// Handle to mupen library
    /// </summary>
    public static IntPtr MupenLibraryHandle = IntPtr.Zero;

    /// <summary>
    /// Static constructor
    /// </summary>
    static Natives() {
        Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)));
        MUPEN_LIBRARY = TransmuteLibraryName("mupen64plus");
        MupenLibraryHandle = LoadLibrary(MUPEN_LIBRARY);
        if (MupenLibraryHandle == IntPtr.Zero)
        {
            throw new Exception("Failed to load mupen64plus library!");
        }
    }

    /// <summary>
    /// GetProcAddress from mupen library
    /// </summary>
    public static IntPtr GetProcAddress(string procedureName)
    {
        return GetProcAddress(MupenLibraryHandle, procedureName);
    }

    /// <summary>
    /// Get an instance of a delegate from mupen Library
    /// </summary>
    public static T GetDelegateInstance<T>(string procedureName)
    {
        IntPtr address = GetProcAddress(procedureName);
        if (address == IntPtr.Zero)
        {
            throw new Exception($"Failed to GetProcAddress for symbol {procedureName} !");
        }

        return Marshal.GetDelegateForFunctionPointer<T>(address);
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Convert library name to be correct given the current platform
    /// </summary>
    public static string TransmuteLibraryName(string name) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"{name}{WindowsExtension}";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return $"{name}{NixExtension}";
        }
        return name;
    }
    #endregion

    #region LoadLibrary
    /// <summary>
    /// Windows LoadLibrary
    /// </summary>
    [DllImport(WindowsLibrary, EntryPoint = "LoadLibrary")] private static extern IntPtr WindowsLoadLibrary(string dllToLoad);

    /// <summary>
    /// Linux LoadLibrary equivalent
    /// </summary>
    [DllImport(LinuxLibrary, EntryPoint = "dlopen")] private static extern IntPtr LinuxLoadLibrary(string dllToLoad, int flags);

    /// <summary>
    /// Platform agnostic LoadLibrary
    /// </summary>
    public static IntPtr LoadLibrary(string dllToLoad) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsLoadLibrary(dllToLoad);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            return LinuxLoadLibrary(dllToLoad, 2);
        }
        return IntPtr.Zero;
    }
    #endregion

    #region GetProcAddress
    /// <summary>
    /// Windows GetProcAddress
    /// </summary>
    [DllImport(WindowsLibrary, EntryPoint = "GetProcAddress")] private static extern IntPtr WindowsGetProcAddress(IntPtr hModule, string procedureName);

    /// <summary>
    /// Linux GetProcAddress equivalent
    /// </summary>
    [DllImport(LinuxLibrary, EntryPoint = "dlsym")] private static extern IntPtr LinuxGetProcAddress(IntPtr hModule, string procedureName);

    /// <summary>
    /// Platform agnostic GetProcAddress
    /// </summary>
    public static IntPtr GetProcAddress(IntPtr hModule, string procedureName) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsGetProcAddress(hModule, procedureName);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            return LinuxGetProcAddress(hModule, procedureName);
        }
        return IntPtr.Zero;
    }
    #endregion

    #region FreeLibrary
    /// <summary>
    /// Windows FreeLibrary
    /// </summary>
    [DllImport(WindowsLibrary, EntryPoint = "FreeLibrary")] private static extern bool WindowsFreeLibrary(IntPtr hModule);

    /// <summary>
    /// Linux FreeLibrary equivalent
    /// </summary>
    [DllImport(LinuxLibrary, EntryPoint = "dlclose")] private static extern int LinuxFreeLibrary(IntPtr hModule);

    /// <summary>
    /// Platform agnostic FreeLibrary
    /// </summary>
    public static bool FreeLibrary(IntPtr hModule) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return WindowsFreeLibrary(hModule);
        }
        else {
            return LinuxFreeLibrary(hModule) == 0;
        }
    }
    #endregion
}


