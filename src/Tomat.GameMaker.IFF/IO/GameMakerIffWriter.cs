using System.Text;

namespace Tomat.GameMaker.IFF.IO;

public sealed class GameMakerIffWriter : IGameMakerIffDataHandler {
    public const int DEFAULT_CAPACITY = 1024 * 1024 * 10; // 10 MB

    public Encoding Encoding { get; }

    public byte[] Data { get; }

    public long Position { get; set; }

    public long Length { get; set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="GameMakerIffWriter"/>.
    /// </summary>
    /// <param name="capacity">
    ///     The base size of the <see cref="Data"/> array.
    /// </param>
    /// <param name="encoding">The <see cref="Encoding"/> to write in.</param>
    public GameMakerIffWriter(int capacity = DEFAULT_CAPACITY, Encoding? encoding = null) {
        Data = new byte[capacity];
        Position = 0;
        Length = 0;
        Encoding = encoding ?? IGameMakerIffDataHandler.DEFAULT_ENCODING;
    }
}
