namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Process-termination seam for <see cref="IApplicationExitService"/> adapters.
/// </summary>
internal interface IProcessTerminator
{
    void Exit(int exitCode);
}
