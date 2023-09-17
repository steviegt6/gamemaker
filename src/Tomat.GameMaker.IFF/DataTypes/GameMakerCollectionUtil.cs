using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes;

public static class GameMakerCollectionUtil<TInterface, TImplementation> where TInterface : IGameMakerSerializable
                                                                         where TImplementation : TInterface, new() {
    public delegate void Read(DeserializationContext ctx, int i, int count);

    public delegate TInterface ReadElement(DeserializationContext ctx, bool notLast);

    public delegate GameMakerPointer<TInterface> ReadElementPointer(DeserializationContext ctx, bool notLast);

    public static List<TInterface> ReadList(
        DeserializationContext ctx,
        int count,
        Read? beforeRead = null,
        Read? afterRead = null,
        ReadElement? readElement = null
    ) {
        readElement ??= (localCtx, _) => {
            var elem = new TImplementation();
            elem.Read(localCtx);
            return elem;
        };

        var list = new List<TInterface>(count);

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(ctx, i, count);
            list.Add(readElement(ctx, i != count - 1));
            afterRead?.Invoke(ctx, i, count);
        }

        return list;
    }

    public static List<GameMakerPointer<TInterface>> ReadPointerList(
        DeserializationContext ctx,
        int count,
        Read? beforeRead = null,
        Read? afterRead = null,
        ReadElementPointer? readElement = null
    ) {
        readElement ??= (localCtx, _) => localCtx.ReadPointerAndObject<TInterface, TImplementation>();

        var list = new List<GameMakerPointer<TInterface>>(count);

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(ctx, i, count);
            list.Add(readElement(ctx, i != count - 1));
            afterRead?.Invoke(ctx, i, count);
        }

        return list;
    }
}

public static class GameMakerCollectionUtil<TInterface> where TInterface : IGameMakerSerializable {
    public delegate void Write(SerializationContext ctx, int i, int c);

    public delegate void WriteElement(SerializationContext ctx, TInterface element);

    public delegate void WriteElementPointer(SerializationContext ctx, GameMakerPointer<TInterface> element);

    public static void WriteList(
        SerializationContext ctx,
        int count,
        List<TInterface> list,
        Write? beforeWrite = null,
        Write? afterWrite = null,
        WriteElement? writeElement = null
    ) {
        writeElement ??= (localCtx, element) => element.Write(localCtx);

        ctx.Write(count);

        for (var i = 0; i < count; i++) {
            beforeWrite?.Invoke(ctx, i, count);
            writeElement(ctx, list[i]);
            afterWrite?.Invoke(ctx, i, count);
        }
    }

    public static void WritePointerList(
        SerializationContext ctx,
        int count,
        List<GameMakerPointer<TInterface>> list,
        Write? beforeWrite = null,
        Write? afterWrite = null,
        WriteElementPointer? writeElement = null,
        WriteElementPointer? writePointer = null
    ) {
        writeElement ??= (localCtx, element) => localCtx.MarkPointerAndWriteObject(element);
        writePointer ??= (localCtx, element) => localCtx.Write(element);

        ctx.Write(count);

        foreach (var element in list)
            writePointer(ctx, element);

        for (var i = 0; i < count; i++) {
            beforeWrite?.Invoke(ctx, i, count);
            writeElement(ctx, list[i]);
            afterWrite?.Invoke(ctx, i, count);
        }
    }

    public static void WriteRemotePointerList(
        SerializationContext ctx,
        int count,
        List<GameMakerPointer<TInterface>> list,
        WriteElementPointer? writePointer = null
    ) {
        writePointer ??= (localCtx, element) => localCtx.Write(element);

        ctx.Write(count);

        foreach (var element in list)
            writePointer(ctx, element);
    }
}

public static class GameMakerCollectionUtil {
    public static List<TInterface> ReadList<TInterface, TImplementation>(
        this DeserializationContext ctx,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? beforeRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? afterRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.ReadElement? readElement = null
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        return ReadList(ctx, ctx.ReadInt32(), beforeRead, afterRead, readElement);
    }

    public static List<TInterface> ReadList<TInterface, TImplementation>(
        this DeserializationContext ctx,
        int count,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? beforeRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? afterRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.ReadElement? readElement = null
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        return GameMakerCollectionUtil<TInterface, TImplementation>.ReadList(ctx, count, beforeRead, afterRead, readElement);
    }

    public static void WriteList<TInterface>(
        this SerializationContext ctx,
        List<TInterface> list,
        GameMakerCollectionUtil<TInterface>.Write? beforeWrite = null,
        GameMakerCollectionUtil<TInterface>.Write? afterWrite = null,
        GameMakerCollectionUtil<TInterface>.WriteElement? writeElement = null
    ) where TInterface : IGameMakerSerializable {
        WriteList(ctx, list.Count, list, beforeWrite, afterWrite, writeElement);
    }

    public static void WriteList<TInterface>(
        this SerializationContext ctx,
        int count,
        List<TInterface> list,
        GameMakerCollectionUtil<TInterface>.Write? beforeWrite = null,
        GameMakerCollectionUtil<TInterface>.Write? afterWrite = null,
        GameMakerCollectionUtil<TInterface>.WriteElement? writeElement = null
    ) where TInterface : IGameMakerSerializable {
        GameMakerCollectionUtil<TInterface>.WriteList(ctx, count, list, beforeWrite, afterWrite, writeElement);
    }

    public static List<GameMakerPointer<TInterface>> ReadPointerList<TInterface, TImplementation>(
        this DeserializationContext ctx,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? beforeRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? afterRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.ReadElementPointer? readElement = null
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        return ReadPointerList(ctx, ctx.ReadInt32(), beforeRead, afterRead, readElement);
    }

    public static List<GameMakerPointer<TInterface>> ReadPointerList<TInterface, TImplementation>(
        this DeserializationContext ctx,
        int count,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? beforeRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.Read? afterRead = null,
        GameMakerCollectionUtil<TInterface, TImplementation>.ReadElementPointer? readElement = null
    ) where TInterface : IGameMakerSerializable
      where TImplementation : TInterface, new() {
        return GameMakerCollectionUtil<TInterface, TImplementation>.ReadPointerList(ctx, count, beforeRead, afterRead, readElement);
    }

    public static void WritePointerList<TInterface>(
        this SerializationContext ctx,
        List<GameMakerPointer<TInterface>> list,
        GameMakerCollectionUtil<TInterface>.Write? beforeWrite = null,
        GameMakerCollectionUtil<TInterface>.Write? afterWrite = null,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writeElement = null,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writePointer = null
    ) where TInterface : IGameMakerSerializable {
        WritePointerList(ctx, list.Count, list, beforeWrite, afterWrite, writeElement, writePointer);
    }

    public static void WritePointerList<TInterface>(
        this SerializationContext ctx,
        int count,
        List<GameMakerPointer<TInterface>> list,
        GameMakerCollectionUtil<TInterface>.Write? beforeWrite = null,
        GameMakerCollectionUtil<TInterface>.Write? afterWrite = null,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writeElement = null,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writePointer = null
    ) where TInterface : IGameMakerSerializable {
        GameMakerCollectionUtil<TInterface>.WritePointerList(ctx, count, list, beforeWrite, afterWrite, writeElement, writePointer);
    }

    public static void WriteRemotePointerList<TInterface>(
        this SerializationContext ctx,
        List<GameMakerPointer<TInterface>> list,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writePointer = null
    ) where TInterface : IGameMakerSerializable {
        WriteRemotePointerList(ctx, list.Count, list, writePointer);
    }

    public static void WriteRemotePointerList<TInterface>(
        this SerializationContext ctx,
        int count,
        List<GameMakerPointer<TInterface>> list,
        GameMakerCollectionUtil<TInterface>.WriteElementPointer? writePointer = null
    ) where TInterface : IGameMakerSerializable {
        GameMakerCollectionUtil<TInterface>.WriteRemotePointerList(ctx, count, list, writePointer);
    }
}
