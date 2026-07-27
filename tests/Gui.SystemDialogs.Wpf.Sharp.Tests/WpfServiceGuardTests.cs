using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;
using Gui.SystemDialogs.Wpf.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp.Tests;

public sealed class WpfFilePickerGuardTests
{
    private static IFilePickerService Create() => new WpfFilePickerService(new FakeWpfDialogBackend());

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class WpfFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new WpfFolderPickerService(new FakeWpfDialogBackend());

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class WpfApplicationExitServiceTests
{
    [Fact]
    public void Exit_WithoutApplication_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        var service = new WpfApplicationExitService(terminator);
        service.Exit(7);
        Assert.Equal(7, terminator.ExitCode);
        Assert.Equal(1, terminator.CallCount);
    }
}
