using System.Diagnostics;
using Tomat.GameBreaker.API.DependencyInjection;

namespace Tomat.GameBreaker.API;

/// <summary>
///     Represents a running GameMaker game.
/// </summary>
public abstract class Game {
    public abstract IServiceProvider ServiceProvider { get; }

    public abstract void Initialize();

    public virtual void WaitForProcessExit(int processId) {
        var process = Process.GetProcessById(processId);
        process.WaitForExit();
    }
}
