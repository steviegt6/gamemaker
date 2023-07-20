using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.GEN8;

public sealed class GameMakerGen8Chunk : AbstractChunk {
    public const string NAME = "GEN8";

    public bool DisableDebug { get; set; }

    public byte FormatId { get; set; }

    public short UnknownInt16 { get; set; }

    public GameMakerPointer<GameMakerString> FileName { get; set; }

    public GameMakerPointer<GameMakerString> Config { get; set; }

    public int LastObjectId { get; set; }

    public int LastTileId { get; set; }

    public int GameId { get; set; }

    public Guid LegacyGuid { get; set; }

    public GameMakerPointer<GameMakerString> GameName { get; set; }

    public int MajorVersion { get; set; }

    public int MinorVersion { get; set; }

    public int ReleaseVersion { get; set; }

    public int BuildVersion { get; set; }

    public int DefaultWindowWidth { get; set; }

    public int DefaultWindowHeight { get; set; }

    public Gen8InfoFlags Info { get; set; }

    public int LicenseCrc32 { get; set; }

    public Memory<byte> LicenseMd5 { get; set; }

    public long Timestamp { get; set; }

    public GameMakerPointer<GameMakerString> DisplayName { get; set; }

    public long ActiveTargets { get; set; }

    public Gen8FunctionClassification FunctionClassifications { get; set; }

    public int SteamAppId { get; set; }

    public int DebuggerPort { get; set; }

    public List<int>? RoomOrder { get; set; }

    public List<long>? GameMaker2RandomUid { get; set; }

    public float GameMaker2Fps { get; set; }

    public bool GameMaker2AllowStatistics { get; set; }

    public Guid GameMaker2GameGuid { get; set; }

    public GameMakerGen8Chunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        DisableDebug = context.ReadBoolean(wide: false);
        context.VersionInfo.FormatId = FormatId = context.ReadByte();
        UnknownInt16 = context.ReadInt16();
        FileName = context.ReadPointerAndObject<GameMakerString>();
        Config = context.ReadPointerAndObject<GameMakerString>();
        LastObjectId = context.ReadInt32();
        LastTileId = context.ReadInt32();
        GameId = context.ReadInt32();
        LegacyGuid = context.ReadGuid();
        GameName = context.ReadPointerAndObject<GameMakerString>();
        MajorVersion = context.ReadInt32();
        MinorVersion = context.ReadInt32();
        ReleaseVersion = context.ReadInt32();
        BuildVersion = context.ReadInt32();
        context.VersionInfo.UpdateTo(new Version(MajorVersion, MinorVersion, ReleaseVersion, BuildVersion));
        DefaultWindowWidth = context.ReadInt32();
        DefaultWindowHeight = context.ReadInt32();
        Info = (Gen8InfoFlags)context.ReadInt32();
        LicenseCrc32 = context.ReadInt32();
        LicenseMd5 = context.ReadBytes(16);
        Timestamp = context.ReadInt64();
        DisplayName = context.ReadPointerAndObject<GameMakerString>();
        ActiveTargets = context.ReadInt64();
        FunctionClassifications = (Gen8FunctionClassification)context.ReadInt64();
        SteamAppId = context.ReadInt32();

        if (FormatId >= 14)
            DebuggerPort = context.ReadInt32();

        var roomCount = context.ReadInt32();
        RoomOrder = new List<int>(roomCount);
        for (var i = 0; i < roomCount; i++)
            RoomOrder.Add(context.ReadInt32());

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        GameMaker2RandomUid = ReadRandomUid(context);
        GameMaker2Fps = context.ReadSingle();
        GameMaker2AllowStatistics = context.ReadBoolean(wide: true);
        GameMaker2GameGuid = context.ReadGuid();
    }

    public override void Write(SerializationContext context) {
        context.Write(DisableDebug, wide: false);
        context.Write(context.VersionInfo.FormatId = FormatId);
        context.Write(UnknownInt16);
        context.Write(FileName);
        context.Write(Config);
        context.Write(LastObjectId);
        context.Write(LastTileId);
        context.Write(GameId);
        context.Write(LegacyGuid.ToByteArray());
        context.Write(GameName);
        context.Write(MajorVersion);
        context.Write(MinorVersion);
        context.Write(ReleaseVersion);
        context.Write(BuildVersion);
        context.VersionInfo.UpdateTo(new Version(MajorVersion, MinorVersion, ReleaseVersion, BuildVersion));
        context.Write(DefaultWindowWidth);
        context.Write(DefaultWindowHeight);
        context.Write((int)Info);
        context.Write(LicenseCrc32);
        context.Write(LicenseMd5);
        context.Write(Timestamp);
        context.Write(DisplayName);
        context.Write(ActiveTargets);
        context.Write((ulong)FunctionClassifications);
        context.Write(SteamAppId);

        if (FormatId >= 14)
            context.Write(DebuggerPort);

        if (!context.VersionInfo.IsAtLeast(GM_2))
            return;

        context.Write(RoomOrder!.Count);
        foreach (var room in RoomOrder)
            context.Write(room);

        WriteRandomUid(context);
        context.Write(GameMaker2Fps);
        context.Write(GameMaker2AllowStatistics, wide: true);
        context.Write(GameMaker2GameGuid.ToByteArray());
    }

    private List<long> ReadRandomUid(DeserializationContext context) {
        var list = new List<long>();

        var rand = new Random((int)(Timestamp & 4294967295L));
        var firstRandom = (long)rand.Next() << 32 | (long)rand.Next();
        if (context.ReadInt64() != firstRandom)
            throw new Exception("Unexpected random UID");

        var infoLocation = Math.Abs((int)(Timestamp & (long)ushort.MaxValue) / 7 + (GameId - DefaultWindowWidth) + RoomOrder!.Count) % 4;

        for (var i = 0; i < 4; i++) {
            if (i == infoLocation) {
                var curr = context.ReadInt64();
                list.Add(curr);

                if (curr == GetInfoNumber(firstRandom, false))
                    continue;

                if (curr != GetInfoNumber(firstRandom, true))
                    throw new Exception("Unexpected random UID");

                context.VersionInfo.WasRunFromIde = true;
            }
            else {
                var first = context.ReadInt32();
                if (first != rand.Next())
                    throw new Exception("Unexpected random UID");

                var second = context.ReadInt32();
                if (second != rand.Next())
                    throw new Exception("Unexpected random UID");

                // list.Add((long)(first << 32) | (long)second);
                list.Add(((long)first << 32) | (long)second);
            }
        }

        return list;
    }

    private void WriteRandomUid(SerializationContext context) {
        GameMaker2RandomUid!.Clear();

        var rand = new Random((int)(Timestamp & 4294967295L));
        var firstRand = (long)rand.Next() << 32 | (long)rand.Next();
        var infoNumber = GetInfoNumber(firstRand, context.VersionInfo.WasRunFromIde);
        var infoLocation = Math.Abs((int)(Timestamp & (long)ushort.MaxValue) / 7 + (GameId - DefaultWindowWidth) + RoomOrder!.Count) % 4;

        context.Write(firstRand);
        GameMaker2RandomUid.Add(infoNumber);

        for (var i = 0; i < 4; i++) {
            if (i == infoLocation) {
                context.Write(infoNumber);
                GameMaker2RandomUid.Add(infoNumber);
            }
            else {
                var first = rand.Next();
                var second = rand.Next();
                context.Write(first);
                context.Write(second);
                GameMaker2RandomUid.Add(((long)first << 32) | (long)second);
            }
        }
    }

    private long GetInfoNumber(long firstRandom, bool wasRunFromIde) {
        var infoNumber = Timestamp;
        if (!wasRunFromIde)
            infoNumber -= 1000;
        var temp = (ulong)infoNumber;
        temp = (temp << 56 & 0xFF00000000000000UL)
             | (temp >> 08 & 0xFF000000000000UL)
             | (temp << 32 & 0xFF0000000000UL)
             | (temp >> 16 & 0xFF00000000UL)
             | (temp << 08 & 0xFF000000UL)
             | (temp >> 24 & 0xFF0000UL)
             | (temp >> 16 & 0xFF00UL)
             | (temp >> 32 & 0xFFUL);
        infoNumber = (long)temp;
        infoNumber ^= firstRandom;
        infoNumber = ~infoNumber;
        infoNumber ^= (long)GameId << 32 | (long)GameId;
        infoNumber ^= (long)(DefaultWindowWidth + (int)Info) << 48
                    | (long)(DefaultWindowHeight + (int)Info) << 32
                    | (long)(DefaultWindowHeight + (int)Info) << 16
                    | (long)(DefaultWindowWidth + (int)Info);
        infoNumber ^= FormatId;
        return infoNumber;
    }
}
