using System;
using System.Collections.Generic;
using Tomat.GameBreaker.API.Marshalling;

namespace Tomat.GameBreaker.ManagedHost.Marshalling;

internal sealed class MarshallingService : IMarshallingService {
    private readonly Dictionary<Type, object> marshallers = new();

    public void RegisterMarshaller<T>(IMarshaller<T> marshaller) {
        marshallers.Add(typeof(T), marshaller);
    }

    public T FromPtr<T>(nint ptr) where T : unmanaged {
        return ((IMarshaller<T>)marshallers[typeof(T)]).FromPtr(ptr);
    }

    public nint ToPtr<T>(T source) where T : unmanaged {
        return ((IMarshaller<T>)marshallers[typeof(T)]).ToPtr(source);
    }
}
