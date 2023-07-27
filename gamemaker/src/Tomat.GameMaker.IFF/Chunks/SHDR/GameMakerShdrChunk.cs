using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Shader;

namespace Tomat.GameMaker.IFF.Chunks.SHDR;

internal sealed class GameMakerShdrChunk : AbstractChunk,
                                           IShdrChunk {
    public const string NAME = "SHDR";

    public List<GameMakerPointer<GameMakerShader>> Shaders { get; set; } = null!;

    public GameMakerShdrChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.Position -= 4;
        var chunkEnd = context.Position + sizeof(int) + context.ReadInt32();

        var count = context.ReadInt32();
        var pointers = new GameMakerPointer<GameMakerShader>[count];

        for (var i = 0; i < count; i++) {
            pointers[i] = new GameMakerPointer<GameMakerShader> {
                Address = context.ReadInt32(),
            };
        }

        Shaders = new List<GameMakerPointer<GameMakerShader>>(count);

        for (var i = 0; i < count; i++) {
            var shader = pointers[i].GetOrInitializePointerObject(context);
            context.Position = pointers[i].Address;
            shader!.Read(context, i < count - 1 ? pointers[i + 1].Address : chunkEnd);
            Shaders.Add(pointers[i]);
        }
    }

    public override void Write(SerializationContext context) {
        context.Write(Shaders!.Count);
        foreach (var shader in Shaders)
            context.Write(shader);
        foreach (var shader in Shaders)
            context.MarkPointerAndWriteObject(shader);
    }
}
