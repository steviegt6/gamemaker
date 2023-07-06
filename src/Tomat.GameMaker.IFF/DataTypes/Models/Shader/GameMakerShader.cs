using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Shader;

public sealed class GameMakerShader : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerShaderType ShaderType { get; set; }

    public GameMakerPointer<GameMakerString> GlslEsVertex { get; set; }

    public GameMakerPointer<GameMakerString> GlslEsFragment { get; set; }

    public GameMakerPointer<GameMakerString> GlslVertex { get; set; }

    public GameMakerPointer<GameMakerString> GlslFragment { get; set; }

    public GameMakerPointer<GameMakerString> Hlsl9Vertex { get; set; }

    public GameMakerPointer<GameMakerString> Hlsl9Fragment { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> Hlsl11VertexBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> Hlsl11PixelBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> PsslVertexBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> PsslPixelBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> CgPsVitaVertexBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> CgPsVitaPixelBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> CgPs3VertexBuffer { get; set; }

    public GameMakerPointer<GameMakerShaderBuffer> CgPs3PixelBuffer { get; set; }

    public List<GameMakerPointer<GameMakerString>>? VertexAttributes { get; set; }

    public int Version { get; set; }

    public void Read(DeserializationContext context, int endPos) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        ShaderType = (GameMakerShaderType)(context.Reader.ReadUInt32() & 0x7FFFFFFF);

        GlslEsVertex = context.ReadPointerAndObject<GameMakerString>();
        GlslEsFragment = context.ReadPointerAndObject<GameMakerString>();
        GlslVertex = context.ReadPointerAndObject<GameMakerString>();
        GlslFragment = context.ReadPointerAndObject<GameMakerString>();
        Hlsl9Vertex = context.ReadPointerAndObject<GameMakerString>();
        Hlsl9Fragment = context.ReadPointerAndObject<GameMakerString>();

        var ptr1 = context.Reader.ReadInt32();
        Hlsl11VertexBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(ptr1);
        var ptr2 = context.Reader.ReadInt32();
        Hlsl11PixelBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(ptr2);

        var count = context.Reader.ReadInt32();
        VertexAttributes = new List<GameMakerPointer<GameMakerString>>(count);
        for (var i = count; i > 0; i--)
            VertexAttributes.Add(context.ReadPointerAndObject<GameMakerString>());

        Version = context.Reader.ReadInt32();

        var ptr3 = context.Reader.ReadInt32();
        PsslVertexBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(ptr3);
        ReadShaderData(context, PsslVertexBuffer, ptr3, context.Reader.ReadInt32());

        var currPtr = context.Reader.ReadInt32();
        PsslPixelBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, PsslPixelBuffer, currPtr, context.Reader.ReadInt32());

        currPtr = context.Reader.ReadInt32();
        CgPsVitaVertexBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, CgPsVitaVertexBuffer, currPtr, context.Reader.ReadInt32());

        currPtr = context.Reader.ReadInt32();
        CgPsVitaPixelBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, CgPsVitaPixelBuffer, currPtr, context.Reader.ReadInt32());

        if (Version >= 2) {
            currPtr = context.Reader.ReadInt32();
            CgPs3VertexBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(currPtr);
            ReadShaderData(context, CgPs3VertexBuffer, currPtr, context.Reader.ReadInt32());

            currPtr = context.Reader.ReadInt32();
            CgPs3PixelBuffer = context.Reader.ReadPointer<GameMakerShaderBuffer>(currPtr);
            ReadShaderData(context, CgPs3PixelBuffer, currPtr, context.Reader.ReadInt32());
        }

        ReadShaderData(context, Hlsl11VertexBuffer, ptr1, -1, ptr2 == 0 ? endPos : ptr2);
        ReadShaderData(context, Hlsl11PixelBuffer, ptr2, -1, ptr3 == 0 ? endPos : ptr3);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write((uint)ShaderType | 0x80000000u);

        context.Writer.Write(GlslEsVertex);
        context.Writer.Write(GlslEsFragment);
        context.Writer.Write(GlslVertex);
        context.Writer.Write(GlslFragment);
        context.Writer.Write(Hlsl9Vertex);
        context.Writer.Write(Hlsl9Fragment);

        context.Writer.Write(Hlsl11VertexBuffer);
        context.Writer.Write(Hlsl11PixelBuffer);

        context.Writer.Write(VertexAttributes!.Count);
        foreach (var attribute in VertexAttributes)
            context.Writer.Write(attribute);

        context.Writer.Write(Version);

        context.Writer.Write(PsslVertexBuffer);
        context.Writer.Write(PsslVertexBuffer.IsNull ? 0 : PsslVertexBuffer.ExpectObject().Buffer.Length);
        context.Writer.Write(PsslPixelBuffer);
        context.Writer.Write(PsslPixelBuffer.IsNull ? 0 : PsslPixelBuffer.ExpectObject().Buffer.Length);

        context.Writer.Write(CgPsVitaVertexBuffer);
        context.Writer.Write(CgPsVitaVertexBuffer.IsNull ? 0 : CgPsVitaVertexBuffer.ExpectObject().Buffer.Length);
        context.Writer.Write(CgPsVitaPixelBuffer);
        context.Writer.Write(CgPsVitaPixelBuffer.IsNull ? 0 : CgPsVitaPixelBuffer.ExpectObject().Buffer.Length);

        if (Version >= 2) {
            context.Writer.Write(CgPs3VertexBuffer);
            context.Writer.Write(CgPs3VertexBuffer.IsNull ? 0 : CgPs3VertexBuffer.ExpectObject().Buffer.Length);
            context.Writer.Write(CgPs3PixelBuffer);
            context.Writer.Write(CgPs3PixelBuffer.IsNull ? 0 : CgPs3PixelBuffer.ExpectObject().Buffer.Length);
        }

        if (!Hlsl11VertexBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(Hlsl11VertexBuffer);
        }

        if (!Hlsl11PixelBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(Hlsl11PixelBuffer);
        }

        if (!PsslVertexBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(PsslVertexBuffer);
        }

        if (!PsslPixelBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(PsslPixelBuffer);
        }

        if (!CgPsVitaVertexBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(CgPsVitaVertexBuffer);
        }

        if (!CgPsVitaPixelBuffer.IsNull) {
            context.Writer.Pad(8);
            context.MarkPointerAndWriteObject(CgPsVitaPixelBuffer);
        }

        if (Version >= 2) {
            if (!CgPs3VertexBuffer.IsNull) {
                context.Writer.Pad(16);
                context.MarkPointerAndWriteObject(CgPs3VertexBuffer);
            }

            if (!CgPs3PixelBuffer.IsNull) {
                context.Writer.Pad(8);
                context.MarkPointerAndWriteObject(CgPs3PixelBuffer);
            }
        }
    }

    void IGameMakerSerializable.Read(DeserializationContext context) {
        throw new InvalidOperationException("Attempted to read shader without a specified length.");
    }

    private static void ReadShaderData(DeserializationContext context, GameMakerPointer<GameMakerShaderBuffer> buffer, int pointer,  int length = -1, int end = -1) {
        if (buffer.IsNull)
            return;

        var returnTo = context.Reader.Position;
        context.Reader.Position = pointer;

        if (length == -1)
            buffer.ExpectObject().Read(context, end - pointer);
        else
            buffer.ExpectObject().Read(context, length);

        context.Reader.Position = returnTo;
    }
}
