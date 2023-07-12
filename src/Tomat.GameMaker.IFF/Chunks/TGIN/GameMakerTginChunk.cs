using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.TextureGroupInfo;

namespace Tomat.GameMaker.IFF.Chunks.TGIN;

public sealed class GameMakerTginChunk : AbstractChunk {
    public const string NAME = "TGIN";

    public GameMakerPointerList<GameMakerTextureGroupInfo> TextureGroupInfos { get; set; } = null!;

    public GameMakerTginChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var chunkVersion = context.Reader.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}!");

        DoFormatCheck(context);

        TextureGroupInfos = context.ReadPointerList<GameMakerTextureGroupInfo>();
    }

    public override void Write(SerializationContext context) {
        context.Writer.Write(1);
        context.Write(TextureGroupInfos);
    }

    private void DoFormatCheck(DeserializationContext context) {
        var start = context.Position - sizeof(int);

        if (context.VersionInfo.IsAtLeast(GM_2_3) && !context.VersionInfo.IsAtLeast(GM_2022_9)) {
            var returnTo = context.Position;

            var count = context.ReadInt32();

            if (count > 0) {
                var tginPointer = context.ReadInt32();
                var secondTginPointer = (count >= 2) ? context.ReadInt32() : start + Size; // EndOffset
                context.Position = tginPointer + 4;

                // Check to see if the pointer located at this address points
                // within this object. If not, the we know we're using a new
                // format!
                var pointer = context.ReadInt32();
                if (pointer < tginPointer || pointer >= secondTginPointer)
                    context.VersionInfo.UpdateTo(GM_2022_9);
            }

            context.Position = returnTo;
        }

        if (context.VersionInfo.IsAtLeast(GM_2022_9) && !context.VersionInfo.IsAtLeast(GM_2023_1)) {
            var returnTo = context.Position;
            context.Position += sizeof(int); // count

            var firstPointer = context.ReadUInt32();

            // Navigate to the fourth list pointer, which is different depending
            // on whether this is 2023.1+ or not (either "FontIDs" or
            // "SpineSpriteIDs").
            context.Position = (int)(firstPointer + 16 + (4 * 3));
            var fourthPointer = context.ReadUInt32();

            // We read either the "TexturePageIDs" count or the pointer to the
            // fifth list pointer. If it's a count, it will be less than the
            // previous pointer. Similarly, we can rely on the next pointer
            // being greater than the fourth pointer. This lets us safely assume
            // that this is a 2023.1+ file.
            if (context.ReadUInt32() <= fourthPointer)
                context.VersionInfo.UpdateTo(GM_2023_1);

            context.Position = returnTo;
        }
    }
}
