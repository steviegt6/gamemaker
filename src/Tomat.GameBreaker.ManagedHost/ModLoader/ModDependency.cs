using System;
using Newtonsoft.Json;
using Tomat.GameBreaker.API.ModLoader;

namespace Tomat.GameBreaker.ManagedHost.ModLoader;

internal sealed class ModDependency : IModDependency {
    [JsonIgnore]
    public string Name => name ?? throw new ArgumentNullException(nameof(name), "Mod dependency had an unspecified name.");

    [JsonIgnore]
    public Version Version {
        get {
            if (parsedVersion is not null)
                return parsedVersion;

            if (version is null)
                throw new ArgumentNullException(nameof(version), "Mod metadata had an unspecified version.");

            if (!Version.TryParse(version, out parsedVersion))
                throw new ArgumentException("Mod metadata had an invalid version.", nameof(version));

            return parsedVersion;
        }
    }

    // Disgusting, evil hack: read to field 'version', then write from field
    // 'parsedVersion' (accessed through property 'Version'.
    // I am realizing after committing this that serialization isn't ever even
    // used... just deserialization. Oh well, keeping this.
    [JsonProperty("version")]
    private string? WritableVersion {
        get => Version.ToString();
        set => version = value;
    }

    [JsonProperty("name")]
    private string? name;

    [JsonIgnore]
    private string? version;

    [JsonIgnore]
    private Version? parsedVersion;
}
