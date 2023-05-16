using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static Modloader64.Mupen64plus.Frontend;

namespace Modloader64.Mupen64plus;

public unsafe class Config {
    public delegate void SectionListCallbackDelegate(IntPtr Context, char* SectionName);
    public delegate void ParameterListCallbackDelegate(IntPtr Context, char* ParamName, M64Type ParamType);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigExternalOpen(char* FileName, IntPtr* Handle);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigExternalClose(IntPtr Handle);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigExternalGetParameter(IntPtr Handle, char* SectionName, char* ParamName, char* ParamPtr, int ParamMaxLength);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigListSections(IntPtr context, SectionListCallbackDelegate SectionListCallback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigOpenSection(char* SectionName, IntPtr* ConfigSectionHandle);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigListParameters(IntPtr ConfigSectionHandle, IntPtr Context, ParameterListCallbackDelegate ParameterListCallback);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int ConfigHasUnsavedChanges(char* SectionName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigDeleteSection(char* SectionName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSaveFile();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSaveSection(char* SectionName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigRevertChanges(char* SectionName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetParameter(IntPtr ConfigSectionHandle, char* ParamName, M64Type ParamType, IntPtr ParamValue);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetParameterHelp(IntPtr ConfigSectionHandle, char* ParamName, char* ParamHelp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigGetParameter(IntPtr ConfigSectionHandle, char* ParamName, M64Type ParamType, IntPtr ParamValue, int MaxSize);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigGetParameterType(IntPtr ConfigSectionHandle, char* ParamName, M64Type* ParamType);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetParameterHelp(IntPtr ConfigSectionHandle, char* ParamName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetDefaultInt(IntPtr ConfigSectionHandle, char* ParamName, int ParamValue, char* ParamHelp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetDefaultFloat(IntPtr ConfigSectionHandle, char* ParamName, float ParamValue, char* ParamHelp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetDefaultBool(IntPtr ConfigSectionHandle, char* ParamName, int ParamValue, char* ParamHelp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSetDefaultString(IntPtr ConfigSectionHandle, char* ParamName, char* ParamValue, char* ParamHelp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int ConfigGetParamInt(IntPtr ConfigSectionHandle, char* ParamName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static float ConfigGetParamFloat(IntPtr ConfigSectionHandle, char* ParamName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int ConfigGetParamBool(IntPtr ConfigSectionHandle, char* ParamName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetParamString(IntPtr ConfigSectionHandle, char* ParamName);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigOverrideUserPaths(char* DataPath, char* CachePath);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetSharedDataFilepath(char* Filename);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetUserConfigPath();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetUserDataPath();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* ConfigGetUserCachePath();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigSendNetplayConfig(char* Data, int Size);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error ConfigReceiveNetplayConfig(char* Data, int Size);
}
