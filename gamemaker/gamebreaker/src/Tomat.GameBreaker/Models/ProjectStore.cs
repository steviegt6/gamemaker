using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tomat.GameBreaker.Models;

public sealed class ProjectStore {
    [JsonProperty("projectPaths")]
    public List<string> ProjectPaths { get; set; } = new();
}
