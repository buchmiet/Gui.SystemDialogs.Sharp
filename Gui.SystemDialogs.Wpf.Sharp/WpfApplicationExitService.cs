using System.Windows;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp;

/// <summary>
/// WPF implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class WpfApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public WpfApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal WpfApplicationExitService(IProcessTerminator terminator)
    {
        _terminator = terminator ?? throw new ArgumentNullException(nameof(terminator));
    }

    public void Exit(int exitCode = 0)
    {
        var app = Application.Current;
        if (app is not null)
        {
            app.Shutdown(exitCode);
            return;
        }

        _terminator.Exit(exitCode);
    }
}
