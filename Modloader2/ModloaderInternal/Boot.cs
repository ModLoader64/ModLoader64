
using Modloader2.Mupen64plus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Modloader2.Mupen64plus.Common;

namespace Modloader2.Modloader2Internal; 

public static class Boot {
    private static IntPtr ConfigCore = IntPtr.Zero;
    private static IntPtr ConfigVideo = IntPtr.Zero;
    private static IntPtr ConfigTransferpak = IntPtr.Zero;
    private static IntPtr Config64DD = IntPtr.Zero;
    private static IntPtr ConfigUIConsole = IntPtr.Zero;
    private static IntPtr CoreLibraryHandle = IntPtr.Zero;
    private static List<IntPtr> LoadedPlugins = new List<IntPtr>();

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
        CoreLibraryHandle = Kernel32.LoadLibrary("mupen64plus.dll");
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
        int version = 0;
        char* pluginName = (char*)IntPtr.Zero;
        nint api = 0;
        nint capabilities = 0;
        if (PluginGetVersion(&type, (IntPtr)(&version), (IntPtr)(&api), &pluginName, (IntPtr)(&capabilities)) != M64Error.M64ERR_SUCCESS) {
            Logger.Error("Failed to get core plugin version info!");
            return false;
        }

        if (type != M64PluginType.M64PLUGIN_CORE) {
            Logger.Error("Core plugin is not a core plugin???");
            return false;
        }

        int ConfigAPIVersion, DebugAPIVersion, VidextAPIVersion;
        if (CoreGetAPIVersions((IntPtr)(&ConfigAPIVersion), (IntPtr)(&DebugAPIVersion), (IntPtr)(&VidextAPIVersion), IntPtr.Zero) != M64Error.M64ERR_SUCCESS) {
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
        const float CONFIG_PARAM_VERSION = 1.00f;
        char* versionString = StringToAnsiString("Version");
        bool saveConfig = false;

        IntPtr configCore = IntPtr.Zero;
        IntPtr configVideo = IntPtr.Zero;
        IntPtr configTransferPak = IntPtr.Zero;
        IntPtr config64DD = IntPtr.Zero;
        IntPtr configUIConsole = IntPtr.Zero;

        var exitCode = Config.ConfigOpenSection(StringToAnsiString("Core"), &configCore);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'Core' configuration section! Got error {exitCode}");
            return false;
        }

        exitCode = Config.ConfigOpenSection(StringToAnsiString("Video-General"), &configVideo);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'Video-General' configuration section! Got Error {exitCode}");
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

        exitCode = Config.ConfigOpenSection(StringToAnsiString("UI-Console"), &configUIConsole);
        if (exitCode != M64Error.M64ERR_SUCCESS) {
            Logger.Error($"Failed to open 'UI-Console' configuration section! Got Error {exitCode}");
            return false;
        }

        ConfigCore = configCore;
        ConfigVideo = configVideo;
        ConfigTransferpak = configTransferPak;
        Config64DD = config64DD;
        ConfigUIConsole = configUIConsole;

        float configParamsVersion;
        if (Config.ConfigGetParameter(ConfigUIConsole, versionString, M64Type.M64TYPE_FLOAT, (IntPtr)(&configParamsVersion), sizeof(float)) != M64Error.M64ERR_SUCCESS) {
            Logger.Warning($"No version number in 'UI-Console' config section! Setting defaults.");
            Config.ConfigDeleteSection(StringToAnsiString("UI-Console"));
            Config.ConfigOpenSection(StringToAnsiString("UI-Console"), &configUIConsole);
            ConfigUIConsole = configUIConsole;
            saveConfig = true;
        }
        else if ((int)configParamsVersion != (int)CONFIG_PARAM_VERSION) {
            Logger.Warning($"Incompatible version {configParamsVersion} in 'UI-Console' config section: current is {configParamsVersion}. Setting defaults.");
            Config.ConfigDeleteSection(StringToAnsiString("UI-Console"));
            Config.ConfigOpenSection(StringToAnsiString("UI-Console"), &configUIConsole);
            saveConfig = true;
        }
        else if (CONFIG_PARAM_VERSION - configParamsVersion >= 0.0001f) {
            float version = CONFIG_PARAM_VERSION;
            Config.ConfigSetParameter(ConfigUIConsole, versionString, M64Type.M64TYPE_FLOAT, (IntPtr)(&version));
            Logger.Info($"Updating parameter set version in 'UI-Console' config section to {version}");
            saveConfig = true;
        }

        Config.ConfigSetDefaultFloat(ConfigUIConsole, versionString, CONFIG_PARAM_VERSION, StringToAnsiString("Config parameter set version number. Please don't change this version number."));
        Config.ConfigSetDefaultString(ConfigUIConsole, StringToAnsiString("PluginDir"), StringToAnsiString("./plugins"), StringToAnsiString("Directory in which to search for plugins"));
        Config.ConfigSetDefaultString(ConfigUIConsole, StringToAnsiString("VideoPlugin"), StringToAnsiString("mupen64plus-video-GLideN64.dll"), StringToAnsiString("Filename of video plugin"));
        Config.ConfigSetDefaultString(ConfigUIConsole, StringToAnsiString("AudioPlugin"), StringToAnsiString("mupen64plus-audio-sdl.dll"), StringToAnsiString("Filename of audio plugin"));
        Config.ConfigSetDefaultString(ConfigUIConsole, StringToAnsiString("InputPlugin"), StringToAnsiString("mupen64plus-input-sdl.dll"), StringToAnsiString("Filename of input plugin"));
        Config.ConfigSetDefaultString(ConfigUIConsole, StringToAnsiString("RspPlugin"), StringToAnsiString("mupen64plus-rsp-hle.dll"), StringToAnsiString("Filename of RSP plugin"));

        // GB shit we're gonna skip
        // N64DD shit we're gonna skip

        if (saveConfig) {
            Config.ConfigSaveSection(StringToAnsiString("UI-Console"));
        }

        return true;
    }

    public static unsafe bool InitializeROM(ref IntPtr romPtr) {
        string romPath = "oot.z64";
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
        int version = 0;
        char* pluginName = (char*)IntPtr.Zero;
        char* pluginDir = Config.ConfigGetParamString(ConfigUIConsole, StringToAnsiString("PluginDir"));

        List<IntPtr> pluginPaths = new List<IntPtr> {
            (IntPtr)Config.ConfigGetParamString(ConfigUIConsole, StringToAnsiString("VideoPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigUIConsole, StringToAnsiString("AudioPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigUIConsole, StringToAnsiString("InputPlugin")),
            (IntPtr)Config.ConfigGetParamString(ConfigUIConsole, StringToAnsiString("RspPlugin"))
        };

        foreach (var _path in pluginPaths) {
            string path = $"{Marshal.PtrToStringAnsi((IntPtr)pluginDir)}/{Marshal.PtrToStringAnsi(_path)}";
            IntPtr handle = Kernel32.LoadLibrary(path);
            if (handle == IntPtr.Zero) {
                Logger.Error($"Plugin {path} failed to load!");
                Kernel32.FreeLibrary(handle);
                continue;
            }

            IntPtr pfn_PluginGetVersion = Kernel32.GetProcAddress(handle, "PluginGetVersion");
            if (pfn_PluginGetVersion == IntPtr.Zero) {
                Logger.Error($"Plugin failed to locate PluginGetVersion for {path}!");
                continue;
            }

            IntPtr pfn_PluginStartup = Kernel32.GetProcAddress(handle, "PluginStartup");
            if (pfn_PluginStartup == IntPtr.Zero) {
                Logger.Error($"Plugin failed to locate PluginStartup for {path}!");
                continue;
            }

            PluginGetVersionDelegate PluginGetVersion = Marshal.GetDelegateForFunctionPointer<PluginGetVersionDelegate>(pfn_PluginGetVersion);
            if (PluginGetVersion == null) {
                Logger.Error($"Plugin failed to locate PluginGetVersionDelegate for {path}!");
                Kernel32.FreeLibrary(handle);
                continue;
            }
            PluginGetVersion(&type, (IntPtr)(&version), IntPtr.Zero, &pluginName, IntPtr.Zero);

            PluginStartupDelegate PluginStartup = Marshal.GetDelegateForFunctionPointer<PluginStartupDelegate>(pfn_PluginStartup);
            if (PluginStartup == null) {
                Logger.Error($"Plugin failed to locate PluginStartupDelegate for {path}!");
                Kernel32.FreeLibrary(handle);
                continue;
            }
            PluginStartup(CoreLibraryHandle, (IntPtr)pluginName, DebugCallback);


            if (Frontend.CoreAttachPlugin(type, handle) != M64Error.M64ERR_SUCCESS) {
                Logger.Error($"Plugin {path} failed to load!");
                Kernel32.FreeLibrary(handle);
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
            Kernel32.FreeLibrary(handle);
        }
    }
}
