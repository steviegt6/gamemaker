using System;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.FORM;
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

    public void Read(DeserializationContext context) {
        var formName = new string(context.Reader.ReadChars(IGameMakerChunk.NAME_LENGTH));
        if (formName != GameMakerFormChunk.NAME)
            throw new IOException("IFF file does not start with FORM chunk.");

        var formSize = context.Reader.ReadInt32();
        Form = new GameMakerFormChunk(formName, formSize);
        Form.Read(context);
    }

    public void Write(SerializationContext context) {
        if (Form is null)
            throw new IOException("Cannot write IFF file without a FORM chunk.");

        context.Writer.Write("FORM"u8.ToArray());
        var formSizePos = context.Writer.BeginLength();
        Form.Write(context);
        context.Writer.EndLength(formSizePos);

        context.Writer.FinalizePointers();
    }

    // TODO: Reader options that lets us specify things like the GameMaker
    // version, for the sake of time optimizations.
    /// <summary>
    ///     Reads a GameMaker IFF file from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <param name="context">The deserialization context.</param>
    /// <returns>
    ///     A <see cref="GameMakerIffFile"/> read from the
    ///     <paramref name="stream"/>.
    /// </returns>
    public static GameMakerIffFile FromStream(Stream stream, out DeserializationContext context) {
        var reader = GameMakerIffReader.FromStream(stream);
        var file = new GameMakerIffFile();
        file.Read(context = new DeserializationContext(reader, file, new GameMakerVersionInfo()));
        return file;
    }
}
