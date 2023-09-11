using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Medallion.Collections;
using Newtonsoft.Json;
using Tomat.GameBreaker.API;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.ModLoader;

internal sealed class DefaultModLoader : IModLoader {
    private class ModIdentity {
        public string Name { get; }

        public Version Version { get; }

        public List<ModIdentity> Dependencies { get; }

        public ModIdentity(IMod mod) {
            Name = mod.Metadata.Name;
            Version = mod.Metadata.Version;
            Dependencies = mod.Metadata.Dependencies?.Select(x => new ModIdentity(x)).ToList() ?? new List<ModIdentity>();
        }

        public ModIdentity(IModDependency dependency) {
            Name = dependency.Name;
            Version = dependency.Version;
            Dependencies = new List<ModIdentity>();
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }

    private readonly Dictionary<string, IMod> loadedMods = new();

    public IEnumerable<IMod> ResolveModsFromDirectory(string directory) {
        var mods = new List<IMod>();

        // Retrieve only the top-level directories beneath the main mod
        // directory. We shouldn't want to encourage unorthodox mod directory
        // structures.
        var directories = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);

        foreach (var modDir in directories) {
            // This is essentially the only hard restriction we put on mod
            // directories aside from no nesting -- the mod directory must
            // match:
            //  - the mod's assembly name,
            //  - the name of the assembly (+ '.dll'),
            //  - and the namespace in which the metadata file is kept (the
            //    metadata file must also be named 'metadata.json').
            // Even the mod name within the metadata file doesn't need to match.
            // This provided canonical name can be thought of as an
            // implementation name, and the given mod name is an interface name.
            var canonicalModName = Path.GetFileName(modDir);
            var modDll = Path.Combine(modDir, canonicalModName + ".dll");

            // TODO: Should we warn in these cases?
            // Alternative question: should we ever expect non-mod directories
            // in these searches? I'd say yes, since this method isn't built
            // with such restrictions in mind.
            if (!File.Exists(modDll))
                continue;

            // Just load the assembly, don't load any types or anything else
            // dangerous yet.
            var modLoadContext = new DefaultLoadContext(canonicalModName);
            var asm = modLoadContext.LoadFromAssemblyPath(modDll);

            // Resolve the mod's metadata. This should be safe.
            using var metadataStream = asm.GetManifestResourceStream(canonicalModName + ".metadata.json");
            if (metadataStream is null)
                throw new FileNotFoundException("Could not find metadata file for mod.", canonicalModName + ".metadata.json");

            using var metadataReader = new StreamReader(metadataStream);
            var metadata = JsonConvert.DeserializeObject<ModMetadata>(metadataReader.ReadToEnd());
            if (metadata is null)
                throw new FileLoadException("Could not deserialize metadata file for mod.", canonicalModName + ".metadata.json");

            var assemblyResolver = new AssemblyResolver(modLoadContext, asm, modDir);

            var mod = new Mod(metadata, modLoadContext, assemblyResolver, asm);
            modLoadContext.RegisterSelf(mod);
            mods.Add(mod);
        }

        return mods;
    }

    public void SortAndRegisterMods(IEnumerable<IMod> mods) {
        // TODO: Keep enforcing this?
        if (loadedMods.Count != 0)
            throw new InvalidOperationException("Cannot load mods more than once.");

        mods = mods.ToList();
        var modIdentities = mods.Select(x => new ModIdentity(x));
        modIdentities = modIdentities.StableOrderTopologicallyBy(x => x.Dependencies);
        var identityDict = modIdentities.ToDictionary(x => x.Name, x => x);
        var modDict = mods.ToDictionary(x => x.Metadata.Name, x => x);

        foreach (var modIdentity in identityDict.Values) {
            var mod = modDict[modIdentity.Name];

            foreach (var dependency in modIdentity.Dependencies) {
                if (!identityDict.TryGetValue(dependency.Name, out var dependencyIdentity))
                    throw new InvalidOperationException($"Mod '{modIdentity.Name}' depends on mod '{dependency.Name}', but it is not loaded.");

                if (dependency.Version < dependencyIdentity.Version || dependencyIdentity.Version.Major > dependency.Version.Major)
                    throw new InvalidOperationException($"Mod '{modIdentity.Name}' depends on mod '{dependency.Name}' version {dependency.Version}, but version {dependencyIdentity.Version} is loaded.");

                var dependencyMod = modDict[dependency.Name];
                mod.AssemblyResolver.AddDependency(dependencyMod.AssemblyResolver);
                mod.LoadContext.RegisterDependency(dependencyMod);
            }
        }

        // Just add them in case we want to allow mods to be loaded more than
        // once in the future (as opposed to directly setting the object).
        foreach (var mod in mods)
            loadedMods.Add(mod.Metadata.Name, mod);
    }

    public void LoadMods(Game game) {
        foreach (var mod in loadedMods.Values) {
            foreach (var type in mod.Assembly.GetTypes()) {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (type.GetConstructor(Type.EmptyTypes) is null)
                    continue;

                if (!typeof(IModInitializer).IsAssignableFrom(type))
                    continue;

                var initializer = (IModInitializer) Activator.CreateInstance(type)!;
                initializer.Mod = mod;
                initializer.Initialize(game);
            }
        }
    }

    public bool TryGetMod(string name, [NotNullWhen(returnValue: true)] out IMod? mod) {
        return loadedMods.TryGetValue(name, out mod);
    }
}
