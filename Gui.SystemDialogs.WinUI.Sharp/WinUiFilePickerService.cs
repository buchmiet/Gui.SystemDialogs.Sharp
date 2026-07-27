using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp;

/// <summary>
/// WinUI / Windows App SDK implementation of <see cref="IFilePickerService"/>.
/// Requires a native window handle via <see cref="IWindowHandleProvider"/>.
/// </summary>
public sealed class WinUiFilePickerService : IFilePickerService
{
    private readonly IWinUiPickerBackend _backend;

    public WinUiFilePickerService(IWindowHandleProvider windowHandleProvider)
        : this(new NativeWinUiPickerBackend(windowHandleProvider))
    {
    }

    internal WinUiFilePickerService(IWinUiPickerBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickOpenFileAsync(ToOpenRequest(options.Filters), cancellationToken);
    }

    public Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickOpenFilesAsync(ToOpenRequest(options.Filters), cancellationToken);
    }

    public Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickSaveFileAsync(ToSaveRequest(options), cancellationToken);
    }

    private static WinUiOpenFileRequest ToOpenRequest(IReadOnlyList<FileDialogFilter> filters) =>
        new(WinRtFileTypeMapper.ToOpenExtensions(filters));

    private static WinUiSaveFileRequest ToSaveRequest(SaveFileDialogOptions options)
    {
        string? defaultExtension = null;
        if (!string.IsNullOrWhiteSpace(options.DefaultExtension))
        {
            var normalized = WinRtFileTypeMapper.NormalizeExtension(options.DefaultExtension);
            defaultExtension = string.IsNullOrEmpty(normalized) || normalized == "*"
                ? ".dat"
                : normalized;
        }

        return new WinUiSaveFileRequest(
            SuggestedFileName: options.SuggestedFileName ?? string.Empty,
            DefaultFileExtension: defaultExtension,
            FileTypeChoices: ToFileTypeChoices(options.Filters));
    }

    private static IReadOnlyList<KeyValuePair<string, IReadOnlyList<string>>> ToFileTypeChoices(
        IReadOnlyList<FileDialogFilter> filters)
    {
        if (filters.Count == 0)
        {
            return [new KeyValuePair<string, IReadOnlyList<string>>("All files", ["."])];
        }

        return filters
            .Select(filter => new KeyValuePair<string, IReadOnlyList<string>>(
                filter.DisplayName,
                WinRtFileTypeMapper.ToSaveExtensions(filter)))
            .ToArray();
    }
}
