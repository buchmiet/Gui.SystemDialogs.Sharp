namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting a single file.
/// </summary>
public sealed record OpenFileDialogOptions
{
    public string? Title { get; init; }

    public IReadOnlyList<FileDialogFilter> Filters { get; init; }
        = [];

    public string? InitialDirectory { get; init; }

    public string? InitialFileName { get; init; }
}
