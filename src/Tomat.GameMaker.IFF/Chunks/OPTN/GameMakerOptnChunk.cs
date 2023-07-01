using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.Chunks.OPTN;

public sealed class GameMakerOptnChunk : AbstractChunk {
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

    public GameMakerList<GameMakerConstant>? Constants { get; set; }

    public GameMakerOptnChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.VersionInfo.OptionBitflag = context.Reader.ReadInt32() == int.MinValue;
        context.Reader.Position -= sizeof(int);

        if (context.VersionInfo.OptionBitflag) {
            UnknownUInt64 = context.Reader.ReadUInt64();
            Options = (OptnOptionFlags)context.Reader.ReadUInt64();
            Scale = context.Reader.ReadInt32();
            WindowColor = context.Reader.ReadUInt32();
            ColorDepth = context.Reader.ReadUInt32();
            Resolution = context.Reader.ReadUInt32();
            Frequency = context.Reader.ReadUInt32();
            VertexSync = context.Reader.ReadUInt32();
            Priority = context.Reader.ReadUInt32();
            SplashBackImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            SplashFrontImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            SplashLoadImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            LoadAlpha = context.Reader.ReadUInt32();
        }
        else {
            Options = 0;
            ReadOptionFlag(context, OptnOptionFlags.Fullscreen);
            ReadOptionFlag(context, OptnOptionFlags.InterpolatePixels);
            ReadOptionFlag(context, OptnOptionFlags.UseNewAudio);
            ReadOptionFlag(context, OptnOptionFlags.NoBorder);
            ReadOptionFlag(context, OptnOptionFlags.ShowCursor);
            Scale = context.Reader.ReadInt32();
            ReadOptionFlag(context, OptnOptionFlags.Sizeable);
            ReadOptionFlag(context, OptnOptionFlags.StayOnTop);
            WindowColor = context.Reader.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ChangeResolution);
            ColorDepth = context.Reader.ReadUInt32();
            Resolution = context.Reader.ReadUInt32();
            Frequency = context.Reader.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.NoButtons);
            VertexSync = context.Reader.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ScreenKey);
            ReadOptionFlag(context, OptnOptionFlags.HelpKey);
            ReadOptionFlag(context, OptnOptionFlags.QuitKey);
            ReadOptionFlag(context, OptnOptionFlags.SaveKey);
            ReadOptionFlag(context, OptnOptionFlags.ScreenShotKey);
            ReadOptionFlag(context, OptnOptionFlags.CloseSec);
            Priority = context.Reader.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.Freeze);
            ReadOptionFlag(context, OptnOptionFlags.ShowProgress);
            SplashBackImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            SplashFrontImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            SplashLoadImage = context.ReadPointerAndObject<GameMakerTextureItem>(context.Reader.ReadInt32(), returnAfter: true);
            ReadOptionFlag(context, OptnOptionFlags.LoadTransparent);
            LoadAlpha = context.Reader.ReadUInt32();
            ReadOptionFlag(context, OptnOptionFlags.ScaleProgress);
            ReadOptionFlag(context, OptnOptionFlags.DisplayErrors);
            ReadOptionFlag(context, OptnOptionFlags.WriteErrors);
            ReadOptionFlag(context, OptnOptionFlags.AbortErrors);
            ReadOptionFlag(context, OptnOptionFlags.VariableErrors);
            ReadOptionFlag(context, OptnOptionFlags.CreationEventOrder);
        }

        Constants = new GameMakerList<GameMakerConstant>();
        Constants.Read(context);
    }

    public override void Write(SerializationContext context) {
        if (context.VersionInfo.OptionBitflag) {
            context.Writer.Write(UnknownUInt64);
            context.Writer.Write((ulong)Options);
            context.Writer.Write(Scale);
            context.Writer.Write(WindowColor);
            context.Writer.Write(ColorDepth);
            context.Writer.Write(Resolution);
            context.Writer.Write(Frequency);
            context.Writer.Write(VertexSync);
            context.Writer.Write(Priority);
            context.Writer.Write(SplashBackImage);
            context.Writer.Write(SplashFrontImage);
            context.Writer.Write(SplashLoadImage);
            context.Writer.Write(LoadAlpha);
        }
        else {
            WriteOptionFlag(context, OptnOptionFlags.Fullscreen);
            WriteOptionFlag(context, OptnOptionFlags.InterpolatePixels);
            WriteOptionFlag(context, OptnOptionFlags.UseNewAudio);
            WriteOptionFlag(context, OptnOptionFlags.NoBorder);
            WriteOptionFlag(context, OptnOptionFlags.ShowCursor);
            context.Writer.Write(Scale);
            WriteOptionFlag(context, OptnOptionFlags.Sizeable);
            WriteOptionFlag(context, OptnOptionFlags.StayOnTop);
            context.Writer.Write(WindowColor);
            WriteOptionFlag(context, OptnOptionFlags.ChangeResolution);
            context.Writer.Write(ColorDepth);
            context.Writer.Write(Resolution);
            context.Writer.Write(Frequency);
            WriteOptionFlag(context, OptnOptionFlags.NoButtons);
            context.Writer.Write(VertexSync);
            WriteOptionFlag(context, OptnOptionFlags.ScreenKey);
            WriteOptionFlag(context, OptnOptionFlags.HelpKey);
            WriteOptionFlag(context, OptnOptionFlags.QuitKey);
            WriteOptionFlag(context, OptnOptionFlags.SaveKey);
            WriteOptionFlag(context, OptnOptionFlags.ScreenShotKey);
            WriteOptionFlag(context, OptnOptionFlags.CloseSec);
            context.Writer.Write(Priority);
            WriteOptionFlag(context, OptnOptionFlags.Freeze);
            WriteOptionFlag(context, OptnOptionFlags.ShowProgress);
            context.Writer.Write(SplashBackImage);
            context.Writer.Write(SplashFrontImage);
            context.Writer.Write(SplashLoadImage);
            WriteOptionFlag(context, OptnOptionFlags.LoadTransparent);
            context.Writer.Write(LoadAlpha);
            WriteOptionFlag(context, OptnOptionFlags.ScaleProgress);
            WriteOptionFlag(context, OptnOptionFlags.DisplayErrors);
            WriteOptionFlag(context, OptnOptionFlags.WriteErrors);
            WriteOptionFlag(context, OptnOptionFlags.AbortErrors);
            WriteOptionFlag(context, OptnOptionFlags.VariableErrors);
            WriteOptionFlag(context, OptnOptionFlags.CreationEventOrder);
        }

        Constants!.Write(context);
    }

    private void ReadOptionFlag(DeserializationContext context, OptnOptionFlags flag) {
        if (context.Reader.ReadBoolean(wide: true))
            Options |= flag;
    }

    private void WriteOptionFlag(SerializationContext context, OptnOptionFlags flag) {
        context.Writer.Write((Options & flag) == flag, wide: true);
    }
}
