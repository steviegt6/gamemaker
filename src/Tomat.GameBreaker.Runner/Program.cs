namespace Tomat.GameBreaker.Runner;

internal static class Program {
    internal static void Main(string[] args) {
        if (args.Length < 1)
            throw new ArgumentException("Expected the runner path to be specified.");

        if (!File.Exists(args[0]))
            throw new FileNotFoundException("The specified runner path does not exist.", args[0]);
    }
}
