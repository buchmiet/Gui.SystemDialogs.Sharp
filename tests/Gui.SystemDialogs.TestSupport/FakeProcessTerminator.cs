using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.TestSupport;

public sealed class FakeProcessTerminator : IProcessTerminator
{
    public int? ExitCode { get; private set; }

    public int CallCount { get; private set; }

    public void Exit(int exitCode)
    {
        CallCount++;
        ExitCode = exitCode;
    }
}
