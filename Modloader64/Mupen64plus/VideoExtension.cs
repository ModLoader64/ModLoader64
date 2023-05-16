﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Modloader64.Mupen64plus;

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
    [FieldOffset(0x00)] public uint Width;
    [FieldOffset(0x04)] public uint Height;
}

public static unsafe class VideoExtension {
    public delegate void M64FunctionDelegate();
    public delegate M64Error VidExtFuncInitDelegate();
    public delegate M64Error VidExtFuncQuitDelegate();
    public delegate M64Error VidExtFuncListModesDelegate(M64_2DSize* SizeArray, IntPtr NumSizes);
    public delegate M64Error VidExtFuncListRatesDelegate(M64_2DSize Size, IntPtr NumRates, IntPtr Rates);
    public delegate M64Error VidExtFuncSetModeDelegate(int Width, int Height, int BitsPerPixel, int ScreenMode, int Flags);
    public delegate M64Error VidExtFuncSetModeWithRateDelegate(int Width, int Height, int RefreshRate, int BitsPerPixel, int ScreenMode, int Flags);
    public delegate M64FunctionDelegate VidExtFuncGLGetProcDelegate(char* Proc);
    public delegate M64Error VidExtFuncGLSetAttrDelegate(GLattr Attr, int Value);
    public delegate M64Error VidExtFuncGLGetAttrDelegate(GLattr Attr, IntPtr ValuePointer);
    public delegate M64Error VidExtFuncGLSwapBufDelegate();
    public delegate M64Error VidExtFuncSetCaptionDelegate(char* Title);
    public delegate M64Error VidExtFuncToggleFSDelegate();
    public delegate M64Error VidExtFuncResizeWindowDelegate(int Width, int Height);
    public delegate uint VidExtFuncGLGetDefaultFramebuffer();

    // TODO: the offsets will be different for 32 bit, we might need a better way to do this
    [StructLayout(LayoutKind.Explicit, Size = 0x74)]
    public struct VideoExtensionFunctions {
        [FieldOffset(0x00)] public uint Functions;
        [FieldOffset(0x04)] public VidExtFuncInitDelegate VidExtFuncInit;
        [FieldOffset(0x0C)] public VidExtFuncQuitDelegate VidExtFuncQuit;
        [FieldOffset(0x14)] public VidExtFuncListModesDelegate VidExtFuncListModes;
        [FieldOffset(0x1C)] public VidExtFuncListRatesDelegate VidExtFuncListRates;
        [FieldOffset(0x24)] public VidExtFuncSetModeDelegate VidExtFuncSetMode;
        [FieldOffset(0x2C)] public VidExtFuncSetModeWithRateDelegate VidExtFuncSetModeWithRate;
        [FieldOffset(0x34)] public VidExtFuncGLGetProcDelegate VidExtFuncGLGetProc;
        [FieldOffset(0x3C)] public VidExtFuncGLSetAttrDelegate VidExtFuncGLSetAttr;
        [FieldOffset(0x44)] public VidExtFuncGLGetAttrDelegate VidExtFuncGLGetAttr;
        [FieldOffset(0x4C)] public VidExtFuncGLSwapBufDelegate VidExtFuncGLSwapBuf;
        [FieldOffset(0x54)] public VidExtFuncSetCaptionDelegate VidExtFuncSetCaption;
        [FieldOffset(0x5C)] public VidExtFuncToggleFSDelegate VidExtFuncToggleFS;
        [FieldOffset(0x64)] public VidExtFuncResizeWindowDelegate VidExtFuncResizeWindow;
        [FieldOffset(0x6C)] public VidExtFuncGLGetDefaultFramebuffer VidExtFuncGLGetDefaultFramebuffer;
    };
}
