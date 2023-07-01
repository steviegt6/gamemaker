using System;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

[Flags]
public enum GameMakerSoundAudioEntryFlags : uint {
    IsEmbedded   = 0x0001,
    IsCompressed = 0x0002,
    Regular      = 0x0064,
}
