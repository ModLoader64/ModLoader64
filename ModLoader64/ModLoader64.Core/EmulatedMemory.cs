﻿using ModLoader.API;
using System.Runtime.CompilerServices;

namespace ModLoader64.Core;

[BoundMemory]
public unsafe class EmulatedMemory : IMemory{
    private const u32 VADDR_BASE = 0x80000000;
    private const u32 MEMORY_SIZE_NORMAL = 0x03F00000;
    private const u32 MEMORY_SIZE = 0x40000000;
    private const u64 VADDR2_BASE = 0x100000000;
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

    /// <summary>
    /// Ensure that an address and size is within the bounds, avoiding any oob issues.
    /// </summary>
    /// <param name="address">address to check</param>
    /// <param name="size">length to check</param>
    /// <returns>true if offset + size is valid</returns>
    public static bool MemorySafetyCheck(ref u64 address, s32 size) {
        const s32 skip = 2;

        address &= VADDR_MASK;

        if (address < 0 || address > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory that is out of bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')}!");
            EmitStackTrace(skip);
            return false;
        }

        if ((address + (u64)size) > MEMORY_SIZE) {
            Logger.Error($"Tried to access emulated memory which exceeds memory bounds! Got KUSEG 0x{address.ToString("X").PadLeft(8, '0')} with size 0x{size:X}!");
            EmitStackTrace(skip);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Rotate an address to correlate to the correct offset in little-endian
    /// </summary>
    /// <param name="address">Input to rotate</param>
    /// <returns></returns>
    private static u64 RotateAddress(u64 address) {
        return ((address >> 2) * 4) + (3 - (address & 3));
    }

    /// <summary>
    /// Read an 8-bit integer value at address
    /// </summary>
    /// <param name="address">Where to read</param>
    /// <returns>Value read, 0 on error</returns>
    public static byte ReadU8(u64 address) {
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
    public static u16 ReadU16(u64 address) {
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
    public static u64 ReadU64(u64 address) {
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
    public static void WriteU8(u64 address, u8 value) {
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
    public static void WriteU16(u64 address, u16 value) {
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
    public static void WriteU32(u64 address, u32 value) {
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
    public static void WriteU64(u64 address, u64 value) {
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

    public static void InvalidateCachedCode()
    {
        Mupen64plus.Memory.InvalidateCachedCode();
    }

    public static sbyte ReadS8(ulong address)
    {
        return Convert.ToSByte(ReadU8(address));
    }

    public static short ReadS16(ulong address)
    {
        return Convert.ToInt16(ReadU16(address));
    }

    public static int ReadS32(ulong address)
    {
        return Convert.ToInt32(ReadU32(address));
    }

    public static long ReadS64(ulong address)
    {
        return Convert.ToInt64(ReadU64(address));
    }

    public static void WriteS8(ulong address, sbyte value)
    {
        Write(address, value);
    }

    public static void WriteS16(ulong address, short value)
    {
        Write(address, value);
    }

    public static void WriteS32(ulong address, int value)
    {
        Write(address, value);
    }

    public static void WriteS64(ulong address, long value)
    {
        Write(address, value);
    }
}

