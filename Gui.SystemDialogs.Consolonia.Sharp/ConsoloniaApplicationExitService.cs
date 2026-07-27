using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Consolonia.Sharp;

/// <summary>
/// Consolonia implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class ConsoloniaApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public ConsoloniaApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal ConsoloniaApplicationExitService(IProcessTerminator terminator)
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
