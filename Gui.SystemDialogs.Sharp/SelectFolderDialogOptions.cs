namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting a directory.
/// </summary>
public sealed record SelectFolderDialogOptions
{
    /// <summary>
    /// Dialog caption. When null the platform default is used.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Directory the dialog should start in. Ignored when null or when the path cannot be resolved.
    /// </summary>
    public string? InitialDirectory { get; init; }
}
