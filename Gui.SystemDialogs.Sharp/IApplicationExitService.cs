namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// GUI-framework-neutral application shutdown contract.
/// </summary>
public interface IApplicationExitService
{
    /// <summary>
    /// Requests process/application exit with the given exit code.
    /// </summary>
    void Exit(int exitCode = 0);
}
