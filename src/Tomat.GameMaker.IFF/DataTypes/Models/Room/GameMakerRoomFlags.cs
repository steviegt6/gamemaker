using System;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

[Flags]
public enum GameMakerRoomFlags {
    EnableViews        = 0x0001,
    ShowColor          = 0x0002,
    ClearDisplayBuffer = 0x0004,
}
