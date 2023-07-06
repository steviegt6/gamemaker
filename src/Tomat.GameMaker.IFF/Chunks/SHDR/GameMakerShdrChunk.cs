using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Shader;

namespace Tomat.GameMaker.IFF.Chunks.SHDR; 

public sealed class GameMakerShdrChunk : AbstractChunk {
    public const string NAME = "SHDR";

    public List<GameMakerShader>? Shaders { get; set; }

    public GameMakerShdrChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.Reader.Position -= 4;
        var chunkEnd = context.Reader.Position + sizeof(int) + context.Reader.ReadInt32();

        var count = context.Reader.ReadInt32();
        var pointers = new int[count];
        for (var i = 0; i < count; i++)
            pointers[i] = context.Reader.ReadInt32();
        
        Shaders = new List<GameMakerShader>(count);

        for (var i = 0; i < count; i++) {
            var shader = new GameMakerShader();
            context.Reader.Position = pointers[i];
            shader.Read(context, i < count - 1 ? pointers[i + 1] : chunkEnd);
            Shaders.Add(shader);
        }
    }

    public override void Write(SerializationContext context) {
        throw new System.NotImplementedException();
    }
}
