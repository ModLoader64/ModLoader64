using ModLoader64.Core;
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
        [FieldOffset(0x00)] public u32 Address;
        [FieldOffset(0x04)] public u32 EndAddress;
        [FieldOffset(0x08)] public u32 Flags;
    };

    #region Delegate Types
    public delegate M64Error DebugSetCoreCompareDelegate();
    public delegate M64Error DebugSetCallbacksDelegate();
    public delegate M64Error DebugSetRunStateDelegate(M64DebuggerRunState runstate);
    public delegate s32 DebugGetStateDelegate(M64DebuggerState statenum);
    public delegate M64Error DebugStepDelegate();
    public delegate void DebugDecodeOpDelegate(u32 instruction, char* op, char* args, s32 pc);
    public delegate void DebugMemGetRecompInfoDelegate(M64DebuggerMemoryInfo recomp_type, u32 address, s32 index);
    public delegate s32 DebugMemGetMemInfoDelegate(M64DebuggerMemoryInfo mem_info_type, u32 address);
    public delegate void DebugMemGetPointerDelegate(M64DebuggerMemoryPointerType mem_ptr_type);
    public delegate u32 DebugMemRead64Delegate(u32 address);
    public delegate u32 DebugMemRead32Delegate(u32 address);
    public delegate u32 DebugMemRead16Delegate(u32 address);
    public delegate u32 DebugMemRead8Delegate(u32 address);
    public delegate void DebugMemWrite64Delegate(u32 address, u64 value);
    public delegate void DebugMemWrite32Delegate(u32 address, u32 value);
    public delegate void DebugMemWrite16Delegate(u32 address, u16 value);
    public delegate void DebugMemWrite8Delegate(u32 address, u8 value);
    public delegate void DebugGetCPUDataPtrDelegate(M64DebuggerCpuData cpu_data_type);
    public delegate s32 DebugBreakpointLookupDelegate(u32 address, u32 size, u32 flags);
    public delegate s32 DebugBreakpointCommandDelegate(M64DebuggerBreakpointCommand command, u32 index, Breakpoint* bkp);
    public delegate void DebugBreakpointTriggeredByDelegate(UIntPtr flags, UIntPtr accessed);
    public delegate u32 DebugVirtualToPhysicalDelegate(u32 address);
    #endregion

    #region Delegate Instances
    public static DebugSetCoreCompareDelegate DebugSetCoreCompare;
    public static DebugSetCallbacksDelegate DebugSetCallbacks;
    public static DebugSetRunStateDelegate DebugSetRunState;
    public static DebugGetStateDelegate DebugGetState;
    public static DebugStepDelegate DebugStep;
    public static DebugDecodeOpDelegate DebugDecodeOp;
    public static DebugMemGetRecompInfoDelegate DebugMemGetRecompInfo;
    public static DebugMemGetMemInfoDelegate DebugMemGetMemInfo;
    public static DebugMemGetPointerDelegate DebugMemGetPointer;
    public static DebugMemRead64Delegate DebugMemRead64;
    public static DebugMemRead32Delegate DebugMemRead32;
    public static DebugMemRead16Delegate DebugMemRead16;
    public static DebugMemRead8Delegate DebugMemRead8;
    public static DebugMemWrite64Delegate DebugMemWrite64;
    public static DebugMemWrite32Delegate DebugMemWrite32;
    public static DebugMemWrite16Delegate DebugMemWrite16;
    public static DebugMemWrite8Delegate DebugMemWrite8;
    public static DebugGetCPUDataPtrDelegate DebugGetCPUDataPtr;
    public static DebugBreakpointLookupDelegate DebugBreakpointLookup;
    public static DebugBreakpointCommandDelegate DebugBreakpointCommand;
    public static DebugBreakpointTriggeredByDelegate DebugBreakpointTriggeredBy;
    public static DebugVirtualToPhysicalDelegate DebugVirtualToPhysical;
    #endregion
    
    static Debugger() {
        DebugSetCoreCompare = Natives.GetDelegateInstance<DebugSetCoreCompareDelegate>("DebugSetCoreCompare");
        DebugSetCallbacks = Natives.GetDelegateInstance<DebugSetCallbacksDelegate>("DebugSetCallbacks");
        DebugSetRunState = Natives.GetDelegateInstance<DebugSetRunStateDelegate>("DebugSetRunState");
        DebugGetState = Natives.GetDelegateInstance<DebugGetStateDelegate>("DebugGetState");
        DebugStep = Natives.GetDelegateInstance<DebugStepDelegate>("DebugStep");
        DebugDecodeOp = Natives.GetDelegateInstance<DebugDecodeOpDelegate>("DebugDecodeOp");
        DebugMemGetRecompInfo = Natives.GetDelegateInstance<DebugMemGetRecompInfoDelegate>("DebugMemGetRecompInfo");
        DebugMemGetMemInfo = Natives.GetDelegateInstance<DebugMemGetMemInfoDelegate>("DebugMemGetMemInfo");
        DebugMemGetPointer = Natives.GetDelegateInstance<DebugMemGetPointerDelegate>("DebugMemGetPointer");
        DebugMemRead64 = Natives.GetDelegateInstance<DebugMemRead64Delegate>("DebugMemRead64");
        DebugMemRead32 = Natives.GetDelegateInstance<DebugMemRead32Delegate>("DebugMemRead32");
        DebugMemRead16 = Natives.GetDelegateInstance<DebugMemRead16Delegate>("DebugMemRead16");
        DebugMemRead8 = Natives.GetDelegateInstance<DebugMemRead8Delegate>("DebugMemRead8");
        DebugMemWrite64 = Natives.GetDelegateInstance<DebugMemWrite64Delegate>("DebugMemWrite64");
        DebugMemWrite32 = Natives.GetDelegateInstance<DebugMemWrite32Delegate>("DebugMemWrite32");
        DebugMemWrite16 = Natives.GetDelegateInstance<DebugMemWrite16Delegate>("DebugMemWrite16");
        DebugMemWrite8 = Natives.GetDelegateInstance<DebugMemWrite8Delegate>("DebugMemWrite8");
        DebugGetCPUDataPtr = Natives.GetDelegateInstance<DebugGetCPUDataPtrDelegate>("DebugGetCPUDataPtr");
        DebugBreakpointLookup = Natives.GetDelegateInstance<DebugBreakpointLookupDelegate>("DebugBreakpointLookup");
        DebugBreakpointCommand = Natives.GetDelegateInstance<DebugBreakpointCommandDelegate>("DebugBreakpointCommand");
        DebugBreakpointTriggeredBy = Natives.GetDelegateInstance<DebugBreakpointTriggeredByDelegate>("DebugBreakpointTriggeredBy");
        DebugVirtualToPhysical = Natives.GetDelegateInstance<DebugVirtualToPhysicalDelegate>("DebugVirtualToPhysical");
    }
}
