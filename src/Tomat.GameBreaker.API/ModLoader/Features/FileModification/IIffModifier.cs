using Tomat.GameMaker.IFF;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameBreaker.API.ModLoader.Features.FileModification; 

/// <summary>
///     An alternative to <see cref="IFileModifier"/> custom-tailored to
///     GameMaker IFF files.
/// </summary>
/// <remarks>
///     This distinction exists to allow for more efficient modifications to
///     IFF files by utilizing a single <see cref="GameMakerIffFile"/> instance.
/// </remarks>
/// <seealso cref="IFileModifier"/>
public interface IIffModifier {
    /// <summary>
    ///     Determines whether this IFF modifier can modify the IFF at the
    ///     given <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path to the IFF to modify.</param>
    /// <param name="context">The IFF file context.</param>
    /// <returns>
    ///     <see langword="true"/> if this IFF modifier can modify IFF file at
    ///     the given <paramref name="path"/>, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     This is called for every IFF that is read, and is not a security
    ///     feature. It is used as a performance optimization to avoid
    ///     unnecessary calls to <see cref="ModifyIff"/> to reduce the rate at
    ///     which we load IFFs into memory and write them to disk.
    ///     <br />
    ///     Returning <see langword="false"/> does not prevent
    ///     <see cref="ModifyIff"/> from being called on the file. If one
    ///     <see cref="IIffModifier"/> returns <see langword="true"/>, all
    ///     <see cref="IIffModifier"/>s will be called on the IFF.
    /// </remarks>
    bool CanModify(string path, FileContext context);
    
    /// <summary>
    ///     Allows for modification to IFF at the given <paramref name="path"/>.
    ///     <br />
    ///     The IFF's data (passed through <paramref name="iffContext"/>) is
    ///     given, allowing for modification to be made.
    /// </summary>
    /// <param name="path">The path to the IFF to modify.</param>
    /// <param name="context">The IFF file context.</param>
    /// <param name="iffContext">
    ///     The IFF deserialization context, including a
    ///     <see cref="GameMakerIffFile"/> instance and its accompanying
    ///     <see cref="GameMakerVersionInfo"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> is modifications where made, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     The return value is not a security feature, but rather an
    ///     optimization. Similarly to <see cref="CanModify"/>, it exists for
    ///     performance reasons: we don't want to write the IFF if we don't
    ///     need to. Mods may need to load the IFF to analyze data, but may not
    ///     need to modify it (or may only need to modify it conditionally). If
    ///     everything works out and no edits are made, we shouldn't bother
    ///     writing the IFF and loading the modified version.
    /// </remarks>
    bool ModifyIff(string path, FileContext context, DeserializationContext iffContext);
}
