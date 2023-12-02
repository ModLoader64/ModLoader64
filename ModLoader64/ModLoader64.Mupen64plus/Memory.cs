using ModLoader64.Core;

namespace ModLoader64.Mupen64plus;

public static class Memory {
    #region Delegate Types
    public delegate IntPtr Memory_GetBaseAddressDelegate();
    public delegate IntPtr ROM_GetBaseAddressDelegate();
    public delegate u32 ROM_GetBaseSizeDelegate();
    public delegate void InvalidateCachedCodeDelegate();
    public delegate void InvalidateSpecificCachedCodeDelegate(u32 address, u32 size);
    #endregion

    #region Delegate Instances
    public static Memory_GetBaseAddressDelegate Memory_GetBaseAddress;
    public static ROM_GetBaseAddressDelegate ROM_GetBaseAddress;
    public static ROM_GetBaseSizeDelegate ROM_GetBaseSize;
    public static InvalidateCachedCodeDelegate InvalidateCachedCode;
    public static InvalidateSpecificCachedCodeDelegate InvalidateSpecificCachedCode;
    #endregion

    static Memory() {
        Memory_GetBaseAddress = Natives.GetDelegateInstance<Memory_GetBaseAddressDelegate>("Memory_GetBaseAddress");
        ROM_GetBaseAddress = Natives.GetDelegateInstance<ROM_GetBaseAddressDelegate>("ROM_GetBaseAddress");
        ROM_GetBaseSize = Natives.GetDelegateInstance<ROM_GetBaseSizeDelegate>("ROM_GetBaseSize");
        InvalidateCachedCode = Natives.GetDelegateInstance<InvalidateCachedCodeDelegate>("InvalidateCachedCode");
        InvalidateSpecificCachedCode = Natives.GetDelegateInstance<InvalidateSpecificCachedCodeDelegate>("InvalidateSpecificCachedCode");
    }
}
