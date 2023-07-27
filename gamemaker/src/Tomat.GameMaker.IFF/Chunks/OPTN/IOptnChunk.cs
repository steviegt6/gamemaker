using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Constant;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;

namespace Tomat.GameMaker.IFF.Chunks.OPTN;

public interface IOptnChunk : IGameMakerChunk {
    ulong UnknownUInt64 { get; set; }

    OptnOptionFlags Options { get; set; }

    int Scale { get; set; }

    uint WindowColor { get; set; }

    uint ColorDepth { get; set; }

    uint Resolution { get; set; }

    uint Frequency { get; set; }

    uint VertexSync { get; set; }

    uint Priority { get; set; }

    GameMakerPointer<GameMakerTextureItem> SplashBackImage { get; set; }

    GameMakerPointer<GameMakerTextureItem> SplashFrontImage { get; set; }

    GameMakerPointer<GameMakerTextureItem> SplashLoadImage { get; set; }

    uint LoadAlpha { get; set; }

    GameMakerList<GameMakerConstant> Constants { get; set; }
}
