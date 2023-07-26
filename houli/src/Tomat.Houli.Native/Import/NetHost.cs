using System.Runtime.InteropServices;

namespace Tomat.Houli.Native.Import;

internal static partial class Nethost {
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

    [StructLayout(LayoutKind.Sequential)]
    public struct GetHostfxrParameters {
        public readonly nint Size;
        public readonly nint AssemblyPath;
        public readonly nint DotnetRoot;

        public GetHostfxrParameters(nint size, nint assemblyPath, nint dotnetRoot) {
            Size = size;
            AssemblyPath = assemblyPath;
            DotnetRoot = dotnetRoot;
        }
    }

    [LibraryImport("nethost.dll", EntryPoint = "get_hostfxr_path", StringMarshalling = Shared.STRING_MARSHALLING)]
    internal static unsafe partial int GetHostfxrPath(char* buffer, nint* bufferSize, GetHostfxrParameters* parameters);
}
