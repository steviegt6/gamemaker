namespace Tomat.GameMaker.IFF.DataTypes.Models.Script; 

public sealed class GameMakerScript : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int CodeId { get; set; }

    public bool Constructor { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        CodeId = context.ReadInt32();

        // New GMS 2.3 constructor scripts.
        if (CodeId >= -1)
            return;

        Constructor = true;
        CodeId = (int)((uint)CodeId & 2147483647u);
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        
        if (Constructor)
            context.Write((int)((uint)CodeId | 2147483648u));
        else
            context.Write(CodeId);
    }
}
