using System;
using System.IO;
using Tomat.GameBreaker;
using Tomat.GameBreaker.Framework.Logging;

Log.Init(new Logger("Tomat.GameBreaker", LogWriter.FromMany(new ConsoleLogWriter(), new FileLogWriter("gamebreaker.log", false), new FileLogWriter(Path.Combine("logs", $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"), false))));

Log.Info($"Tomat.GameBreaker v{typeof(Program).Assembly.GetName().Version}");
Log.Debug("Starting application...");

using var app = new GameBreakerApplication();

Log.Debug("Entering main window loop...");
while (!app.ShouldExit())
    app.UpdateWindows();
