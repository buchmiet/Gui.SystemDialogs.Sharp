using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.Sharp.Tests;

public sealed class EnvironmentProcessTerminatorTests
{
    [Fact]
    public void FakeProcessTerminator_RecordsExitCode()
    {
        var terminator = new FakeProcessTerminator();
        terminator.Exit(42);
        Assert.Equal(42, terminator.ExitCode);
        Assert.Equal(1, terminator.CallCount);
    }
}
