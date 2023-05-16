using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Modloader64.Mupen64plus.Frontend;

namespace Modloader64.Mupen64plus;

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


    public static uint SwapBytes(uint x) {
        // swap adjacent 16-bit blocks
        x = (x >> 16) | (x << 16);
        // swap adjacent 8-bit blocks
        return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
    }

    public static unsafe void OnFrame(int FrameCount) {
        uint ptr = 0x801CA0D0 + 20;

        if (FrameCount > 100) {
            Memory.Memory_Write8(0x8011A644U, 0);
            Memory.Memory_Write8(0x8011A645U, 0);
            Memory.Memory_Write8(0x8011A646U, 0);
            for (uint index = 3; index < 24; index++) {
                Memory.Memory_Write8(0x8011A644U + index, 1);
            }
            Memory.Memory_Write8(0x8011A648U, 0);
            Memory.Memory_Write8(0x8011A649U, 0);
            Memory.Memory_Write8(0x8011A64AU, 0);

            Memory.Memory_Write16(0x8011A5FEU, 0x20);
            Memory.Memory_Write64(0x8011A670U, 0xFFFFFFFFFFFFFF);

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
