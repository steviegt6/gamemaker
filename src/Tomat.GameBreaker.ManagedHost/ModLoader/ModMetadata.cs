using System;
using Newtonsoft.Json;
using Tomat.GameBreaker.API.ModLoader;
using Tomat.GameBreaker.ManagedHost.Utilities;

namespace Tomat.GameBreaker.ManagedHost.ModLoader;

internal sealed class ModMetadata : IModMetadata {
    [JsonProperty("metadata_version")]
    public int MetadataVersion { get; private set; }

    [JsonIgnore]
    public string Name => name ?? throw new ArgumentNullException(nameof(name), "Mod metadata had an unspecified name.");

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

    [JsonProperty("dependencies")]
    [JsonConverter(typeof(InterfaceJsonConverter<IModDependency, ModDependency>))]
    public IModDependency[]? Dependencies { get; }

    // Disgusting, evil hack: read to field 'version', then write from field
    // 'parsedVersion' (accessed through property 'Version'.
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
