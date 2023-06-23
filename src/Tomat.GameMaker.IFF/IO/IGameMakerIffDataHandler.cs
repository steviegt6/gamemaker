using System.Text;

namespace Tomat.GameMaker.IFF.IO;

/// <summary>
///     Represents an object which can handle data belonging to a GameMaker IFF
///     file, whether it be reading or writing.
/// </summary>
public interface IGameMakerIffDataHandler {
    /// <summary>
    ///     The default encoding to use when reading or writing strings, UTF-8
    ///     without a BOM.
    /// </summary>
    public static readonly Encoding DEFAULT_ENCODING = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    /// <summary>
    ///     The data to read or write.
    /// </summary>
    byte[] Data { get; }

    /// <summary>
    ///     The position within <see cref="Data"/>.
    /// </summary>
    long Position { get; set; }

    /// <summary>
    ///     The length of <see cref="Data"/> while reading or actual length of
    ///     <see cref="Data"/> while writing.
    /// </summary>
    long Length { get; }
    
    /// <summary>
    ///     The encoding to use when reading or writing strings.
    /// </summary>
    Encoding Encoding { get; }
}
