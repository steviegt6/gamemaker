using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.IO;

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
        var formName = new string(context.Reader.ReadChars(IGameMakerChunk.NAME_LENGTH));
        if (formName != GameMakerFormChunk.NAME)
            throw new IOException("IFF file does not start with FORM chunk.");

        var formSize = context.Reader.ReadInt32();
        Form = new GameMakerFormChunk(formName, formSize);
        Form.Read(context);
        return IGameMakerChunk.NAME_LENGTH + sizeof(int) + formSize;
    }

    public int Write(SerializationContext context) {
        if (Form is null)
            throw new IOException("Cannot write IFF file without a FORM chunk.");

        context.Writer.Write("FORM"u8.ToArray());
        var sizePos = context.Writer.Position;
        context.Writer.Write(0); // Placeholder for size.
        var size = Form.Write(context);
        var endPos = context.Writer.Position;
        context.Writer.Position = sizePos;
        context.Writer.Write(size);
        context.Writer.Position = endPos;

        return IGameMakerChunk.NAME_LENGTH + sizeof(int) + size;
    }

    // TODO: Reader options that lets us specify things like the GameMaker
    // version, for the sake of time optimizations.
    /// <summary>
    ///     Reads a GameMaker IFF file from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <returns>
    ///     A <see cref="GameMakerIffFile"/> read from the
    ///     <paramref name="stream"/>.
    /// </returns>
    public static GameMakerIffFile FromStream(Stream stream) {
        var reader = GameMakerIffReader.FromStream(stream);
        var file = new GameMakerIffFile();
        file.Read(new DeserializationContext(reader, file));
        return file;
    }
}
