using ModLoader64.Mupen64plus;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModLoader64.Core; 

public enum ForceMemorySizeOption : int {
    DISABLED = 0,
    SIZE_8MB,
    SIZE_4MB
}

public enum R4300EmulationMode : int {
    PURE_INTERPRETER = 0,
    CACHED_INTERPRETER,
    DYNAREC
}

public class ConfigVersion {
    public float Mupen;
    public int ModLoader;

    public ConfigVersion() {
        Mupen = 0;
        ModLoader = 0;
    }

    public ConfigVersion(float mupen, int modloader) {
        Mupen = mupen;
        ModLoader = modloader;
    }
}

public abstract class ConfigBase {
    public ConfigVersion Version = new ConfigVersion();
    public string ConfigSection = "NULL";

    public abstract void SetDefaults();
    public abstract string GetRealConfigName(string input);

    public IEnumerable<(IntPtr Name, M64Type Type, IntPtr ValuePointer, string _Name)> GetConfigParameters() {
        foreach (var prop in this.GetType().GetProperties()) {
            string propName = GetRealConfigName(prop.Name);
            var type = ConvertToM64Type(prop.PropertyType);
            if (type == null) {
                Logger.Error($"Skipping {propName}");
                continue;
            }

            

            var propValue = prop.GetValue(this);
            var valuePointer = ConvertValueToPointer(propValue, prop);
            IntPtr cstr = Marshal.StringToHGlobalAnsi(propName);
            yield return (cstr, type.Value, valuePointer, propName);
        }
    }

    private M64Type? ConvertToM64Type(Type type) {
        if (type == typeof(int) || type.IsEnum) {
            return M64Type.M64TYPE_INT;
        }
        else if (type == typeof(float)) {
            return M64Type.M64TYPE_FLOAT;
        }
        else if (type == typeof(bool)) {
            return M64Type.M64TYPE_BOOL;
        }
        else if (type == typeof(string)) {
            return M64Type.M64TYPE_STRING;
        }

        return null;
    }

    private unsafe IntPtr ConvertValueToPointer(object value, PropertyInfo prop) {
        Type type = prop.PropertyType;
        if (type.IsValueType) {
            if (type.IsEnum) {
                value = (int)value;
            }

            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            handle.Free();
            return ptr;
        }
        else if (type == typeof(string)) {
            var str = (string)value;
            return Marshal.StringToHGlobalAnsi(str);
        }

        return IntPtr.Zero;
    }
}

public class ConfigCore : ConfigBase {
    public R4300EmulationMode R4300Emulator { get; set; } // Use Pure Interpreter if 0, Cached Interpreter if 1, or Dynamic Recompiler if 2 or more
    public ForceMemorySizeOption ForceMemorySize { get; set; } // Use to force the reported memory size. 1 for 8mb, 2 for 4mb, 0 for disabled

    public bool AutoStateSlotIncrement { get; set; } // Increment the save state slot after each save operation
    public bool EnableDebugger { get; set; } // Activate the R4300 debugger when ROM execution begins, if core was built with Debugger support
    public bool RandomizeInterrupt { get; set; } // Randomize PI/SI Interrupt Timing

    public int CountPerOp { get; set; } // Force number of cycles per emulated instruction
    public int CountPerOpDenomPot { get; set; } // Reduce number of cycles per update by power of two when set greater than 0 (overclock)
    public int CurrentStateSlot { get; set; } // Save state slot (0-9) to use when saving/loading the emulator state
    public int SiDmaDuration { get; set; } // Duration of SI DMA (-1: use per game settings)

    public string PluginDir { get; set; }
    public string VideoPlugin { get; set; }
    public string AudioPlugin  { get; set; }
    public string InputPlugin  { get; set; }
    public string RspPlugin { get; set; }
    public string ScreenshotPath { get; set; } // Path to directory where screenshots are saved. If this is blank, the default value of ${UserDataPath}/screenshot will be used
    public string SaveStatePath { get; set; } // Path to directory where emulator save states (snapshots) are saved. If this is blank, the default value of ${UserDataPath}/save will be used
    public string SaveSRAMPath { get; set; } // Path to directory where SRAM/EEPROM data (in-game saves) are stored. If this is blank, the default value of ${UserDataPath}/save will be used
    public string SharedDataPath { get; set; } // Path to a directory to search when looking for shared data files
    public string GbCameraVideoCaptureBackend1 { get; set; } // Gameboy Camera Video Capture backend

    [Obsolete("The M64p OSD is disabled in ModLoader64.")]
    public bool OnScreenDisplay { get; set; } // Draw on-screen display if True, otherwise don't draw OSD

    public bool NoCompiledJump { get; set; } // Disable compiled jump commands in dynamic recompiler (should be set to False)

    [Obsolete("You should prefer ForceMemorySize for ModLoader64.")]
    public bool DisableExtraMem { get; set; } // Disable 4MB expansion RAM pack. May be necessary for some games

    public ConfigCore() {
        SetDefaults();
    }

    public override void SetDefaults() {
        ConfigSection = "Core";
        Version = new ConfigVersion(1.01f, 1);

        R4300Emulator = R4300EmulationMode.DYNAREC;
        ForceMemorySize = ForceMemorySizeOption.DISABLED;
        AutoStateSlotIncrement = false;
        EnableDebugger = false;
        RandomizeInterrupt = true;
        CountPerOp = 0;
        CountPerOpDenomPot = 0;
        CurrentStateSlot = 0;
        SiDmaDuration = -1;
        PluginDir = "plugins";
        VideoPlugin = "mupen64plus-video-GLideN64.dll";
        AudioPlugin = "mupen64plus-audio-sdl.dll";
        InputPlugin = "mupen64plus-input-sdl.dll";
        RspPlugin = "mupen64plus-rsp-hle.dll";
        ScreenshotPath = "screenshots";
        SaveStatePath = "savestates";
        SaveSRAMPath = "saves";
        SharedDataPath = "";
        GbCameraVideoCaptureBackend1 = "";
        OnScreenDisplay = false;
        NoCompiledJump = false;
        DisableExtraMem = false;
    }

    public override string GetRealConfigName(string input) {
        return input;
    }
}

public class ConfigCoreEvents : ConfigBase {
    public SDL_KeyCode Keyboard_SaveSlot0 { get; set; } // SDL keysym for save slot 0
    public SDL_KeyCode Keyboard_SaveSlot1 { get; set; } // SDL keysym for save slot 1
    public SDL_KeyCode Keyboard_SaveSlot2 { get; set; } // SDL keysym for save slot 2
    public SDL_KeyCode Keyboard_SaveSlot3 { get; set; } // SDL keysym for save slot 3
    public SDL_KeyCode Keyboard_SaveSlot4 { get; set; } // SDL keysym for save slot 4
    public SDL_KeyCode Keyboard_SaveSlot5 { get; set; } // SDL keysym for save slot 5
    public SDL_KeyCode Keyboard_SaveSlot6 { get; set; } // SDL keysym for save slot 6
    public SDL_KeyCode Keyboard_SaveSlot7 { get; set; } // SDL keysym for save slot 7
    public SDL_KeyCode Keyboard_SaveSlot8 { get; set; } // SDL keysym for save slot 8
    public SDL_KeyCode Keyboard_SaveSlot9 { get; set; } // SDL keysym for save slot 9

    public SDL_KeyCode Keyboard_Stop { get; set; } // SDL keysym for stopping the emulator
    public SDL_KeyCode Keyboard_Fullscreen { get; set; } // SDL keysym for switching between fullscreen/windowed modes
    public SDL_KeyCode Keyboard_SaveState { get; set; } // SDL keysym for saving the emulator state
    public SDL_KeyCode Keyboard_LoadState { get; set; } // SDL keysym for loading the emulator state
    public SDL_KeyCode Keyboard_IncrementSaveSlot { get; set; } // SDL keysym for advancing the save state slot
    public SDL_KeyCode Keyboard_Reset { get; set; } // SDL keysym for resetting the emulator
    public SDL_KeyCode Keyboard_SpeedDown { get; set; } // SDL keysym for slowing down the emulator
    public SDL_KeyCode Keyboard_SpeedUp { get; set; } // SDL keysym for speeding up the emulator
    public SDL_KeyCode Keyboard_Screenshot { get; set; } // SDL keysym for taking a screenshot
    public SDL_KeyCode Keyboard_Pause { get; set; } // SDL keysym for pausing the emulator
    public SDL_KeyCode Keyboard_Mute { get; set; } // SDL keysym for muting/unmuting the sound
    public SDL_KeyCode Keyboard_DecreaseVolume { get; set; } // SDL keysym for increasing the volume
    public SDL_KeyCode Keyboard_IncreaseVolume { get; set; } // SDL keysym for decreasing the volume
    public SDL_KeyCode Keyboard_FastForward { get; set; } // SDL keysym for temporarily going really fast
    public SDL_KeyCode Keyboard_SpeedLimit { get; set; } // SDL keysym for toggling the framerate limiter
    public SDL_KeyCode Keyboard_FrameAdvance { get; set; } // SDL keysym for advancing by one frame when paused
    public SDL_KeyCode Keyboard_GSButton { get; set; } // SDL keysym for pressing the game shark button

    public string Joy_Stop { get; set; } // Joystick event string for stopping the emulator
    public string Joy_Fullscreen { get; set; } // Joystick event string for switching between fullscreen/windowed modes
    public string Joy_SaveState { get; set; } // Joystick event string for saving the emulator state
    public string Joy_LoadState { get; set; } // Joystick event string for loading the emulator state
    public string Joy_IncrementSaveSlot { get; set; } // Joystick event string for advancing the save state slot
    public string Joy_Reset { get; set; } // Joystick event string for resetting the emulator
    public string Joy_SpeedDown { get; set; } // Joystick event string for slowing down the emulator
    public string Joy_SpeedUp { get; set; } // Joystick event string for speeding up the emulator
    public string Joy_Screenshot { get; set; } // Joystick event string for taking a screenshot
    public string Joy_Pause { get; set; } // Joystick event string for pausing the emulator
    public string Joy_Mute { get; set; } // Joystick event string for muting/unmuting the sound
    public string Joy_DecreaseVolume { get; set; } // Joystick event string for increasing the volume
    public string Joy_IncreaseVolume { get; set; } // Joystick event string for decreasing the volume
    public string Joy_FastForward { get; set; } // Joystick event string for fast-forward
    public string Joy_SpeedLimit { get; set; } // [Doesn't have a ConfigSetDefaultString call in M64p]

    public string Joy_FrameAdvance { get; set; } // Joystick event string for advancing by one frame when paused
    public string Joy_GSButton { get; set; } // Joystick event string for pressing the game shark button

    public ConfigCoreEvents() {
        SetDefaults();
    }

    public override void SetDefaults() {
        ConfigSection = "CoreEvents";
        Version = new ConfigVersion(1.00f, 1);

        Keyboard_SaveSlot0 = SDL_KeyCode.SDLK_0; // "Kbd Mapping Slot 0"                        
        Keyboard_SaveSlot1 = SDL_KeyCode.SDLK_1; // "Kbd Mapping Slot 1"                         
        Keyboard_SaveSlot2 = SDL_KeyCode.SDLK_2; // "Kbd Mapping Slot 2"                        
        Keyboard_SaveSlot3 = SDL_KeyCode.SDLK_3; // "Kbd Mapping Slot 3"                         
        Keyboard_SaveSlot4 = SDL_KeyCode.SDLK_4; // "Kbd Mapping Slot 4"                         
        Keyboard_SaveSlot5 = SDL_KeyCode.SDLK_5; // "Kbd Mapping Slot 5"                        
        Keyboard_SaveSlot6 = SDL_KeyCode.SDLK_6; // "Kbd Mapping Slot 6"                         
        Keyboard_SaveSlot7 = SDL_KeyCode.SDLK_7; // "Kbd Mapping Slot 7"                         
        Keyboard_SaveSlot8 = SDL_KeyCode.SDLK_8; // "Kbd Mapping Slot 8"                        
        Keyboard_SaveSlot9 = SDL_KeyCode.SDLK_9; // "Kbd Mapping Slot 9"

        Keyboard_Stop = SDL_KeyCode.SDLK_ESCAPE; // "Kbd Mapping Stop"
        Keyboard_Fullscreen = SDL_KeyCode.SDLK_F11; // "Kbd Mapping Fullscreen"
        Keyboard_SaveState = SDL_KeyCode.SDLK_F5; // "Kbd Mapping Save State"
        Keyboard_LoadState = SDL_KeyCode.SDLK_F7; // "Kbd Mapping Load State"
        Keyboard_IncrementSaveSlot = SDL_KeyCode.SDLK_KP_PLUS; // "Kbd Mapping Increment Slot"
        Keyboard_Reset = SDL_KeyCode.SDLK_F9; // "Kbd Mapping Reset"
        Keyboard_SpeedDown = SDL_KeyCode.SDLK_F1; // "Kbd Mapping Speed Down"
        Keyboard_SpeedUp = SDL_KeyCode.SDLK_F2; // "Kbd Mapping Speed Up"
        Keyboard_Screenshot = SDL_KeyCode.SDLK_F12; // "Kbd Mapping Screenshot"
        Keyboard_Pause = SDL_KeyCode.SDLK_p; // "Kbd Mapping Pause"
        Keyboard_Mute = SDL_KeyCode.SDLK_m; // "Kbd Mapping Mute"
        Keyboard_DecreaseVolume = SDL_KeyCode.SDLK_LEFTBRACKET; // "Kbd Mapping Decrease Volume"
        Keyboard_IncreaseVolume = SDL_KeyCode.SDLK_RIGHTBRACKET; // "Kbd Mapping Increase Volume"
        Keyboard_FastForward = SDL_KeyCode.SDLK_f; // "Kbd Mapping Fast Forward"
        Keyboard_SpeedLimit = SDL_KeyCode.SDLK_y; // "Kbd Mapping Speed Limiter Toggle"
        Keyboard_FrameAdvance = SDL_KeyCode.SDLK_SLASH; // "Kbd Mapping Frame Advance"
        Keyboard_GSButton = SDL_KeyCode.SDLK_g; // "Kbd Mapping Gameshark"

        Joy_Stop = ""; // "Joy Mapping Stop"
        Joy_Fullscreen = ""; // "Joy Mapping Fullscreen"
        Joy_SaveState = ""; // "Joy Mapping Save State"
        Joy_LoadState = ""; // "Joy Mapping Load State"
        Joy_IncrementSaveSlot = ""; // "Joy Mapping Increment Slot"
        Joy_Reset = ""; // "Joy Mapping Reset"
        Joy_SpeedDown = ""; // "Joy Mapping Speed Down"
        Joy_SpeedUp = ""; // "Joy Mapping Speed Up"
        Joy_Screenshot = ""; // "Joy Mapping Screenshot"
        Joy_Pause = ""; // "Joy Mapping Pause"
        Joy_Mute = ""; // "Joy Mapping Mute"
        Joy_DecreaseVolume = ""; // "Joy Mapping Decrease Volume"
        Joy_IncreaseVolume = ""; // "Joy Mapping Increase Volume"
        Joy_FastForward = ""; // "Joy Mapping Fast Forward"
        Joy_SpeedLimit = ""; // "Joy Mapping Speed Limiter Toggle"
        Joy_FrameAdvance = ""; // "Joy Mapping Frame Advance"
        Joy_GSButton = ""; // "Joy Mapping Gameshark"
    }

    private string GetInputFunc(string input) {
        if (input.Contains("SaveSlot")) {
            return "Slot";
        }
        else if (input.Contains("SaveState")) {
            return "Save State";
        }
        else if (input.Contains("LoadState")) {
            return "Load State";
        }
        else if (input.Contains("IncrementSaveSlot")) {
            return "Increment Slot";
        }
        else if (input.Contains("SpeedDown")) {
            return "Speed Down";
        }
        else if (input.Contains("SpeedUp")) {
            return "Speed Up";
        }
        else if (input.Contains("DecreaseVolume")) {
            return "Decrease Volume";
        }
        else if (input.Contains("IncreaseVolume")) {
            return "Increase Volume";
        }
        else if (input.Contains("FastForward")) {
            return "Fast Forward";
        }
        else if (input.Contains("SpeedLimit")) {
            return "Speed Limiter Toggle";
        }
        else if (input.Contains("GSButton")) {
            return "Gameshark";
        }

        return input;
    }

    public override string GetRealConfigName(string input) {
        string func = "";

        var saveSlotsKbd = input.Split("Keyboard_SaveSlot");
        if (saveSlotsKbd.Length > 1 ) {
            return $"Kbd Mapping Slot {saveSlotsKbd[1]}";
        }

        var kbd = input.Split("Keyboard_");
        if (kbd.Length > 0) {
            return $"Kbd Mapping {func}";
        }

        var joy = input.Split("Joy_");
        if (joy.Length > 0) {
            return $"Joy Mapping {func}";
        }

        return input;
    }
}

public class ConfigVideoGeneral : ConfigBase {
    public bool Fullscreen { get; set; } // Use fullscreen mode if true or windowed mode if false.
    public bool VerticalSync { get; set; } // If true, activate the SDL_GL_SWAP_CONTROL attribute.
    public int ScreenWidth { get; set; } // Width of output window or fullscreen width.
    public int ScreenHeight { get; set; } // Height of output window or fullscreen height.

    public ConfigVideoGeneral() {
        SetDefaults();
    }

    public override void SetDefaults() {
        ConfigSection = "Video-General";
        Version = new ConfigVersion(1.0f, 1);
        Fullscreen = false;
        VerticalSync = true;
        ScreenWidth = 640;
        ScreenHeight = 480;
    }

    public override string GetRealConfigName(string input) {
        return input;
    }
}


public class Configuration {
    [JsonIgnore] private const string ConfigFile = "config/ML64Config.json";

    public ConfigCore Core { get; set; } = new ConfigCore();
    public ConfigCoreEvents CoreEvents { get; set; } = new ConfigCoreEvents();
    public ConfigVideoGeneral VideoGeneral { get; set; } = new ConfigVideoGeneral();
    public ConfigGLN64 GLN64 { get; set; } = new ConfigGLN64();

    /// <summary>
    /// Save a configuration
    /// </summary>
    /// <param name="config"></param>
    public static void Save(ref Configuration config) {
        try {
            string json = JsonSerializer.Serialize(config);
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception e) {
            Logger.Error($"[Configuration] An error occurred during write: {e.Message}");
        }
    }

    /// <summary>
    /// Load a configuration
    /// </summary>
    /// <param name="config"></param>
    /// <returns>true if new file</returns>
    public static bool Load(out Configuration config) {
        try {
            if (File.Exists(ConfigFile)) {
                string json = File.ReadAllText(ConfigFile);
                config = JsonSerializer.Deserialize<Configuration>(json);
                return false;
            }
        }
        catch (Exception e) {
            Logger.Error($"[Configuration] An error occurred during load: {e.Message}");
        }

        config = new Configuration();
        return true;
    }

}

