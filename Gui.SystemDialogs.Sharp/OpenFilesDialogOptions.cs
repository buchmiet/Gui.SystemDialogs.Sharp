namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting multiple files.
/// </summary>
public sealed record OpenFilesDialogOptions
{
    public string? Title { get; init; }

    public IReadOnlyList<FileDialogFilter> Filters { get; init; }
        = [];

    public string? InitialDirectory { get; init; }
}
