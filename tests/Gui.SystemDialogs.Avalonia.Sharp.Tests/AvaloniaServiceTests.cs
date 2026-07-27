using Gui.SystemDialogs.Avalonia.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.Avalonia.Sharp.Tests;

public sealed class AvaloniaFilePickerGuardTests
{
    private static IFilePickerService Create() => new AvaloniaFilePickerService(() => null);

    [Fact] public Task OpenFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFilesAsync_NullOptions_Throws() => FilePickerGuardAssertions.OpenFilesAsync_NullOptions_Throws(Create());
    [Fact] public Task SaveFileAsync_NullOptions_Throws() => FilePickerGuardAssertions.SaveFileAsync_NullOptions_Throws(Create());
    [Fact] public Task OpenFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFileAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task OpenFilesAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.OpenFilesAsync_PreCancelledToken_Throws(Create());
    [Fact] public Task SaveFileAsync_PreCancelledToken_Throws() => FilePickerGuardAssertions.SaveFileAsync_PreCancelledToken_Throws(Create());
}

public sealed class AvaloniaFolderPickerGuardTests
{
    private static IFolderPickerService Create() => new AvaloniaFolderPickerService(() => null);

    [Fact] public Task SelectFolderAsync_NullOptions_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_NullOptions_Throws(Create());
    [Fact] public Task SelectFolderAsync_PreCancelledToken_Throws() => FolderPickerGuardAssertions.SelectFolderAsync_PreCancelledToken_Throws(Create());
}

public sealed class AvaloniaNoTopLevelTests
{
    [Fact]
    public async Task OpenFile_NoTopLevel_ReturnsNull()
    {
        var service = new AvaloniaFilePickerService(() => null);
        Assert.Null(await service.OpenFileAsync(new OpenFileDialogOptions(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task OpenFiles_NoTopLevel_ReturnsEmptyCollection()
    {
        var service = new AvaloniaFilePickerService(() => null);
        Assert.Empty(await service.OpenFilesAsync(new OpenFilesDialogOptions(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SaveFile_NoTopLevel_ReturnsNull()
    {
        var service = new AvaloniaFilePickerService(() => null);
        Assert.Null(await service.SaveFileAsync(new SaveFileDialogOptions(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SelectFolder_NoTopLevel_ReturnsNull()
    {
        var service = new AvaloniaFolderPickerService(() => null);
        Assert.Null(await service.SelectFolderAsync(new SelectFolderDialogOptions(), TestContext.Current.CancellationToken));
    }
}

public sealed class AvaloniaApplicationExitServiceTests
{
    [Fact]
    public void Exit_WithoutLifetime_UsesTerminator()
    {
        var terminator = new FakeProcessTerminator();
        var service = new AvaloniaApplicationExitService(terminator);
        service.Exit(3);
        Assert.Equal(3, terminator.ExitCode);
    }
}
