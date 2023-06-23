using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.Contexts;

public record SerializationContext(GameMakerIffWriter Writer, GameMakerIffFile IffFile);
