﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader64.Core;

public static class SDLK {
    public const int SCANCODE_MASK = 1 << 30;
}


public enum SDL_KeyCode : int {
    SDLK_UNKNOWN = 0,

    SDLK_RETURN = '\r',
    SDLK_ESCAPE = '\x1B',
    SDLK_BACKSPACE = '\b',
    SDLK_TAB = '\t',
    SDLK_SPACE = ' ',
    SDLK_EXCLAIM = '!',
    SDLK_QUOTEDBL = '"',
    SDLK_HASH = '#',
    SDLK_PERCENT = '%',
    SDLK_DOLLAR = '$',
    SDLK_AMPERSAND = '&',
    SDLK_QUOTE = '\'',
    SDLK_LEFTPAREN = '(',
    SDLK_RIGHTPAREN = ')',
    SDLK_ASTERISK = '*',
    SDLK_PLUS = '+',
    SDLK_COMMA = ',',
    SDLK_MINUS = '-',
    SDLK_PERIOD = '.',
    SDLK_SLASH = '/',
    SDLK_0 = '0',
    SDLK_1 = '1',
    SDLK_2 = '2',
    SDLK_3 = '3',
    SDLK_4 = '4',
    SDLK_5 = '5',
    SDLK_6 = '6',
    SDLK_7 = '7',
    SDLK_8 = '8',
    SDLK_9 = '9',
    SDLK_COLON = ':',
    SDLK_SEMICOLON = ';',
    SDLK_LESS = '<',
    SDLK_EQUALS = '=',
    SDLK_GREATER = '>',
    SDLK_QUESTION = '?',
    SDLK_AT = '@',

    /*
       Skip uppercase letters
     */

    SDLK_LEFTBRACKET = '[',
    SDLK_BACKSLASH = '\\',
    SDLK_RIGHTBRACKET = ']',
    SDLK_CARET = '^',
    SDLK_UNDERSCORE = '_',
    SDLK_BACKQUOTE = '`',
    SDLK_a = 'a',
    SDLK_b = 'b',
    SDLK_c = 'c',
    SDLK_d = 'd',
    SDLK_e = 'e',
    SDLK_f = 'f',
    SDLK_g = 'g',
    SDLK_h = 'h',
    SDLK_i = 'i',
    SDLK_j = 'j',
    SDLK_k = 'k',
    SDLK_l = 'l',
    SDLK_m = 'm',
    SDLK_n = 'n',
    SDLK_o = 'o',
    SDLK_p = 'p',
    SDLK_q = 'q',
    SDLK_r = 'r',
    SDLK_s = 's',
    SDLK_t = 't',
    SDLK_u = 'u',
    SDLK_v = 'v',
    SDLK_w = 'w',
    SDLK_x = 'x',
    SDLK_y = 'y',
    SDLK_z = 'z',
    SDLK_CAPSLOCK = SDLK.SCANCODE_MASK | (SDL_ScanCode.CAPSLOCK),

    SDLK_F1 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F1),
    SDLK_F2 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F2),
    SDLK_F3 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F3),
    SDLK_F4 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F4),
    SDLK_F5 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F5),
    SDLK_F6 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F6),
    SDLK_F7 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F7),
    SDLK_F8 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F8),
    SDLK_F9 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F9),
    SDLK_F10 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F10),
    SDLK_F11 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F11),
    SDLK_F12 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F12),

    SDLK_PRINTSCREEN = SDLK.SCANCODE_MASK | (SDL_ScanCode.PRINTSCREEN),
    SDLK_SCROLLLOCK = SDLK.SCANCODE_MASK | (SDL_ScanCode.SCROLLLOCK),
    SDLK_PAUSE = SDLK.SCANCODE_MASK | (SDL_ScanCode.PAUSE),
    SDLK_INSERT = SDLK.SCANCODE_MASK | (SDL_ScanCode.INSERT),
    SDLK_HOME = SDLK.SCANCODE_MASK | (SDL_ScanCode.HOME),
    SDLK_PAGEUP = SDLK.SCANCODE_MASK | (SDL_ScanCode.PAGEUP),
    SDLK_DELETE = '\x7F',
    SDLK_END = SDLK.SCANCODE_MASK | (SDL_ScanCode.END),
    SDLK_PAGEDOWN = SDLK.SCANCODE_MASK | (SDL_ScanCode.PAGEDOWN),
    SDLK_RIGHT = SDLK.SCANCODE_MASK | (SDL_ScanCode.RIGHT),
    SDLK_LEFT = SDLK.SCANCODE_MASK | (SDL_ScanCode.LEFT),
    SDLK_DOWN = SDLK.SCANCODE_MASK | (SDL_ScanCode.DOWN),
    SDLK_UP = SDLK.SCANCODE_MASK | (SDL_ScanCode.UP),

    SDLK_NUMLOCKCLEAR = SDLK.SCANCODE_MASK | (SDL_ScanCode.NUMLOCKCLEAR),
    SDLK_KP_DIVIDE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_DIVIDE),
    SDLK_KP_MULTIPLY = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MULTIPLY),
    SDLK_KP_MINUS = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MINUS),
    SDLK_KP_PLUS = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_PLUS),
    SDLK_KP_ENTER = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_ENTER),
    SDLK_KP_1 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_1),
    SDLK_KP_2 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_2),
    SDLK_KP_3 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_3),
    SDLK_KP_4 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_4),
    SDLK_KP_5 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_5),
    SDLK_KP_6 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_6),
    SDLK_KP_7 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_7),
    SDLK_KP_8 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_8),
    SDLK_KP_9 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_9),
    SDLK_KP_0 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_0),
    SDLK_KP_PERIOD = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_PERIOD),

    SDLK_APPLICATION = SDLK.SCANCODE_MASK | (SDL_ScanCode.APPLICATION),
    SDLK_POWER = SDLK.SCANCODE_MASK | (SDL_ScanCode.POWER),
    SDLK_KP_EQUALS = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_EQUALS),
    SDLK_F13 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F13),
    SDLK_F14 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F14),
    SDLK_F15 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F15),
    SDLK_F16 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F16),
    SDLK_F17 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F17),
    SDLK_F18 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F18),
    SDLK_F19 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F19),
    SDLK_F20 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F20),
    SDLK_F21 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F21),
    SDLK_F22 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F22),
    SDLK_F23 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F23),
    SDLK_F24 = SDLK.SCANCODE_MASK | (SDL_ScanCode.F24),
    SDLK_EXECUTE = SDLK.SCANCODE_MASK | (SDL_ScanCode.EXECUTE),
    SDLK_HELP = SDLK.SCANCODE_MASK | (SDL_ScanCode.HELP),
    SDLK_MENU = SDLK.SCANCODE_MASK | (SDL_ScanCode.MENU),
    SDLK_SELECT = SDLK.SCANCODE_MASK | (SDL_ScanCode.SELECT),
    SDLK_STOP = SDLK.SCANCODE_MASK | (SDL_ScanCode.STOP),
    SDLK_AGAIN = SDLK.SCANCODE_MASK | (SDL_ScanCode.AGAIN),
    SDLK_UNDO = SDLK.SCANCODE_MASK | (SDL_ScanCode.UNDO),
    SDLK_CUT = SDLK.SCANCODE_MASK | (SDL_ScanCode.CUT),
    SDLK_COPY = SDLK.SCANCODE_MASK | (SDL_ScanCode.COPY),
    SDLK_PASTE = SDLK.SCANCODE_MASK | (SDL_ScanCode.PASTE),
    SDLK_FIND = SDLK.SCANCODE_MASK | (SDL_ScanCode.FIND),
    SDLK_MUTE = SDLK.SCANCODE_MASK | (SDL_ScanCode.MUTE),
    SDLK_VOLUMEUP = SDLK.SCANCODE_MASK | (SDL_ScanCode.VOLUMEUP),
    SDLK_VOLUMEDOWN = SDLK.SCANCODE_MASK | (SDL_ScanCode.VOLUMEDOWN),
    SDLK_KP_COMMA = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_COMMA),
    SDLK_KP_EQUALSAS400 =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_EQUALSAS400),

    SDLK_ALTERASE = SDLK.SCANCODE_MASK | (SDL_ScanCode.ALTERASE),
    SDLK_SYSREQ = SDLK.SCANCODE_MASK | (SDL_ScanCode.SYSREQ),
    SDLK_CANCEL = SDLK.SCANCODE_MASK | (SDL_ScanCode.CANCEL),
    SDLK_CLEAR = SDLK.SCANCODE_MASK | (SDL_ScanCode.CLEAR),
    SDLK_PRIOR = SDLK.SCANCODE_MASK | (SDL_ScanCode.PRIOR),
    SDLK_RETURN2 = SDLK.SCANCODE_MASK | (SDL_ScanCode.RETURN2),
    SDLK_SEPARATOR = SDLK.SCANCODE_MASK | (SDL_ScanCode.SEPARATOR),
    SDLK_OUT = SDLK.SCANCODE_MASK | (SDL_ScanCode.OUT),
    SDLK_OPER = SDLK.SCANCODE_MASK | (SDL_ScanCode.OPER),
    SDLK_CLEARAGAIN = SDLK.SCANCODE_MASK | (SDL_ScanCode.CLEARAGAIN),
    SDLK_CRSEL = SDLK.SCANCODE_MASK | (SDL_ScanCode.CRSEL),
    SDLK_EXSEL = SDLK.SCANCODE_MASK | (SDL_ScanCode.EXSEL),

    SDLK_KP_00 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_00),
    SDLK_KP_000 = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_000),
    SDLK_THOUSANDSSEPARATOR =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.THOUSANDSSEPARATOR),
    SDLK_DECIMALSEPARATOR =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.DECIMALSEPARATOR),
    SDLK_CURRENCYUNIT = SDLK.SCANCODE_MASK | (SDL_ScanCode.CURRENCYUNIT),
    SDLK_CURRENCYSUBUNIT =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.CURRENCYSUBUNIT),
    SDLK_KP_LEFTPAREN = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_LEFTPAREN),
    SDLK_KP_RIGHTPAREN = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_RIGHTPAREN),
    SDLK_KP_LEFTBRACE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_LEFTBRACE),
    SDLK_KP_RIGHTBRACE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_RIGHTBRACE),
    SDLK_KP_TAB = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_TAB),
    SDLK_KP_BACKSPACE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_BACKSPACE),
    SDLK_KP_A = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_A),
    SDLK_KP_B = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_B),
    SDLK_KP_C = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_C),
    SDLK_KP_D = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_D),
    SDLK_KP_E = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_E),
    SDLK_KP_F = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_F),
    SDLK_KP_XOR = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_XOR),
    SDLK_KP_POWER = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_POWER),
    SDLK_KP_PERCENT = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_PERCENT),
    SDLK_KP_LESS = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_LESS),
    SDLK_KP_GREATER = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_GREATER),
    SDLK_KP_AMPERSAND = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_AMPERSAND),
    SDLK_KP_DBLAMPERSAND =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_DBLAMPERSAND),
    SDLK_KP_VERTICALBAR =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_VERTICALBAR),
    SDLK_KP_DBLVERTICALBAR =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_DBLVERTICALBAR),
    SDLK_KP_COLON = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_COLON),
    SDLK_KP_HASH = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_HASH),
    SDLK_KP_SPACE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_SPACE),
    SDLK_KP_AT = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_AT),
    SDLK_KP_EXCLAM = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_EXCLAM),
    SDLK_KP_MEMSTORE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMSTORE),
    SDLK_KP_MEMRECALL = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMRECALL),
    SDLK_KP_MEMCLEAR = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMCLEAR),
    SDLK_KP_MEMADD = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMADD),
    SDLK_KP_MEMSUBTRACT =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMSUBTRACT),
    SDLK_KP_MEMMULTIPLY =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMMULTIPLY),
    SDLK_KP_MEMDIVIDE = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_MEMDIVIDE),
    SDLK_KP_PLUSMINUS = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_PLUSMINUS),
    SDLK_KP_CLEAR = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_CLEAR),
    SDLK_KP_CLEARENTRY = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_CLEARENTRY),
    SDLK_KP_BINARY = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_BINARY),
    SDLK_KP_OCTAL = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_OCTAL),
    SDLK_KP_DECIMAL = SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_DECIMAL),
    SDLK_KP_HEXADECIMAL =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KP_HEXADECIMAL),

    SDLK_LCTRL = SDLK.SCANCODE_MASK | (SDL_ScanCode.LCTRL),
    SDLK_LSHIFT = SDLK.SCANCODE_MASK | (SDL_ScanCode.LSHIFT),
    SDLK_LALT = SDLK.SCANCODE_MASK | (SDL_ScanCode.LALT),
    SDLK_LGUI = SDLK.SCANCODE_MASK | (SDL_ScanCode.LGUI),
    SDLK_RCTRL = SDLK.SCANCODE_MASK | (SDL_ScanCode.RCTRL),
    SDLK_RSHIFT = SDLK.SCANCODE_MASK | (SDL_ScanCode.RSHIFT),
    SDLK_RALT = SDLK.SCANCODE_MASK | (SDL_ScanCode.RALT),
    SDLK_RGUI = SDLK.SCANCODE_MASK | (SDL_ScanCode.RGUI),

    SDLK_MODE = SDLK.SCANCODE_MASK | (SDL_ScanCode.MODE),

    SDLK_AUDIONEXT = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIONEXT),
    SDLK_AUDIOPREV = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOPREV),
    SDLK_AUDIOSTOP = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOSTOP),
    SDLK_AUDIOPLAY = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOPLAY),
    SDLK_AUDIOMUTE = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOMUTE),
    SDLK_MEDIASELECT = SDLK.SCANCODE_MASK | (SDL_ScanCode.MEDIASELECT),
    SDLK_WWW = SDLK.SCANCODE_MASK | (SDL_ScanCode.WWW),
    SDLK_MAIL = SDLK.SCANCODE_MASK | (SDL_ScanCode.MAIL),
    SDLK_CALCULATOR = SDLK.SCANCODE_MASK | (SDL_ScanCode.CALCULATOR),
    SDLK_COMPUTER = SDLK.SCANCODE_MASK | (SDL_ScanCode.COMPUTER),
    SDLK_AC_SEARCH = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_SEARCH),
    SDLK_AC_HOME = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_HOME),
    SDLK_AC_BACK = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_BACK),
    SDLK_AC_FORWARD = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_FORWARD),
    SDLK_AC_STOP = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_STOP),
    SDLK_AC_REFRESH = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_REFRESH),
    SDLK_AC_BOOKMARKS = SDLK.SCANCODE_MASK | (SDL_ScanCode.AC_BOOKMARKS),

    SDLK_BRIGHTNESSDOWN =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.BRIGHTNESSDOWN),
    SDLK_BRIGHTNESSUP = SDLK.SCANCODE_MASK | (SDL_ScanCode.BRIGHTNESSUP),
    SDLK_DISPLAYSWITCH = SDLK.SCANCODE_MASK | (SDL_ScanCode.DISPLAYSWITCH),
    SDLK_KBDILLUMTOGGLE =
        SDLK.SCANCODE_MASK | (SDL_ScanCode.KBDILLUMTOGGLE),
    SDLK_KBDILLUMDOWN = SDLK.SCANCODE_MASK | (SDL_ScanCode.KBDILLUMDOWN),
    SDLK_KBDILLUMUP = SDLK.SCANCODE_MASK | (SDL_ScanCode.KBDILLUMUP),
    SDLK_EJECT = SDLK.SCANCODE_MASK | (SDL_ScanCode.EJECT),
    SDLK_SLEEP = SDLK.SCANCODE_MASK | (SDL_ScanCode.SLEEP),
    SDLK_APP1 = SDLK.SCANCODE_MASK | (SDL_ScanCode.APP1),
    SDLK_APP2 = SDLK.SCANCODE_MASK | (SDL_ScanCode.APP2),

    SDLK_AUDIOREWIND = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOREWIND),
    SDLK_AUDIOFASTFORWARD = SDLK.SCANCODE_MASK | (SDL_ScanCode.AUDIOFASTFORWARD),

    SDLK_SOFTLEFT = SDLK.SCANCODE_MASK | (SDL_ScanCode.SOFTLEFT),
    SDLK_SOFTRIGHT = SDLK.SCANCODE_MASK | (SDL_ScanCode.SOFTRIGHT),
    SDLK_CALL = SDLK.SCANCODE_MASK | (SDL_ScanCode.CALL),
    SDLK_ENDCALL = SDLK.SCANCODE_MASK | (SDL_ScanCode.ENDCALL)
};
