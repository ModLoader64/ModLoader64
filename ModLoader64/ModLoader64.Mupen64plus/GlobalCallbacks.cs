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

    private static ImGuiTest ImGui;

    public static unsafe void OnFrame(int FrameCount) {
        if (FrameCount > 200) {
            ImGui = new ImGuiTest();
            ImGui.Initialize(MUPEN_LIBRARY);
            Core.EmulatedMemory.Write(0x8011A604, (u16)999);
            Core.EmulatedMemory.Write32(0x83000000, 0xDEADBEEF);
            if (Core.EmulatedMemory.Read32(0x83000000) != 0xDEADBEEF) {
                //PluginLogger.Error($"We are fucked! {Core.EmulatedMemory.Read32(0x83000000).ToString("X")}\n");
            }
        }

        //Core.EmulatedRom.Write8(0x3E, (u8)'J');
        //PluginLogger.Info(Core.EmulatedRom.Read32(0x00).ToString("X").PadLeft(8, '0')); // 80 37 12 40 for oot
        u8 l0 = Core.EmulatedRom.Read8(0x00); // 80
        u8 l1 = Core.EmulatedRom.Read8(0x01); // 37
        u8 r0 = Core.EmulatedRom.Read8(0x02); // 12
        u8 r1 = Core.EmulatedRom.Read8(0x03); // 40
        //PluginLogger.Info($"{l0.ToString("X").PadLeft(2, '0')} {l1.ToString("X").PadLeft(2, '0')} {r0.ToString("X").PadLeft(2, '0')} {r1.ToString("X").PadLeft(2, '0')}");
    }

    public static void OnVI() {
        bool open = true;
        if (ImGui != null && ImGui.Initialized) {
            ImGui.Begin("Test", ref open, 0);
            ImGui.End();
        }
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

