
using System.Runtime.InteropServices;
using ModLoader64.Mupen64plus;

namespace ModLoader64.Core;

public static class Boot {
    private static IntPtr ConfigCore = IntPtr.Zero;
    private static IntPtr ConfigCoreEvents = IntPtr.Zero;
    private static IntPtr ConfigVideo = IntPtr.Zero;
    private static IntPtr ConfigVideoGlideN64 = IntPtr.Zero;
    private static IntPtr ConfigTransferpak = IntPtr.Zero;
    private static IntPtr Config64DD = IntPtr.Zero;
    private static IntPtr CoreLibraryHandle = IntPtr.Zero;
    private static List<IntPtr> LoadedPlugins = new List<IntPtr>();
    private static List<IntPtr> AllocatedStrings = new List<IntPtr>();

    /// <summary>
    /// Convert string to a cstring
    /// </summary>
    /// <param name="input">input string</param>
    /// <returns></returns>
    public static unsafe char* StringToAnsiString(string input) {
        char* output = (char*)Marshal.StringToHGlobalAnsi(input);

        if ((IntPtr)output == IntPtr.Zero) {
            Logger.Warning($"Failed to convert string {input}!");
        }

        AllocatedStrings.Append((IntPtr)output);
        return output;
    }

    /// <summary>
    /// Debug callback that is invoked by Mupen64plus (TODO: move somewhere more relevant)
    /// </summary>
    /// <param name="Context">String representing the context the debug message is from</param>
    /// <param name="Level">The M64Message level of the message</param>
    /// <param name="Message">The message to be output</param>
    private static unsafe void DebugCallback(IntPtr Context, M64Message Level, char* Message) {
        if (Level > M64Message.M64MSG_VERBOSE) {
            Level = 0;
        }
        Mupen64plus.Logger.Log(Level, Marshal.PtrToStringAnsi(Context), Marshal.PtrToStringAnsi((IntPtr)Message));
    }

    public static unsafe bool InitializeCoreStartup() {
        // should get the same handle as the one that dotnet loaded?
        CoreLibraryHandle = Natives.LoadLibrary(Natives.TransmuteLibraryName("mupen64plus"));
        if (CoreLibraryHandle == IntPtr.Zero) {
            Logger.Error($"Couldn't load core library!");
            return false;
        }

        var exitCode = Frontend.CoreStartup(Frontend.CORE_API_VERSION, 
            StringToAnsiString("config"), StringToAnsiString("data"),  (IntPtr)StringToAnsiString("Core"),
            DebugCallback, IntPtr.Zero, null
        );

        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Couldn't start Mupen64Plus core library! Got error {exitCode}");
            return false;
        }

        M64PluginType type = M64PluginType.M64PLUGIN_NULL;
        s32 version = 0;
        char* pluginName = (char*)IntPtr.Zero;
        nint api = 0;
        nint capabilities = 0;
        if (Mupen64plus.Common.PluginGetVersion(&type, (IntPtr)(&version), (IntPtr)(&api), &pluginName, (IntPtr)(&capabilities)) != M64Error.M64ERR_SUCCESS) {
            Logger.Error("Failed to get core plugin version info!");
            return false;
        }

        if (type != M64PluginType.M64PLUGIN_CORE) {
            Logger.Error("Core plugin is not a core plugin???");
            return false;
        }

        s32 ConfigAPIVersion, DebugAPIVersion, VidextAPIVersion;
        if (Mupen64plus.Common.CoreGetAPIVersions((IntPtr)(&ConfigAPIVersion), (IntPtr)(&DebugAPIVersion), (IntPtr)(&VidextAPIVersion), IntPtr.Zero) != M64Error.M64ERR_SUCCESS) {
            Logger.Error("Core library broken; no CoreAPIVersionFunc() function found.");
            return false;
        }

        if ((capabilities & (nint)M64Capabilities.M64CAPS_DYNAREC) != 0) {
            Logger.Info("\tIncludes support for Dynamic Recompiler");
        }
        if ((capabilities & (nint)M64Capabilities.M64CAPS_DEBUGGER) != 0) {
            Logger.Info("\tIncludes support for MIPS r4300 Debugger");
        }
        if ((capabilities & (nint)M64Capabilities.M64CAPS_CORE_COMPARE) != 0) {
            Logger.Info("\tIncludes support for r4300 Core Comparison");
        }

        return true;
    }

    public static unsafe  bool InitializeConfig() {
        IntPtr configCore = IntPtr.Zero;
        IntPtr configCoreEvents = IntPtr.Zero;
        IntPtr configVideo = IntPtr.Zero;
        IntPtr configVideoGlideN64 = IntPtr.Zero;
        IntPtr configTransferPak = IntPtr.Zero;
        IntPtr config64DD = IntPtr.Zero;

        var exitCode = Config.ConfigOpenSection(StringToAnsiString("Core"), &configCore);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'Core' configuration section! Got error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("CoreEvents"), &configCoreEvents);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'CoreEvents' configuration section! Got error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("Video-General"), &configVideo);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'Video-General' configuration section! Got Error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("Video-GlideN64"), &configVideoGlideN64);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'Video-GlideN64' configuration section! Got error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("TransferPak"), &configTransferPak);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'TransferPak' configuration section! Got Error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("64DD"), &config64DD);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open '64DD' configuration section! Got Error {exitCode}");
            return false;
        }

        ConfigCore = configCore;
        ConfigCoreEvents = configCoreEvents;
        ConfigVideo = configVideo;
        ConfigVideoGlideN64 = configVideoGlideN64;
        ConfigTransferpak = configTransferPak;
        Config64DD = config64DD;

        UpdateMupenConfig();

        return true;
    }

    public static unsafe void UpdateMupenConfig() {
        foreach (var (Name, Type, ValuePointer, _Name) in ModLoader64.Config.Core.GetConfigParameters()) {
            Logger.Info($"Core: {_Name} {Type}");

            if (ValuePointer != IntPtr.Zero) {
                Config.ConfigSetParameter(ConfigCore, (char*)Name, Type, ValuePointer);
                if (Type == M64Type.M64TYPE_STRING) {
                    Marshal.FreeHGlobal(ValuePointer);
                }
            }
            else {
                Logger.Error($"\tFailed to parse value");
            }

            Marshal.FreeHGlobal(Name);
        }

        foreach (var (Name, Type, ValuePointer, _Name) in ModLoader64.Config.CoreEvents.GetConfigParameters()) {
            Logger.Info($"CoreEvents: {_Name} {Type}");

            if (ValuePointer != IntPtr.Zero) {
                Config.ConfigSetParameter(ConfigCoreEvents, (char*)Name, Type, ValuePointer);
                if (Type == M64Type.M64TYPE_STRING) {
                    Marshal.FreeHGlobal(ValuePointer);
                }
            }
            else {
                Logger.Error($"\tFailed to parse value");
            }

            Marshal.FreeHGlobal(Name);
        }

        foreach (var (Name, Type, ValuePointer, _Name) in ModLoader64.Config.VideoGeneral.GetConfigParameters()) {
            Logger.Info($"Video-General: {_Name} {Type}");

            if (ValuePointer != IntPtr.Zero) {
                Config.ConfigSetParameter(ConfigVideo, (char*)Name, Type, ValuePointer);
                if (Type == M64Type.M64TYPE_STRING) {
                    Marshal.FreeHGlobal(ValuePointer);
                }
            }
            else {
                Logger.Error($"\tFailed to parse value");
            }

            Marshal.FreeHGlobal(Name);
        }

        foreach (var (Name, Type, ValuePointer, _Name) in ModLoader64.Config.GLN64.GetConfigParameters()) {
            Logger.Info($"Video-GlideN64: {_Name} {Type}");

            if (ValuePointer != IntPtr.Zero) {
                Config.ConfigSetParameter(ConfigVideoGlideN64, (char*)Name, Type, ValuePointer);
                if (Type == M64Type.M64TYPE_STRING) {
                    Marshal.FreeHGlobal(ValuePointer);
                }
            }
            else {
                Logger.Error($"\tFailed to parse value");
            }

            Marshal.FreeHGlobal(Name);
        }
    }

    public static unsafe bool InitializeROM(ref IntPtr romPtr) {
        string romPath = ModLoader64.rom;
        byte[] romData = File.ReadAllBytes(romPath);
        romPtr = Marshal.AllocHGlobal(romData.Length);
        if (romPtr == IntPtr.Zero) {
            Logger.Error("Failed to allocate space for rom!");
            return false;
        }
        Marshal.Copy(romData, 0, romPtr, romData.Length);

        if (Frontend.CoreDoCommand(M64Command.M64CMD_ROM_OPEN, romData.Length, romPtr) != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Core failed to open rom image file {romPath}!");
            return false;
        }

        return true;
    }

    public static unsafe bool InitializePlugins() {
        M64PluginType type = M64PluginType.M64PLUGIN_NULL;
        s32 version = 0;
        char* pluginName = (char*)IntPtr.Zero;
        char* pluginDir = Config.ConfigGetParamString(ConfigCore, StringToAnsiString("PluginDir"));

        List<IntPtr> pluginPaths = new List<IntPtr> {
            (IntPtr)Config.ConfigGetParamString(ConfigCore, StringToAnsiString("VideoPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigCore, StringToAnsiString("AudioPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigCore, StringToAnsiString("InputPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigCore, StringToAnsiString("RspPlugin"))
        };

        foreach (var _path in pluginPaths) {
            string path = $"{Marshal.PtrToStringAnsi((IntPtr)pluginDir)}/{Marshal.PtrToStringAnsi(_path)}";
            IntPtr handle = Natives.LoadLibrary(path);
            if (handle == IntPtr.Zero) {
                Logger.Error($"Plugin {path} failed to load!");
                Natives.FreeLibrary(handle);
                continue;
            }

            IntPtr pfn_PluginGetVersion = Natives.GetProcAddress(handle, "PluginGetVersion");
            if (pfn_PluginGetVersion == IntPtr.Zero) {
                Logger.Error($"Plugin failed to locate PluginGetVersion for {path}!");
                continue;
            }

            IntPtr pfn_PluginStartup = Natives.GetProcAddress(handle, "PluginStartup");
            if (pfn_PluginStartup == IntPtr.Zero) {
                Logger.Error($"Plugin failed to locate PluginStartup for {path}!");
                continue;
            }

            Mupen64plus.Common.PluginGetVersionDelegate PluginGetVersion = Marshal.GetDelegateForFunctionPointer<Mupen64plus.Common.PluginGetVersionDelegate>(pfn_PluginGetVersion);
            if (PluginGetVersion == null) {
                Logger.Error($"Plugin failed to locate PluginGetVersionDelegate for {path}!");
                Natives.FreeLibrary(handle);
                continue;
            }
            PluginGetVersion(&type, (IntPtr)(&version), IntPtr.Zero, &pluginName, IntPtr.Zero);

            Mupen64plus.Common.PluginStartupDelegate PluginStartup = Marshal.GetDelegateForFunctionPointer<Mupen64plus.Common.PluginStartupDelegate>(pfn_PluginStartup);
            if (PluginStartup == null) {
                Logger.Error($"Plugin failed to locate PluginStartupDelegate for {path}!");
                Natives.FreeLibrary(handle);
                continue;
            }
            PluginStartup(CoreLibraryHandle, (IntPtr)pluginName, DebugCallback);


            if (Frontend.CoreAttachPlugin(type, handle) != M64Error.M64ERR_SUCCESS) {
                Logger.Error($"Plugin {path} failed to load!");
                Natives.FreeLibrary(handle);
                continue;
            }

            LoadedPlugins.Add(handle);
            Logger.Debug($"Got {type} plugin {Marshal.PtrToStringAnsi((IntPtr)pluginName)} with version {version}");
        }

        return true;
    }

    public static void StartGo() {
        GlobalCallbacks.SetupCallbacks();
        Frontend.CoreDoCommand(M64Command.M64CMD_EXECUTE, 0, IntPtr.Zero);
    }

    public static void ShutdownPlugins() {
        foreach (var handle in LoadedPlugins) {
            Natives.FreeLibrary(handle);
        }

        foreach (var handle in AllocatedStrings) {
            Marshal.FreeHGlobal(handle);
        }
    }
}
