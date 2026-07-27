using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Wpf.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp.Tests;

public sealed class WpfFilePickerMappingTests
{
    [Fact]
    public async Task OpenFileAsync_MapsOptionsAndReturnsSelectedPath()
    {
        var backend = new FakeWpfDialogBackend { OpenFileResult = @"C:\tmp\a.png" };
        var service = new WpfFilePickerService(backend);

        var result = await service.OpenFileAsync(
            new OpenFileDialogOptions
            {
                Title = "Open image",
                InitialDirectory = @"C:\images",
                InitialFileName = "photo",
                Filters =
                [
                    new FileDialogFilter
                    {
                        DisplayName = "Images",
                        Patterns = ["*.png", "*.jpg"]
                    }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\tmp\a.png", result);
        Assert.NotNull(backend.LastOpenFileRequest);
        Assert.Equal("Open image", backend.LastOpenFileRequest.Title);
        Assert.Equal("Images|*.png;*.jpg", backend.LastOpenFileRequest.Filter);
        Assert.Equal(@"C:\images", backend.LastOpenFileRequest.InitialDirectory);
        Assert.Equal("photo", backend.LastOpenFileRequest.InitialFileName);
        Assert.False(backend.LastOpenFileRequest.Multiselect);
    }

    [Fact]
    public async Task OpenFileAsync_CancelledDialog_ReturnsNull()
    {
        var backend = new FakeWpfDialogBackend { OpenFileResult = null };
        var service = new WpfFilePickerService(backend);

        var result = await service.OpenFileAsync(
            new OpenFileDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task OpenFilesAsync_MapsMultiselectAndReturnsPaths()
    {
        var backend = new FakeWpfDialogBackend
        {
            OpenFilesResult = [@"C:\a.txt", @"C:\b.txt"]
        };
        var service = new WpfFilePickerService(backend);

        var result = await service.OpenFilesAsync(
            new OpenFilesDialogOptions
            {
                Title = "Open many",
                InitialDirectory = @"C:\docs",
                Filters =
                [
                    new FileDialogFilter { DisplayName = "Text", Patterns = ["*.txt"] }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal([@"C:\a.txt", @"C:\b.txt"], result);
        Assert.NotNull(backend.LastOpenFilesRequest);
        Assert.Equal("Open many", backend.LastOpenFilesRequest.Title);
        Assert.Equal("Text|*.txt", backend.LastOpenFilesRequest.Filter);
        Assert.Equal(@"C:\docs", backend.LastOpenFilesRequest.InitialDirectory);
        Assert.Null(backend.LastOpenFilesRequest.InitialFileName);
        Assert.True(backend.LastOpenFilesRequest.Multiselect);
    }

    [Fact]
    public async Task OpenFilesAsync_CancelledDialog_ReturnsEmpty()
    {
        var backend = new FakeWpfDialogBackend { OpenFilesResult = [] };
        var service = new WpfFilePickerService(backend);

        var result = await service.OpenFilesAsync(
            new OpenFilesDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Empty(result);
    }

    [Fact]
    public async Task SaveFileAsync_MapsOptionsIncludingNormalizedExtension()
    {
        var backend = new FakeWpfDialogBackend { SaveFileResult = @"C:\out.json" };
        var service = new WpfFilePickerService(backend);

        var result = await service.SaveFileAsync(
            new SaveFileDialogOptions
            {
                Title = "Save",
                InitialDirectory = @"C:\out",
                SuggestedFileName = "data",
                DefaultExtension = ".json",
                ConfirmOverwrite = false,
                Filters =
                [
                    new FileDialogFilter { DisplayName = "JSON", Patterns = ["*.json"] }
                ]
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\out.json", result);
        Assert.NotNull(backend.LastSaveFileRequest);
        Assert.Equal("Save", backend.LastSaveFileRequest.Title);
        Assert.Equal("JSON|*.json", backend.LastSaveFileRequest.Filter);
        Assert.Equal(@"C:\out", backend.LastSaveFileRequest.InitialDirectory);
        Assert.Equal("data", backend.LastSaveFileRequest.FileName);
        Assert.Equal("json", backend.LastSaveFileRequest.DefaultExt);
        Assert.False(backend.LastSaveFileRequest.OverwritePrompt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SaveFileAsync_BlankDefaultExtension_MapsToNull(string? extension)
    {
        var backend = new FakeWpfDialogBackend();
        var service = new WpfFilePickerService(backend);

        await service.SaveFileAsync(
            new SaveFileDialogOptions { DefaultExtension = extension },
            TestContext.Current.CancellationToken);

        Assert.Null(backend.LastSaveFileRequest!.DefaultExt);
    }

    [Fact]
    public async Task OpenFileAsync_BlankOptionalPaths_AreOmitted()
    {
        var backend = new FakeWpfDialogBackend();
        var service = new WpfFilePickerService(backend);

        await service.OpenFileAsync(
            new OpenFileDialogOptions
            {
                Title = null,
                InitialDirectory = "  ",
                InitialFileName = ""
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(string.Empty, backend.LastOpenFileRequest!.Title);
        Assert.Null(backend.LastOpenFileRequest.InitialDirectory);
        Assert.Null(backend.LastOpenFileRequest.InitialFileName);
        Assert.Equal("All Files|*.*", backend.LastOpenFileRequest.Filter);
    }
}

public sealed class WpfFolderPickerMappingTests
{
    [Fact]
    public async Task SelectFolderAsync_MapsOptionsAndReturnsPath()
    {
        var backend = new FakeWpfDialogBackend { SelectFolderResult = @"C:\folder" };
        var service = new WpfFolderPickerService(backend);

        var result = await service.SelectFolderAsync(
            new SelectFolderDialogOptions
            {
                Title = "Pick folder",
                InitialDirectory = @"C:\start"
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(@"C:\folder", result);
        Assert.NotNull(backend.LastSelectFolderRequest);
        Assert.Equal("Pick folder", backend.LastSelectFolderRequest.Title);
        Assert.Equal(@"C:\start", backend.LastSelectFolderRequest.InitialDirectory);
    }

    [Fact]
    public async Task SelectFolderAsync_CancelledDialog_ReturnsNull()
    {
        var backend = new FakeWpfDialogBackend { SelectFolderResult = null };
        var service = new WpfFolderPickerService(backend);

        var result = await service.SelectFolderAsync(
            new SelectFolderDialogOptions(),
            TestContext.Current.CancellationToken);

        Assert.Null(result);
    }
}
