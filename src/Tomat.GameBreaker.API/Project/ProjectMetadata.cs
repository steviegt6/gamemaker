using System;
using Newtonsoft.Json;

namespace Tomat.GameBreaker.API.Project;

public sealed class ProjectMetadata {
    [JsonProperty]
    public string? Name { get; set; }

    [JsonProperty("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonProperty("projectHash")]
    public string? ProjectHash { get; set; }

    [JsonProperty("gameMakerVersion")]
    public Version? GameMakerVersion { get; set; }
}
