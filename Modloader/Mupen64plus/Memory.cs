using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Modloader2.Mupen64plus.Frontend;

namespace Modloader2.Mupen64plus;

public static class Memory {
    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static UInt64 Memory_Read64(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint Memory_Read32(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static ushort Memory_Read16(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static byte Memory_Read8(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static double Memory_ReadF64(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static float tMemory_ReadF32(uint address);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_Write64(uint address, UInt64 value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_Write32(uint address, uint value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_Write16(uint address, ushort value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_Write8(uint address, byte value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_WriteF64(uint address, double value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Memory_WriteF32(uint address, float value);

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Memory_GetBaseAddress();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr ROM_GetBaseAddress();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static uint ROM_GetBaseSize();

    [DllImport(MUPEN_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
    public extern static void InvalidateCachedCode();
}
