using Tomat.GameMaker.IFF.DataTypes.Models.FunctionEntry;
using Tomat.GameMaker.IFF.DataTypes.Models.Local;

namespace Tomat.GameMaker.IFF.Chunks.FUNC;

internal sealed class GameMakerFuncChunk : AbstractChunk,
                                           IFuncChunk {
    public const string NAME = "FUNC";

    public GameMakerList<GameMakerFunctionEntry> FunctionEntries { get; set; } = null!;

    public GameMakerList<GameMakerLocalsEntry> Locals { get; set; } = null!;

    public GameMakerFuncChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        FunctionEntries = new GameMakerList<GameMakerFunctionEntry>();
        Locals = new GameMakerList<GameMakerLocalsEntry>();

        if (context.VersionInfo.FormatId <= 14) {
            var startPos = context.Position;

            while (context.Position + 12 <= startPos + Size) {
                var entry = new GameMakerFunctionEntry();
                entry.Read(context);
                FunctionEntries.Add(entry);
            }
        }
        else {
            FunctionEntries.Read(context);
            Locals.Read(context);
        }
    }

    public override void Write(SerializationContext context) {
        if (context.VersionInfo.FormatId <= 14) {
            foreach (var entry in FunctionEntries)
                entry.Write(context);
        }
        else {
            FunctionEntries.Write(context);
            Locals.Write(context);
        }
    }
}
