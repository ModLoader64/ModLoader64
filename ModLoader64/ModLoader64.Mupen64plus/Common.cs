using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static Frontend;

/// <summary>
/// Enumeration which represents the plugin types in the mupen64plus api
/// </summary>
public enum M64PluginType {
    M64PLUGIN_NULL = 0,
    M64PLUGIN_RSP = 1,
    M64PLUGIN_GFX,
    M64PLUGIN_AUDIO,
    M64PLUGIN_INPUT,
    M64PLUGIN_CORE
};

/// <summary>
/// 'Parameter' types which represent events emitted from the mupen64plus core library
/// </summary>
public enum CoreParam {
    M64CORE_EMU_STATE = 1,
    M64CORE_VIDEO_MODE,
    M64CORE_SAVESTATE_SLOT,
    M64CORE_SPEED_FACTOR,
    M64CORE_SPEED_LIMITER,
    M64CORE_VIDEO_SIZE,
    M64CORE_AUDIO_VOLUME,
    M64CORE_AUDIO_MUTE,
    M64CORE_INPUT_GAMESHARK,
    M64CORE_STATE_LOADCOMPLETE,
    M64CORE_STATE_SAVECOMPLETE
};

/// <summary>
/// Enumeration used for exit codes from the mupen64plus core library
/// </summary>
public enum M64Error {
    M64ERR_SUCCESS = 0,
    M64ERR_NOT_INIT,        /* Function is disallowed before InitMupen64Plus() is called */
    M64ERR_ALREADY_INIT,    /* InitMupen64Plus() was called twice */
    M64ERR_INCOMPATIBLE,    /* API versions between components are incompatible */
    M64ERR_INPUT_ASSERT,    /* Invalid parameters for function call, such as ParamValue=NULL for GetCoreParameter() */
    M64ERR_INPUT_INVALID,   /* Invalid input data, such as ParamValue="maybe" for SetCoreParameter() to set a BOOL-type value */
    M64ERR_INPUT_NOT_FOUND, /* The input parameter(s) specified a particular item which was not found */
    M64ERR_NO_MEMORY,       /* Memory allocation failed */
    M64ERR_FILES,           /* Error opening, creating, reading, or writing to a file */
    M64ERR_INTERNAL,        /* Internal error (bug) */
    M64ERR_INVALID_STATE,   /* Current program state does not allow operation */
    M64ERR_PLUGIN_FAIL,     /* A plugin function returned a fatal error */
    M64ERR_SYSTEM_FAIL,     /* A system function call, such as an SDL or file operation, failed */
    M64ERR_UNSUPPORTED,     /* Function call is not supported (ie, core not built with debugger) */
    M64ERR_WRONG_TYPE       /* A given input type parameter cannot be used for desired operation */
}

public enum M64Type {
    M64TYPE_INT = 1,
    M64TYPE_FLOAT,
    M64TYPE_BOOL,
    M64TYPE_STRING
};

public enum M64Capabilities {
    M64CAPS_DYNAREC = 1,
    M64CAPS_DEBUGGER = 2,
    M64CAPS_CORE_COMPARE = 4
};

public enum M64Command {
    M64CMD_NOP = 0,
    M64CMD_ROM_OPEN,
    M64CMD_ROM_CLOSE,
    M64CMD_ROM_GET_HEADER,
    M64CMD_ROM_GET_SETTINGS,
    M64CMD_EXECUTE,
    M64CMD_STOP,
    M64CMD_PAUSE,
    M64CMD_RESUME,
    M64CMD_CORE_STATE_QUERY,
    M64CMD_STATE_LOAD,
    M64CMD_STATE_SAVE,
    M64CMD_STATE_SET_SLOT,
    M64CMD_SEND_SDL_KEYDOWN,
    M64CMD_SEND_SDL_KEYUP,
    M64CMD_SET_FRAME_CALLBACK,
    M64CMD_TAKE_NEXT_SCREENSHOT,
    M64CMD_CORE_STATE_SET,
    M64CMD_READ_SCREEN,
    M64CMD_RESET,
    M64CMD_ADVANCE_FRAME,
    M64CMD_SET_MEDIA_LOADER,
    M64CMD_PIF_OPEN
};

[Flags]
public enum ButtonFlags : ushort {
    R_DPAD = (1 << 0),
    L_DPAD = (1 << 1),
    D_DPAD = (1 << 2),
    U_DPAD = (1 << 3),
    START_BUTTON = (1 << 4),
    Z_TRIG = (1 << 5),
    B_BUTTON = (1 << 6),
    A_BUTTON = (1 << 7),

    R_CBUTTON = (1 << 8),
    L_CBUTTON = (1 << 9),
    D_CBUTTON = (1 << 10),
    U_CBUTTON = (1 << 11),
    R_TRIG = (1 << 12),
    L_TRIG = (1 << 13),
    Reserved1 = (1 << 14),
    Reserved2 = (1 << 15),
};

[StructLayout(LayoutKind.Explicit, Size = 0x04)]
public struct Buttons {
    [FieldOffset(0x00)] ButtonFlags Button;
    [FieldOffset(0x02)] sbyte XAxis;
    [FieldOffset(0x03)] sbyte YAxis;
};

[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct CheatCode {
    [FieldOffset(0x00)] public uint Address;
    [FieldOffset(0x04)] public uint Value;
};

[StructLayout(LayoutKind.Explicit, Size = 0x138)]
public unsafe struct RomSettings {
    [FieldOffset(0x000)] public char* GoodName;
    [FieldOffset(0x100)] public char* MD5;
    [FieldOffset(0x121)] public byte SaveType;
    [FieldOffset(0x122)] public byte Status;
    [FieldOffset(0x123)] public byte Players;
    [FieldOffset(0x124)] public byte Rumble;
    [FieldOffset(0x125)] public byte TransferPak;
    [FieldOffset(0x126)] public byte MemPak;
    [FieldOffset(0x127)] public byte BioPak;
    [FieldOffset(0x128)] public byte DisableExtraMemory;
    [FieldOffset(0x12C)] public uint CountPerOp;
    [FieldOffset(0x130)] public uint SiDmaDuration;
    [FieldOffset(0x134)] public uint AiDmaModifier;
}

public unsafe class Common {
    public delegate M64Error PluginGetVersionDelegate(M64PluginType* PluginType, IntPtr PluginVersion, IntPtr APIVersion, char** PluginNamePtr, IntPtr Capabilities);
    public delegate M64Error PluginStartupDelegate(IntPtr CoreLibHandle, IntPtr Context, DebugCallbackDelegate DebugCallback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error PluginGetVersion(M64PluginType* PluginType, IntPtr PluginVersion, IntPtr APIVersion, char** PluginNamePtr, IntPtr Capabilities);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error CoreGetAPIVersions(IntPtr ConfigVersion, IntPtr DebugVersion, IntPtr VidextVersion, IntPtr ExtraVersion);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* CoreErrorMessage(M64Error ReturnCode);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error PluginStartup(IntPtr CoreLibHandle, IntPtr Context, DebugCallbackDelegate DebugCallback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error PluginShutdown();
}
