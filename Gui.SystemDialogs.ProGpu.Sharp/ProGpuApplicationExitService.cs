using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.ProGpu.Sharp;

/// <summary>
/// ProGPU implementation of <see cref="IApplicationExitService"/>.
/// </summary>
public sealed class ProGpuApplicationExitService : IApplicationExitService
{
    private readonly IProcessTerminator _terminator;

    public ProGpuApplicationExitService()
        : this(new EnvironmentProcessTerminator())
    {
    }

    internal ProGpuApplicationExitService(IProcessTerminator terminator)
    {
        _terminator = terminator ?? throw new ArgumentNullException(nameof(terminator));
    }

    public void Exit(int exitCode = 0) => _terminator.Exit(exitCode);
}
