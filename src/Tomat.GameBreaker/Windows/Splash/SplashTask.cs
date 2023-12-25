using System;

namespace Tomat.GameBreaker.Windows.Splash;

public sealed class SplashTask {
    public float Weight { get; }

    public float Progress { get; set; }

    public string Name { get; }

    public Action<SplashTask> Task { get; }

    public SplashTask(float weight, float progress, string name, Action<SplashTask> task) {
        Weight = weight;
        Progress = progress;
        Name = name;
        Task = task;
    }
}
