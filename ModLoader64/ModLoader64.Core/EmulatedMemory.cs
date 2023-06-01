using ModLoader.API;
using System.Runtime.CompilerServices;

namespace ModLoader64.Core;

[BoundMemory]
public unsafe class EmulatedMemory : IMemory {
    private const u32 VADDR_MASK = 0x0FFFFFFF;
    private const u32 MEMORY_SIZE = 0x00800000; //0x03E00000;
    private static u8* _Memory = null;

    private static u8* Memory {
        get {
            if (_Memory == null) {
                _Memory = (u8*)Mupen64plus.Memory.Memory_GetBaseAddress();
            }
            return _Memory;
        }
    }

    private static void EmitStackTrace(s32 skip) {
        Logger.Error(new System.Diagnostics.StackTrace(skip, true).ToString());
    }

    public static bool MemorySafetyCheck(ref u32 address, s32 size) {
        s32 skip = 2;

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

    private static u32 RotateAddress(u32 address) {
        return ((address >> 2) * 4) + (3 - (address & 3));
    }

    public static byte ReadU8(u32 address) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return 0;
        }

        return Memory[address];
    }

    public static u16 ReadU16(u32 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u16>())) {
            return 0;
        }

        u32 lo = RotateAddress(address);
        u32 hi = RotateAddress(address + 1);

        return (u16)(((u16)Memory[lo] << 8) | (u16)Memory[hi]);
    }

    public static u32 ReadU32(u32 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return 0;
        }

        return *((u32*)(Memory + address));
    }

    public static u64 ReadU64(u32 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return 0;
        }

        u64 lo = *((u32*)(Memory + address + 4));
        u64 hi = *((u32*)(Memory + address));
        return (hi << 32) | lo;
    }

    public static f32 ReadF32(u32 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<f32>())) {
            return 0;
        }

        return *((f32*)(Memory + address));
    }

    public static f64 ReadF64(u32 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<f64>())) {
            return 0;
        }

        s64 lo = *((u32*)(Memory + address + 4));
        s64 hi = *((u32*)(Memory + address));
        return BitConverter.Int64BitsToDouble((hi << 32) | lo);
    }

    public static void WriteU8(u32 address, u8 value) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return;
        }

        Memory[address] = value;
    }

    public static void WriteU16(u32 address, u16 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u16>())) {
            return;
        }

        u32 lo = RotateAddress(address);
        u32 hi = RotateAddress(address + 1);

        Memory[lo] = (u8)(value >> 8);
        Memory[hi] = (u8)(value & 0xFF);
    }

    public static void WriteU32(u32 address, u32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }

        *((u32*)(Memory + address)) = value;
    }

    public static void WriteU64(u32 address, u64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return;
        }

        u64 hi = value >> 32;
        u64 lo = value & 0xFFFFFFFF;
        *((u32*)(Memory + address + 4)) = (u32)lo;
        *((u32*)(Memory + address)) = (u32)hi;
    }

    public static void WriteF32(u32 address, f32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }
        *((f32*)(Memory + address)) = value;
    }

    public static void WriteF64(u32 address, f64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return;
        }

        u64 dirty = BitConverter.DoubleToUInt64Bits(value);
        u64 hi = dirty >> 32;
        u64 lo = dirty & 0xFFFFFFFF;
        *((u32*)(Memory + address + 4)) = (u32)lo;
        *((u32*)(Memory + address)) = (u32)hi;
    }

    public static void Write<T>(u32 address, T value) where T : unmanaged {
        if (!typeof(T).IsPrimitive) {
            throw new InvalidOperationException("T must be a primitive type!");
        }

        if (typeof(T) == typeof(f32)) {
            WriteF32(address, (f32)(object)value);
        }
        else if (typeof(T) == typeof(f64)) {
            WriteF64(address, (f64)(object)value);
        }
        else if (typeof(T) == typeof(u8)) {
            WriteU8(address, (u8)(object)value);
        }
        else if (typeof(T) == typeof(s8)) {
            WriteU8(address, (u8)((s8)(object)value));
        }
        else if (typeof(T) == typeof(u16)) {
            WriteU16(address, (u16)(object)value);
        }
        else if (typeof(T) == typeof(s16)) {
            WriteU16(address, (u16)((s16)(object)value));
        }
        else if (typeof(T) == typeof(u32)) {
            WriteU32(address, (u32)(object)value);
        }
        else if (typeof(T) == typeof(s32)) {
            WriteU32(address, (u32)((s32)(object)value));
        }
        else if (typeof(T) == typeof(u64)) {
            WriteU64(address, (u64)(object)value);
        }
        else if (typeof(T) == typeof(s64)) {
            WriteU64(address, (u64)((s64)(object)value));
        }
        else {
            throw new InvalidOperationException("Unsupported type!");
        }
    }


    public static T Read<T>(uint address) where T : unmanaged {
        if (!typeof(T).IsPrimitive) {
            throw new InvalidOperationException("T Must be a primitive type!");
        }

        int size = Unsafe.SizeOf<T>();
        if (typeof(T) == typeof(f32)) {
            return (T)(object)ReadF32(address);
        }
        else if (typeof(T) == typeof(f64)) {
            return (T)(object)ReadF64(address);
        }
        else {
            switch (size) {
                case 1:
                    return (T)(object)ReadU8(address);
                case 2:
                    return (T)(object)ReadU16(address);
                case 4:
                    return (T)(object)ReadU32(address);
                case 8:
                    return (T)(object)ReadU64(address);
                default:
                    throw new InvalidOperationException($"T has an invalid size? {size}");
            }
        }
    }

    public static sbyte ReadS8(uint address)
    {
        throw new NotImplementedException();
    }

    public static short ReadS16(uint address)
    {
        throw new NotImplementedException();
    }

    public static int ReadS32(uint address)
    {
        throw new NotImplementedException();
    }

    public static long ReadS64(uint address)
    {
        throw new NotImplementedException();
    }

    public static void WriteS8(uint address, sbyte value)
    {
        throw new NotImplementedException();
    }

    public static void WriteS16(uint address, short value)
    {
        throw new NotImplementedException();
    }

    public static void WriteS32(uint address, int value)
    {
        throw new NotImplementedException();
    }

    public static void WriteS64(uint address, long value)
    {
        throw new NotImplementedException();
    }

    public static void InvalidateCachedCode()
    {
        Mupen64plus.Memory.InvalidateCachedCode();
    }
}

