using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Extension;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.EXTN;

internal sealed class GameMakerExtnChunk : AbstractChunk,
                                           IExtnChunk {
    public const string NAME = "EXTN";

    public GameMakerPointerList<GameMakerExtension> Extensions { get; set; } = null!;

    public GameMakerExtnChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        CheckFormatAndUpdateVersion(context);
        CheckFormatAndUpdateVersion2(context);

        Extensions = context.ReadPointerList<GameMakerExtension>();

        if (!context.VersionInfo.IsAtLeast(GM_1_0_0_9999))
            return;

        foreach (var extension in Extensions)
            extension.ExpectObject().ProductId = context.ReadGuid();
    }

    public override void Write(SerializationContext context) {
        context.Write(Extensions);

        foreach (var extension in Extensions!) {
            if (extension.TryGetObject(out var obj) && obj.ProductId.HasValue)
                context.Write(obj.ProductId.Value.ToByteArray());
        }
    }

    private void CheckFormatAndUpdateVersion(DeserializationContext context) {
        if (!context.VersionInfo.IsAtLeast(GM_2_3) || context.VersionInfo.IsAtLeast(GM_2022_6))
            return;

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

    private void CheckFormatAndUpdateVersion2(DeserializationContext context)
    {
        if (!context.VersionInfo.IsAtLeast(GM_2022_6) || context.VersionInfo.IsAtLeast(GM_2023_4))
            return;

        var oldPos = context.Position;

        var extensionCount = context.ReadInt32();

        if (extensionCount > 0) {
            context.Position = (int)context.ReadUInt32();
            context.Position += sizeof(int) * 3;

            var filesPtr = context.ReadInt32();
            var optionsPtr = context.ReadInt32();

            if (filesPtr > optionsPtr)
                context.VersionInfo.UpdateTo(GM_2023_4);
        }

        context.Position = oldPos;
    }
}
