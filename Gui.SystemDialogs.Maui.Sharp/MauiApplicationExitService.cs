using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Maui.Sharp;

/// <summary>
/// MAUI implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class MauiApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public MauiApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal MauiApplicationExitService(IProcessTerminator terminator)
    {
        _terminator = terminator ?? throw new ArgumentNullException(nameof(terminator));
    }

    public void Exit(int exitCode = 0) => _terminator.Exit(exitCode);
}
