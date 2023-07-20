using System.Reflection;
using GObject;

namespace Tomat.GameBreaker.Util;

public static class ResourceHelper {
    public const string RESOURCES_DIR = "resources";

    public static byte[] GetResourceAsBytes(Assembly assembly, string resource) {
        if (!resource.StartsWith($"{RESOURCES_DIR}."))
            resource = $"{RESOURCES_DIR}.{resource}";
        return assembly.ReadResourceAsByteArray(resource);
    }

    public static Gdk.Texture GetResourceAsTexture(Assembly assembly, string resource) {
        return GetResourceAsTexture(GetResourceAsBytes(assembly, resource));
    }

    public static Gdk.Texture GetResourceAsTexture(byte[] resource) {
        var buf = GdkPixbuf.PixbufLoader.FromBytes(resource);
        return Gdk.Texture.NewForPixbuf(buf);
    }
}
