using ModLoader64.API;
using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static Frontend;

public static class GlobalCallbacks {
    private delegate void FrameCallbackDelegate(int FrameCount);
    private delegate void ResetCallbackDelegate(bool HardReset);
    private delegate void CommonCallbackDelegate();


    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    private extern static void VISetCallback(CommonCallbackDelegate callback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    private extern static void ResetSetCallback(ResetCallbackDelegate callback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    private extern static void PauseSetCallback(CommonCallbackDelegate callback);

    public static unsafe void OnFrame(int FrameCount) {
        if (FrameCount > 200) {
            Core.EmulatedMemory.Write(0x8011A604, (u16)999);
            Core.EmulatedMemory.Write32(0x83000000, 0xDEADBEEF);
            if (Core.EmulatedMemory.Read32(0x83000000) != 0xDEADBEEF) {
                PluginLogger.Error("We are fucked!\n");
            }

            Core.EmulatedMemory.Write32(0x100000004, 0xDEADBEEF);
            if (Core.EmulatedMemory.Read32(0x100000004) != 0xDEADBEEF) {
                PluginLogger.Error("We are fucked!\n");
            }
        }
    }

    public static void OnVI() {
    }

    public static void OnReset(bool HardReset) {
    }

    public static void OnPause() {
    }

    public static void SetupCallbacks() {
        CoreDoCommand(M64Command.M64CMD_SET_FRAME_CALLBACK, 0, Marshal.GetFunctionPointerForDelegate<FrameCallbackDelegate>(OnFrame));
        VISetCallback(OnVI);
        ResetSetCallback(OnReset);
        PauseSetCallback(OnPause);
    }
}

