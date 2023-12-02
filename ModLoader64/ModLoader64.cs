using System.Runtime.InteropServices;

namespace ModLoader64;

using Core;
using Mupen64plus;
using Logger = Core.Logger;

/// <summary>
/// Main ModLoader64 class
/// </summary>
public class ModLoader64 {
    public static bool CoreWasInitialized = false;
    public static bool RomWasLoaded = false;
    public static Configuration Config = new Configuration();

    private static IntPtr romPtr = IntPtr.Zero;

    /// <summary>
    /// Initializes Mupen64plus
    /// </summary>
    /// <returns>Returns true if success, otherwise false</returns>
    private static unsafe bool Initialize() {
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
}

