namespace Gui.SystemDialogs.Sharp;

/// <summary>
/// Options for selecting a destination file.
/// </summary>
public sealed record SaveFileDialogOptions
{
    /// <summary>
    /// Dialog caption. When null the platform default is used.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// File types offered as save formats, in the order they should be presented.
    /// <para>
    /// Adapters pass these through unchanged and never add an entry of their own, so an empty list
    /// means "no constraint". To offer an escape hatch alongside a specific type, add an explicit
    /// filter whose pattern is <c>*.*</c>.
    /// </para>
    /// </summary>
    public IReadOnlyList<FileDialogFilter> Filters { get; init; }
        = [];

    /// <summary>
    /// Directory the dialog should start in. Ignored when null or when the path cannot be resolved.
    /// </summary>
    public string? InitialDirectory { get; init; }

    /// <summary>
    /// File name to pre-fill in the dialog. Ignored when null.
    /// </summary>
    public string? SuggestedFileName { get; init; }

    /// <summary>
    /// Extension without a leading dot, for example "json". When null, adapters fall back to the
    /// first usable extension in <see cref="Filters"/>.
    /// </summary>
    public string? DefaultExtension { get; init; }

    /// <summary>
    /// Whether the dialog prompts before replacing an existing file. Defaults to <see langword="true"/>.
    /// </summary>
    public bool ConfirmOverwrite { get; init; } = true;
}
