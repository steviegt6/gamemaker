namespace Tomat.GameBreaker.API.Structures; 

public unsafe struct RToken {
    private int kind;
    private uint type;
    private int index;
    private int index2;
    private RValue value;
    private int itemNumber;
    private RToken* items;
    private int position;
}
