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

    public GameMakerPointerList<GameMakerPointerList<GameMakerObjectEvent>> Events { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        SpriteId = context.ReadInt32();
        Visible = context.ReadBoolean(wide: true);
        if (context.VersionInfo.IsAtLeast(GM_2022_5))
            Managed = context.ReadBoolean(wide: true);
        Solid = context.ReadBoolean(wide: true);
        Depth = context.ReadInt32();
        Persistent = context.ReadBoolean(wide: true);
        ParentObjectId = context.ReadInt32();
        MaskSpriteId = context.ReadInt32();

        PhysicsProperties.IsEnabled = context.ReadBoolean(wide: true);
        PhysicsProperties.Sensor = context.ReadBoolean(wide: true);
        PhysicsProperties.CollisionShape = (GameMakerObjectCollisionShape)context.ReadInt32();
        PhysicsProperties.Density = context.ReadSingle();
        PhysicsProperties.Restitution = context.ReadSingle();
        PhysicsProperties.Group = context.ReadInt32();
        PhysicsProperties.LinearDamping = context.ReadSingle();
        PhysicsProperties.AngularDamping = context.ReadSingle();
        var vertexCount = context.ReadInt32();
        PhysicsProperties.Friction = context.ReadSingle();
        PhysicsProperties.IsAwake = context.ReadBoolean(wide: true);
        PhysicsProperties.IsKinematic = context.ReadBoolean(wide: true);
        PhysicsProperties.Vertices = new List<GameMakerObjectPhysicsVertex>(vertexCount);

        for (var i = vertexCount; i > 0; i--) {
            var vertex = new GameMakerObjectPhysicsVertex();
            vertex.Read(context);
            PhysicsProperties.Vertices.Add(vertex);
        }

        Events = context.ReadPointerList<GameMakerPointerList<GameMakerObjectEvent>>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(SpriteId);
        context.Write(Visible, wide: true);
        if (context.VersionInfo.IsAtLeast(GM_2022_5))
            context.Write(Managed, wide: true);
        context.Write(Solid, wide: true);
        context.Write(Depth);
        context.Write(Persistent, wide: true);
        context.Write(ParentObjectId);
        context.Write(MaskSpriteId);
        context.Write(PhysicsProperties.IsEnabled, wide: true);
        context.Write(PhysicsProperties.Sensor, wide: true);
        context.Write((int)PhysicsProperties.CollisionShape);
        context.Write(PhysicsProperties.Density);
        context.Write(PhysicsProperties.Restitution);
        context.Write(PhysicsProperties.Group);
        context.Write(PhysicsProperties.LinearDamping);
        context.Write(PhysicsProperties.AngularDamping);
        context.Write(PhysicsProperties.Vertices.Count);
        context.Write(PhysicsProperties.Friction);
        context.Write(PhysicsProperties.IsAwake, wide: true);
        context.Write(PhysicsProperties.IsKinematic, wide: true);
        foreach (var vertex in PhysicsProperties.Vertices)
            vertex.Write(context);
        context.Write(Events);
    }
}
