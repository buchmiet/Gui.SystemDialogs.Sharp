namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Describes one selectable file type in a file dialog.
/// </summary>
public sealed record FileDialogFilter
{
    /// <summary>
    /// User-visible filter name, for example "PNG images".
    /// </summary>
    public required string DisplayName { get; init; }

    /// <summary>
    /// File patterns, for example "*.png" or "*.jpg".
    /// </summary>
    public required IReadOnlyList<string> Patterns { get; init; }
}
