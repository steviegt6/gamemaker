using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

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
        Name = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        AudioEntryFlags = (GameMakerSoundAudioEntryFlags)context.Reader.ReadUInt32();
        AudioType = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        AudioFileName = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        AudioEffects = context.Reader.ReadUInt32();
        Volume = context.Reader.ReadSingle();
        Pitch = context.Reader.ReadSingle();

        if (context.VersionInfo.FormatId >= 14) {
            GroupId = context.Reader.ReadInt32();
            AudioId = context.Reader.ReadInt32();
        }
        else {
            GroupId = -1;
            AudioId = context.Reader.ReadInt32();
            Preload = context.Reader.ReadBoolean(wide: true);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write((uint)AudioEntryFlags);
        context.Writer.Write(AudioType);
        context.Writer.Write(AudioFileName);
        context.Writer.Write(AudioEffects);
        context.Writer.Write(Volume);
        context.Writer.Write(Pitch);

        if (context.VersionInfo.FormatId >= 14) {
            context.Writer.Write(GroupId);
            context.Writer.Write(AudioId);
        }
        else {
            context.Writer.Write(AudioId);
            context.Writer.Write(Preload, wide: true);
        }
    }
}
