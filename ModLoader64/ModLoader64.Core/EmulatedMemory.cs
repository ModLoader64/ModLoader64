using ModLoader64.Core;
using System;
using System.Runtime.CompilerServices;

namespace ModLoader64.Core;

public static unsafe class EmulatedMemory {
    private const u32 VADDR_MASK = 0x0FFFFFFF;
    private const u32 MEMORY_SIZE = 0x03F00000;

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

    public static bool MemorySafetyCheck(ref u64 address, s32 size) {
        const s32 skip = 2;

        address &= VADDR_MASK;

        if (address < 0 || address > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory that is out of bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')}!");
            EmitStackTrace(skip);
            return false;
        }

        if ((address + (u64)size) > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory which exceeds memory bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')} with size 0x{size:X}");
            EmitStackTrace(skip);
            return false;
        }

        return true;
    }

    private static u64 RotateAddress(u64 address) {
        return ((address >> 2) * 4) + (3 - (address & 3));
    }

    /// <summary>
    /// Read an 8-bit integer value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static byte Read8(u64 address) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return 0;
        }

        return Memory[address];
    }

    /// <summary>
    /// Read an 16-bit integer value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static u16 Read16(u64 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u16>())) {
            return 0;
        }

        u64 lo = RotateAddress(address);
        u64 hi = RotateAddress(address + 1);

        return (u16)(((u16)Memory[lo] << 8) | (u16)Memory[hi]);
    }

    /// <summary>
    /// Read an 32-bit integer value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static u32 Read32(u64 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return 0;
        }

        return *((u32*)(Memory + address));
    }

    /// <summary>
    /// Read an 64-bit integer value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static u64 Read64(u64 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return 0;
        }

        u64 lo = *((u64*)(Memory + address + 4));
        u64 hi = *((u64*)(Memory + address));
        return (hi << 32) | lo;
    }

    /// <summary>
    /// Read an 32-bit floating point value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static f32 ReadF32(u64 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<f32>())) {
            return 0;
        }

        return *((f32*)(Memory + address));
    }

    /// <summary>
    /// Read an 64-bit floating point value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static f64 ReadF64(u64 address) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<f64>())) {
            return 0;
        }

        s64 lo = *((u32*)(Memory + address + 4));
        s64 hi = *((u32*)(Memory + address));
        return BitConverter.Int64BitsToDouble((hi << 32) | lo);
    }

    /// <summary>
    /// Write a 8-bit integer to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write8(u64 address, u8 value) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return;
        }

        Memory[address] = value;
    }

    /// <summary>
    /// Write a 16-bit integer to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write16(u64 address, u16 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u16>())) {
            return;
        }

        u64 lo = RotateAddress(address);
        u64 hi = RotateAddress(address + 1);

        Memory[lo] = (u8)(value >> 8);
        Memory[hi] = (u8)(value & 0xFF);
    }

    /// <summary>
    /// Write a 32-bit integer to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write32(u64 address, u32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }

        *((u32*)(Memory + address)) = value;
    }

    /// <summary>
    /// Write a 64-bit integer to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write64(u64 address, u64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return;
        }

        u64 hi = value >> 32;
        u64 lo = value & 0xFFFFFFFF;
        *((u32*)(Memory + address + 4)) = (u32)lo;
        *((u32*)(Memory + address)) = (u32)hi;
    }

    /// <summary>
    /// Write a 32-bit floating point value to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void WriteF32(u64 address, f32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }
        *((f32*)(Memory + address)) = value;
    }

    /// <summary>
    /// Write a 64-bit floating point value to memory address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void WriteF64(u64 address, f64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return;
        }

        u64 dirty = BitConverter.DoubleToUInt64Bits(value);
        u64 hi = dirty >> 32;
        u64 lo = dirty & 0xFFFFFFFF;
        *((u32*)(Memory + address + 4)) = (u32)lo;
        *((u32*)(Memory + address)) = (u32)hi;
    }

    /// <summary>
    /// Write a primitive value of type T, with sizeof(T) to memory address
    /// </summary>
    /// <typeparam name="T">Primitive type</typeparam>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    /// 
    [Obsolete("Write<T> may produce unwanted behavior. Use the function for the specific type/size where possible.")]
    public static void Write<T>(u64 address, T value) where T : unmanaged {
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
            Write8(address, (u8)(object)value);
        }
        else if (typeof(T) == typeof(s8)) {
            Write8(address, (u8)((s8)(object)value));
        }
        else if (typeof(T) == typeof(u16)) {
            Write16(address, (u16)(object)value);
        }
        else if (typeof(T) == typeof(s16)) {
            Write16(address, (u16)((s16)(object)value));
        }
        else if (typeof(T) == typeof(u32)) {
            Write32(address, (u32)(object)value);
        }
        else if (typeof(T) == typeof(s32)) {
            Write32(address, (u32)((s32)(object)value));
        }
        else if (typeof(T) == typeof(u64)) {
            Write64(address, (u64)(object)value);
        }
        else if (typeof(T) == typeof(s64)) {
            Write64(address, (u64)((s64)(object)value));
        }
        else {
            throw new InvalidOperationException("Unsupported type!");
        }
    }

    /// <summary>
    /// Read a value of type T, with size sizeof(T) at memory address
    /// </summary>
    /// <typeparam name="T">Type of read value</typeparam>
    /// <param name="address">Address to read</param>
    /// <returns>Value read</returns>
    /// 
    [Obsolete("Read<T> may produce unwanted behavior. Use the function for the specific type/size where possible.")]
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

