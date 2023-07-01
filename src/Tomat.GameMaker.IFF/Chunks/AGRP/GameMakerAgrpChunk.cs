using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.AGRP;

// TODO: Load audio group files (audiogroup{x}.dat).
public sealed class GameMakerAgrpChunk : AbstractChunk {
    public const string NAME = "AGRP";

    public GameMakerPointerList<GameMakerAudioGroup>? AudioGroups { get; set; }

    public GameMakerAgrpChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        AudioGroups = new GameMakerPointerList<GameMakerAudioGroup>();
        AudioGroups.Read(context);
    }

    public override void Write(SerializationContext context) {
        AudioGroups!.Write(context);
    }
}
