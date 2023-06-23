using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF; 

/// <summary>
///     Represents a GameMaker IFF file.
/// </summary>
public sealed class GameMakerIffFile : IGameMakerSerializable {
    /// <summary>
    ///     Gets or sets the FORM chunk of the file. This is the only chunk
    ///     within a GameMaker IFF file, and other chunks are contained within
    ///     this chunk.
    /// </summary>
    public GameMakerFormChunk? Form { get; set; }

    public int Read(DeserializationContext context) {
        Form = new GameMakerFormChunk();
        return Form.Read(context);
    }

    public int Write(SerializationContext context) {
        if (Form is null)
            throw new IOException("Cannot write IFF file without a FORM chunk.");

        return Form.Write(context);
    }
}
