using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Shader;

namespace Tomat.GameMaker.IFF.Chunks.SHDR;

public interface IShdrChunk : IGameMakerChunk {
    List<GameMakerPointer<GameMakerShader>> Shaders { get; set; }
}
