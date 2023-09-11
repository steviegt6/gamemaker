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
        ShaderType = (GameMakerShaderType)(context.ReadUInt32() & 0x7FFFFFFF);

        GlslEsVertex = context.ReadPointerAndObject<GameMakerString>();
        GlslEsFragment = context.ReadPointerAndObject<GameMakerString>();
        GlslVertex = context.ReadPointerAndObject<GameMakerString>();
        GlslFragment = context.ReadPointerAndObject<GameMakerString>();
        Hlsl9Vertex = context.ReadPointerAndObject<GameMakerString>();
        Hlsl9Fragment = context.ReadPointerAndObject<GameMakerString>();

        var ptr1 = context.ReadInt32();
        Hlsl11VertexBuffer = context.ReadPointer<GameMakerShaderBuffer>(ptr1);
        var ptr2 = context.ReadInt32();
        Hlsl11PixelBuffer = context.ReadPointer<GameMakerShaderBuffer>(ptr2);

        var count = context.ReadInt32();
        VertexAttributes = new List<GameMakerPointer<GameMakerString>>(count);
        for (var i = count; i > 0; i--)
            VertexAttributes.Add(context.ReadPointerAndObject<GameMakerString>());

        Version = context.ReadInt32();

        var ptr3 = context.ReadInt32();
        PsslVertexBuffer = context.ReadPointer<GameMakerShaderBuffer>(ptr3);
        ReadShaderData(context, PsslVertexBuffer, ptr3, context.ReadInt32());

        var currPtr = context.ReadInt32();
        PsslPixelBuffer = context.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, PsslPixelBuffer, currPtr, context.ReadInt32());

        currPtr = context.ReadInt32();
        CgPsVitaVertexBuffer = context.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, CgPsVitaVertexBuffer, currPtr, context.ReadInt32());

        currPtr = context.ReadInt32();
        CgPsVitaPixelBuffer = context.ReadPointer<GameMakerShaderBuffer>(currPtr);
        ReadShaderData(context, CgPsVitaPixelBuffer, currPtr, context.ReadInt32());

        if (Version >= 2) {
            currPtr = context.ReadInt32();
            CgPs3VertexBuffer = context.ReadPointer<GameMakerShaderBuffer>(currPtr);
            ReadShaderData(context, CgPs3VertexBuffer, currPtr, context.ReadInt32());

            currPtr = context.ReadInt32();
            CgPs3PixelBuffer = context.ReadPointer<GameMakerShaderBuffer>(currPtr);
            ReadShaderData(context, CgPs3PixelBuffer, currPtr, context.ReadInt32());
        }

        ReadShaderData(context, Hlsl11VertexBuffer, ptr1, -1, ptr2 == 0 ? endPos : ptr2);
        ReadShaderData(context, Hlsl11PixelBuffer, ptr2, -1, ptr3 == 0 ? endPos : ptr3);
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((uint)ShaderType | 0x80000000u);

        context.Write(GlslEsVertex);
        context.Write(GlslEsFragment);
        context.Write(GlslVertex);
        context.Write(GlslFragment);
        context.Write(Hlsl9Vertex);
        context.Write(Hlsl9Fragment);

        context.Write(Hlsl11VertexBuffer);
        context.Write(Hlsl11PixelBuffer);

        context.Write(VertexAttributes!.Count);
        foreach (var attribute in VertexAttributes)
            context.Write(attribute);

        context.Write(Version);

        context.Write(PsslVertexBuffer);
        context.Write(PsslVertexBuffer.IsNull ? 0 : PsslVertexBuffer.ExpectObject().Buffer.Length);
        context.Write(PsslPixelBuffer);
        context.Write(PsslPixelBuffer.IsNull ? 0 : PsslPixelBuffer.ExpectObject().Buffer.Length);

        context.Write(CgPsVitaVertexBuffer);
        context.Write(CgPsVitaVertexBuffer.IsNull ? 0 : CgPsVitaVertexBuffer.ExpectObject().Buffer.Length);
        context.Write(CgPsVitaPixelBuffer);
        context.Write(CgPsVitaPixelBuffer.IsNull ? 0 : CgPsVitaPixelBuffer.ExpectObject().Buffer.Length);

        if (Version >= 2) {
            context.Write(CgPs3VertexBuffer);
            context.Write(CgPs3VertexBuffer.IsNull ? 0 : CgPs3VertexBuffer.ExpectObject().Buffer.Length);
            context.Write(CgPs3PixelBuffer);
            context.Write(CgPs3PixelBuffer.IsNull ? 0 : CgPs3PixelBuffer.ExpectObject().Buffer.Length);
        }

        if (!Hlsl11VertexBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(Hlsl11VertexBuffer);
        }

        if (!Hlsl11PixelBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(Hlsl11PixelBuffer);
        }

        if (!PsslVertexBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(PsslVertexBuffer);
        }

        if (!PsslPixelBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(PsslPixelBuffer);
        }

        if (!CgPsVitaVertexBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(CgPsVitaVertexBuffer);
        }

        if (!CgPsVitaPixelBuffer.IsNull) {
            context.Align(8);
            context.MarkPointerAndWriteObject(CgPsVitaPixelBuffer);
        }

        if (Version >= 2) {
            if (!CgPs3VertexBuffer.IsNull) {
                context.Align(16);
                context.MarkPointerAndWriteObject(CgPs3VertexBuffer);
            }

            if (!CgPs3PixelBuffer.IsNull) {
                context.Align(8);
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

        var returnTo = context.Position;
        context.Position = pointer;

        if (length == -1)
            buffer.ExpectObject().Read(context, end - pointer);
        else
            buffer.ExpectObject().Read(context, length);

        context.Position = returnTo;
    }
}
