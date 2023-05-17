using System;
using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

using static Frontend;

public static unsafe class Debugger {
    public enum M64DebuggerRunState {
        M64P_DBG_RUNSTATE_PAUSED = 0,
        M64P_DBG_RUNSTATE_STEPPING,
        M64P_DBG_RUNSTATE_RUNNING
    };

    public enum M64DebuggerState {
        M64P_DBG_RUN_STATE = 1,
        M64P_DBG_PREVIOUS_PC,
        M64P_DBG_NUM_BREAKPOINTS,
        M64P_DBG_CPU_DYNACORE,
        M64P_DBG_CPU_NEXT_INTERRUPT
    };

    public enum M64DebuggerMemoryInfo {
        M64P_DBG_MEM_TYPE = 1,
        M64P_DBG_MEM_FLAGS,
        M64P_DBG_MEM_HAS_RECOMPILED,
        M64P_DBG_MEM_NUM_RECOMPILED,
        M64P_DBG_RECOMP_OPCODE = 16,
        M64P_DBG_RECOMP_ARGS,
        M64P_DBG_RECOMP_ADDR
    };

    public enum M64DebuggerMemoryPointerType {
        M64P_DBG_PTR_RDRAM = 1,
        M64P_DBG_PTR_PI_REG,
        M64P_DBG_PTR_SI_REG,
        M64P_DBG_PTR_VI_REG,
        M64P_DBG_PTR_RI_REG,
        M64P_DBG_PTR_AI_REG
    }

    public enum M64DebuggerCpuData {
        M64P_CPU_PC = 1,
        M64P_CPU_REG_REG,
        M64P_CPU_REG_HI,
        M64P_CPU_REG_LO,
        M64P_CPU_REG_COP0,
        M64P_CPU_REG_COP1_DOUBLE_PTR,
        M64P_CPU_REG_COP1_SIMPLE_PTR,
        M64P_CPU_REG_COP1_FGR_64,
        M64P_CPU_TLB
    };

    public enum M64DebuggerBreakpointCommand {
        M64P_BKP_CMD_ADD_ADDR = 1,
        M64P_BKP_CMD_ADD_STRUCT,
        M64P_BKP_CMD_REPLACE,
        M64P_BKP_CMD_REMOVE_ADDR,
        M64P_BKP_CMD_REMOVE_IDX,
        M64P_BKP_CMD_ENABLE,
        M64P_BKP_CMD_DISABLE,
        M64P_BKP_CMD_CHECK
    };

    [StructLayout(LayoutKind.Explicit, Size = 0x0C)]
    public struct Breakpoint {
        [FieldOffset(0x00)] public uint Address;
        [FieldOffset(0x04)] public uint EndAddress;
        [FieldOffset(0x08)] public uint Flags;
    };

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error DebugSetCoreCompare();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error DebugSetCallbacks();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error DebugSetRunState(M64DebuggerRunState runstate);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int DebugGetState(M64DebuggerState statenum);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static M64Error DebugStep();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugDecodeOp(uint instruction, char* op, char* args, int pc);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemGetRecompInfo(M64DebuggerMemoryInfo recomp_type, uint address, int index);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int DebugMemGetMemInfo(M64DebuggerMemoryInfo mem_info_type, uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemGetPointer(M64DebuggerMemoryPointerType mem_ptr_type);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DebugMemRead64(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DebugMemRead32(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DebugMemRead16(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DebugMemRead8(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemWrite64(uint address, UInt64 value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemWrite32(uint address, uint value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemWrite16(uint address, ushort value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugMemWrite8(uint address, byte value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugGetCPUDataPtr(M64DebuggerCpuData cpu_data_type);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int DebugBreakpointLookup(uint address, uint size, uint flags);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static int DebugBreakpointCommand(M64DebuggerBreakpointCommand command, uint index, Breakpoint* bkp);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void DebugBreakpointTriggeredBy(UIntPtr flags, UIntPtr accessed);
    
    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint DebugVirtualToPhysical(uint address);
}
