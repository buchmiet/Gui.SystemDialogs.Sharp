using Gui.SystemDialogs.ProGpu.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.ProGpu.Sharp.Tests;

public sealed class ProGpuFilePickerGuardTests
{
    private static IFilePickerService Create() => new ProGpuFilePickerService();

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class ProGpuFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new ProGpuFolderPickerService();

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class ProGpuApplicationExitServiceTests
{
    [Fact]
    public void Exit_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        new ProGpuApplicationExitService(terminator).Exit(11);
        Assert.Equal(11, terminator.ExitCode);
    }
}
