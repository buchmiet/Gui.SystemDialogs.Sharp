namespace Gui.SystemDialogs.WinUI.Sharp;

internal sealed record WinUiOpenFileRequest(IReadOnlyList<string> FileTypeFilter);

internal sealed record WinUiSaveFileRequest(
    string SuggestedFileName,
    string? DefaultFileExtension,
    IReadOnlyList<KeyValuePair<string, IReadOnlyList<string>>> FileTypeChoices);

internal sealed record WinUiFolderRequest;

internal interface IWinUiPickerBackend
{
    Task<string?> PickOpenFileAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> PickOpenFilesAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken);

    Task<string?> PickSaveFileAsync(
        WinUiSaveFileRequest request,
        CancellationToken cancellationToken);

    Task<string?> PickFolderAsync(
        WinUiFolderRequest request,
        CancellationToken cancellationToken);
}
