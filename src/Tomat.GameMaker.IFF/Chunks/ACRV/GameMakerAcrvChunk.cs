using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

/* chunk ACRV {
 *     align[4];
 *     int32 version;
 *     list<model {
 *         GMAnimationCurve curve;
 *         align[4];
 *     }*> curves;
 * }
 */

internal sealed class GameMakerAcrvChunk : AbstractChunk,
                                           IAcrvChunk {
    public const string NAME = "ACRV";
    public const int VERSION = 1;

    public int ChunkVersion { get; set; }

    public GameMakerPointerList<GameMakerAnimationCurve> AnimationCurves { get; set; } = null!;

    public GameMakerAcrvChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.GmAlign(4);
        ChunkVersion = context.ReadInt32().Expect(VERSION, new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}."));
        AnimationCurves = context.ReadPointerList<GameMakerAnimationCurve>();
    }

    public override void Write(SerializationContext context) {
        context.GmAlign(4);
        context.Write(ChunkVersion.Expect(VERSION, new InvalidDataException($"Expected chunk version 1, got {ChunkVersion}.")));
        context.Write(AnimationCurves);
    }
}
