using Tomat.GameMaker.IFF.DataTypes.Models.Audio;

namespace Tomat.GameMaker.IFF.Chunks.AGRP;

// TODO: Load audio group files (audiogroup{x}.dat).
internal sealed class GameMakerAgrpChunk : AbstractChunk,
                                           IAgrpChunk {
    public const string NAME = "AGRP";

    public GameMakerPointerList<GameMakerAudioGroup> AudioGroups { get; set; } = null!;

    public GameMakerAgrpChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        AudioGroups = context.ReadPointerList<GameMakerAudioGroup>();
    }

    public override void Write(SerializationContext context) {
        context.Write(AudioGroups);
    }
}
