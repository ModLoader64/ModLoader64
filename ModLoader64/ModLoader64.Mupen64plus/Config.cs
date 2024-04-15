using ModLoader64.Core;

namespace ModLoader64.Mupen64plus;

public unsafe class Config {
    #region Delegate Types
    public delegate void SectionListCallbackDelegate(IntPtr Context, char* SectionName);
    public delegate void ParameterListCallbackDelegate(IntPtr Context, char* ParamName, M64Type ParamType);
    //
    public delegate M64Error ConfigExternalOpenDelegate(char* FileName, IntPtr Handle);
    public delegate M64Error ConfigExternalCloseDelegate(IntPtr Handle);
    public delegate M64Error ConfigExternalGetParameterDelegate(IntPtr Handle, char* SectionName, char* ParamName, char* ParamPtr, s32 ParamMaxLength);
    public delegate M64Error ConfigListSectionsDelegate(IntPtr context, SectionListCallbackDelegate SectionListCallback);
    public delegate M64Error ConfigOpenSectionDelegate(char* SectionName, IntPtr* ConfigSectionHandle);
    public delegate M64Error ConfigListParametersDelegate(IntPtr ConfigSectionHandle, IntPtr Context, ParameterListCallbackDelegate ParameterListCallback);
    public delegate s32 ConfigHasUnsavedChangesDelegate(char* SectionName);
    public delegate M64Error ConfigDeleteSectionDelegate(char* SectionName);
    public delegate M64Error ConfigSaveFileDelegate();
    public delegate M64Error ConfigSaveSectionDelegate(char* SectionName);
    public delegate M64Error ConfigRevertChangesDelegate(char* SectionName);
    public delegate M64Error ConfigSetParameterDelegate(IntPtr ConfigSectionHandle, char* ParamName, M64Type ParamType, IntPtr ParamValue);
    public delegate M64Error ConfigSetParameterHelpDelegate(IntPtr ConfigSectionHandle, char* ParamName, char* ParamHelp);
    public delegate M64Error ConfigGetParameterDelegate(IntPtr ConfigSectionHandle, char* ParamName, M64Type ParamType, IntPtr ParamValue, s32 MaxSize);
    public delegate M64Error ConfigGetParameterTypeDelegate(IntPtr ConfigSectionHandle, char* ParamName, M64Type* ParamType);
    public delegate char* ConfigGetParameterHelpDelegate(IntPtr ConfigSectionHandle, char* ParamName);
    public delegate M64Error ConfigSetDefaultIntDelegate(IntPtr ConfigSectionHandle, char* ParamName, s32 ParamValue, char* ParamHelp);
    public delegate M64Error ConfigSetDefaultFloatDelegate(IntPtr ConfigSectionHandle, char* ParamName, f32 ParamValue, char* ParamHelp);
    public delegate M64Error ConfigSetDefaultBoolDelegate(IntPtr ConfigSectionHandle, char* ParamName, s32 ParamValue, char* ParamHelp);
    public delegate M64Error ConfigSetDefaultStringDelegate(IntPtr ConfigSectionHandle, char* ParamName, char* ParamValue, char* ParamHelp);
    public delegate s32 ConfigGetParamIntDelegate(IntPtr ConfigSectionHandle, char* ParamName);
    public delegate f32 ConfigGetParamFloatDelegate(IntPtr ConfigSectionHandle, char* ParamName);
    public delegate s32 ConfigGetParamBoolDelegate(IntPtr ConfigSectionHandle, char* ParamName);
    public delegate char* ConfigGetParamStringDelegate(IntPtr ConfigSectionHandle, char* ParamName);
    public delegate M64Error ConfigOverrideUserPathsDelegate(char* DataPath, char* CachePath);
    public delegate char* ConfigGetSharedDataFilepathDelegate(char* Filename);
    public delegate char* ConfigGetUserConfigPathDelegate();
    public delegate char* ConfigGetUserDataPathDelegate();
    public delegate char* ConfigGetUserCachePathDelegate();
    public delegate M64Error ConfigSendNetplayConfigDelegate(char* Data, s32 Size);
    public delegate M64Error ConfigReceiveNetplayConfigDelegate(char* Data, s32 Size);
    #endregion

    #region Delegate Instances
    public static ConfigExternalOpenDelegate ConfigExternalOpen;
    public static ConfigExternalCloseDelegate ConfigExternalClose;
    public static ConfigExternalGetParameterDelegate ConfigExternalGetParameter;
    public static ConfigListSectionsDelegate ConfigListSections;
    public static ConfigOpenSectionDelegate ConfigOpenSection;
    public static ConfigListParametersDelegate ConfigListParameters;
    public static ConfigHasUnsavedChangesDelegate ConfigHasUnsavedChanges;
    public static ConfigDeleteSectionDelegate ConfigDeleteSection;
    public static ConfigSaveFileDelegate ConfigSaveFile;
    public static ConfigSaveSectionDelegate ConfigSaveSection;
    public static ConfigRevertChangesDelegate ConfigRevertChanges;
    public static ConfigSetParameterDelegate ConfigSetParameter;
    public static ConfigSetParameterHelpDelegate ConfigSetParameterHelp;
    public static ConfigGetParameterDelegate ConfigGetParameter;
    public static ConfigGetParameterTypeDelegate ConfigGetParameterType;
    public static ConfigGetParameterHelpDelegate ConfigGetParameterHelp;
    public static ConfigSetDefaultIntDelegate ConfigSetDefaultInt;
    public static ConfigSetDefaultFloatDelegate ConfigSetDefaultFloat;
    public static ConfigSetDefaultBoolDelegate ConfigSetDefaultBool;
    public static ConfigSetDefaultStringDelegate ConfigSetDefaultString;
    public static ConfigGetParamIntDelegate ConfigGetParamInt;
    public static ConfigGetParamFloatDelegate ConfigGetParamFloat;
    public static ConfigGetParamBoolDelegate ConfigGetParamBool;
    public static ConfigGetParamStringDelegate ConfigGetParamString;
    public static ConfigOverrideUserPathsDelegate ConfigOverrideUserPaths;
    public static ConfigGetSharedDataFilepathDelegate ConfigGetSharedDataFilepath;
    public static ConfigGetUserConfigPathDelegate ConfigGetUserConfigPath;
    public static ConfigGetUserDataPathDelegate ConfigGetUserDataPath;
    public static ConfigGetUserCachePathDelegate ConfigGetUserCachePath;
    public static ConfigSendNetplayConfigDelegate ConfigSendNetplayConfig;
    public static ConfigReceiveNetplayConfigDelegate ConfigReceiveNetplayConfig;
    #endregion

    static Config() {
        ConfigExternalOpen = Natives.GetDelegateInstance<ConfigExternalOpenDelegate>("ConfigExternalOpen");
        ConfigExternalClose = Natives.GetDelegateInstance<ConfigExternalCloseDelegate>("ConfigExternalClose");
        ConfigExternalGetParameter = Natives.GetDelegateInstance<ConfigExternalGetParameterDelegate>("ConfigExternalGetParameter");
        ConfigListSections = Natives.GetDelegateInstance<ConfigListSectionsDelegate>("ConfigListSections");
        ConfigOpenSection = Natives.GetDelegateInstance<ConfigOpenSectionDelegate>("ConfigOpenSection");
        ConfigListParameters = Natives.GetDelegateInstance<ConfigListParametersDelegate>("ConfigListParameters");
        ConfigHasUnsavedChanges = Natives.GetDelegateInstance<ConfigHasUnsavedChangesDelegate>("ConfigHasUnsavedChanges");
        ConfigDeleteSection = Natives.GetDelegateInstance<ConfigDeleteSectionDelegate>("ConfigDeleteSection");
        ConfigSaveFile = Natives.GetDelegateInstance<ConfigSaveFileDelegate>("ConfigSaveFile");
        ConfigSaveSection = Natives.GetDelegateInstance<ConfigSaveSectionDelegate>("ConfigSaveSection");
        ConfigRevertChanges = Natives.GetDelegateInstance<ConfigRevertChangesDelegate>("ConfigRevertChanges");
        ConfigSetParameter = Natives.GetDelegateInstance<ConfigSetParameterDelegate>("ConfigSetParameter");
        ConfigSetParameterHelp = Natives.GetDelegateInstance<ConfigSetParameterHelpDelegate>("ConfigSetParameterHelp");
        ConfigGetParameter = Natives.GetDelegateInstance<ConfigGetParameterDelegate>("ConfigGetParameter");
        ConfigGetParameterType = Natives.GetDelegateInstance<ConfigGetParameterTypeDelegate>("ConfigGetParameterType");
        ConfigGetParameterHelp = Natives.GetDelegateInstance<ConfigGetParameterHelpDelegate>("ConfigGetParameterHelp");
        ConfigSetDefaultInt = Natives.GetDelegateInstance<ConfigSetDefaultIntDelegate>("ConfigSetDefaultInt");
        ConfigSetDefaultFloat = Natives.GetDelegateInstance<ConfigSetDefaultFloatDelegate>("ConfigSetDefaultFloat");
        ConfigSetDefaultBool = Natives.GetDelegateInstance<ConfigSetDefaultBoolDelegate>("ConfigSetDefaultBool");
        ConfigSetDefaultString = Natives.GetDelegateInstance<ConfigSetDefaultStringDelegate>("ConfigSetDefaultString");
        ConfigGetParamInt = Natives.GetDelegateInstance<ConfigGetParamIntDelegate>("ConfigGetParamInt");
        ConfigGetParamFloat = Natives.GetDelegateInstance<ConfigGetParamFloatDelegate>("ConfigGetParamFloat");
        ConfigGetParamBool = Natives.GetDelegateInstance<ConfigGetParamBoolDelegate>("ConfigGetParamBool");
        ConfigGetParamString = Natives.GetDelegateInstance<ConfigGetParamStringDelegate>("ConfigGetParamString");
        ConfigOverrideUserPaths = Natives.GetDelegateInstance<ConfigOverrideUserPathsDelegate>("ConfigOverrideUserPaths");
        ConfigGetSharedDataFilepath = Natives.GetDelegateInstance<ConfigGetSharedDataFilepathDelegate>("ConfigGetSharedDataFilepath");
        ConfigGetUserConfigPath = Natives.GetDelegateInstance<ConfigGetUserConfigPathDelegate>("ConfigGetUserConfigPath");
        ConfigGetUserDataPath = Natives.GetDelegateInstance<ConfigGetUserDataPathDelegate>("ConfigGetUserDataPath");
        ConfigGetUserCachePath = Natives.GetDelegateInstance<ConfigGetUserCachePathDelegate>("ConfigGetUserCachePath");
        ConfigSendNetplayConfig = Natives.GetDelegateInstance<ConfigSendNetplayConfigDelegate>("ConfigSendNetplayConfig");
        ConfigReceiveNetplayConfig = Natives.GetDelegateInstance<ConfigReceiveNetplayConfigDelegate>("ConfigReceiveNetplayConfig");
    }
}
