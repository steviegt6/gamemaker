using System;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

[Flags]
public enum Gen8InfoFlags : uint {
    Fullscreen        = 0x00001,
    SyncVertex1       = 0x00002,
    SyncVertex2       = 0x00004,
    Interpolate       = 0x00008,
    Scale             = 0x00010,
    ShowCursor        = 0x00020,
    Sizeable          = 0x00040,
    ScreenKey         = 0x00080,
    SyncVertex3       = 0x00100,
    StudioVersionB1   = 0x00200,
    StudioVersionB2   = 0x00400,
    StudioVersionB3   = 0x00800,
    SteamOrPlayer     = 0x01000,
    LocalDataEnabled  = 0x02000,
    BorderlessWindow  = 0x04000,
    DefaultCodeKind   = 0x08000,
    LicenseExclusions = 0x10000,
}
