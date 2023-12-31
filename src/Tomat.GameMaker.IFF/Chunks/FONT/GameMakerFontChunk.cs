﻿using System;
using Tomat.GameMaker.IFF.DataTypes.Models.Font;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FONT;

// TODO: Statically cache the generated padding.
internal sealed class GameMakerFontChunk : AbstractChunk,
                                           IFontChunk {
    public const string NAME = "FONT";

    private static Memory<byte>? cachedPadding;

    public GameMakerPointerList<GameMakerFont> Fonts { get; set; } = null!;

    public Memory<byte>? Padding { get; set; }

    public GameMakerFontChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        DoFormatCheck(context);

        Fonts = context.ReadPointerList<GameMakerFont>();
        Padding = context.ReadBytes(512);
    }

    public override void Write(SerializationContext context) {
        context.Write(Fonts);

        if (!Padding.HasValue) {
            if (cachedPadding.HasValue) {
                context.Write(cachedPadding.Value);
            }
            else {
                for (var i = 0; i < 0x80; i++)
                    context.Write((ushort)i);
                for (var i = 0; i < 0x80; i++)
                    context.Write((ushort)0x3f);

                // 512
                const int length = 0x80 * sizeof(ushort) * 2;
                cachedPadding = new Memory<byte>(context.Data, context.Position - length, length);
            }
        }
        else {
            context.Write(Padding.Value);
        }
    }

    private void DoFormatCheck(DeserializationContext context) {
        // Check for new "Ascender" field introduced in 2022.2, by attempting to
        // parse old font data format.
        if (context.VersionInfo.IsAtLeast(GM_2_3) && !context.VersionInfo.IsAtLeast(GM_2022_2)) {
            var returnTo = context.Position;

            var fontCount = context.ReadInt32();

            if (fontCount > 0) {
                var lowerBound = context.Position + (fontCount * sizeof(int));
                var upperBound = (returnTo + Size) - 512; // EndOffset - 512

                var firstFontPointer = context.ReadInt32();
                var endPointer = (fontCount >= 2 ? context.ReadInt32() : upperBound);

                context.Position = firstFontPointer + (11 * 4);

                var glyphCount = context.ReadInt32();
                var invalidFormat = false;

                if (glyphCount > 0) {
                    var glyphPointerOffset = context.Position;

                    if (glyphCount >= 2) {
                        // Check validity of first glyph.
                        var firstGlyph = context.ReadInt32() + (7 * 2);
                        var secondGlyph = context.ReadInt32();
                        if (firstGlyph < lowerBound || firstGlyph > upperBound)
                            invalidFormat = true;
                        if (secondGlyph < lowerBound || secondGlyph > upperBound)
                            invalidFormat = true;

                        if (!invalidFormat) {
                            // Check the length of the end of this glyph.
                            context.Position = firstGlyph;
                            var kerningLength = context.ReadUInt16() * 4;
                            context.Position += kerningLength;

                            if (context.Position != secondGlyph)
                                invalidFormat = true;
                        }
                    }

                    if (!invalidFormat) {
                        // Check the last glyph.
                        context.Position = glyphPointerOffset + ((glyphCount - 1) * 4);

                        var lastGlyph = context.ReadInt32() + (7 * 2);
                        if (lastGlyph < lowerBound || lastGlyph > upperBound)
                            invalidFormat = true;

                        if (!invalidFormat) {
                            // Check the length of the end of this glyph (done
                            // when checking `endPointer` below).
                            context.Position = lastGlyph;
                            var kerningLength = context.ReadUInt16() * 4;
                            context.Position += kerningLength;

                            // If we only have one font, align to the chunk
                            // boundary.
                            if (fontCount == 1)
                                context.Align(context.VersionInfo.ChunkAlignment);
                        }
                    }
                }

                // We didn't end up where we expected, so this is most likely
                // 2022.2+ font data.
                if (invalidFormat || context.Position != endPointer)
                    context.VersionInfo.UpdateTo(GM_2022_2);
            }

            context.Position = returnTo;
        }
    }
}
