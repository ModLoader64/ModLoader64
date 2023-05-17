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
        uint ptr = 0x801CA0D0 + 20;

        if (FrameCount > 100) {
            Core.EmulatedMemory.Write8(0x8011A644U, 0);
            Core.EmulatedMemory.Write8(0x8011A645U, 0);
            Core.EmulatedMemory.Write8(0x8011A646U, 0);
            for (uint index = 3; index < 24; index++) {
                Core.EmulatedMemory.Write8(0x8011A644U + index, 1);
            }
            Core.EmulatedMemory.Write8(0x8011A648U, 0);
            Core.EmulatedMemory.Write8(0x8011A649U, 0);
            Core.EmulatedMemory.Write8(0x8011A64AU, 0);

            Core.EmulatedMemory.Write16(0x8011A5FEU, 0x20);
            Core.EmulatedMemory.Write64(0x8011A670U, 0xFFFFFFFFFFFFFF);

            Memory.InvalidateCachedCode();
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

