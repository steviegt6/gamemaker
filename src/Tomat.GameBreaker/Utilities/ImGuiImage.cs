using System;
using System.Buffers;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tomat.GameBreaker.Utilities;

public sealed unsafe class ImGuiImage : IDisposable {
    private readonly Image<Rgba32> image;
    private MemoryHandle handle;

    public int Width => image.Width;

    public int Height => image.Height;

    public void* Ptr => handle.Pointer;

    public int Length => image.Width * image.Height * sizeof(Rgba32);

    public ImGuiImage(Image<Rgba32> image) {
        this.image = image;

        if (!this.image.DangerousTryGetSinglePixelMemory(out var mem))
            throw new Exception("Failed to get single pixel memory for image");

        handle = mem.Pin();
    }

    public void Dispose() {
        handle.Dispose();
        image.Dispose();
    }

    public RawImage AsRaw() {
        // I was hoping to figure out a way to make the memory just use the
        // pointer, but oh well. We could have also used the memory from
        // DangerousTryGetSinglePixelMemory, but that's a Memory<TPixel>, not a
        // Memory<byte>. We have an EVIL ToArray (Memmove) call, but I'll suck
        // it up.
        return new RawImage(Width, Height, new Memory<byte>(new Span<byte>(Ptr, Length).ToArray()));
    }
}
