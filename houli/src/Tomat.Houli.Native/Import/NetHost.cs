using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static partial class NetHost {
    /*public unsafe struct GetHostfxrParameters {
        public nint Size;
        private nint assemblyPath;
        private nint dotnetRoot;

        public string? AssemblyPath {
            get => AnsiUniArchitectureDependentStringMarshaller.ConvertToManaged((byte*)assemblyPath);
            set => assemblyPath = (nint)AnsiUniArchitectureDependentStringMarshaller.ConvertToUnmanaged(value);
        }

        public string? DotnetRoot {
            get => AnsiUniArchitectureDependentStringMarshaller.ConvertToManaged((byte*)dotnetRoot);
            set => dotnetRoot = (nint)AnsiUniArchitectureDependentStringMarshaller.ConvertToUnmanaged(value);
        }

        public GetHostfxrParameters(nint size, string assemblyPath, string dotnetRoot) {
            Size = size;
            AssemblyPath = assemblyPath;
            DotnetRoot = dotnetRoot;
        }
    }*/

    private const StringMarshalling string_marshalling =
#if x64
        StringMarshalling.Utf16;
#elif x86
        StringMarshalling.Utf8;
#elif AnyCPU
        StringMarshalling.Utf8; // We want to let AnyCPU compile.
#else
#error "Unsupported architecture"
#endif

    public unsafe struct GetHostfxrParameters {
        public nint Size;
        public char* AssemblyPath;
        public char* DotnetRoot;

        public GetHostfxrParameters(nint size, char* assemblyPath, char* dotnetRoot) {
            Size = size;
            AssemblyPath = assemblyPath;
            DotnetRoot = dotnetRoot;
        }
    }

    [LibraryImport("nethost.dll", EntryPoint = "get_hostfxr_path", StringMarshalling = string_marshalling)]
    internal static unsafe partial int GetHostfxrPath(char* buffer, nint* bufferSize, GetHostfxrParameters* parameters);
}
