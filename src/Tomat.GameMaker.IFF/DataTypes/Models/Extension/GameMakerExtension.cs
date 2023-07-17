using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Extension;

public sealed class GameMakerExtension : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> EmptyString { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> ClassName { get; set; }

    public GameMakerPointerList<GameMakerExtensionFile>? Files {
        get {
            // filesPointer?.Object ?? files ?? null;
            if (filesPointer.HasValue && filesPointer.Value.TryGetObject(out var obj))
                return obj;

            return files;
        }
    }

    public GameMakerPointer<GameMakerPointerList<GameMakerExtensionOption>> Options { get; set; }

    public Guid? ProductId { get; set; }

    private GameMakerPointer<GameMakerPointerList<GameMakerExtensionFile>>? filesPointer;
    private GameMakerPointerList<GameMakerExtensionFile>? files;

    public void Read(DeserializationContext context) {
        EmptyString = context.ReadPointerAndObject<GameMakerString>();
        Name = context.ReadPointerAndObject<GameMakerString>();
        ClassName = context.ReadPointerAndObject<GameMakerString>();

        if (context.VersionInfo.IsAtLeast(GM_2022_6)) {
            filesPointer = context.ReadPointerAndObject<GameMakerPointerList<GameMakerExtensionFile>>();
            Options = context.ReadPointerAndObject<GameMakerPointerList<GameMakerExtensionOption>>();
        }
        else {
            files = context.ReadPointerList<GameMakerExtensionFile>();
        }
    }

    public void Write(SerializationContext context) {
        context.Write(EmptyString);
        context.Write(Name);
        context.Write(ClassName);

        if (context.VersionInfo.IsAtLeast(GM_2022_6)) {
            context.Write(filesPointer!.Value);
            context.Write(Options);
            context.MarkPointerAndWriteObject(filesPointer.Value);
            context.MarkPointerAndWriteObject(Options);
        }
        else {
            context.Write(files!);
        }
    }
}
