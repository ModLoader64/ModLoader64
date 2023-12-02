using System.Runtime.InteropServices;

namespace ModLoader64.Mupen64plus;

public enum GLattr {
    M64P_GL_DOUBLEBUFFER = 1,
    M64P_GL_BUFFER_SIZE,
    M64P_GL_DEPTH_SIZE,
    M64P_GL_RED_SIZE,
    M64P_GL_GREEN_SIZE,
    M64P_GL_BLUE_SIZE,
    M64P_GL_ALPHA_SIZE,
    M64P_GL_SWAP_CONTROL,
    M64P_GL_MULTISAMPLEBUFFERS,
    M64P_GL_MULTISAMPLESAMPLES,
    M64P_GL_CONTEXT_MAJOR_VERSION,
    M64P_GL_CONTEXT_MINOR_VERSION,
    M64P_GL_CONTEXT_PROFILE_MASK
};

[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct M64_2DSize {
    [FieldOffset(0x00)] public u32 Width;
    [FieldOffset(0x04)] public u32 Height;
}

public static unsafe class VideoExtension {
    public delegate void M64FunctionDelegate();
    public delegate M64Error VidExtFuncInitDelegate();
    public delegate M64Error VidExtFuncQuitDelegate();
    public delegate M64Error VidExtFuncListModesDelegate(M64_2DSize* SizeArray, IntPtr NumSizes);
    public delegate M64Error VidExtFuncListRatesDelegate(M64_2DSize Size, IntPtr NumRates, IntPtr Rates);
    public delegate M64Error VidExtFuncSetModeDelegate(s32 Width, s32 Height, s32 BitsPerPixel, s32 ScreenMode, s32 Flags);
    public delegate M64Error VidExtFuncSetModeWithRateDelegate(s32 Width, s32 Height, s32 RefreshRate, s32 BitsPerPixel, s32 ScreenMode, s32 Flags);
    public delegate M64FunctionDelegate VidExtFuncGLGetProcDelegate(s8* Proc);
    public delegate M64Error VidExtFuncGLSetAttrDelegate(GLattr Attr, s32 Value);
    public delegate M64Error VidExtFuncGLGetAttrDelegate(GLattr Attr, IntPtr ValuePointer);
    public delegate M64Error VidExtFuncGLSwapBufDelegate();
    public delegate M64Error VidExtFuncSetCaptionDelegate(s8* Title);
    public delegate M64Error VidExtFuncToggleFSDelegate();
    public delegate M64Error VidExtFuncResizeWindowDelegate(s32 Width, s32 Height);
    public delegate u32 VidExtFuncGLGetDefaultFramebuffer();

    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    // FIXME: C# Marshall is stupid and doesn't like pointers at 4-byte offsets??
    [StructLayout(LayoutKind.Explicit, Size = 0x74)]
    public struct VideoExtensionFunctions {
        [FieldOffset(0x00)] public u32 Functions;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x04)] public VidExtFuncInitDelegate VidExtFuncInit;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x0C)] public VidExtFuncQuitDelegate VidExtFuncQuit;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x14)] public VidExtFuncListModesDelegate VidExtFuncListModes;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x1C)] public VidExtFuncListRatesDelegate VidExtFuncListRates;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x24)] public VidExtFuncSetModeDelegate VidExtFuncSetMode;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x2C)] public VidExtFuncSetModeWithRateDelegate VidExtFuncSetModeWithRate;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x34)] public VidExtFuncGLGetProcDelegate VidExtFuncGLGetProc;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x3C)] public VidExtFuncGLSetAttrDelegate VidExtFuncGLSetAttr;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x44)] public VidExtFuncGLGetAttrDelegate VidExtFuncGLGetAttr;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x4C)] public VidExtFuncGLSwapBufDelegate VidExtFuncGLSwapBuf;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x54)] public VidExtFuncSetCaptionDelegate VidExtFuncSetCaption;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x5C)] public VidExtFuncToggleFSDelegate VidExtFuncToggleFS;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x64)] public VidExtFuncResizeWindowDelegate VidExtFuncResizeWindow;
        [MarshalAs(UnmanagedType.FunctionPtr)] [FieldOffset(0x6C)] public VidExtFuncGLGetDefaultFramebuffer VidExtFuncGLGetDefaultFramebuffer;
    };
}
