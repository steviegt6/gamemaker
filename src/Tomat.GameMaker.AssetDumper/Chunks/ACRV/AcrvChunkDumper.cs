using Newtonsoft.Json;
using Tomat.GameMaker.Decompiler;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.ACRV;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.AssetDumper.Chunks.ACRV;

public sealed class AcrvChunkDumper : AbstractChunkDumper<IAcrvChunk> {
    public override void DumpChunk(DeserializationContext context, IAcrvChunk chunk, IGameMakerDecompiler? decompiler, string directory) {
        base.DumpChunk(context, chunk, decompiler, directory);

        string getCurveName(GameMakerPointer<GameMakerAnimationCurve> pCurve) {
            /*if (!pCurve.TryGetObject(out var curve))
                return "<null>";

            if (!curve.Name.TryGetObject(out var name))
                return "<null>";
            
            return name.Value;*/

            // We can safely assume the name will always be included... for now.
            return pCurve.ExpectObject().Name.ExpectObject().Value;
        }

        var overview = new Dictionary<string, object> {
            { "chunkVersion", chunk.ChunkVersion },
            { "animationCurves", chunk.AnimationCurves.Select(getCurveName).ToList() },
        };
        var overviewJson = JsonConvert.SerializeObject(overview, Formatting.Indented);
        File.WriteAllText(Path.Combine(directory, "overview.json"), overviewJson);

        var dir = Path.Combine(directory, "animation_curves");
        Directory.CreateDirectory(dir);

        foreach (var gamemakerCurve in chunk.AnimationCurves.Select(x => x.ExpectObject())) {
            var name = gamemakerCurve.Name.ExpectObject().Value;
            var curve = FromGameMaker(gamemakerCurve);

            var path = Path.Combine(directory, "animation_curves", name + ".json");
            var curveJson = JsonConvert.SerializeObject(curve, Formatting.Indented);
            File.WriteAllText(path, curveJson);
        }
    }

    private static AnimationCurve FromGameMaker(GameMakerAnimationCurve curve) {
        return new AnimationCurve {
            Name = curve.Name.ExpectObject().Value,
            GraphType = curve.GraphType,
            Channels = curve.Channels.Select(FromGameMaker).ToList(),
        };
    }

    private static AnimationCurveChannel FromGameMaker(GameMakerAnimationCurveChannel channel) {
        return new AnimationCurveChannel {
            Name = channel.Name.ExpectObject().Value,
            FunctionType = channel.FunctionType,
            Iterations = channel.Iterations,
            Points = channel.Points.Select(FromGameMaker).ToList(),
        };
    }

    private static AnimationCurveChannelPoint FromGameMaker(GameMakerAnimationCurveChannelPoint point) {
        return new AnimationCurveChannelPoint {
            X = point.X,
            Value = point.Value,
            BezierX0 = point.BezierX0,
            BezierY0 = point.BezierY0,
            BezierX1 = point.BezierX1,
            BezierY1 = point.BezierY1,
        };
    }
}

public sealed class AnimationCurve {
    [JsonProperty("name")]
    public required string? Name { get; init; }

    [JsonProperty("graphType")]
    public required GameMakerAnimationCurveGraphType GraphType { get; init; }

    [JsonProperty("channels")]
    public required List<AnimationCurveChannel> Channels { get; init; }
}

public sealed class AnimationCurveChannel {
    public required string? Name { get; init; }

    public required GameMakerAnimationCurveChannelFunctionType FunctionType { get; init; }

    public required ushort Iterations { get; init; }

    public required List<AnimationCurveChannelPoint> Points { get; init; }
}

public sealed class AnimationCurveChannelPoint {
    public required float X { get; init; }

    public required float Value { get; init; }

    public required float BezierX0 { get; init; }

    public required float BezierY0 { get; init; }

    public required float BezierX1 { get; init; }

    public required float BezierY1 { get; init; }
}
