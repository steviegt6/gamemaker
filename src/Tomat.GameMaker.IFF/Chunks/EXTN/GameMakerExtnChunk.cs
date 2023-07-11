﻿using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Extension;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.EXTN;

public sealed class GameMakerExtnChunk : AbstractChunk {
    public const string NAME = "EXTN";

    public GameMakerPointerList<GameMakerExtension>? Extensions { get; set; }

    public GameMakerExtnChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Extensions = new GameMakerPointerList<GameMakerExtension>();
        Extensions.Read(context);

        CheckFormatAndUpdateVersion(context);

        if (!context.VersionInfo.IsAtLeast(GM_1_0_0_9999))
            return;

        foreach (var extension in Extensions)
            extension.ExpectObject().ProductId = context.ReadGuid();
    }

    public override void Write(SerializationContext context) {
        Extensions!.Write(context);

        foreach (var extension in Extensions!) {
            if (extension.Object?.ProductId is { } guid)
                context.Write(guid.ToByteArray());
        }
    }

    private void CheckFormatAndUpdateVersion(DeserializationContext context) {
        if (!context.VersionInfo.IsAtLeast(GM_2_3) || context.VersionInfo.IsAtLeast(GM_2022_6)) {
            return;
        }

        var is20226 = true;
        var oldPos = context.Position;

        var extensionCount = context.ReadInt32();

        if (extensionCount > 0) {
            var firstExtensionPointer = context.ReadInt32();
            var firstExtensionEndPointer = extensionCount >= 2 ? context.ReadInt32() : oldPos + Size;

            context.Position = firstExtensionPointer + 12;
            var newPointer1 = context.ReadInt32();
            var newPointer2 = context.ReadInt32();

            if (newPointer1 != context.Position)
                is20226 = false;
            else if (newPointer2 <= context.Position || newPointer2 >= oldPos + Size)
                is20226 = false;
            else {
                context.Position = newPointer2;
                var optionCount = context.ReadInt32();

                if (optionCount > 0) {
                    var newOffsetCheck = context.Position + (sizeof(int) * (optionCount - 1));

                    if (newOffsetCheck >= oldPos + Size) {
                        is20226 = false;
                    }
                    else {
                        context.Position += sizeof(int) * (optionCount - 1);
                        newOffsetCheck = context.ReadInt32();

                        if (newOffsetCheck < 0 || newOffsetCheck >= oldPos + Size)
                            is20226 = false;
                        else
                            context.Position = newOffsetCheck;
                    }
                }

                if (is20226) {
                    if (extensionCount == 1) {
                        context.Position += 16;
                        context.Pad(16);
                    }

                    if (context.Position != firstExtensionEndPointer)
                        is20226 = false;
                }
            }
        }
        else {
            is20226 = false;
        }

        context.Position = oldPos;

        if (is20226)
            context.VersionInfo.UpdateTo(GM_2022_6);
    }
}
