using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Avalonia.Sharp;

/// <summary>
/// Avalonia implementation of <see cref="IFilePickerService"/> using <see cref="IStorageProvider"/>.
/// Owner window is supplied via constructor — not through the neutral contract.
/// </summary>
public sealed class AvaloniaFilePickerService : IFilePickerService
{
    private readonly Func<TopLevel?> _topLevelProvider;

    /// <summary>
    /// Creates a service that resolves the active <see cref="TopLevel"/> via <paramref name="topLevelProvider"/>.
    /// </summary>
    public AvaloniaFilePickerService(Func<TopLevel?> topLevelProvider)
    {
        _topLevelProvider = topLevelProvider ?? throw new ArgumentNullException(nameof(topLevelProvider));
    }

    /// <summary>
    /// Creates a service that uses the desktop main window (or the first open window) as <see cref="TopLevel"/>.
    /// </summary>
    public AvaloniaFilePickerService()
        : this(ResolveDefaultTopLevel)
    {
    }

    public async Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var storage = GetStorageProvider();
        if (storage is null || !storage.CanOpen)
        {
            return null;
        }

        var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = false,
            SuggestedFileName = options.InitialFileName,
            SuggestedStartLocation = await TryGetStartFolderAsync(storage, options.InitialDirectory, cancellationToken)
                .ConfigureAwait(true),
            FileTypeFilter = AvaloniaFilePickerMapper.ToFilePickerTypes(options.Filters)
        }).ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();

        var file = files.Count > 0 ? files[0] : null;
        return file?.TryGetLocalPath();
    }

    public async Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var storage = GetStorageProvider();
        if (storage is null || !storage.CanOpen)
        {
            return [];
        }

        var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = true,
            SuggestedStartLocation = await TryGetStartFolderAsync(storage, options.InitialDirectory, cancellationToken)
                .ConfigureAwait(true),
            FileTypeFilter = AvaloniaFilePickerMapper.ToFilePickerTypes(options.Filters)
        }).ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();

        return files
            .Select(static f => f.TryGetLocalPath())
            .Where(static p => !string.IsNullOrWhiteSpace(p))
            .Cast<string>()
            .ToArray();
    }

    public async Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var storage = GetStorageProvider();
        if (storage is null || !storage.CanSave)
        {
            return null;
        }

        var fileTypes = AvaloniaFilePickerMapper.ToFilePickerTypes(options.Filters);
        var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = options.Title,
            SuggestedFileName = options.SuggestedFileName,
            DefaultExtension = AvaloniaFilePickerMapper.NormalizeExtension(options.DefaultExtension)
                ?? AvaloniaFilePickerMapper.TryGetDefaultExtension(fileTypes),
            ShowOverwritePrompt = options.ConfirmOverwrite,
            SuggestedStartLocation = await TryGetStartFolderAsync(storage, options.InitialDirectory, cancellationToken)
                .ConfigureAwait(true),
            FileTypeChoices = fileTypes
        }).ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();
        return file?.TryGetLocalPath();
    }

    private IStorageProvider? GetStorageProvider() => _topLevelProvider()?.StorageProvider;

    private static async Task<IStorageFolder?> TryGetStartFolderAsync(
        IStorageProvider storage,
        string? initialDirectory,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(initialDirectory))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            return await storage.TryGetFolderFromPathAsync(Path.GetFullPath(initialDirectory))
                .ConfigureAwait(true);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static TopLevel? ResolveDefaultTopLevel()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow
                   ?? desktop.Windows.OfType<Window>().FirstOrDefault(static w => w.IsActive)
                   ?? desktop.Windows.OfType<Window>().FirstOrDefault();
        }

        return null;
    }
}
