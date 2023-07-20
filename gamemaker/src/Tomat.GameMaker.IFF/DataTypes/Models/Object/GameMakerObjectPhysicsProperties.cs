using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object;

public record struct GameMakerObjectPhysicsProperties {
    public bool IsEnabled { get; set; }

    public bool Sensor { get; set; }

    public GameMakerObjectCollisionShape CollisionShape { get; set; }

    public float Density { get; set; }

    public float Restitution { get; set; }

    public int Group { get; set; }

    public float LinearDamping { get; set; }

    public float AngularDamping { get; set; }

    public List<GameMakerObjectPhysicsVertex> Vertices { get; set; }

    public float Friction { get; set; }

    public bool IsAwake { get; set; }

    public bool IsKinematic { get; set; }
}
