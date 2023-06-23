using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.Contexts;

public record DeserializationContext(GameMakerIffReader Reader, GameMakerIffFile IffFile);
