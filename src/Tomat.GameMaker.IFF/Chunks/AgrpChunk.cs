using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     The <c>AGRP</c> chunk, which contains audio groups.
/// </summary>
public interface IAgrpChunk : IGameMakerChunk {
    /// <summary>
    ///     The list of audio groups.
    /// </summary>
    List<GameMakerPointer<IString>> AudioGroups { get; set; }
}

// TODO: Load audio group files (audiogroup{x}.dat).
internal sealed class GameMakerAgrpChunk : AbstractChunk,
                                           IAgrpChunk {
    public const string NAME = "AGRP";

    public List<GameMakerPointer<IString>> AudioGroups { get; set; } = null!;

    public GameMakerAgrpChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        AudioGroups = context.ReadPointerList<IString, GameMakerString>();
    }

    public override void Write(SerializationContext context) {
        context.WritePointerList(AudioGroups);
    }
}
