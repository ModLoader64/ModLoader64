using ModLoader64.Core;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static Frontend;
using static VideoExtension;

public static unsafe class Plugin {
    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    [StructLayout(LayoutKind.Explicit, Size = 0x58)]
    public struct AudioInfo {
        [FieldOffset(0x00)] public u8* RDRAM;
        [FieldOffset(0x08)] public u8* DMEM;
        [FieldOffset(0x10)] public u8* IMEM;

        [FieldOffset(0x18)] public UIntPtr MI_INTR_REG;

        [FieldOffset(0x20)] public UIntPtr AI_DRAM_ADDR_REG;
        [FieldOffset(0x28)] public UIntPtr AI_LEN_REG;
        [FieldOffset(0x30)] public UIntPtr AI_CONTROL_REG;
        [FieldOffset(0x38)] public UIntPtr AI_STATUS_REG;
        [FieldOffset(0x40)] public UIntPtr AI_DACRATE_REG;
        [FieldOffset(0x48)] public UIntPtr AI_BITRATE_REG;

        [FieldOffset(0x50)] public M64FunctionDelegate CheckInterrupts;
    };

    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    [StructLayout(LayoutKind.Explicit, Size = 0xF4)]
    public struct GfxInfo {
        [FieldOffset(0x00)] public u8* HEADER;
        [FieldOffset(0x08)] public u8* RDRAM;
        [FieldOffset(0x10)] public u8* DMEM;
        [FieldOffset(0x18)] public u8* IMEM;

        [FieldOffset(0x20)] public u32* MI_INTR_REG;

        [FieldOffset(0x28)] public u32* DPC_START_REG;
        [FieldOffset(0x30)] public u32* DPC_END_REG;
        [FieldOffset(0x38)] public u32* DPC_CURRENT_REG;
        [FieldOffset(0x40)] public u32* DPC_STATUS_REG;
        [FieldOffset(0x48)] public u32* DPC_CLOCK_REG;
        [FieldOffset(0x50)] public u32* DPC_BUFBUSY_REG;
        [FieldOffset(0x58)] public u32* DPC_PIPEBUSY_REG;
        [FieldOffset(0x60)] public u32* DPC_TMEM_REG;

        [FieldOffset(0x68)] public u32* VI_STATUS_REG;
        [FieldOffset(0x70)] public u32* VI_ORIGIN_REG;
        [FieldOffset(0x78)] public u32* VI_WIDTH_REG;
        [FieldOffset(0x80)] public u32* VI_INTR_REG;
        [FieldOffset(0x88)] public u32* VI_V_CURRENT_LINE_REG;
        [FieldOffset(0x90)] public u32* VI_TIMING_REG;
        [FieldOffset(0x98)] public u32* VI_V_SYNC_REG;
        [FieldOffset(0xA0)] public u32* VI_H_SYNC_REG;
        [FieldOffset(0xA8)] public u32* VI_LEAP_REG;
        [FieldOffset(0xB0)] public u32* VI_H_START_REG;
        [FieldOffset(0xB8)] public u32* VI_V_START_REG;
        [FieldOffset(0xC0)] public u32* VI_V_BURST_REG;
        [FieldOffset(0xC8)] public u32* VI_X_SCALE_REG;
        [FieldOffset(0xD0)] public u32* VI_Y_SCALE_REG;


        [FieldOffset(0xD8)] public M64FunctionDelegate CheckInterrupts;

        [FieldOffset(0xE0)] public u32 Version;

        [FieldOffset(0xE4)] public u32* SP_STATUS_REG;
        [FieldOffset(0xEC)] public u32* RDRAM_SIZE;
    };

    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    [StructLayout(LayoutKind.Explicit, Size = 0xD0)]
    public struct RSPInfo {
        [FieldOffset(0x00)] public IntPtr RDRAM;
        [FieldOffset(0x08)] public IntPtr DMEM;
        [FieldOffset(0x10)] public IntPtr IMEM;

        [FieldOffset(0x18)] public UIntPtr MI_INTR_REG;

        [FieldOffset(0x20)] public UIntPtr SP_MEM_ADDR_REG;
        [FieldOffset(0x28)] public UIntPtr SP_DRAM_ADDR_REG;
        [FieldOffset(0x30)] public UIntPtr SP_RD_LEN_REG;
        [FieldOffset(0x38)] public UIntPtr SP_WR_LEN_REG;
        [FieldOffset(0x40)] public UIntPtr SP_STATUS_REG;
        [FieldOffset(0x48)] public UIntPtr SP_DMA_FULL_REG;
        [FieldOffset(0x50)] public UIntPtr SP_DMA_BUSY_REG;
        [FieldOffset(0x58)] public UIntPtr SP_PC_REG;
        [FieldOffset(0x60)] public UIntPtr SP_SEMAPHORE_REG;

        [FieldOffset(0x68)] public UIntPtr DPC_START_REG;
        [FieldOffset(0x70)] public UIntPtr DPC_END_REG;
        [FieldOffset(0x78)] public UIntPtr DPC_CURRENT_REG;
        [FieldOffset(0x80)] public UIntPtr DPC_STATUS_REG;
        [FieldOffset(0x88)] public UIntPtr DPC_CLOCK_REG;
        [FieldOffset(0x90)] public UIntPtr DPC_BUFBUSY_REG;
        [FieldOffset(0x98)] public UIntPtr DPC_PIPEBUSY_REG;
        [FieldOffset(0xA0)] public UIntPtr DPC_TMEM_REG;

        [FieldOffset(0xA8)] public M64FunctionDelegate CheckInterrupts;
        [FieldOffset(0xB0)] public M64FunctionDelegate ProcessDlistList;
        [FieldOffset(0xB8)] public M64FunctionDelegate ProcessAlistList;
        [FieldOffset(0xC0)] public M64FunctionDelegate ProcessRdpList;
        [FieldOffset(0xC8)] public M64FunctionDelegate ShowCFB;
    };

    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct ControlInfo {
        [FieldOffset(0x00)] public s32 Present;
        [FieldOffset(0x04)] public s32 RawData;
        [FieldOffset(0x08)] public s32 Plugin;
        [FieldOffset(0x0C)] public s32 Type;
    }

    #region Delegate Types
    public delegate s32 RomOpenDelegate();
    public delegate void RomClosedDelegate();
    public delegate void ChangeWindowDelegate();
    public delegate s32 InitiateGFXDelegate(GfxInfo Gfx_Info);
    public delegate void MoveScreenDelegate(s32 x, s32 y);
    public delegate void ProcessDListDelegate();
    public delegate void ProcessRDPListDelegate();
    public delegate void ShowCFBDelegate();
    public delegate void UpdateScreenDelegate();
    public delegate void ViStatusChangedDelegate();
    public delegate void ViWidthChangedDelegate();
    public delegate void ReadScreen2Delegate(s32* dest, s32* width, s32* height, s32 front);
    public delegate void SetRenderingCallbackDelegate();
    public delegate void ResizeVideoOutputDelegate(s32 width, s32 height);
    public delegate void FBReadDelegate(u32 addr);
    public delegate void FBWriteDelegate(u32 addr, u32 size);
    public delegate void FBGetFrameBufferInfoDelegate(s32* p);
    public delegate void AiDacrateChangedDelegate(s32 SystemType);
    public delegate void AiLenChangedDelegate();
    public delegate s32 InitiateAudioDelegate(AudioInfo Audio_Info);
    public delegate void ProcessAListDelegate();
    public delegate void SetSpeedFactorDelegate(s32 percent);
    public delegate void VolumeUpDelegate();
    public delegate void VolumeDownDelegate();
    public delegate s32 VolumeGetLevelDelegate();
    public delegate void VolumeSetLevelDelegate(s32 level);
    public delegate void VolumeMuteDelegate();
    public delegate char* VolumeGetStringDelegate();
    public delegate void ControllerCommandDelegate(s32 Control, u8* Command);
    public delegate void GetKeysDelegate(s32 Control, Buttons* Keys);
    public delegate void InitiateControllersDelegate(ControlInfo ControlInfo);
    public delegate void ReadControllerDelegate(s32 Control, u8* Command);
    public delegate void SDL_KeyDownDelegate(s32 keymod, s32 keysym);
    public delegate void SDL_KeyUpDelegate(s32 keymod, s32 keysym);
    public delegate void RenderCallbackDelegate();
    public delegate void SendVRUWordDelegate(u16 length, u16* word, u8 lang);
    public delegate void SetMicStateDelegate(s32 state);
    public delegate void ReadVRUResultsDelegate(u16* error_flags, u16* num_results, u16* mic_level, u16* voice_level, u16* voice_length, u16* matches);
    public delegate void ClearVRUWordsDelegate(u8 length);
    public delegate void SetVRUWordMaskDelegate(u8 length, u8* mask);
    public delegate u32 DoRspCyclesDelegate(u32 Cycles);
    public delegate void InitiateRSPDelegate(RSPInfo Rsp_Info, u32* CycleCount);
    #endregion

    #region Delegate Instances
    public static RomOpenDelegate RomOpen;
    public static RomClosedDelegate RomClosed;
    public static ChangeWindowDelegate ChangeWindow;
    public static InitiateGFXDelegate InitiateGFX;
    public static MoveScreenDelegate MoveScreen;
    public static ProcessDListDelegate ProcessDList;
    public static ProcessRDPListDelegate ProcessRDPList;
    public static ShowCFBDelegate ShowCFB;
    public static UpdateScreenDelegate UpdateScreen;
    public static ViStatusChangedDelegate ViStatusChanged;
    public static ViWidthChangedDelegate ViWidthChanged;
    public static ReadScreen2Delegate ReadScreen2;
    public static SetRenderingCallbackDelegate SetRenderingCallback;
    public static ResizeVideoOutputDelegate ResizeVideoOutput;
    public static FBReadDelegate FBRead;
    public static FBWriteDelegate FBWrite;
    public static FBGetFrameBufferInfoDelegate FBGetFrameBufferInfo;
    public static AiDacrateChangedDelegate AiDacrateChanged;
    public static AiLenChangedDelegate AiLenChanged;
    public static InitiateAudioDelegate InitiateAudio;
    public static ProcessAListDelegate ProcessAList;
    public static SetSpeedFactorDelegate SetSpeedFactor;
    public static VolumeUpDelegate VolumeUp;
    public static VolumeDownDelegate VolumeDown;
    public static VolumeGetLevelDelegate VolumeGetLevel;
    public static VolumeSetLevelDelegate VolumeSetLevel;
    public static VolumeMuteDelegate VolumeMute;
    public static VolumeGetStringDelegate VolumeGetString;
    public static ControllerCommandDelegate ControllerCommand;
    public static GetKeysDelegate GetKeys;
    public static InitiateControllersDelegate InitiateControllers;
    public static ReadControllerDelegate ReadController;
    public static SDL_KeyDownDelegate SDL_KeyDown;
    public static SDL_KeyUpDelegate SDL_KeyUp;
    public static RenderCallbackDelegate RenderCallback;
    public static SendVRUWordDelegate SendVRUWord;
    public static SetMicStateDelegate SetMicState;
    public static ReadVRUResultsDelegate ReadVRUResults;
    public static ClearVRUWordsDelegate ClearVRUWords;
    public static SetVRUWordMaskDelegate SetVRUWordMask;
    public static DoRspCyclesDelegate DoRspCycles;
    public static InitiateRSPDelegate InitiateRSP;
    #endregion

    static Plugin() {
        RomOpen = Natives.GetDelegateInstance<RomOpenDelegate>("RomOpen");
        RomClosed = Natives.GetDelegateInstance<RomClosedDelegate>("RomClosed");
        ChangeWindow = Natives.GetDelegateInstance<ChangeWindowDelegate>("ChangeWindow");
        InitiateGFX = Natives.GetDelegateInstance<InitiateGFXDelegate>("InitiateGFX");
        MoveScreen = Natives.GetDelegateInstance<MoveScreenDelegate>("MoveScreen");
        ProcessDList = Natives.GetDelegateInstance<ProcessDListDelegate>("ProcessDList");
        ProcessRDPList = Natives.GetDelegateInstance<ProcessRDPListDelegate>("ProcessRDPList");
        ShowCFB = Natives.GetDelegateInstance<ShowCFBDelegate>("ShowCFB");
        UpdateScreen = Natives.GetDelegateInstance<UpdateScreenDelegate>("UpdateScreen");
        ViStatusChanged = Natives.GetDelegateInstance<ViStatusChangedDelegate>("ViStatusChanged");
        ViWidthChanged = Natives.GetDelegateInstance<ViWidthChangedDelegate>("ViWidthChanged");
        ReadScreen2 = Natives.GetDelegateInstance<ReadScreen2Delegate>("ReadScreen2");
        SetRenderingCallback = Natives.GetDelegateInstance<SetRenderingCallbackDelegate>("SetRenderingCallback");
        ResizeVideoOutput = Natives.GetDelegateInstance<ResizeVideoOutputDelegate>("ResizeVideoOutput");
        FBRead = Natives.GetDelegateInstance<FBReadDelegate>("FBRead");
        FBWrite = Natives.GetDelegateInstance<FBWriteDelegate>("FBWrite");
        FBGetFrameBufferInfo = Natives.GetDelegateInstance<FBGetFrameBufferInfoDelegate>("FBGetFrameBufferInfo");
        AiDacrateChanged = Natives.GetDelegateInstance<AiDacrateChangedDelegate>("AiDacrateChanged");
        AiLenChanged = Natives.GetDelegateInstance<AiLenChangedDelegate>("AiLenChanged");
        InitiateAudio = Natives.GetDelegateInstance<InitiateAudioDelegate>("InitiateAudio");
        ProcessAList = Natives.GetDelegateInstance<ProcessAListDelegate>("ProcessAList");
        SetSpeedFactor = Natives.GetDelegateInstance<SetSpeedFactorDelegate>("SetSpeedFactor");
        VolumeUp = Natives.GetDelegateInstance<VolumeUpDelegate>("VolumeUp");
        VolumeDown = Natives.GetDelegateInstance<VolumeDownDelegate>("VolumeDown");
        VolumeGetLevel = Natives.GetDelegateInstance<VolumeGetLevelDelegate>("VolumeGetLevel");
        VolumeSetLevel = Natives.GetDelegateInstance<VolumeSetLevelDelegate>("VolumeSetLevel");
        VolumeMute = Natives.GetDelegateInstance<VolumeMuteDelegate>("VolumeMute");
        VolumeGetString = Natives.GetDelegateInstance<VolumeGetStringDelegate>("VolumeGetString");
        ControllerCommand = Natives.GetDelegateInstance<ControllerCommandDelegate>("ControllerCommand");
        GetKeys = Natives.GetDelegateInstance<GetKeysDelegate>("GetKeys");
        InitiateControllers = Natives.GetDelegateInstance<InitiateControllersDelegate>("InitiateControllers");
        ReadController = Natives.GetDelegateInstance<ReadControllerDelegate>("ReadController");
        SDL_KeyDown = Natives.GetDelegateInstance<SDL_KeyDownDelegate>("SDL_KeyDown");
        SDL_KeyUp = Natives.GetDelegateInstance<SDL_KeyUpDelegate>("SDL_KeyUp");
        RenderCallback = Natives.GetDelegateInstance<RenderCallbackDelegate>("RenderCallback");
        SendVRUWord = Natives.GetDelegateInstance<SendVRUWordDelegate>("SendVRUWord");
        SetMicState = Natives.GetDelegateInstance<SetMicStateDelegate>("SetMicState");
        ReadVRUResults = Natives.GetDelegateInstance<ReadVRUResultsDelegate>("ReadVRUResults");
        ClearVRUWords = Natives.GetDelegateInstance<ClearVRUWordsDelegate>("ClearVRUWords");
        SetVRUWordMask = Natives.GetDelegateInstance<SetVRUWordMaskDelegate>("SetVRUWordMask");
        DoRspCycles = Natives.GetDelegateInstance<DoRspCyclesDelegate>("DoRspCycles");
        InitiateRSP = Natives.GetDelegateInstance<InitiateRSPDelegate>("InitiateRSP");
    }
}
