using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static VideoExtension;

public static unsafe class Frontend
{
    /// <summary>
    /// Delegate for the DebugCallback parameter
    /// </summary>
    /// <param name="Context">String which represents the context from which the callback was triggered</param>
    /// <param name="Level">M64Message value representing the level of the Message parameter</param>
    /// <param name="Message">String to be output as debug</param>
    public delegate void DebugCallbackDelegate(IntPtr Context, M64Message Level, char* Message);
    public delegate void StateCallbackDelegate(IntPtr Context, CoreParam ParamType, s32 NewValue);

    /// <summary>
    /// The name of the library to load the functions from
    /// </summary>
    public const string MUPEN_LIBRARY = "mupen64plus.dll";

    /// <summary>
    /// Mupen64plus API version
    /// </summary>
    public static readonly s32 CORE_API_VERSION = 0x020001;


    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreStartup(s32 APIVersion, char* ConfigPath, char* DataPath, IntPtr Context, DebugCallbackDelegate DebugCallback, IntPtr Context2, StateCallbackDelegate StateCallback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreShutdown();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreAttachPlugin(M64PluginType PluginType, IntPtr PluginLibHandle);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreDetachPlugin(M64PluginType PluginType);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreDoCommand(M64Command Command, s32 ParamInt, IntPtr ParamPtr);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreOverrideVidExt(VideoExtensionFunctions* VideoFunctionStruct);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreAddCheat(char* CheatName, CheatCode* CodeList, s32 NumCodes);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreCheatEnabled(char* CheatName, s32 Enabled);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreGetRomSettings(RomSettings* RomSettings, s32 RomSettingsLength, s32 Crc1, s32 Crc2);
}

