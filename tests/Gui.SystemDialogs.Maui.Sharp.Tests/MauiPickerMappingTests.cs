using Gui.SystemDialogs.Maui.Sharp;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Maui.Sharp.Tests;

public sealed class MauiFilePickerMappingTests
{
    [Fact]
    public async Task OpenFileAsync_MapsTitleAndExtensions()
    {
        var backend = new FakeMauiPickerBackend { OpenFileResult = @"C:\doc.pdf" };
        var service = new MauiFilePickerService(backend);

        var result = await service.OpenFileAsync(
            new OpenFileDialogOptions
            {
                Title = "Open document",
                Filters =
                [
                    new FileDialogFilter
                    {
                        DisplayName = "PDF",
                        Patterns = ["*.pdf", "*.PDF"]
                    }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\doc.pdf", result);
        Assert.Equal("Open document", backend.LastOpenFileRequest!.Title);
        Assert.Equal([".pdf"], backend.LastOpenFileRequest.Extensions);
    }

    [Fact]
    public async Task OpenFileAsync_NoFilters_UsesWildcard()
    {
        var backend = new FakeMauiPickerBackend();
        var service = new MauiFilePickerService(backend);

        await service.OpenFileAsync(
            new OpenFileDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Equal(["*"], backend.LastOpenFileRequest!.Extensions);
    }

    [Fact]
    public async Task OpenFilesAsync_Cancelled_ReturnsEmpty()
    {
        var backend = new FakeMauiPickerBackend { OpenFilesResult = [] };
        var service = new MauiFilePickerService(backend);

        Assert.Empty(await service.OpenFilesAsync(
            new OpenFilesDialogOptions { Title = "Many" },
            TestContext.Current.CancellationToken));
        Assert.Equal("Many", backend.LastOpenFilesRequest!.Title);
    }

    [Fact]
    public async Task SaveFileAsync_MapsSuggestedNameExtensionAndChoices()
    {
        var backend = new FakeMauiPickerBackend { SaveFileResult = @"C:\out.txt" };
        var service = new MauiFilePickerService(backend);

        var result = await service.SaveFileAsync(
            new SaveFileDialogOptions
            {
                SuggestedFileName = "notes",
                DefaultExtension = ".txt",
                Filters =
                [
                    new FileDialogFilter { DisplayName = "Text", Patterns = ["*.txt"] }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\out.txt", result);
        Assert.Equal("notes", backend.LastSaveFileRequest!.SuggestedFileName);
        Assert.Equal(".txt", backend.LastSaveFileRequest.DefaultFileExtension);
        Assert.Equal(
            [new KeyValuePair<string, IReadOnlyList<string>>("Text", [".txt"])],
            backend.LastSaveFileRequest.FileTypeChoices);
    }

    [Fact]
    public async Task SaveFileAsync_WildcardDefaultExtension_IsOmitted()
    {
        var backend = new FakeMauiPickerBackend();
        var service = new MauiFilePickerService(backend);

        await service.SaveFileAsync(
            new SaveFileDialogOptions { DefaultExtension = "*" },
            TestContext.Current.CancellationToken);

        Assert.Null(backend.LastSaveFileRequest!.DefaultFileExtension);
        Assert.Equal(
            [new KeyValuePair<string, IReadOnlyList<string>>("All files", ["."])],
            backend.LastSaveFileRequest.FileTypeChoices);
    }

    [Fact]
    public async Task OpenFileAsync_Cancelled_ReturnsNull()
    {
        var backend = new FakeMauiPickerBackend { OpenFileResult = null };
        var service = new MauiFilePickerService(backend);

        Assert.Null(await service.OpenFileAsync(
            new OpenFileDialogOptions(),
            TestContext.Current.CancellationToken));
    }
}

public sealed class MauiFolderPickerMappingTests
{
    [Fact]
    public async Task SelectFolderAsync_ReturnsSelectedPath()
    {
        var backend = new FakeMauiPickerBackend { FolderResult = @"C:\folder" };
        var service = new MauiFolderPickerService(backend);

        Assert.Equal(
            @"C:\folder",
            await service.SelectFolderAsync(
                new SelectFolderDialogOptions(),
                TestContext.Current.CancellationToken));
        Assert.NotNull(backend.LastFolderRequest);
    }

    [Fact]
    public async Task SelectFolderAsync_Cancelled_ReturnsNull()
    {
        var backend = new FakeMauiPickerBackend { FolderResult = null };
        var service = new MauiFolderPickerService(backend);

        Assert.Null(await service.SelectFolderAsync(
            new SelectFolderDialogOptions(),
            TestContext.Current.CancellationToken));
    }
}
