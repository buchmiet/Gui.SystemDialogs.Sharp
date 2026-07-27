using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp;

/// <summary>
/// WPF implementation of <see cref="IFilePickerService"/> using common item dialogs.
/// </summary>
public sealed class WpfFilePickerService : IFilePickerService
{
    private readonly IWpfDialogBackend _backend;

    public WpfFilePickerService()
        : this(new NativeWpfDialogBackend())
    {
    }

    internal WpfFilePickerService(IWpfDialogBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var path = _backend.ShowOpenFile(ToOpenRequest(options, multiselect: false));
        return Task.FromResult(path);
    }

    public Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var paths = _backend.ShowOpenFiles(ToOpenRequest(options));
        return Task.FromResult(paths);
    }

    public Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var path = _backend.ShowSaveFile(ToSaveRequest(options));
        return Task.FromResult(path);
    }

    private static WpfOpenFileRequest ToOpenRequest(
        OpenFileDialogOptions options,
        bool multiselect) =>
        new(
            Title: options.Title ?? string.Empty,
            Filter: WpfFileFilterFormat.Format(options.Filters),
            InitialDirectory: NullIfWhiteSpace(options.InitialDirectory),
            InitialFileName: NullIfWhiteSpace(options.InitialFileName),
            Multiselect: multiselect);

    private static WpfOpenFileRequest ToOpenRequest(OpenFilesDialogOptions options) =>
        new(
            Title: options.Title ?? string.Empty,
            Filter: WpfFileFilterFormat.Format(options.Filters),
            InitialDirectory: NullIfWhiteSpace(options.InitialDirectory),
            InitialFileName: null,
            Multiselect: true);

    private static WpfSaveFileRequest ToSaveRequest(SaveFileDialogOptions options) =>
        new(
            Title: options.Title ?? string.Empty,
            Filter: WpfFileFilterFormat.Format(options.Filters),
            InitialDirectory: NullIfWhiteSpace(options.InitialDirectory),
            FileName: NullIfWhiteSpace(options.SuggestedFileName),
            DefaultExt: NormalizeDefaultExt(options.DefaultExtension),
            OverwritePrompt: options.ConfirmOverwrite);

    private static string? NormalizeDefaultExt(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return null;
        }

        return extension.Trim().TrimStart('.');
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
