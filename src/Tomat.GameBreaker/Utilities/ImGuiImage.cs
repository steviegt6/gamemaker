using System;
using System.Buffers;
using System.Numerics;
using Silk.NET.Core;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tomat.GameBreaker.Utilities;

public sealed unsafe class ImGuiImage : IDisposable {
    private readonly Image<Rgba32> image;
    private MemoryHandle handle;

    public int Width => image.Width;

    public int Height => image.Height;

    public void* Ptr => handle.Pointer;

    public nint Native => (nint)Ptr;

    public Vector2 Size => new(Width, Height);

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

    public nint AsOpenGlImage(IWindow window) {
        var gl = GL.GetApi(window.GLContext);

        gl.GenTextures(1, out uint texture);
        gl.BindTexture(TextureTarget.Texture2D, texture);

        gl.TextureParameterI(texture, TextureParameterName.TextureMinFilter, new[] { (int)GLEnum.Linear });
        gl.TextureParameterI(texture, TextureParameterName.TextureMagFilter, new[] { (int)GLEnum.Linear });
        gl.TextureParameterI(texture, TextureParameterName.TextureWrapS, new[] { (int)GLEnum.ClampToEdge });
        gl.TextureParameterI(texture, TextureParameterName.TextureWrapT, new[] { (int)GLEnum.ClampToEdge });

        gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba, (uint)Width, (uint)Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Ptr);
        return (nint)texture;
    }
}
