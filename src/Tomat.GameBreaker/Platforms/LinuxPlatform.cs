﻿using Tomat.GameBreaker.Windowing;

namespace Tomat.GameBreaker.Platforms; 

internal sealed class LinuxPlatform : IPlatform {
    public void MakeWindowTransparent(ImGuiWindow window) {
        throw new System.NotImplementedException();
    }
}
