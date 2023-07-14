using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.Decompiler;

public sealed record DecompilerContext(DeserializationContext DeserializationContext, IGameMakerDecompiler Decompiler);
