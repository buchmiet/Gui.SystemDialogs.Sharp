namespace Gui.SystemDialogs.Maui.Sharp.Tests;

internal sealed class FakeMauiPickerBackend : IMauiPickerBackend
{
    public MauiOpenPickRequest? LastOpenFileRequest { get; private set; }
    public MauiOpenPickRequest? LastOpenFilesRequest { get; private set; }
    public MauiSavePickRequest? LastSaveFileRequest { get; private set; }
    public MauiFolderRequest? LastFolderRequest { get; private set; }

    public string? OpenFileResult { get; init; }
    public IReadOnlyList<string> OpenFilesResult { get; init; } = [];
    public string? SaveFileResult { get; init; }
    public string? FolderResult { get; init; }

    public Task<string?> PickOpenFileAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastOpenFileRequest = request;
        return Task.FromResult(OpenFileResult);
    }

    public Task<IReadOnlyList<string>> PickOpenFilesAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastOpenFilesRequest = request;
        return Task.FromResult(OpenFilesResult);
    }

    public Task<string?> PickSaveFileAsync(
        MauiSavePickRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastSaveFileRequest = request;
        return Task.FromResult(SaveFileResult);
    }

    public Task<string?> PickFolderAsync(
        MauiFolderRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastFolderRequest = request;
        return Task.FromResult(FolderResult);
    }
}
