using System.IO;

namespace Tomat.GameBreaker.API.Project;

/// <summary>
///     A GameBreaker project.
/// </summary>
public sealed class GameBreakerProject {
    public const string METADATA_FILE_NAME = "project.json";
    public const string LOCAL_METADATA_FILE_NAME = "project.user.json";

    public ProjectMetadata? Metadata { get; set; }

    public LocalProjectMetadata? LocalMetadata { get; set; }

    public static GameBreakerProject LoadFromDirectory(string directory) {
        var metadataPath = Path.Combine(directory, METADATA_FILE_NAME);
        var localMetadataPath = Path.Combine(directory, LOCAL_METADATA_FILE_NAME);
        throw new System.NotImplementedException();
    }

    public static GameBreakerProject CreateInDirectory(string directory, string projectName, string gamePath) {
        throw new System.NotImplementedException();
    }
}
