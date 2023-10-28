namespace Tomat.GameBreaker.API.Marshalling;

public interface IMarshallingService {
    void RegisterMarshaller<T>(IMarshaller<T> marshaller);

    T FromPtr<T>(nint ptr) where T : unmanaged;

    nint ToPtr<T>(T source) where T : unmanaged;
}
