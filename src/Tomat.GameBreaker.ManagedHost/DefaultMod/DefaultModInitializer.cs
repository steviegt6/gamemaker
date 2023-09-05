using System;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.DefaultMod;

internal class DefaultModInitializer : IModInitializer {
    public IMod Mod { get; set; } = null!;

    public void Initialize(Game game) {
        Console.WriteLine($"Hello, this is {nameof(DefaultModInitializer)} speaking!");
    }
}
