namespace Tomat.GameBreaker.API.Marshalling;

public interface IMarshaller<T> {
    T FromPtr(nint ptr);

    nint ToPtr(T source);
}
