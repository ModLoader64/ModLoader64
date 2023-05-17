using ModLoader64.Core;
using System;
using System.Runtime.CompilerServices;

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

    private static void EmitStackTrace(int skip) {
        Logger.Error(new System.Diagnostics.StackTrace(skip, true).ToString());
    }

    public static bool MemorySafetyCheck(ref uint address, int size) {
        int skip = 2;

        address &= VADDR_MASK;
        if (address < 0 || address > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory that is out of bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')}!");
            EmitStackTrace(skip);
            return false;
        }

        if (address + size < 0 || address + size > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory which exceeds memory bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')} with size 0x{size:X}");
            EmitStackTrace(skip);
            return false;
        }

        return true;
    }

    private static uint RotateAddress(uint address) {
        return ((address >> 2) * 4) + (3 - (address & 3));
    }

    public static byte Read8(uint address) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<byte>())) {
            return 0;
        }

        return Memory[address];
    }

    public static ushort Read16(uint address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<ushort>())) {
            return 0;
        }

        uint lo = RotateAddress(address);
        uint hi = RotateAddress(address + 1);

        return (ushort)(((ushort)Memory[lo] << 8) | (ushort)Memory[hi]);
    }

    public static uint Read32(uint address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<uint>())) {
            return 0;
        }

        return *((uint*)(Memory + address));
    }

    public static UInt64 Read64(uint address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<UInt64>())) {
            return 0;
        }

        UInt64 lo = *((uint*)(Memory + address + 4));
        UInt64 hi = *((uint*)(Memory + address));
        return (hi << 32) | lo;
    }

    public static float ReadF32(uint address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<float>())) {
            return 0;
        }

        return *((float*)(Memory + address));
    }

    public static double ReadF64(uint address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<double>())) {
            return 0;
        }

        Int64 lo = *((uint*)(Memory + address + 4));
        Int64 hi = *((uint*)(Memory + address));
        return BitConverter.Int64BitsToDouble((hi << 32) | lo);
    }

    public static void Write8(uint address, byte value) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<byte>())) {
            return;
        }

        Memory[address] = value;
    }

    public static void Write16(uint address, ushort value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<ushort>())) {
            return;
        }

        uint lo = RotateAddress(address);
        uint hi = RotateAddress(address + 1);

        Memory[lo] = (byte)(value >> 8);
        Memory[hi] = (byte)(value & 0xFF);
    }

    public static void Write32(uint address, uint value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<uint>())) {
            return;
        }

        *((uint*)(Memory + address)) = value;
    }

    public static void Write64(uint address, UInt64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<UInt64>())) {
            return;
        }

        UInt64 hi = value >> 32;
        UInt64 lo = value & 0xFFFFFFFF;
        *((uint*)(Memory + address + 4)) = (uint)lo;
        *((uint*)(Memory + address)) = (uint)hi;
    }

    public static void WriteF32(uint address, float value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<uint>())) {
            return;
        }
        *((float*)(Memory + address)) = value;
    }

    public static void WriteF64(uint address, double value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<UInt64>())) {
            return;
        }

        UInt64 dirty = BitConverter.DoubleToUInt64Bits(value);
        UInt64 hi = dirty >> 32;
        UInt64 lo = dirty & 0xFFFFFFFF;
        *((uint*)(Memory + address + 4)) = (uint)lo;
        *((uint*)(Memory + address)) = (uint)hi;
    }

    public static void Write<T>(uint address, T value) where T : unmanaged {
        if (!typeof(T).IsPrimitive) {
            throw new InvalidOperationException("T Must be a primitive type!");
        }

        int size = Unsafe.SizeOf<T>();
        if (typeof(T) == typeof(float)) {
            WriteF32(address, (float)(object)value);
        }
        else if (typeof(T) == typeof(double)) {
            WriteF64(address, (double)(object)value);
        }
        else {
            switch(size) {
                case 1:
                    Write8(address, (byte)(object)value);
                    break;
                case 2:
                    Write16(address, (ushort)(object)value);
                    break;
                case 4:
                    Write32(address, (uint)(object)value);
                    break;
                case 8:
                    Write64(address, (UInt64)(object)value);
                    break;
                default:
                    throw new InvalidOperationException($"T has an invalid size? {size}");
            }
        }
    }

    public static T Read<T>(uint address) where T : unmanaged {
        if (!typeof(T).IsPrimitive) {
            throw new InvalidOperationException("T Must be a primitive type!");
        }

        int size = Unsafe.SizeOf<T>();
        if (typeof(T) == typeof(float)) {
            return (T)(object)ReadF32(address);
        }
        else if (typeof(T) == typeof(double)) {
            return (T)(object)ReadF64(address);
        }
        else {
            switch (size) {
                case 1:
                    return (T)(object)Read8(address);
                case 2:
                    return (T)(object)Read16(address);
                case 4:
                    return (T)(object)Read32(address);
                case 8:
                    return (T)(object)Read64(address);
                default:
                    throw new InvalidOperationException($"T has an invalid size? {size}");
            }
        }
    }
}

