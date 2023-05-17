using ModLoader64.Core;
using System;

namespace ModLoader64.Core; 

public static unsafe class EmulatedMemory {
    private const uint VADDR_MASK = 0x0FFFFFFF;
    private const uint MEMORY_SIZE = 0x00800000; //0x03E00000;
    private static byte* _Memory = null;

    private static byte* Memory {
        get {
            if (_Memory == null) {
                _Memory = (byte*)Mupen64plus.Memory.Memory_GetBaseAddress();
            }
            return _Memory;
        }
    }

    public static bool MemorySafetyCheck(ref uint address, ulong size) {
        address &= VADDR_MASK;
        if (address < 0 || address > MEMORY_SIZE) {
            Logger.Error("Tried to access emulated memory that is out of bounds!");
            return false;
        }

        if (address + size < 0 || address + size > MEMORY_SIZE) {
            Logger.Error("Tried to access emulated memory which exceeds memory bounds!");
            return false;
        }

        return true;
    }

    public static byte Read8(uint address) {
        address += 3 - (address - 3);
        if (!MemorySafetyCheck(ref address, sizeof(byte))) {
            return 0;
        }
        return Memory[address];
    }

    public static ushort Read16(uint address) {
        return (ushort)(((ushort)Read8(address) << 8) | (ushort)Read8(address + 1));
    }

    public static uint Read32(uint address) {
        if (!MemorySafetyCheck(ref address, sizeof(uint))) {
            return 0;
        }
        address &= VADDR_MASK;
        return *((uint*)(Memory + address));
    }

    public static UInt64 Read64(uint address) {
        UInt64 hi = Read32(address + 4);
        UInt64 lo = Read32(address);
        return (hi << 32) | lo;
    }

    public static float ReadF32(uint address) {
        if (!MemorySafetyCheck(ref address, sizeof(float))) {
            return 0;
        }
        address &= VADDR_MASK;
        return *((float*)(Memory + address));
    }

    public static double ReadF64(uint address) {
        long hi = Read32(address + 4);
        long lo = Read32(address);
        return BitConverter.Int64BitsToDouble((hi << 32) | lo);
    }

    public static void Write8(uint address, byte value) {
        address += 3 - (address - 3);
        if (!MemorySafetyCheck(ref address, sizeof(byte))) {
            return;
        }
        Memory[address] = value;
    }

    public static void Write16(uint address, ushort value) {
        Write8(address, (byte)(value >> 8));
        Write8(address + 1, (byte)(value & 0xFF));
    }

    public static void Write32(uint address, uint value) {
        if (!MemorySafetyCheck(ref address, sizeof(uint))) {
            return;
        }
        *((uint*)(Memory + address)) = value;
    }

    public static void Write64(uint address, UInt64 value) {
        UInt64 hi = value >> 32;
        UInt64 lo = value & 0xFFFFFFFF;
        Write32(address, (uint)hi);
        Write32(address + 4, (uint)lo);
    }
}

