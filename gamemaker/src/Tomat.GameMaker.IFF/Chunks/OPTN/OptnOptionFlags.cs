using System;

namespace Tomat.GameMaker.IFF.Chunks.OPTN;

[Flags]
public enum OptnOptionFlags : ulong {
    Fullscreen                 = 0x00000001,
    InterpolatePixels          = 0x00000002,
    UseNewAudio                = 0x00000004,
    NoBorder                   = 0x00000008,
    ShowCursor                 = 0x00000010,
    Sizeable                   = 0x00000020,
    StayOnTop                  = 0x00000040,
    ChangeResolution           = 0x00000080,
    NoButtons                  = 0x00000100,
    ScreenKey                  = 0x00000200,
    HelpKey                    = 0x00000400,
    QuitKey                    = 0x00000800,
    SaveKey                    = 0x00001000,
    ScreenShotKey              = 0x00002000,
    CloseSec                   = 0x00004000,
    Freeze                     = 0x00008000,
    ShowProgress               = 0x00010000,
    LoadTransparent            = 0x00020000,
    ScaleProgress              = 0x00040000,
    DisplayErrors              = 0x00080000,
    WriteErrors                = 0x00100000,
    AbortErrors                = 0x00200000,
    VariableErrors             = 0x00400000,
    CreationEventOrder         = 0x00800000,
    UseFrontTouch              = 0x01000000,
    UseRearTouch               = 0x02000000,
    UseFastCollision           = 0x04000000,
    FastCollisionCompatibility = 0x08000000,
    DisableSandbox             = 0x10000000,
    CopyOnWriteEnabled         = 0x20000000,
}
