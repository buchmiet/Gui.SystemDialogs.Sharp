using Gui.SystemDialogs.Maui.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.Maui.Sharp.Tests;

public sealed class MauiFilePickerGuardTests
{
    private static IFilePickerService Create() => new MauiFilePickerService(new FakeMauiPickerBackend());

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class MauiFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new MauiFolderPickerService(new FakeMauiPickerBackend());

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class MauiApplicationExitServiceTests
{
    [Fact]
    public void Exit_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        new MauiApplicationExitService(terminator).Exit(4);
        Assert.Equal(4, terminator.ExitCode);
    }
}
