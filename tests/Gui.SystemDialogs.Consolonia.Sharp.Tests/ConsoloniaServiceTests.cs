using Gui.SystemDialogs.Consolonia.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.Consolonia.Sharp.Tests;

public sealed class ConsoloniaFilePickerGuardTests
{
    private static IFilePickerService Create() => new ConsoloniaFilePickerService(() => null);

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class ConsoloniaFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new ConsoloniaFolderPickerService(() => null);

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class ConsoloniaNoTopLevelTests
{
    [Fact]
    public async Task OpenFile_NoTopLevel_ReturnsNull()
    {
        Assert.Null(await new ConsoloniaFilePickerService(() => null)
            .OpenFileAsync(new OpenFileDialogOptions(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task OpenFiles_NoTopLevel_ReturnsEmptyCollection()
    {
        Assert.Empty(await new ConsoloniaFilePickerService(() => null)
            .OpenFilesAsync(new OpenFilesDialogOptions(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SelectFolder_NoTopLevel_ReturnsNull()
    {
        Assert.Null(await new ConsoloniaFolderPickerService(() => null)
            .SelectFolderAsync(new SelectFolderDialogOptions(), TestContext.Current.CancellationToken));
    }
}

public sealed class ConsoloniaApplicationExitServiceTests
{
    [Fact]
    public void Exit_WithoutLifetime_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        new ConsoloniaApplicationExitService(terminator).Exit(9);
        Assert.Equal(9, terminator.ExitCode);
    }
}
