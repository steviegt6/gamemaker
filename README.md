# gamemaker

This is a monorepository for GameMaker reverse engineering.

# Tomat.GameMaker

Various GameMaker-related projects and utilities. The main project is `Tomat.GameMaker.IFF`, which is a feature-complete GameMaker IFF/WAD serializer and deserializer.

Technically, it should be capable of arbitrary modifications; it lacks helper utilities for convenient editing, but oh well!

# Tomat.GameBreaker

YYC-compatible runtime modding API and mod loader.

## Building

I personally use MSVC for compiling `Tomat.GameBreaker.Host`, you'll have to write your own build scripts if you don't want to use MSBuild.

Once built, add it to the folder with the game you wish to mod and rename the dll to `dbgcore.dll` (for proxying). Also be sure to copy nethost.dll.

You'll need .NET 7 installed, too, of course.

You'll want to add a file named `gamebreaker.json` to the game you want to mod; since you're building yourself, you'll want to specify output directories (these are my personal settings):

```json
{
    "actAsUniprox": true,
    "dotnet_runtimeconfig_path": "C:\\Users\\xxlen\\Documents\\Repositories\\steviegt6\\gamemaker\\src\\Tomat.GameBreaker.ManagedHost\\bin\\Debug\\net7.0\\Tomat.GameBreaker.ManagedHost.runtimeconfig.json",
    "dotnet_assembly_path": "C:\\Users\\xxlen\\Documents\\Repositories\\steviegt6\\gamemaker\\src\\Tomat.GameBreaker.ManagedHost\\bin\\Debug\\net7.0\\Tomat.GameBreaker.ManagedHost.dll"
}
```

Make sure they point to the build output of `Tomat.GameBreaker.ManagedHost` (which you can build normally).

Once this is done, you should be able to run the game with GameBreaker injected.