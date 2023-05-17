using System;
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

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static s32 RomOpen();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void RomClosed();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ChangeWindow();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static s32 InitiateGFX(GfxInfo Gfx_Info);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void MoveScreen(s32 x, s32 y);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ProcessDList();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ProcessRDPList();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ShowCFB();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void UpdateScreen();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ViStatusChanged();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ViWidthChanged();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ReadScreen2(s32* dest, s32* width, s32* height, s32 front);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetRenderingCallback();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ResizeVideoOutput(s32 width, s32 height);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBRead(u32 addr);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBWrite(u32 addr, u32 size);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBGetFrameBufferInfo(s32* p);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void AiDacrateChanged(s32 SystemType);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void AiLenChanged();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static s32 InitiateAudio(AudioInfo Audio_Info);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ProcessAList();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSpeedFactor(s32 percent);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeUp();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeDown();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static s32 VolumeGetLevel();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeSetLevel(s32 level);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeMute();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* VolumeGetString();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ControllerCommand(s32 Control, u8* Command);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetKeys(s32 Control, Buttons* Keys);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void InitiateControllers(ControlInfo ControlInfo);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ReadController(s32 Control, u8* Command);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SDL_KeyDown(s32 keymod, s32 keysym);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SDL_KeyUp(s32 keymod, s32 keysym);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void RenderCallback();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SendVRUWord(u16 length, u16* word, u8 lang);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetMicState(s32 state);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ReadVRUResults(u16* error_flags, u16* num_results, u16* mic_level, u16* voice_level, u16* voice_length, u16* matches);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ClearVRUWords(u8 length);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVRUWordMask(u8 length, u8* mask);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static u32 DoRspCycles(u32 Cycles);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void InitiateRSP(RSPInfo Rsp_Info, u32* CycleCount);
}
