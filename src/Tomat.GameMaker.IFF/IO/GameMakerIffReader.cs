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
}
