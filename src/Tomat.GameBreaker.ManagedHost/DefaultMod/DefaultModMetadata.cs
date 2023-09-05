using System;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.DefaultMod;

internal sealed class DefaultModMetadata : IModMetadata {
    public int MetadataVersion => 1;

    public string Name => "GameBreaker";

    public Version Version => typeof(Program).Assembly.GetName().Version ?? throw new InvalidOperationException("Assembly version is null.");

    public IModDependency[]? Dependencies => null;
}
