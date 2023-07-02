using System.Linq;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.TXTR;

public sealed class GameMakerTxtrChunk : AbstractChunk {
    public const string NAME = "TXTR";

    public GameMakerPointerList<GameMakerTexturePage>? TexturePages { get; set; }

    public GameMakerTxtrChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        DoFormatCheck(context);

        TexturePages = new GameMakerPointerList<GameMakerTexturePage>();
        TexturePages.Read(context);
    }

    public override void Write(SerializationContext context) {
        TexturePages!.Write(context);
        foreach (var page in TexturePages)
            page.Object!.TextureData.Object!.Write(context);

        context.Writer.Pad(4);
    }

    private static void DoFormatCheck(DeserializationContext context) {
        if (context.VersionInfo.Version >= GameMakerVersionInfo.GM_2_3) {
            var begin = context.Reader.Position;

            if (context.VersionInfo.Version < GameMakerVersionInfo.GM_2022_3) {
                // Check for 2022.3+
                var textureCount = context.Reader.ReadInt32();

                if (textureCount == 1) {
                    // If there isn't a zero after the first texture, then this
                    // is 2022.3 (the pointer was shifted back by four bytes,
                    // where alignment padding used to always be).
                    context.Reader.Position += 16;
                    if (context.Reader.ReadInt32() != 0)
                        context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_3);
                }
                else if (textureCount >= 2) {
                    // If the difference between the first two pointers is
                    // sixteen, then this is 2022.3.
                    var firstPointer = context.Reader.ReadInt32();
                    var secondPointer = context.Reader.ReadInt32();
                    if (secondPointer - firstPointer == 16)
                        context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_3);
                }
            }

            // Check for 2022.5+ by looking for discrepancies in the Bz2 format.
            if (context.VersionInfo.Version >= GameMakerVersionInfo.GM_2022_3 && context.VersionInfo.Version < GameMakerVersionInfo.GM_2022_5) {
                context.Reader.Position = begin;
                var textureCount = context.Reader.ReadInt32();

                for (var i = 0; i < textureCount; i++) {
                    // Go to each texture, and then to each texture's data.
                    context.Reader.Position = begin + 4 + (i * 4);
                    // Go to texture at an offset.
                    context.Reader.Position = context.Reader.ReadInt32() + 12;
                    // Go to texture data.
                    context.Reader.Position = context.Reader.ReadInt32();
                    var header = context.Reader.ReadBytes(4).ToArray();

                    if (header.SequenceEqual(GameMakerTextureData.QOI_BZIP2_HEADER)) {
                        context.Reader.Position += 4; // Skip width and height.

                        // Now check actual Bz2 headers.
                        if (context.Reader.ReadByte() != 'B' || context.Reader.ReadByte() != 'Z' || context.Reader.ReadByte() != 'h') {
                            context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_5);
                            break;
                        }

                        context.Reader.ReadByte();

                        // Block header is the starting digits of pi.
                        if (context.Reader.ReadUInt24() != 0x594131)
                            context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_5);
                        else if (context.Reader.ReadUInt24() != 0x595326)
                            context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_5);

                        // All we needed to do is check a single QoI+BZ2
                        // texture.
                        break;
                    }
                }
            }

            context.Reader.Position = begin;
        }
    }
}
