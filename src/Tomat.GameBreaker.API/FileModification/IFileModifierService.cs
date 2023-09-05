using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameBreaker.API.FileModification;

/// <summary>
///     A service that can modify files in the game.
/// </summary>
/// <seealso cref="IFileModifier"/>
/// <seealso cref="IIffModifier"/>
public interface IFileModifierService {
    /// <summary>
    ///     Adds a <see cref="IFileModifier"/> to the service.
    /// </summary>
    /// <param name="modifier">The modifier to add.</param>
    void AddFileModifier(IFileModifier modifier);

    /// <summary>
    ///     Adds a <see cref="IIffModifier"/> to the service.
    /// </summary>
    /// <param name="modifier">
    ///     The IFF modifier to add.
    /// </param>
    void AddIffModifier(IIffModifier modifier);

    /// <summary>
    ///     Attempts to modify the file at the given <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the file to modify.</param>
    /// <param name="context">The context of the file.</param>
    /// <param name="newPath">The newly-written-to path.</param>
    /// <returns>
    ///     <see langword="true"/> if the file was modified, otherwise
    ///     <see langword="false"/>.
    /// </returns>
    bool TryModifyFile(string path, FileContext context, [NotNullWhen(returnValue: true)] out string? newPath);
}
