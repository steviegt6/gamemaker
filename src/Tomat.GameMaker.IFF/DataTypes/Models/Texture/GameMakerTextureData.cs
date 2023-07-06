using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.BZip2;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Texture;

public sealed class GameMakerTextureData : IGameMakerSerializable {
    public Memory<byte> Data { get; set; }

    public bool IsQoi { get; set; }

    public bool IsBZip2 { get; set; }

    public short QoiWidth { get; set; }

    public short QoiHeight { get; set; }

    public uint QoiLength { get; set; }

    public static readonly byte[] PNG_HEADER = { 137, 80, 78, 71, 13, 10, 26, 10 };
    public static readonly byte[] QOI_HEADER = { 102, 105, 111, 113 }; // qoif
    public static readonly byte[] QOI_BZIP2_HEADER = { 50, 122, 111, 113 }; // qoz2

    public int LengthOffset { get; set; }

    public void Read(DeserializationContext context) {
        var begin = context.Reader.Position;
        var header = context.Reader.ReadBytes(8).ToArray();

        if (!header.SequenceEqual(PNG_HEADER)) {
            context.Reader.Position = begin;

            if (header.Take(4).SequenceEqual(QOI_BZIP2_HEADER)) {
                IsQoi = true;
                IsBZip2 = true;
                context.Reader.Position += 4; // header size
                QoiWidth = context.Reader.ReadInt16();
                QoiHeight = context.Reader.ReadInt16();

                if (context.VersionInfo.IsAtLeast(GM_2022_5))
                    QoiLength = context.Reader.ReadUInt32();

                // TODO: Queue data to be decompressed instead of doing it here.
                Data = Decompress(context);

                context.VersionInfo.UpdateTo(GM_2022_1);
            }
            else if (header.Take(4).SequenceEqual(QOI_HEADER)) {
                IsQoi = true;

                var dataBegin = context.Reader.Position;
                context.Reader.Position += 4 + sizeof(short) + sizeof(short); // header size + width + height
                var length = context.Reader.ReadInt32();
                context.Reader.Position = dataBegin;
                Data = context.Reader.ReadBytes(length + 12).ToArray();

                context.VersionInfo.UpdateTo(GM_2022_1);
            }
            else
                throw new InvalidDataException("Invalid texture data header.");

            return;
        }

        int type;

        do {
            var length = (uint)context.Reader.ReadByte() << 24 | (uint)context.Reader.ReadByte() << 16 | (uint)context.Reader.ReadByte() << 8 | (ulong)context.Reader.ReadByte();
            type = context.Reader.ReadInt32();
            context.Reader.Position += (int)length + 4;
        }
        while (type != 0x444E4549); // IEND

        var textureLength = context.Reader.Position - begin;
        context.Reader.Position = begin;
        Data = context.Reader.ReadBytes(textureLength).ToArray();
    }

    public void Write(SerializationContext context) {
        context.Writer.Pad(128);
        context.Writer.Pointers[this] = context.Writer.Position;

        if (IsQoi && IsBZip2) {
            context.Writer.Write(QOI_BZIP2_HEADER);
            context.Writer.Write(QoiWidth);
            context.Writer.Write(QoiHeight);
            if (context.VersionInfo.IsAtLeast(GM_2022_5))
                context.Writer.Write(QoiLength);
            using var input = new MemoryStream(Data.ToArray());
            using var output = new MemoryStream(1024);
            BZip2.Compress(input, output, false, 9);
            var final = output.ToArray();
            context.Writer.Write(final);
            WriteLength(context, Data.Length + 8);
        }
        else {
            context.Writer.Write(Data);
            WriteLength(context, Data.Length);
        }
    }

    private void WriteLength(SerializationContext context, int length) {
        if (!context.VersionInfo.IsAtLeast(GM_2022_3))
            return;

        var begin = context.Writer.Position;
        context.Writer.Position = LengthOffset;
        context.Writer.Write(length);
        context.Writer.Position = begin;
    }

    private static byte[] Decompress(DeserializationContext context) {
        using var ms = new MemoryStream(context.Reader.Data);
        ms.Seek(context.Reader.Position, SeekOrigin.Begin);
        var decompressed = new MemoryStream(1024);
        BZip2.Decompress(ms, decompressed, false);
        return decompressed.ToArray();
    }
}
