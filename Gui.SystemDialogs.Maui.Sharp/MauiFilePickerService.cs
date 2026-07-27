using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.Maui.Sharp;

/// <summary>
/// .NET MAUI (Windows) implementation of <see cref="IFilePickerService"/>.
/// Open uses MAUI <c>FilePicker</c>; save uses WinRT pickers with an owner HWND.
/// </summary>
public sealed class MauiFilePickerService : IFilePickerService
{
    private readonly IMauiPickerBackend _backend;

    public MauiFilePickerService(IWindowHandleProvider windowHandleProvider)
        : this(new NativeMauiPickerBackend(windowHandleProvider))
    {
    }

    internal MauiFilePickerService(IMauiPickerBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickOpenFileAsync(ToOpenRequest(options.Title, options.Filters), cancellationToken);
    }

    public Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickOpenFilesAsync(ToOpenRequest(options.Title, options.Filters), cancellationToken);
    }

    public Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickSaveFileAsync(ToSaveRequest(options), cancellationToken);
    }

    private static MauiOpenPickRequest ToOpenRequest(
        string? title,
        IReadOnlyList<FileDialogFilter> filters) =>
        new(title, WinRtFileTypeMapper.ToOpenExtensions(filters));

    private static MauiSavePickRequest ToSaveRequest(SaveFileDialogOptions options)
    {
        string? defaultExtension = null;
        if (!string.IsNullOrWhiteSpace(options.DefaultExtension))
        {
            var ext = WinRtFileTypeMapper.NormalizeExtension(options.DefaultExtension);
            if (!string.IsNullOrEmpty(ext) && ext != "*")
            {
                defaultExtension = ext;
            }
        }

        return new MauiSavePickRequest(
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
