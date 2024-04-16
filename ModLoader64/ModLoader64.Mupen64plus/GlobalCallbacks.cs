using ModLoader.API;
using ModLoader64.Core;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static Frontend;

public static class GlobalCallbacks {
    #region Delegate Types
    public delegate void FrameCallbackDelegate(int FrameCount);
    public delegate void ResetCallbackDelegate(bool HardReset);
    public delegate void CommonCallbackDelegate();
    //
    public delegate void VISetCallbackDelegate(CommonCallbackDelegate callback);
    public delegate void ResetSetCallbackDelegate(ResetCallbackDelegate callback);
    public delegate void PauseSetCallbackDelegate(CommonCallbackDelegate callback);
    public delegate u32 InstallCodeCallbackDelegate(u32 address, CommonCallbackDelegate pfn);
    public delegate void UninstallCodeCallbackDelegate(u32 uuid);
    #endregion

    #region Delegate Instances
    public static VISetCallbackDelegate VISetCallback;
    public static ResetSetCallbackDelegate ResetSetCallback;
    public static PauseSetCallbackDelegate PauseSetCallback;
    public static InstallCodeCallbackDelegate InstallCodeCallback;
    public static UninstallCodeCallbackDelegate UninstallCodeCallback;
    #endregion
    // Binding event objects.
    private static readonly EventNewVi viEvent = new EventNewVi();
    private static readonly EventNewFrame frameEvent = new EventNewFrame(0);

    static GlobalCallbacks() {
        VISetCallback = Natives.GetDelegateInstance<VISetCallbackDelegate>("VISetCallback");
        ResetSetCallback = Natives.GetDelegateInstance<ResetSetCallbackDelegate>("ResetSetCallback");
        PauseSetCallback = Natives.GetDelegateInstance<PauseSetCallbackDelegate>("PauseSetCallback");
        InstallCodeCallback = Natives.GetDelegateInstance<InstallCodeCallbackDelegate>("InstallCodeCallback");
        UninstallCodeCallback = Natives.GetDelegateInstance<UninstallCodeCallbackDelegate>("UninstallCodeCallback");
    }

    public static unsafe void OnFrame(int FrameCount)
    {
        frameEvent.frame = FrameCount;
        PubEventBus.bus.PushEvent(frameEvent);
    }

    public static void OnVI()
    {
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
    }
}

