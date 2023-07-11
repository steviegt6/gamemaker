using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sound;

public sealed class GameMakerSound : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerSoundAudioEntryFlags AudioEntryFlags { get; set; }

    public GameMakerPointer<GameMakerString> AudioType { get; set; }

    public GameMakerPointer<GameMakerString> AudioFileName { get; set; }

    public uint AudioEffects { get; set; }

    public float Volume { get; set; }

    public float Pitch { get; set; }

    public int AudioId { get; set; }

    /// <summary>
    /// </summary>
    /// <remarks>
    ///     In older versions, this can also be a <see cref="Preload"/> boolean,
    ///     but it's always true, therefor we don't need to care.
    /// </remarks>
    public int GroupId { get; set; }

    /// <summary>
    /// </summary>
    /// <remarks>
    ///     Legacy (<see cref="GameMakerVersionInfo.FormatId"/> &lt; 14).
    /// </remarks>
    public bool Preload { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        AudioEntryFlags = (GameMakerSoundAudioEntryFlags)context.ReadUInt32();
        AudioType = context.ReadPointerAndObject<GameMakerString>();
        AudioFileName = context.ReadPointerAndObject<GameMakerString>();
        AudioEffects = context.ReadUInt32();
        Volume = context.ReadSingle();
        Pitch = context.ReadSingle();

        if (context.VersionInfo.FormatId >= 14) {
            GroupId = context.ReadInt32();
            AudioId = context.ReadInt32();
        }
        else {
            GroupId = -1;
            AudioId = context.ReadInt32();
            Preload = context.ReadBoolean(wide: true);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((uint)AudioEntryFlags);
        context.Write(AudioType);
        context.Write(AudioFileName);
        context.Write(AudioEffects);
        context.Write(Volume);
        context.Write(Pitch);

        if (context.VersionInfo.FormatId >= 14) {
            context.Write(GroupId);
            context.Write(AudioId);
        }
        else {
            context.Write(AudioId);
            context.Write(Preload, wide: true);
        }
    }
}
