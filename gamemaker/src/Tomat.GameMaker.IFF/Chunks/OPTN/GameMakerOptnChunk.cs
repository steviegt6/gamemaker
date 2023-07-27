using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Constant;
using Tomat.GameMaker.IFF.DataTypes.Models.Texture;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.OPTN;

internal sealed class GameMakerOptnChunk : AbstractChunk,
                                           IOptnChunk {
    public const string NAME = "OPTN";

    public ulong UnknownUInt64 { get; set; }

    public OptnOptionFlags Options { get; set; }

    public int Scale { get; set; }

    public uint WindowColor { get; set; }

    public uint ColorDepth { get; set; }

    public uint Resolution { get; set; }

    public uint Frequency { get; set; }

    public uint VertexSync { get; set; }

    public uint Priority { get; set; }

    public GameMakerPointer<GameMakerTextureItem> SplashBackImage { get; set; }

    public GameMakerPointer<GameMakerTextureItem> SplashFrontImage { get; set; }

    public GameMakerPointer<GameMakerTextureItem> SplashLoadImage { get; set; }

    public uint LoadAlpha { get; set; }

    public GameMakerList<GameMakerConstant> Constants { get; set; } = null!;

    public GameMakerOptnChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        context.VersionInfo.OptionBitflag = context.ReadInt32() == int.MinValue;
        context.Position -= sizeof(int);

        if (context.VersionInfo.OptionBitflag) {
            UnknownUInt64 = context.ReadUInt64();
            Options = (OptnOptionFlags)context.ReadUInt64();
            Scale = context.ReadInt32();
            WindowColor = context.ReadUInt32();
            ColorDepth = context.ReadUInt32();
            Resolution = context.ReadUInt32();
            Frequency = context.ReadUInt32();
            VertexSync = context.ReadUInt32();
            Priority = context.ReadUInt32();
            SplashBackImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            SplashFrontImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            SplashLoadImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            LoadAlpha = context.ReadUInt32();
        }
        else {
            Options = 0;
            ReadOptionFlag(context, OptnOptionFlags.Fullscreen);
            ReadOptionFlag(context, OptnOptionFlags.InterpolatePixels);
            ReadOptionFlag(context, OptnOptionFlags.UseNewAudio);
            ReadOptionFlag(context, OptnOptionFlags.NoBorder);
            ReadOptionFlag(context, OptnOptionFlags.ShowCursor);
            Scale = context.ReadInt32();
            ReadOptionFlag(context, OptnOptionFlags.Sizeable);
            ReadOptionFlag(context, OptnOptionFlags.StayOnTop);
            WindowColor = context.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ChangeResolution);
            ColorDepth = context.ReadUInt32();
            Resolution = context.ReadUInt32();
            Frequency = context.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.NoButtons);
            VertexSync = context.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ScreenKey);
            ReadOptionFlag(context, OptnOptionFlags.HelpKey);
            ReadOptionFlag(context, OptnOptionFlags.QuitKey);
            ReadOptionFlag(context, OptnOptionFlags.SaveKey);
            ReadOptionFlag(context, OptnOptionFlags.ScreenShotKey);
            ReadOptionFlag(context, OptnOptionFlags.CloseSec);
            Priority = context.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.Freeze);
            ReadOptionFlag(context, OptnOptionFlags.ShowProgress);
            SplashBackImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            SplashFrontImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            SplashLoadImage = context.ReadPointerAndObject<GameMakerTextureItem>();
            ReadOptionFlag(context, OptnOptionFlags.LoadTransparent);
            LoadAlpha = context.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ScaleProgress);
            ReadOptionFlag(context, OptnOptionFlags.DisplayErrors);
            ReadOptionFlag(context, OptnOptionFlags.WriteErrors);
            ReadOptionFlag(context, OptnOptionFlags.AbortErrors);
            ReadOptionFlag(context, OptnOptionFlags.VariableErrors);
            ReadOptionFlag(context, OptnOptionFlags.CreationEventOrder);
        }

        Constants = context.ReadList<GameMakerConstant>();
    }

    public override void Write(SerializationContext context) {
        if (context.VersionInfo.OptionBitflag) {
            context.Write(UnknownUInt64);
            context.Write((ulong)Options);
            context.Write(Scale);
            context.Write(WindowColor);
            context.Write(ColorDepth);
            context.Write(Resolution);
            context.Write(Frequency);
            context.Write(VertexSync);
            context.Write(Priority);
            context.Write(SplashBackImage);
            context.Write(SplashFrontImage);
            context.Write(SplashLoadImage);
            context.Write(LoadAlpha);
        }
        else {
            WriteOptionFlag(context, OptnOptionFlags.Fullscreen);
            WriteOptionFlag(context, OptnOptionFlags.InterpolatePixels);
            WriteOptionFlag(context, OptnOptionFlags.UseNewAudio);
            WriteOptionFlag(context, OptnOptionFlags.NoBorder);
            WriteOptionFlag(context, OptnOptionFlags.ShowCursor);
            context.Write(Scale);
            WriteOptionFlag(context, OptnOptionFlags.Sizeable);
            WriteOptionFlag(context, OptnOptionFlags.StayOnTop);
            context.Write(WindowColor);
            WriteOptionFlag(context, OptnOptionFlags.ChangeResolution);
            context.Write(ColorDepth);
            context.Write(Resolution);
            context.Write(Frequency);
            WriteOptionFlag(context, OptnOptionFlags.NoButtons);
            context.Write(VertexSync);
            WriteOptionFlag(context, OptnOptionFlags.ScreenKey);
            WriteOptionFlag(context, OptnOptionFlags.HelpKey);
            WriteOptionFlag(context, OptnOptionFlags.QuitKey);
            WriteOptionFlag(context, OptnOptionFlags.SaveKey);
            WriteOptionFlag(context, OptnOptionFlags.ScreenShotKey);
            WriteOptionFlag(context, OptnOptionFlags.CloseSec);
            context.Write(Priority);
            WriteOptionFlag(context, OptnOptionFlags.Freeze);
            WriteOptionFlag(context, OptnOptionFlags.ShowProgress);
            context.Write(SplashBackImage);
            context.Write(SplashFrontImage);
            context.Write(SplashLoadImage);
            WriteOptionFlag(context, OptnOptionFlags.LoadTransparent);
            context.Write(LoadAlpha);
            WriteOptionFlag(context, OptnOptionFlags.ScaleProgress);
            WriteOptionFlag(context, OptnOptionFlags.DisplayErrors);
            WriteOptionFlag(context, OptnOptionFlags.WriteErrors);
            WriteOptionFlag(context, OptnOptionFlags.AbortErrors);
            WriteOptionFlag(context, OptnOptionFlags.VariableErrors);
            WriteOptionFlag(context, OptnOptionFlags.CreationEventOrder);
        }

        context.Write(Constants);
    }

    private void ReadOptionFlag(IGameMakerIffReader reader, OptnOptionFlags flag) {
        if (reader.ReadBoolean(wide: true))
            Options |= flag;
    }

    private void WriteOptionFlag(IGameMakerIffWriter writer, OptnOptionFlags flag) {
        writer.Write((Options & flag) == flag, wide: true);
    }
}
