using System.Linq;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.TXTR;

public sealed class GameMakerTxtrChunk : AbstractChunk {
    public const string NAME = "TXTR";

    public GameMakerPointerList<GameMakerTexturePage> TexturePages { get; set; } = null!;

    public GameMakerTxtrChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        DoFormatCheck(context);

        TexturePages = context.ReadPointerList<GameMakerTexturePage>();
    }

    public override void Write(SerializationContext context) {
        context.Write(TexturePages);
        foreach (var page in TexturePages)
            page.ExpectObject().TextureData.ExpectObject().Write(context);

        context.Pad(4);
    }

    private static void DoFormatCheck(DeserializationContext context) {
        if (context.VersionInfo.IsAtLeast(GM_2_3)) {
            var begin = context.Position;

            if (!context.VersionInfo.IsAtLeast(GM_2022_3)) {
                // Check for 2022.3+
                var textureCount = context.ReadInt32();

                if (textureCount == 1) {
                    // If there isn't a zero after the first texture, then this
                    // is 2022.3 (the pointer was shifted back by four bytes,
                    // where alignment padding used to always be).
                    context.Position += 16;
                    if (context.ReadInt32() != 0)
                        context.VersionInfo.UpdateTo(GM_2022_3);
                }
                else if (textureCount >= 2) {
                    // If the difference between the first two pointers is
                    // sixteen, then this is 2022.3.
                    var firstPointer = context.ReadInt32();
                    var secondPointer = context.ReadInt32();
                    if (secondPointer - firstPointer == 16)
                        context.VersionInfo.UpdateTo(GM_2022_3);
                }
            }

            // Check for 2022.5+ by looking for discrepancies in the Bz2 format.
            if (context.VersionInfo.IsAtLeast(GM_2022_3) && !context.VersionInfo.IsAtLeast(GM_2022_5)) {
                context.Position = begin;
                var textureCount = context.ReadInt32();

                for (var i = 0; i < textureCount; i++) {
                    // Go to each texture, and then to each texture's data.
                    context.Position = begin + 4 + (i * 4);
                    // Go to texture at an offset.
                    context.Position = context.ReadInt32() + 12;
                    // Go to texture data.
                    context.Position = context.ReadInt32();
                    var header = context.ReadBytes(4).ToArray();

                    if (header.SequenceEqual(GameMakerTextureData.QOI_BZIP2_HEADER)) {
                        context.Position += 4; // Skip width and height.

                        // Now check actual Bz2 headers.
                        if (context.ReadByte() != 'B' || context.ReadByte() != 'Z' || context.ReadByte() != 'h') {
                            context.VersionInfo.UpdateTo(GM_2022_5);
                            break;
                        }

                        context.ReadByte();

                        // Block header is the starting digits of pi.
                        if (context.ReadUInt24() != 0x594131)
                            context.VersionInfo.UpdateTo(GM_2022_5);
                        else if (context.ReadUInt24() != 0x595326)
                            context.VersionInfo.UpdateTo(GM_2022_5);

                        // All we needed to do is check a single QoI+BZ2
                        // texture.
                        break;
                    }
                }
            }

            context.Position = begin;
        }
    }
}
