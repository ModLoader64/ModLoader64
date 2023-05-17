using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Modloader2.Mupen64plus.Frontend;
using static Modloader2.Mupen64plus.VideoExtension;

namespace Modloader2.Mupen64plus;

public static unsafe class Plugin {
    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    [StructLayout(LayoutKind.Explicit, Size = 0x58)]
    public struct AudioInfo {
        [FieldOffset(0x00)] public byte* RDRAM;
        [FieldOffset(0x08)] public byte* DMEM;
        [FieldOffset(0x10)] public byte* IMEM;

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
        [FieldOffset(0x00)] public byte* HEADER;
        [FieldOffset(0x08)] public byte* RDRAM;
        [FieldOffset(0x10)] public byte* DMEM;
        [FieldOffset(0x18)] public byte* IMEM;

        [FieldOffset(0x20)] public uint* MI_INTR_REG;

        [FieldOffset(0x28)] public uint* DPC_START_REG;
        [FieldOffset(0x30)] public uint* DPC_END_REG;
        [FieldOffset(0x38)] public uint* DPC_CURRENT_REG;
        [FieldOffset(0x40)] public uint* DPC_STATUS_REG;
        [FieldOffset(0x48)] public uint* DPC_CLOCK_REG;
        [FieldOffset(0x50)] public uint* DPC_BUFBUSY_REG;
        [FieldOffset(0x58)] public uint* DPC_PIPEBUSY_REG;
        [FieldOffset(0x60)] public uint* DPC_TMEM_REG;

        [FieldOffset(0x68)] public uint* VI_STATUS_REG;
        [FieldOffset(0x70)] public uint* VI_ORIGIN_REG;
        [FieldOffset(0x78)] public uint* VI_WIDTH_REG;
        [FieldOffset(0x80)] public uint* VI_INTR_REG;
        [FieldOffset(0x88)] public uint* VI_V_CURRENT_LINE_REG;
        [FieldOffset(0x90)] public uint* VI_TIMING_REG;
        [FieldOffset(0x98)] public uint* VI_V_SYNC_REG;
        [FieldOffset(0xA0)] public uint* VI_H_SYNC_REG;
        [FieldOffset(0xA8)] public uint* VI_LEAP_REG;
        [FieldOffset(0xB0)] public uint* VI_H_START_REG;
        [FieldOffset(0xB8)] public uint* VI_V_START_REG;
        [FieldOffset(0xC0)] public uint* VI_V_BURST_REG;
        [FieldOffset(0xC8)] public uint* VI_X_SCALE_REG;
        [FieldOffset(0xD0)] public uint* VI_Y_SCALE_REG;


        [FieldOffset(0xD8)] public M64FunctionDelegate CheckInterrupts;

        [FieldOffset(0xE0)] public uint Version;

        [FieldOffset(0xE4)] public uint* SP_STATUS_REG;
        [FieldOffset(0xEC)] public uint* RDRAM_SIZE;
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
        [FieldOffset(0x00)] public int Present;
        [FieldOffset(0x04)] public int RawData;
        [FieldOffset(0x08)] public int Plugin;
        [FieldOffset(0x0C)] public int Type;
    }

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int RomOpen();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void RomClosed();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ChangeWindow();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int InitiateGFX(GfxInfo Gfx_Info);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void MoveScreen(int x, int y);

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
    public extern static void ReadScreen2(int* dest, int* width, int* height, int front);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetRenderingCallback();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ResizeVideoOutput(int width, int height);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBRead(uint addr);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBWrite(uint addr, uint size);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void FBGetFrameBufferInfo(int* p);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void AiDacrateChanged(int SystemType);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void AiLenChanged();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int InitiateAudio(AudioInfo Audio_Info);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ProcessAList();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSpeedFactor(int percent);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeUp();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeDown();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int VolumeGetLevel();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeSetLevel(int level);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void VolumeMute();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static char* VolumeGetString();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ControllerCommand(int Control, byte* Command);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetKeys(int Control, Buttons* Keys);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void InitiateControllers(ControlInfo ControlInfo);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ReadController(int Control, byte* Command);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SDL_KeyDown(int keymod, int keysym);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SDL_KeyUp(int keymod, int keysym);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void RenderCallback();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SendVRUWord(ushort length, ushort* word, byte lang);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetMicState(int state);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ReadVRUResults(ushort* error_flags, ushort* num_results, ushort* mic_level, ushort* voice_level, ushort* voice_length, ushort* matches);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void ClearVRUWords(byte length);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVRUWordMask(byte length, byte* mask);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DoRspCycles(uint Cycles);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void InitiateRSP(RSPInfo Rsp_Info, uint* CycleCount);
}
