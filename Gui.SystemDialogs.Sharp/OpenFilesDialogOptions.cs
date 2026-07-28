namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting multiple files.
/// </summary>
public sealed record OpenFilesDialogOptions
{
    /// <summary>
    /// Dialog caption. When null the platform default is used.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// File types the dialog accepts, in the order they should be offered.
    /// <para>
    /// Adapters pass these through unchanged and never add an entry of their own, so an empty list
    /// means "no constraint" and the dialog accepts any file. To offer an escape hatch alongside a
    /// specific type, add an explicit filter whose pattern is <c>*.*</c>.
    /// </para>
    /// </summary>
    public IReadOnlyList<FileDialogFilter> Filters { get; init; }
        = [];

    /// <summary>
    /// Directory the dialog should start in. Ignored when null or when the path cannot be resolved.
    /// </summary>
    public string? InitialDirectory { get; init; }
}
