using System;
using System.IO;
using Tomat.GameBreaker;
using Tomat.GameBreaker.Logging;

var logger = new Logger("Tomat.GameBreaker", LogWriter.FromMany(new ConsoleLogWriter(), new FileLogWriter("gamebreaker.log", false), new FileLogWriter(Path.Combine("logs", $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"), false)));

using var app = new GameBreakerApplication();
