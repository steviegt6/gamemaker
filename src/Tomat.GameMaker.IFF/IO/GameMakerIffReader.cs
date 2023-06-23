using System.IO;
using System.Text;

namespace Tomat.GameMaker.IFF.IO;

/// <summary>
///     Responsible for reading data from a GameMaker IFF file.
/// </summary>
public sealed class GameMakerIffReader : IGameMakerIffDataHandler {
    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public long Position { get; set; }

    public long Length => Data.Length;

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffReader"/>.
    /// </summary>
    /// <param name="data">The <see cref="Data"/> to read from.</param>
    /// <param name="encoding">The <see cref="Encoding"/> to read in.</param>
    public GameMakerIffReader(byte[] data, Encoding? encoding = null) {
        Data = data;
        Position = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffReader"/> from
    ///     a <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <returns>An instance of <see cref="GameMakerIffReader"/>.</returns>
    /// <exception cref="IOException">
    ///     When the stream fails to read up to its length.
    /// </exception>
    public static GameMakerIffReader FromStream(Stream stream) {
        var data = new byte[stream.Length];
        var dataLength = stream.Read(data, 0, data.Length);
        if (dataLength != data.Length)
            throw new IOException($"Expected to read {data.Length} bytes, but only read {dataLength} bytes.");

        return new GameMakerIffReader(data);
    }
}
