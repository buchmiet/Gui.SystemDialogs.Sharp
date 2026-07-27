using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;
using Gui.SystemDialogs.WinUI.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp.Tests;

public sealed class WinUiFilePickerGuardTests
{
    private static IFilePickerService Create() => new WinUiFilePickerService(new FakeWinUiPickerBackend());

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class WinUiFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new WinUiFolderPickerService(new FakeWinUiPickerBackend());

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class WinUiApplicationExitServiceTests
{
    [Fact]
    public void Exit_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        new WinUiApplicationExitService(terminator).Exit(5);
        Assert.Equal(5, terminator.ExitCode);
    }
}
