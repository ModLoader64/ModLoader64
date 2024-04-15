using System.Runtime.InteropServices;

namespace ModLoader64;

using Core;
using ModLoader.API;
using Mupen64plus;
using System.Reflection;
using Logger = Core.Logger;

/// <summary>
/// Main ModLoader64 class
/// </summary>
/// 
[Binding("Mupen64Plus")]
public class ModLoader64  : IBinding
{
    public static bool CoreWasInitialized = false;
    public static bool RomWasLoaded = false;
    public static Configuration Config = new Configuration();

    private static IntPtr romPtr = IntPtr.Zero;
    public static string rom = "";

    /// <summary>
    /// Initializes Mupen64plus
    /// </summary>
    /// <returns>Returns true if success, otherwise false</returns>
    public static unsafe bool Initialize() {
        if (Boot.InitializeCoreStartup() == false) {
            Logger.Error("InitializeCoreStartup failed!");
            return false;
        }

        CoreWasInitialized = true;

        if (Boot.InitializeConfig() == false) {
            Logger.Error("InitializeConfig failed!");
            return false;
        }

        if (Boot.InitializeROM(ref romPtr) == false) {
            Logger.Error("InitializeROM failed!");
            return false;
        }

        RomWasLoaded = true;

        // Handle cheat codes

        Boot.InitializePlugins();

        return true;
    }

    public static bool LoadState(string file)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Entrypoint of ModLoader64
    /// </summary>
    /// <param name="args">parameters passed to the program</param>
    /// <returns>Exit code</returns>
    public static s32 Main(string[] args) {
        s32 exit = 0;
        Logger.Info("Hello, world!");

        if (Configuration.Load(out Config)) {
            Configuration.Save(ref Config);
        }

        if (!Initialize()) {
            exit = 1;
        }

        Boot.StartGo();


        Boot.ShutdownPlugins();
        if (romPtr != IntPtr.Zero) {
            Marshal.FreeHGlobal(romPtr);
        }

        if (CoreWasInitialized) {
            if (RomWasLoaded) {
                Frontend.CoreDoCommand(M64Command.M64CMD_ROM_CLOSE, 0, IntPtr.Zero);
            }
            Frontend.CoreShutdown();
        }
        Configuration.Save(ref Config);

        return exit;
    }

    public static bool ChangeGameFile(string file)
    {
        rom = file;
        return true;
    }

    public static void InitBinding()
    {
        Initialize();
    }

    public static bool SaveState(string file)
    {
        throw new NotImplementedException();
    }

    public static void SetGameFile(string file)
    {
        rom = file;
    }

    public static void StartBinding()
    {
        Logger.Info("Hello, world!");

        Boot.StartGo();

        Boot.ShutdownPlugins();
        if (romPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(romPtr);
        }

        if (CoreWasInitialized)
        {
            if (RomWasLoaded)
            {
                Frontend.CoreDoCommand(M64Command.M64CMD_ROM_CLOSE, 0, IntPtr.Zero);
            }
            Frontend.CoreShutdown();
        }
    }

    public static void StopBinding()
    {
        throw new NotImplementedException();
    }

    public static void TogglePause()
    {
        throw new NotImplementedException();
    }
}
