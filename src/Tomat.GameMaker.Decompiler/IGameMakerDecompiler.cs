using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models.Code;

namespace Tomat.GameMaker.Decompiler;

public interface IGameMakerDecompiler {
    DecompilerResult DecompileFunction(DecompilerContext context, GameMakerCode code);

    Dictionary<string, DecompilerResult> DecompileIffFile(DecompilerContext context);
}
