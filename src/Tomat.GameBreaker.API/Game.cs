using Tomat.GameBreaker.API.DependencyInjection;

namespace Tomat.GameBreaker.API;

/// <summary>
///     Represents a running GameMaker game.
/// </summary>
public abstract class Game {
    public abstract IServiceProvider ServiceProvider { get; }
}
