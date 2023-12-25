using System.Reflection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tomat.GameBreaker.Utilities;

public static class ImageExt {
    public static void FromAssemblyResource(string path, out ImGuiImage image) {
        var assembly = Assembly.GetCallingAssembly();
        FromAssemblyResource(assembly, path, out image);
    }

    public static void FromAssemblyResource(Assembly assembly, string path, out ImGuiImage image) {
        var nameCandidates = new[] {
            path,
            assembly.GetName().Name! + "." + path,
        };

        foreach (var name in nameCandidates) {
            using var stream = assembly.GetManifestResourceStream(name);
            if (stream is null)
                continue;

            using var rgbImg = Image.Load<Rgba32>(stream);
            image = new ImGuiImage(rgbImg);
            return;
        }

        image = new ImGuiImage(new Image<Rgba32>(0, 0));
    }
}
