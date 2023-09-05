using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tomat.GameBreaker.API.ModLoader;

public interface IModLoader {
    /// <summary>
    ///     Resolves a collection of mods from the given directory.
    /// </summary>
    /// <param name="directory">The directory to resolve mods from.</param>
    /// <returns>A collection of resolved mods.</returns>
    /// <remarks>
    ///     This does not do any loading. It just provides awareness of mods.
    ///     <br />
    ///     Dependency validation, assembly resolution, etc. are handled
    ///     elsewhere. Providers for such data are created here, though.
    /// </remarks>
    IEnumerable<IMod> ResolveModsFromDirectory(string directory);

    /// <summary>
    ///     Sorts and registers the given <paramref name="mods"/>.
    /// </summary>
    /// <param name="mods">The mods to sort and register.</param>
    /// <remarks>
    ///     This is not directly tied to <see cref="ResolveModsFromDirectory"/>.
    ///     <br />
    ///     You may register any number of mods from any number of calls to the
    ///     aforementioned method, then combine their outputs and pass them
    ///     to this method.
    /// </remarks>
    void SortAndRegisterMods(IEnumerable<IMod> mods);

    /// <summary>
    ///     Loads the mods in the context of the given <paramref name="game"/>.
    /// </summary>
    /// <param name="game">The game for which to load mods.</param>
    /// <remarks>
    ///     This handles the invocation of initializers and other relevant
    ///     operations.
    /// </remarks>
    void LoadMods(Game game);

    /// <summary>
    ///     Attempts to get a mod by name.
    /// </summary>
    /// <param name="name">The name of the mod.</param>
    /// <param name="mod">The mod.</param>
    /// <returns>
    ///     <see langword="true" /> if the mod was found; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    bool TryGetMod(string name, [NotNullWhen(true)] out IMod? mod);
}
