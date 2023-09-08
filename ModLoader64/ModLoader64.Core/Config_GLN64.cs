using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader64.Core;

public enum GLN64_AspectRatio : int {
    Stretch = 0,
    FourByThree,
    SixteenByNine,
    Adjust
}

public enum GLN64_BufferSwapMode : int {
    VI_Update = 0,
    VI_Origin,
    Buffer_Update
}

public enum GLN64_BilinearMode : int {
    N64 = 0,
    Standard = 1
}

public enum GLN64_RDRAMImageDitheringMode : int {
    Disable = 0,
    Bayer,
    Magic_Square,
    Blue_Noise
}

public enum GLN64_CorrectTextrectCoords : int {
    Off = 0,
    Auto,
    Force
}

public enum GLN64_EnableNativeResTexrects : int {
    Off = 0,
    Optimized,
    Unoptimized
}

public enum GLN64_BackgroundsMode : int {
    One_Piece = 0,
    Stripped
}

public enum GLN64_N64DepthCompare : int {
    Off = 0,
    Fast,
    Compatible
}

public enum GLN64_CopyColorToRDRAM : int {
    DoNotCopy = 0,
    CopyInSync,
    DoubleBuffer,
    TripleBuffer
}

public enum GLN64_CopyDepthToRDRAM : int {
    DoNotCopy = 0,
    CopyFromVideoMemory,
    UseSoftwareRender
}

public enum GLN64_TextureFilterMode : int {
    None,
    Smooth1,
    Smooth2,
    Smooth3,
    Smooth4,
    Sharp1,
    Sharp2
}

public enum GLN64_TextureEnhancementMode : int {
    None,
    StoreAsIs,
    X2,
    X2SAI,
    HQ2X,
    HQ2XS,
    LQ2X,
    LQ2XS,
    HQ4X,
    X2BRZ,
    X3BRZ,
    X4BRZ,
    X5BRZ,
    X6BRZ
}

public enum GLN64_CounterPos : int {
    TopLeft = (1 << 0),
    TopCenter = (1 << 1),
    TopRight = (1 << 2),
    BottomLeft = (1 << 3),
    BottomCenter = (1 << 4),
    BottomRight = (1 << 5)
}

public class ConfigGLN64 : ConfigBase {
    public bool ThreadedVideo { get; set; }
    public bool FXAA { get; set; }
    public bool enableHalosRemoval { get; set; }
    public bool EnableDitheringPattern { get; set; }
    public bool EnableHiresNoiseDithering { get; set; }
    public bool DitheringQuantization { get; set; }
    public bool EnableLOD { get; set; }
    public bool EnableHWLighting { get; set; }
    public bool EnableCoverage { get; set; }
    public bool EnableClipping { get; set; }
    public bool EnableShadersStorage { get; set; }
    public bool EnableLegacyBlending { get; set; }
    public bool EnableHybridFilter { get; set; }
    public bool EnableInaccurateTextureCoordinates { get; set; }
    public bool EnableFragmentDepthWrite { get; set; }
    public bool EnableCustomSettings { get; set; }
    public bool ForcePolygonOffset { get; set; }
    public bool EnableFBEmulation { get; set; }
    public bool EnableCopyAuxiliaryToRDRAM { get; set; }
    public bool ForceDepthBufferClear { get; set; }
    public bool DisableFBInfo { get; set; }
    public bool FBInfoReadColorChunk { get; set; }
    public bool FBInfoReadDepthChunk { get; set; }
    public bool EnableCopyColorFromRDRAM { get; set; }
    public bool EnableCopyDepthToMainDepthBuffer { get; set; }
    public bool EnableOverscan { get; set; }
    public bool txDeposterize { get; set; }
    public bool txFilterIgnoreBG { get; set; }
    public bool txHiresEnable { get; set; }
    public bool txHiresFullAlphaChannel { get; set; }
    public bool txHresAltCRC { get; set; }
    public bool txCacheCompression { get; set; }
    public bool txForce16bpp { get; set; }
    public bool txSaveCache { get; set; }
    public bool txDump { get; set; }
    public bool txEnhancedTextureFileStorage { get; set; }
    public bool txHiresTextureFileStorage { get; set; }
    public bool txNoTextureFileStorage { get; set; }
    public bool ForceGammaCorrection { get; set; }
    public bool ShowFPS { get; set; }
    public bool ShowVIS { get; set; }
    public bool ShowPercent { get; set; }
    public bool ShowInternalResolution { get; set; }
    public bool ShowRenderingResolution { get; set; }
    public bool ShowStatistics { get; set; }

    public float PolygonOffsetFactor { get; set; }
    public float PolygonOffsetUnits { get; set; }
    public float GammaCorrectionLevel { get; set; }

    public int MultiSampling { get; set; }
    public int UseNativeResolutionFactor { get; set; }
    public int anisotropy { get; set; }
    public int EnableTexCoordBounds { get; set; }
    public int OverscanPalLeft { get; set; }
    public int OverscanPalRight { get; set; }
    public int OverscanPalTop { get; set; }
    public int OverscanPalBottom { get; set; }
    public int OverscanNtscLeft { get; set; }
    public int OverscanNtscRight { get; set; }
    public int OverscanNtscTop { get; set; }
    public int OverscanNtscBottom { get; set; }
    public int txCacheSize { get; set; }
    public int txHiresVramLimit { get; set; }
    public int fontSize { get; set; }
    public int DebugDumpMode { get; set; }

    public string txPath { get; set; }
    public string txCachePath { get; set; }
    public string txDumpPath { get; set; }
    public string fontName { get; set; }
    public string fontColor { get; set; }
    public string hkTexDump  { get; set; }
    public string hkHdTexReload { get; set; }
    public string hkHdTexToggle  { get; set; }
    public string hkTexCoordBounds { get; set; }
    public string hkNativeResTexrects { get; set; }
    public string hkVsync { get; set; }
    public string hkFBEmulation { get; set; }
    public string hkN64DepthCompare { get; set; }
    public string hkOsdVis { get; set; }
    public string hkOsdFps  { get; set; }
    public string hkOsdPercent { get; set; }
    public string hkOsdInternalResolution { get; set; }
    public string hkOsdRenderingResolution { get; set; }
    public string hkForceGammaCorrection { get; set; }
    public string hkInaccurateTexCords { get; set; }

    public GLN64_AspectRatio AspectRatio { get; set; }
    public GLN64_BufferSwapMode BufferSwapMode { get; set; }
    public GLN64_BilinearMode bilinearMode { get; set; }
    public GLN64_RDRAMImageDitheringMode RDRAMImageDitheringMode { get; set; }
    public GLN64_CorrectTextrectCoords CorrectTexrectCoords { get; set; }
    public GLN64_EnableNativeResTexrects EnableNativeResTexrects { get; set; }
    public GLN64_BackgroundsMode BackgroundsMode { get; set; }
    public GLN64_N64DepthCompare EnableN64DepthCompare { get; set; }
    public GLN64_CopyColorToRDRAM EnableCopyColorToRDRAM { get; set; }
    public GLN64_CopyDepthToRDRAM EnableCopyDepthToRDRAM { get; set; }
    public GLN64_TextureFilterMode txFilterMode { get; set; }
    public GLN64_TextureEnhancementMode txEnhancementMode { get; set; }
    public GLN64_CounterPos CountersPos { get; set; }

    public ConfigGLN64() {
        SetDefaults();
    }

    public override void SetDefaults() {
        ConfigSection = "Video-GLideN64";
        Version = new ConfigVersion(29.0f, 1);

        ThreadedVideo = false;
        FXAA = false;
        enableHalosRemoval = false;
        EnableDitheringPattern = false;
        EnableHiresNoiseDithering = false;
        DitheringQuantization = true;
        EnableLOD = true;
        EnableHWLighting = false;
        EnableCoverage = false;
        EnableClipping = true;
        EnableShadersStorage = true;
        EnableLegacyBlending = false;
        EnableHybridFilter = true;
        EnableInaccurateTextureCoordinates = false;
        EnableFragmentDepthWrite = true;
        EnableCustomSettings = true;
        ForcePolygonOffset = false;
        EnableFBEmulation = true;
        EnableCopyAuxiliaryToRDRAM = false;
        ForceDepthBufferClear = false;
        DisableFBInfo = true;
        FBInfoReadColorChunk = false;
        FBInfoReadDepthChunk = true;
        EnableCopyColorFromRDRAM = false;
        EnableCopyDepthToMainDepthBuffer = false;
        EnableOverscan = false;
        txDeposterize = false;
        txFilterIgnoreBG = false;
        txHiresEnable = false;
        txHiresFullAlphaChannel = true;
        txHresAltCRC = false;
        txCacheCompression = false;
        txForce16bpp = false;
        txSaveCache = true;
        txDump = false;
        txEnhancedTextureFileStorage = false;
        txHiresTextureFileStorage = false;
        txNoTextureFileStorage = false;
        ForceGammaCorrection = false;
        ShowFPS = true;
        ShowVIS = true;
        ShowPercent = false;
        ShowInternalResolution = false;
        ShowRenderingResolution = false;
        ShowStatistics = false;

        PolygonOffsetFactor = 0.0f;
        PolygonOffsetUnits = 0.0f;
        GammaCorrectionLevel = 2.0f;

        MultiSampling = 0;
        UseNativeResolutionFactor = 0;
        anisotropy = 16;
        EnableTexCoordBounds = 0;
        OverscanPalLeft = 0;
        OverscanPalRight = 0;
        OverscanPalTop = 0;
        OverscanPalBottom = 0;
        OverscanNtscLeft = 0;
        OverscanNtscRight = 0;
        OverscanNtscTop = 0;
        OverscanNtscBottom = 0;
        txCacheSize = 100;
        txHiresVramLimit = 0;
        fontSize = 18;
        DebugDumpMode = 0;

        txPath = "textures/hires_texture";
        txCachePath = "textures/cache";
        txDumpPath = "textures/dump";
        fontName = "arial.ttf";
        fontColor = "B5E61D";
        hkTexDump = "";
        hkHdTexReload = "";
        hkHdTexToggle = "";
        hkTexCoordBounds = "";
        hkNativeResTexrects = "";
        hkVsync = "";
        hkFBEmulation = "";
        hkN64DepthCompare = "";
        hkOsdVis = "";
        hkOsdFps = "";
        hkOsdPercent = "";
        hkOsdInternalResolution = "";
        hkOsdRenderingResolution = "";
        hkForceGammaCorrection = "";
        hkInaccurateTexCords = "";

        AspectRatio = GLN64_AspectRatio.Adjust;
        BufferSwapMode = GLN64_BufferSwapMode.VI_Update;
        bilinearMode = GLN64_BilinearMode.Standard;
        RDRAMImageDitheringMode = GLN64_RDRAMImageDitheringMode.Blue_Noise;
        CorrectTexrectCoords = GLN64_CorrectTextrectCoords.Off;
        EnableNativeResTexrects = GLN64_EnableNativeResTexrects.Off;
        BackgroundsMode = GLN64_BackgroundsMode.Stripped;
        EnableN64DepthCompare = GLN64_N64DepthCompare.Off;
        EnableCopyColorToRDRAM = GLN64_CopyColorToRDRAM.DoubleBuffer;
        EnableCopyDepthToRDRAM = GLN64_CopyDepthToRDRAM.UseSoftwareRender;
        txFilterMode = GLN64_TextureFilterMode.None;
        txEnhancementMode = GLN64_TextureEnhancementMode.None;
        CountersPos = GLN64_CounterPos.BottomLeft;
    }

    public override string GetRealConfigName(string input) {
        return input;
    }
}


