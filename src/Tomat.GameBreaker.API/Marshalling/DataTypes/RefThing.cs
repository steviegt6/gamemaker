namespace Tomat.GameBreaker.API.Marshalling.DataTypes;

public unsafe interface IRefThing<T> where T : unmanaged {
    T* Thing { get; set; }

    int RefCount { get; set; }

    int Size { get; set; }
}
