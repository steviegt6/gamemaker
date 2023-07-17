namespace Tomat.GameBreaker.API.Project;

/// <summary>
///     The project type.
/// </summary>
public enum ProjectType {
    /// <summary>
    ///     This project deals with a GameMaker game compiled to bytecode.
    /// </summary>
    Vm,

    /// <summary>
    ///     The project deals with a GameMaker game compiled to native.
    /// </summary>
    Yyc,
}
