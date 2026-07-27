using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp;

/// <summary>
/// WinUI implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class WinUiApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public WinUiApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal WinUiApplicationExitService(IProcessTerminator terminator)
    {
        _terminator = terminator ?? throw new ArgumentNullException(nameof(terminator));
    }

    public void Exit(int exitCode = 0)
    {
        try
        {
            Microsoft.UI.Xaml.Application.Current?.Exit();
        }
        catch
        {
            // Fall through to process terminator.
        }

        _terminator.Exit(exitCode);
    }
}
