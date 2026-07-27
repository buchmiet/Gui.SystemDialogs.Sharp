using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Avalonia.Sharp;

/// <summary>
/// Avalonia implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class AvaloniaApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public AvaloniaApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal AvaloniaApplicationExitService(IProcessTerminator terminator)
    {
        _terminator = terminator ?? throw new ArgumentNullException(nameof(terminator));
    }

    public void Exit(int exitCode = 0)
    {
        if (Application.Current?.ApplicationLifetime is IControlledApplicationLifetime controlled)
        {
            controlled.Shutdown(exitCode);
            return;
        }

        _terminator.Exit(exitCode);
    }
}
