using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object;

public sealed class GameMakerObject : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int SpriteId { get; set; }

    public bool Visible { get; set; }

    public bool Managed { get; set; }

    public bool Solid { get; set; }

    public int Depth { get; set; }

    public bool Persistent { get; set; }

    public int ParentObjectId { get; set; }

    public int MaskSpriteId { get; set; }

    public GameMakerObjectPhysicsProperties PhysicsProperties;

    public GameMakerPointerList<GameMakerPointerList<GameMakerObjectEvent>>? Events { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        SpriteId = context.Reader.ReadInt32();
        Visible = context.Reader.ReadBoolean(wide: true);
        if (context.VersionInfo.IsAtLeast(GM_2022_5))
            Managed = context.Reader.ReadBoolean(wide: true);
        Solid = context.Reader.ReadBoolean(wide: true);
        Depth = context.Reader.ReadInt32();
        Persistent = context.Reader.ReadBoolean(wide: true);
        ParentObjectId = context.Reader.ReadInt32();
        MaskSpriteId = context.Reader.ReadInt32();

        PhysicsProperties.IsEnabled = context.Reader.ReadBoolean(wide: true);
        PhysicsProperties.Sensor = context.Reader.ReadBoolean(wide: true);
        PhysicsProperties.CollisionShape = (GameMakerObjectCollisionShape)context.Reader.ReadInt32();
        PhysicsProperties.Density = context.Reader.ReadSingle();
        PhysicsProperties.Restitution = context.Reader.ReadSingle();
        PhysicsProperties.Group = context.Reader.ReadInt32();
        PhysicsProperties.LinearDamping = context.Reader.ReadSingle();
        PhysicsProperties.AngularDamping = context.Reader.ReadSingle();
        var vertexCount = context.Reader.ReadInt32();
        PhysicsProperties.Friction = context.Reader.ReadSingle();
        PhysicsProperties.IsAwake = context.Reader.ReadBoolean(wide: true);
        PhysicsProperties.IsKinematic = context.Reader.ReadBoolean(wide: true);
        PhysicsProperties.Vertices = new List<GameMakerObjectPhysicsVertex>(vertexCount);

        for (var i = vertexCount; i > 0; i--) {
            var vertex = new GameMakerObjectPhysicsVertex();
            vertex.Read(context);
            PhysicsProperties.Vertices.Add(vertex);
        }

        Events = new GameMakerPointerList<GameMakerPointerList<GameMakerObjectEvent>>();
        Events.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(SpriteId);
        context.Writer.Write(Visible, wide: true);
        if (context.VersionInfo.IsAtLeast(GM_2022_5))
            context.Writer.Write(Managed, wide: true);
        context.Writer.Write(Solid, wide: true);
        context.Writer.Write(Depth);
        context.Writer.Write(Persistent, wide: true);
        context.Writer.Write(ParentObjectId);
        context.Writer.Write(MaskSpriteId);
        context.Writer.Write(PhysicsProperties.IsEnabled, wide: true);
        context.Writer.Write(PhysicsProperties.Sensor, wide: true);
        context.Writer.Write((int)PhysicsProperties.CollisionShape);
        context.Writer.Write(PhysicsProperties.Density);
        context.Writer.Write(PhysicsProperties.Restitution);
        context.Writer.Write(PhysicsProperties.Group);
        context.Writer.Write(PhysicsProperties.LinearDamping);
        context.Writer.Write(PhysicsProperties.AngularDamping);
        context.Writer.Write(PhysicsProperties.Vertices.Count);
        context.Writer.Write(PhysicsProperties.Friction);
        context.Writer.Write(PhysicsProperties.IsAwake, wide: true);
        context.Writer.Write(PhysicsProperties.IsKinematic, wide: true);
        foreach (var vertex in PhysicsProperties.Vertices)
            vertex.Write(context);
        Events!.Write(context);
    }
}
