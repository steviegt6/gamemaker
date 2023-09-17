using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.SubFunctionInfo; 

// model SubFunctionInfo {
//     int32 compiledIndex;
//     string* undecorated;
//     int32 relativeOffset;
//     int32 numArgs;
//     array {
//         string* arg;
//     } args;
//     int32 numLocals;
//     array {
//         string* local;
//     } locals;
// }

public interface ISubFunctionInfo {
    int CompiledIndex { get; set; }

    GameMakerPointer<IString> Undecorated { get; set; }

    int RelativeOffset { get; set; }
    
    List<GameMakerPointer<IString>> 
}
