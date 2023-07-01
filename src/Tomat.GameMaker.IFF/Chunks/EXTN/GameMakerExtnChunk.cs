using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
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

        if (context.VersionInfo.Version < GameMakerVersionInfo.GM_1_0_0_9999)
            return;

        foreach (var extension in Extensions)
            extension.Object!.ProductId = context.Reader.ReadGuid();
    }

    public override void Write(SerializationContext context) {
        Extensions!.Write(context);

        foreach (var extension in Extensions!) {
            if (extension.Object?.ProductId is { } guid)
                context.Writer.Write(guid.ToByteArray());
        }
    }

    private void CheckFormatAndUpdateVersion(DeserializationContext context) {
        if (context.VersionInfo.Version < GameMakerVersionInfo.GM_2_3 || context.VersionInfo.Version >= GameMakerVersionInfo.GM_2022_6) {
            return;
        }

        var is20226 = true;
        var oldPos = context.Reader.Position;

        var extensionCount = context.Reader.ReadInt32();

        if (extensionCount > 0) {
            var firstExtensionPointer = context.Reader.ReadInt32();
            var firstExtensionEndPointer = extensionCount >= 2 ? context.Reader.ReadInt32() : oldPos + Size;

            context.Reader.Position = firstExtensionPointer + 12;
            var newPointer1 = context.Reader.ReadInt32();
            var newPointer2 = context.Reader.ReadInt32();

            if (newPointer1 != context.Reader.Position)
                is20226 = false;
            else if (newPointer2 <= context.Reader.Position || newPointer2 >= oldPos + Size)
                is20226 = false;
            else {
                context.Reader.Position = newPointer2;
                var optionCount = context.Reader.ReadInt32();

                if (optionCount > 0) {
                    var newOffsetCheck = context.Reader.Position + (sizeof(int) * (optionCount - 1));

                    if (newOffsetCheck >= oldPos + Size) {
                        is20226 = false;
                    }
                    else {
                        context.Reader.Position += sizeof(int) * (optionCount - 1);
                        newOffsetCheck = context.Reader.ReadInt32();

                        if (newOffsetCheck < 0 || newOffsetCheck >= oldPos + Size)
                            is20226 = false;
                        else
                            context.Reader.Position = newOffsetCheck;
                    }
                }

                if (is20226) {
                    if (extensionCount == 1) {
                        context.Reader.Position += 16;
                        context.Reader.Pad(16);
                    }

                    if (context.Reader.Position != firstExtensionEndPointer)
                        is20226 = false;
                }
            }
        }
        else {
            is20226 = false;
        }

        context.Reader.Position = oldPos;

        if (is20226)
            context.VersionInfo.Update(GameMakerVersionInfo.GM_2022_6);
    }
}
