using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystemEmitter;

public sealed class GameMakerParticleSystemEmitter : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public bool Enabled { get; set; }

    public GameMakerParticleSystemEmitMode Mode { get; set; }

    public int EmitCount { get; set; }

    public GameMakerParticleSystemDistribution Distribution { get; set; }

    public GameMakerParticleSystemEmitterShape Shape { get; set; }

    public float RegionX { get; set; }

    public float RegionY { get; set; }

    public float RegionW { get; set; }

    public float RegionH { get; set; }

    public float Rotation { get; set; }

    public int SpriteId { get; set; }

    public GameMakerParticleSystemTexture Texture { get; set; }

    public bool SpriteAnimate { get; set; }

    public bool SpriteStretch { get; set; }

    public bool SpriteRandom { get; set; }

    public int StartColor { get; set; }

    public int MidColor { get; set; }

    public int EndColor { get; set; }

    public bool AdditiveBlend { get; set; }

    public float LifetimeMin { get; set; }

    public float LifetimeMax { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public float SizeMin { get; set; }

    public float SizeMax { get; set; }

    public float SizeIncrease { get; set; }

    public float SizeWiggle { get; set; }

    public float SpeedMin { get; set; }

    public float SpeedMax { get; set; }

    public float SpeedIncrease { get; set; }

    public float SpeedWiggle { get; set; }

    public float GravityForce { get; set; }

    public float GravityDirection { get; set; }

    public float DirectionMin { get; set; }

    public float DirectionMax { get; set; }

    public float DirectionWiggle { get; set; }

    public float OrientationMin { get; set; }

    public float OrientationMax { get; set; }

    public float OrientationIncrease { get; set; }

    public float OrientationWiggle { get; set; }

    public bool OrientationRelative { get; set; }

    public void Read(DeserializationContext context) {
        Enabled = context.ReadBoolean(wide: true);
        Mode = (GameMakerParticleSystemEmitMode)context.ReadUInt32();
        EmitCount = context.ReadInt32();
        Distribution = (GameMakerParticleSystemDistribution)context.ReadUInt32();
        Shape = (GameMakerParticleSystemEmitterShape)context.ReadUInt32();
        RegionX = context.ReadSingle();
        RegionY = context.ReadSingle();
        RegionW = context.ReadSingle();
        RegionH = context.ReadSingle();
        Rotation = context.ReadSingle();
        SpriteId = context.ReadInt32();
        Texture = (GameMakerParticleSystemTexture)context.ReadInt32();
        SpriteAnimate = context.ReadBoolean(wide: true);
        SpriteStretch = context.ReadBoolean(wide: true);
        SpriteRandom = context.ReadBoolean(wide: true);
        StartColor = context.ReadInt32();
        MidColor = context.ReadInt32();
        EndColor = context.ReadInt32();
        AdditiveBlend = context.ReadBoolean(wide: true);
        LifetimeMin = context.ReadSingle();
        LifetimeMax = context.ReadSingle();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();
        SizeMin = context.ReadSingle();
        SizeMax = context.ReadSingle();
        SizeIncrease = context.ReadSingle();
        SizeWiggle = context.ReadSingle();
        SpeedMin = context.ReadSingle();
        SpeedMax = context.ReadSingle();
        SpeedIncrease = context.ReadSingle();
        SpeedWiggle = context.ReadSingle();
        GravityForce = context.ReadSingle();
        GravityDirection = context.ReadSingle();
        DirectionMin = context.ReadSingle();
        DirectionMax = context.ReadSingle();
        DirectionWiggle = context.ReadSingle();
        OrientationMin = context.ReadSingle();
        OrientationMax = context.ReadSingle();
        OrientationIncrease = context.ReadSingle();
        OrientationWiggle = context.ReadSingle();
        OrientationRelative = context.ReadBoolean(wide: true);
    }

    public void Write(SerializationContext context) {
        context.Write(Enabled, wide: true);
        context.Write((uint)Mode);
        context.Write(EmitCount);
        context.Write((uint)Distribution);
        context.Write((uint)Shape);
        context.Write(RegionX);
        context.Write(RegionY);
        context.Write(RegionW);
        context.Write(RegionH);
        context.Write(Rotation);
        context.Write(SpriteId);
        context.Write((int)Texture);
        context.Write(SpriteAnimate, wide: true);
        context.Write(SpriteStretch, wide: true);
        context.Write(SpriteRandom, wide: true);
        context.Write(StartColor);
        context.Write(MidColor);
        context.Write(EndColor);
        context.Write(AdditiveBlend, wide: true);
        context.Write(LifetimeMin);
        context.Write(LifetimeMax);
        context.Write(ScaleX);
        context.Write(ScaleY);
        context.Write(SizeMin);
        context.Write(SizeMax);
        context.Write(SizeIncrease);
        context.Write(SizeWiggle);
        context.Write(SpeedMin);
        context.Write(SpeedMax);
        context.Write(SpeedIncrease);
        context.Write(SpeedWiggle);
        context.Write(GravityForce);
        context.Write(GravityDirection);
        context.Write(DirectionMin);
        context.Write(DirectionMax);
        context.Write(DirectionWiggle);
        context.Write(OrientationMin);
        context.Write(OrientationMax);
        context.Write(OrientationIncrease);
        context.Write(OrientationWiggle);
        context.Write(OrientationRelative, wide: true);
    }
}
