using System;

namespace Tomat.GameBreaker.Windowing;

public interface IWindow : IDisposable {
    string Title { get; set; }

    int Width { get; set; }

    int Height { get; set; }

    int X { get; set; }

    int Y { get; set; }
}
