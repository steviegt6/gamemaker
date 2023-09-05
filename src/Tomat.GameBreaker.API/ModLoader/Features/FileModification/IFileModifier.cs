namespace Tomat.GameBreaker.API.ModLoader.Features.FileModification;

/// <summary>
///     Allows mods to perform modifications on files retrieved through
///     <c>ReadBundleFile</c> and <c>ReadSaveFile</c>.
/// </summary>
/// <remarks>
///     If you'd specifically like to modify IFF (WAD) files, consider using
///     <see cref="IIffModifier"/> instead.
/// </remarks>
/// <seealso cref="IIffModifier"/>
public interface IFileModifier {
    /// <summary>
    ///     Determines whether this file modifier can modify the file at the
    ///     given <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path to the file to modify.</param>
    /// <param name="context">The file context.</param>
    /// <returns>
    ///     <see langword="true"/> if this file modifier can modify the file at
    ///     the given <paramref name="path"/>, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     This is called for every file that is read, and is not a security
    ///     feature. It is used as a performance optimization to avoid
    ///     unnecessary calls to <see cref="ModifyFile"/> to reduce the rate at
    ///     which we load files into memory and write them to disk.
    ///     <br />
    ///     Returning <see langword="false"/> does not prevent
    ///     <see cref="ModifyFile"/> from being called on the file. If one
    ///     <see cref="IFileModifier"/> returns <see langword="true"/>, all
    ///     <see cref="IFileModifier"/>s will be called on the file.
    /// </remarks>
    bool CanModify(string path, FileContext context);

    /// <summary>
    ///     Allows for modifications to files at the given
    ///     <paramref name="path"/>.
    ///     <br />
    ///     The file's <paramref name="data"/> is passed by reference, allowing
    ///     for arbitrary modifications to be made.
    /// </summary>
    /// <param name="path">The path to the file to modify.</param>
    /// <param name="context">The file context.</param>
    /// <param name="data">The mutable file data.</param>
    /// <returns>
    ///     <see langword="true"/> is modifications where made, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     The return value is not a security feature, but rather an
    ///     optimization. Similarly to <see cref="CanModify"/>, it exists for
    ///     performance reasons: we don't want to write the file if we don't
    ///     need to. Mods may need to load the file to analyze data, but may not
    ///     need to modify it (or may only need to modify it conditionally). If
    ///     everything works out and no edits are made, we shouldn't bother
    ///     writing the file and loading the modified version.
    /// </remarks>
    bool ModifyFile(string path, FileContext context, ref byte[] data);
}
