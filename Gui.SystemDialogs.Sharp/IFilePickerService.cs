namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// GUI-framework-neutral local file picker contract.
/// Does not open or write files — only returns selected local paths.
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// Selects one existing local file.
    /// Returns null when the dialog is cancelled.
    /// </summary>
    Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Selects multiple existing local files.
    /// Returns an empty collection when the dialog is cancelled.
    /// </summary>
    Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Selects a destination path for a file.
    /// Returns null when the dialog is cancelled.
    /// The method does not create or open the file.
    /// </summary>
    Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default);
}
