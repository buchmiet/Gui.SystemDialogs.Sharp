namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting a destination file.
/// </summary>
public sealed record SaveFileDialogOptions
{
    public string? Title { get; init; }

    public IReadOnlyList<FileDialogFilter> Filters { get; init; }
        = [];

    public string? InitialDirectory { get; init; }

    public string? SuggestedFileName { get; init; }

    /// <summary>
    /// Extension without a leading dot, for example "json".
    /// </summary>
    public string? DefaultExtension { get; init; }

    public bool ConfirmOverwrite { get; init; } = true;
}
