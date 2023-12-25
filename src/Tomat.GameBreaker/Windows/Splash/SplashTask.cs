using System;
using System.Threading.Tasks;

namespace Tomat.GameBreaker.Windows.Splash;

public sealed class SplashTask {
    public float Weight { get; }

    public float Progress { get; set; }

    public string Name { get; }

    public Func<SplashTask, Task> Task { get; }

    public SplashTask(float weight, float progress, string name, Func<SplashTask, Task> task) {
        Weight = weight;
        Progress = progress;
        Name = name;
        Task = task;
    }
}
