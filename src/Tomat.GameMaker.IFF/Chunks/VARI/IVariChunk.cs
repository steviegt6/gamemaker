using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models.Variable;

namespace Tomat.GameMaker.IFF.Chunks.VARI;

public interface IVariChunk : IGameMakerChunk {
    List<GameMakerVariable> Variables { get; set; }
}
