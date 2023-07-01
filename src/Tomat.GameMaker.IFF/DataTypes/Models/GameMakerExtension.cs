﻿using System;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models; 

public sealed class GameMakerExtension : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> EmptyString { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> ClassName { get; set; }

    public GameMakerPointerList<GameMakerExtensionFile>? Files => filesPointer?.Object ?? files ?? null;

    public GameMakerPointer<GameMakerPointerList<GameMakerExtensionOption>> Options { get; set; }

    public Guid? ProductId { get; set; }

    private GameMakerPointer<GameMakerPointerList<GameMakerExtensionFile>>? filesPointer = null;
    private GameMakerPointerList<GameMakerExtensionFile>? files = null;

    public void Read(DeserializationContext context) {
        EmptyString = context.ReadPointerAndObject<GameMakerString>();
        Name = context.ReadPointerAndObject<GameMakerString>();
        ClassName = context.ReadPointerAndObject<GameMakerString>();

        if (context.VersionInfo.Version >= GameMakerVersionInfo.GM_2022_6) {
            filesPointer = context.ReadPointerAndObject<GameMakerPointerList<GameMakerExtensionFile>>();
            Options = context.ReadPointerAndObject<GameMakerPointerList<GameMakerExtensionOption>>();
        }
        else {
            files = new GameMakerPointerList<GameMakerExtensionFile>();
            files.Read(context);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(EmptyString);
        context.Writer.Write(Name);
        context.Writer.Write(ClassName);

        if (context.VersionInfo.Version >= GameMakerVersionInfo.GM_2022_6) {
            context.Writer.Write(filesPointer!.Value);
            context.Writer.Write(Options);
            filesPointer.Value.WriteObject(context);
            filesPointer.Value.Object!.Write(context);
            Options.WriteObject(context);
            Options.Object!.Write(context);
        }
        else {
            Files!.Write(context);
        }
    }
}