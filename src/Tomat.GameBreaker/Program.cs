using System;
using System.IO;
using ImGuiNET;
using Tomat.GameBreaker;
using Tomat.GameBreaker.Logging;
using Tomat.GameBreaker.Windowing;

var writers = LogWriter.FromMany(new ConsoleLogWriter(), new FileLogWriter("gamebreaker.log", false), new FileLogWriter(Path.Combine("logs", $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"), false));
Log.Logger = new Logger("Tomat.GameBreaker", writers);


ImGuiNET.ImGui.CreateContext();


return 0;

var logger = Log.GetLogger(typeof(Program));

logger.Debug($"Tomat.GameBreaker v{typeof(Program).Assembly.GetName().Version}");
logger.Debug("Initializing...");

var windowProvider = createWindowProvider();
logger.Debug($"Using {nameof(IWindowProvider)}: {windowProvider.GetType().FullName}");

logger.Debug("Initializing window provider...");
if (!windowProvider.Initialize()) {
    logger.Fatal("Failed to initialize window provider!");
    return 1;
}

logger.Debug("Creating window...");
const SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_HIDDEN | SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
var window = SDL_CreateWindow("GameBreaker", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 1280, 720, flags);
if (window == nint.Zero) {
    logger.Fatal($"Failed to create window: {SDL_GetError()}");
    return 1;
}

logger.Debug("Creating renderer...");
var renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

return 0;

static IWindowProvider createWindowProvider() {
    return new Sdl2WindowProvider();
}
