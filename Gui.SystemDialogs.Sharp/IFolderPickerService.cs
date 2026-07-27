namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// GUI-framework-neutral local directory picker contract.
/// Returns a selected local path only — does not create directories.
/// </summary>
public interface IFolderPickerService
{
    /// <summary>
    /// Selects an existing local directory.
    /// Returns null when the dialog is cancelled.
    /// </summary>
    Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default);
}
