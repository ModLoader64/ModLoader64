using ModLoader64.Core;

namespace ModLoader64.Mupen64plus;

public static unsafe class Frontend
{
    #region Delegate Types
    /// <summary>
    /// Delegate for the DebugCallback parameter
    /// </summary>
    /// <param name="Context">String which represents the context from which the callback was triggered</param>
    /// <param name="Level">M64Message value representing the level of the Message parameter</param>
    /// <param name="Message">String to be output as debug</param>
    public delegate void DebugCallbackDelegate(IntPtr Context, M64Message Level, char* Message);

    public delegate void StateCallbackDelegate(IntPtr Context, CoreParam ParamType, s32 NewValue);
    //

    public delegate M64Error CoreStartupDelegate(s32 APIVersion, char* ConfigPath, char* DataPath, IntPtr Context, DebugCallbackDelegate DebugCallback, IntPtr Context2, StateCallbackDelegate StateCallback);

    public delegate M64Error CoreShutdownDelegate();

    public delegate M64Error CoreAttachPluginDelegate(M64PluginType PluginType, IntPtr PluginLibHandle);

    public delegate M64Error CoreDetachPluginDelegate(M64PluginType PluginType);

    public delegate M64Error CoreDoCommandDelegate(M64Command Command, s32 ParamInt, IntPtr ParamPtr);

    public delegate M64Error CoreOverrideVidExtDelegate(IntPtr VideoFunctionStruct);

    public delegate M64Error CoreAddCheatDelegate(char* CheatName, CheatCode* CodeList, s32 NumCodes);

    public delegate M64Error CoreCheatEnabledDelegate(char* CheatName, s32 Enabled);

    public delegate M64Error CoreGetRomSettingsDelegate(RomSettings* RomSettings, s32 RomSettingsLength, s32 Crc1, s32 Crc2);
    #endregion

    #region Delegate Instances
    public static CoreStartupDelegate CoreStartup;
    public static CoreShutdownDelegate CoreShutdown;
    public static CoreAttachPluginDelegate CoreAttachPlugin;
    public static CoreDetachPluginDelegate CoreDetachPlugin;
    public static CoreDoCommandDelegate CoreDoCommand;
    public static CoreOverrideVidExtDelegate CoreOverrideVidExt;
    public static CoreAddCheatDelegate CoreAddCheat;
    public static CoreCheatEnabledDelegate CoreCheatEnabled;
    public static CoreGetRomSettingsDelegate CoreGetRomSettings;
    #endregion

    /// <summary>
    /// Mupen64plus API version
    /// </summary>
    public static readonly s32 CORE_API_VERSION = 0x020001;

    /// <summary>
    /// Static constructor
    /// </summary>
    static Frontend() {
        CoreStartup = Natives.GetDelegateInstance<CoreStartupDelegate>("CoreStartup");
        CoreShutdown = Natives.GetDelegateInstance<CoreShutdownDelegate>("CoreShutdown");
        CoreAttachPlugin = Natives.GetDelegateInstance<CoreAttachPluginDelegate>("CoreAttachPlugin");
        CoreDetachPlugin = Natives.GetDelegateInstance<CoreDetachPluginDelegate>("CoreDetachPlugin");
        CoreDoCommand = Natives.GetDelegateInstance<CoreDoCommandDelegate>("CoreDoCommand");
        CoreOverrideVidExt = Natives.GetDelegateInstance<CoreOverrideVidExtDelegate>("CoreOverrideVidExt");
        CoreAddCheat = Natives.GetDelegateInstance<CoreAddCheatDelegate>("CoreAddCheat");
        CoreCheatEnabled = Natives.GetDelegateInstance<CoreCheatEnabledDelegate>("CoreCheatEnabled");
        CoreGetRomSettings = Natives.GetDelegateInstance<CoreGetRomSettingsDelegate>("CoreGetRomSettings");
    }
}

