using System.Runtime.CompilerServices;

namespace ModLoader64.Core; 

public static unsafe class EmulatedRom {
    private const u32 MAX_ROM_SIZE = 0x20000000; // 0x3FF0000 + 0x300000 = 0x42F0000 (about 66 mb) is for sure addressable, however I have allocated 512mb to play with in the core.

    private static u8* _Rom = null;
    private static u32 _RomSize = 0;

    private static u8* Rom {
        get {
            if (_Rom == null) {
                _Rom = (u8*)Mupen64plus.Memory.ROM_GetBaseAddress();
            }
            return _Rom;
        }
    }

    private static u32 RomSize {
        get {
            if (_RomSize == 0) {
                _RomSize = Mupen64plus.Memory.ROM_GetBaseSize();
            }
            return _RomSize;
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

        if (address < 0 || address > MAX_ROM_SIZE) {
            Logger.Error($"Tried to access emulated rom that is out of bounds! Got 0x{address.ToString("X").PadLeft(8, '0')}! Rom size is 0x{RomSize.ToString("X").PadLeft(8, '0')}, max is 0x{MAX_ROM_SIZE.ToString("X").PadLeft(8, '0')}!");
            EmitStackTrace(skip);
            return false;
        }

        if ((address + (u64)size) > MAX_ROM_SIZE) {
            Logger.Error($"Tried to access emulated memory which exceeds memory bounds! Got 0x{address.ToString("X").PadLeft(8, '0')} with size 0x{size:X}! Rom size is 0x{RomSize.ToString("X").PadLeft(8, '0')}, max is 0x{MAX_ROM_SIZE.ToString("X").PadLeft(8, '0')}!");
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
    public static byte Read8(u64 address) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return 0;
        }

        return Rom[address];
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

        return (u16)(((u16)Rom[lo] << 8) | (u16)Rom[hi]);
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

        return *((u32*)(Rom + address));
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

        u64 lo = *((u64*)(Rom + address + 4));
        u64 hi = *((u64*)(Rom + address));
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

        return *((f32*)(Rom + address));
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

        s64 lo = *((u32*)(Rom + address + 4));
        s64 hi = *((u32*)(Rom + address));
        return BitConverter.Int64BitsToDouble((hi << 32) | lo);
    }

    /// <summary>
    /// Write a 8-bit integer to rom address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write8(u64 address, u8 value) {
        address = RotateAddress(address);
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u8>())) {
            return;
        }

        Rom[address] = value;
    }

    /// <summary>
    /// Write a 16-bit integer to rom address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write16(u64 address, u16 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u16>())) {
            return;
        }

        u64 lo = RotateAddress(address);
        u64 hi = RotateAddress(address + 1);

        Rom[lo] = (u8)(value >> 8);
        Rom[hi] = (u8)(value & 0xFF);
    }

    /// <summary>
    /// Write a 32-bit integer to rom address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write32(u64 address, u32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }

        *((u32*)(Rom + address)) = value;
    }

    /// <summary>
    /// Write a 64-bit integer to rom address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void Write64(u64 address, u64 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u64>())) {
            return;
        }

        u64 hi = value >> 32;
        u64 lo = value & 0xFFFFFFFF;
        *((u32*)(Rom + address + 4)) = (u32)lo;
        *((u32*)(Rom + address)) = (u32)hi;
    }

    /// <summary>
    /// Write a 32-bit floating point value to rom address
    /// </summary>
    /// <param name="address">Where to write</param>
    /// <param name="value">Value to write</param>
    public static void WriteF32(u64 address, f32 value) {
        if (!MemorySafetyCheck(ref address, Unsafe.SizeOf<u32>())) {
            return;
        }

        *((f32*)(Rom + address)) = value;
    }

    /// <summary>
    /// Write a 64-bit floating point value to rom address
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
        *((u32*)(Rom + address + 4)) = (u32)lo;
        *((u32*)(Rom + address)) = (u32)hi;
    }

    /// <summary>
    /// Write a primitive value of type T, with sizeof(T) to rom address
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


