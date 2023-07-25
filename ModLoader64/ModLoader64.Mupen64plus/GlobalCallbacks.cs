using ModLoader.API;
using ModLoader64.API;
using System;
using System.Reflection;
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
    // Binding event objects.
    private static readonly EventNewVi viEvent = new EventNewVi();
    private static readonly EventNewFrame frameEvent = new EventNewFrame(0);

    public static unsafe void OnFrame(int FrameCount) {
        frameEvent.frame = FrameCount;
        PubEventBus.bus.PushEvent(frameEvent);
        if (FrameCount > 200) {
            Core.EmulatedMemory.Write(0x8011A604, (u16)999);
            Core.EmulatedMemory.WriteU32(0x83000000, 0xDEADBEEF);
            if (Core.EmulatedMemory.ReadU32(0x83000000) != 0xDEADBEEF) {
                PluginLogger.Error("We are fucked!\n");
            }

            Core.EmulatedMemory.WriteU32(0x100000004, 0xDEADBEEF);
            if (Core.EmulatedMemory.ReadU32(0x100000004) != 0xDEADBEEF) {
                PluginLogger.Error("We are fucked!\n");
            }
        }
    }

    public static void OnVI() {
        PubEventBus.bus.PushEvent(viEvent);
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

        // This is bullshittery of the highest order. For the love of christ find a better way to do this.
        foreach (Type t in Assembly.LoadFile(Path.GetFullPath($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/ImGui.NET.dll")).GetTypes())
        {
            if (t.Name == "ImGuiInit")
            {
                t.GetMethod("Init")!.Invoke(null, Array.Empty<object>());
                break;
            }
        }
    }
}

