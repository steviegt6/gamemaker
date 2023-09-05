using JetBrains.Annotations;

namespace Tomat.GameBreaker.API.ModLoader;

/// <summary>
///     A mod initializer which may be used to initialize a mod.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
public interface IModInitializer {
    /// <summary>
    ///     The mod that this initializer belongs to.
    /// </summary>
    /// <remarks>
    ///     This will *NOT* be set in the constructor. It's handled by the
    ///     <see cref="IModLoader"/> implementation.
    /// </remarks>
    IMod Mod { get; set; }

    /// <summary>
    ///     Initializes a mod in the context of the <paramref name="game"/>.
    /// </summary>
    /// <param name="game">The game to initialize in the context of.</param>
    void Initialize(Game game);
}
