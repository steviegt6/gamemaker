using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Tomat.GameBreaker.Framework.Windowing;
using Tomat.GameBreaker.Utilities;

namespace Tomat.GameBreaker.Windows.Splash;

internal sealed class SplashWindow : GameBreakerWindow {
    public bool Completed { get; private set; }

    public event Action? OnCompleted;

    private readonly List<SplashTask> tasks = new();
    private readonly List<SplashTask> queuedTasks = new();

    private ImGuiImage splashImage = null!;
    private nint splashImageGl;
    private ImGuiViewportPtr? viewportPtr;

    private float TaskProgress => tasks.Sum(t => t.Progress * t.Weight) / tasks.Sum(t => t.Weight);

    public SplashWindow(Application app, ref WindowOptions options) : base(app, ref options) {
        options = options with {
            Size = new Vector2D<int>(640, 400),
            Title = "Splash",
            WindowBorder = WindowBorder.Hidden,
            IsVisible = false,
            TransparentFramebuffer = true,
        };
    }

    public override void Initialize(IWindow window) {
        base.Initialize(window);
        
        Window.Load += () => {
            Window.Center();
            Window.IsVisible = true;

            ImageExt.FromAssemblyResource("resources.splash_image.png", out splashImage);
            splashImageGl = splashImage.AsOpenGlImage(window);
        };
    }

    protected override void Render(double delta) {
        base.Render(delta);

        viewportPtr ??= ImGui.GetMainViewport();

        var drawList = ImGui.GetBackgroundDrawList(viewportPtr.Value);
        drawList.AddImage(splashImageGl, Vector2.Zero, splashImage.Size);

        drawList.AddText(new Vector2(15, 15), 0xFFFFFFFF, "Tomat.GameBreaker by Tomat");
        drawList.AddText(new Vector2(15, 30), 0xFFFFFFFF, $"v{typeof(Program).Assembly.GetName().Version}");
        drawList.AddText(new Vector2(15, 45), 0xFFFFFFFF, "https://github.com/steviegt6/gamemaker");

        var progressBgStart = new Vector2(99, 350);
        var progressBgSize = new Vector2(442, 30);

        var progressStart = progressBgStart + new Vector2(0, 20);
        var progressSize = new Vector2(progressBgSize.X * TaskProgress, 10);

        drawList.AddRectFilled(progressStart, progressStart + progressSize, 0xD0FFFFFF);

        var unfinished = tasks.Where(x => x.Progress < 1f).Select(x => x.Name).ToList();
        if (unfinished.Count > 0) {
            drawList.PushClipRect(progressBgStart, progressBgStart + progressBgSize, true);
            drawList.AddText(progressStart + new Vector2(5, -20), 0xFFFFFFFF, string.Join(" | ", unfinished));
            drawList.PopClipRect();
        }

        if (TaskProgress < 1f || Completed)
            return;

        OnCompleted?.Invoke();
        Completed = true;
    }

    public void StartTask(string taskName, float taskWeight, Func<SplashTask, Task> taskAction) {
        var task = new SplashTask(taskWeight, 0f, taskName, taskAction);
        tasks.Add(task);
        Task.Run(async () => await taskAction(task));
    }

    public void QueueTask(string taskName, float taskWeight, Func<SplashTask, Task> taskAction) {
        var task = new SplashTask(taskWeight, 0f, taskName, taskAction);
        queuedTasks.Add(task);
    }

    public void RunQueuedTasks() {
        foreach (var task in queuedTasks) {
            tasks.Add(task);
            Task.Run(async () => await task.Task(task));
        }

        queuedTasks.Clear();
    }
}
