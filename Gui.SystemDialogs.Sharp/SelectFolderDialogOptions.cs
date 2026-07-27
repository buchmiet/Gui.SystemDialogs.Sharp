namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting a directory.
/// </summary>
public sealed record SelectFolderDialogOptions
{
    public string? Title { get; init; }

    public string? InitialDirectory { get; init; }
}
