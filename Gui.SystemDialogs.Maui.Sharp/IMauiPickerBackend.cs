namespace Gui.SystemDialogs.Maui.Sharp;

internal sealed record MauiOpenPickRequest(
    string? Title,
    IReadOnlyList<string> Extensions);

internal sealed record MauiSavePickRequest(
    string SuggestedFileName,
    string? DefaultFileExtension,
    IReadOnlyList<KeyValuePair<string, IReadOnlyList<string>>> FileTypeChoices);

internal sealed record MauiFolderRequest;

internal interface IMauiPickerBackend
{
    Task<string?> PickOpenFileAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> PickOpenFilesAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken);

    Task<string?> PickSaveFileAsync(
        MauiSavePickRequest request,
        CancellationToken cancellationToken);

    Task<string?> PickFolderAsync(
        MauiFolderRequest request,
        CancellationToken cancellationToken);
}
