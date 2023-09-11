using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.String; 

// model String {
//     int32 length;
//     char[] data;
// }

public interface IString : IGameMakerSerializable {
    string Value { get; set; }
}
