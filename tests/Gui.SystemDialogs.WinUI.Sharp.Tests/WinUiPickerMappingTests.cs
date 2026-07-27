using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.WinUI.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp.Tests;

public sealed class WinUiFilePickerMappingTests
{
    [Fact]
    public async Task OpenFileAsync_MapsFiltersAndReturnsPath()
    {
        var backend = new FakeWinUiPickerBackend { OpenFileResult = @"C:\a.png" };
        var service = new WinUiFilePickerService(backend);

        var result = await service.OpenFileAsync(
            new OpenFileDialogOptions
            {
                Filters =
                [
                    new FileDialogFilter
                    {
                        DisplayName = "Images",
                        Patterns = ["*.png", "*.PNG", "*.jpg"]
                    }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\a.png", result);
        Assert.Equal([".png", ".jpg"], backend.LastOpenFileRequest!.FileTypeFilter);
    }

    [Fact]
    public async Task OpenFileAsync_NoFilters_UsesWildcard()
    {
        var backend = new FakeWinUiPickerBackend();
        var service = new WinUiFilePickerService(backend);

        await service.OpenFileAsync(
            new OpenFileDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Equal(["*"], backend.LastOpenFileRequest!.FileTypeFilter);
    }

    [Fact]
    public async Task OpenFileAsync_Cancelled_ReturnsNull()
    {
        var backend = new FakeWinUiPickerBackend { OpenFileResult = null };
        var service = new WinUiFilePickerService(backend);

        Assert.Null(await service.OpenFileAsync(
            new OpenFileDialogOptions(),
            TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task OpenFilesAsync_Cancelled_ReturnsEmpty()
    {
        var backend = new FakeWinUiPickerBackend { OpenFilesResult = [] };
        var service = new WinUiFilePickerService(backend);

        Assert.Empty(await service.OpenFilesAsync(
            new OpenFilesDialogOptions(),
            TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SaveFileAsync_MapsSuggestedNameExtensionAndChoices()
    {
        var backend = new FakeWinUiPickerBackend { SaveFileResult = @"C:\out.json" };
        var service = new WinUiFilePickerService(backend);

        var result = await service.SaveFileAsync(
            new SaveFileDialogOptions
            {
                SuggestedFileName = "report",
                DefaultExtension = "json",
                Filters =
                [
                    new FileDialogFilter { DisplayName = "JSON", Patterns = ["*.json"] },
                    new FileDialogFilter { DisplayName = "Text", Patterns = ["*.txt"] }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\out.json", result);
        Assert.NotNull(backend.LastSaveFileRequest);
        Assert.Equal("report", backend.LastSaveFileRequest.SuggestedFileName);
        Assert.Equal(".json", backend.LastSaveFileRequest.DefaultFileExtension);
        Assert.Collection(
            backend.LastSaveFileRequest.FileTypeChoices,
            json =>
            {
                Assert.Equal("JSON", json.Key);
                Assert.Equal([".json"], json.Value);
            },
            text =>
            {
                Assert.Equal("Text", text.Key);
                Assert.Equal([".txt"], text.Value);
            });
    }

    [Fact]
    public async Task SaveFileAsync_NoFilters_UsesAllFilesChoice()
    {
        var backend = new FakeWinUiPickerBackend();
        var service = new WinUiFilePickerService(backend);

        await service.SaveFileAsync(
            new SaveFileDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Equal(
            [new KeyValuePair<string, IReadOnlyList<string>>("All files", ["."])],
            backend.LastSaveFileRequest!.FileTypeChoices);
        Assert.Null(backend.LastSaveFileRequest.DefaultFileExtension);
    }

    [Theory]
    [InlineData("*", ".dat")]
    [InlineData("*.*", ".dat")]
    public async Task SaveFileAsync_WildcardDefaultExtension_FallsBackToDat(
        string extension,
        string expected)
    {
        var backend = new FakeWinUiPickerBackend();
        var service = new WinUiFilePickerService(backend);

        await service.SaveFileAsync(
            new SaveFileDialogOptions { DefaultExtension = extension },
            TestContext.Current.CancellationToken);

        Assert.Equal(expected, backend.LastSaveFileRequest!.DefaultFileExtension);
    }
}

public sealed class WinUiFolderPickerMappingTests
{
    [Fact]
    public async Task SelectFolderAsync_Cancelled_ReturnsNull()
    {
        var backend = new FakeWinUiPickerBackend { FolderResult = null };
        var service = new WinUiFolderPickerService(backend);

        Assert.Null(await service.SelectFolderAsync(
            new SelectFolderDialogOptions { Title = "Folder" },
            TestContext.Current.CancellationToken));
        Assert.NotNull(backend.LastFolderRequest);
    }

    [Fact]
    public async Task SelectFolderAsync_ReturnsSelectedPath()
    {
        var backend = new FakeWinUiPickerBackend { FolderResult = @"C:\picked" };
        var service = new WinUiFolderPickerService(backend);

        Assert.Equal(
            @"C:\picked",
            await service.SelectFolderAsync(
                new SelectFolderDialogOptions(),
                TestContext.Current.CancellationToken));
    }
}
