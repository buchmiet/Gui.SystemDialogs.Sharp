namespace Gui.SystemDialogs.WinUI.Sharp.Tests;

internal sealed class FakeWinUiPickerBackend : IWinUiPickerBackend
{
    public WinUiOpenFileRequest? LastOpenFileRequest { get; private set; }
    public WinUiOpenFileRequest? LastOpenFilesRequest { get; private set; }
    public WinUiSaveFileRequest? LastSaveFileRequest { get; private set; }
    public WinUiFolderRequest? LastFolderRequest { get; private set; }

    public string? OpenFileResult { get; init; }
    public IReadOnlyList<string> OpenFilesResult { get; init; } = [];
    public string? SaveFileResult { get; init; }
    public string? FolderResult { get; init; }

    public Task<string?> PickOpenFileAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastOpenFileRequest = request;
        return Task.FromResult(OpenFileResult);
    }

    public Task<IReadOnlyList<string>> PickOpenFilesAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastOpenFilesRequest = request;
        return Task.FromResult(OpenFilesResult);
    }

    public Task<string?> PickSaveFileAsync(
        WinUiSaveFileRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastSaveFileRequest = request;
        return Task.FromResult(SaveFileResult);
    }

    public Task<string?> PickFolderAsync(
        WinUiFolderRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastFolderRequest = request;
        return Task.FromResult(FolderResult);
    }
}
