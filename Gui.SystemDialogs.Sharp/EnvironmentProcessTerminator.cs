namespace Gui.SystemDialogs.Sharp;

internal sealed class EnvironmentProcessTerminator : IProcessTerminator
{
    public void Exit(int exitCode) => Environment.Exit(exitCode);
}
