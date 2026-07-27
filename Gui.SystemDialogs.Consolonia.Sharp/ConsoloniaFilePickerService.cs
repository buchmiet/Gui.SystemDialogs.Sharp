using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Consolonia.Sharp;

/// <summary>
/// Consolonia implementation of <see cref="IFilePickerService"/>.
/// Uses Avalonia <see cref="IStorageProvider"/> — under Consolonia this resolves to terminal UI pickers.
/// </summary>
public sealed class ConsoloniaFilePickerService(Func<TopLevel?> topLevelProvider) : IFilePickerService
{
    private readonly Func<TopLevel?> _topLevelProvider = topLevelProvider ?? throw new ArgumentNullException(nameof(topLevelProvider));

    public ConsoloniaFilePickerService()
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
            FileTypeFilter = ConsoloniaFilePickerMapper.ToFilePickerTypes(options.Filters)
        }).ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();
        return files.Count > 0 ? files[0].TryGetLocalPath() : null;
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
            FileTypeFilter = ConsoloniaFilePickerMapper.ToFilePickerTypes(options.Filters)
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

        var fileTypes = ConsoloniaFilePickerMapper.ToFilePickerTypes(options.Filters);
        var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = options.Title,
            SuggestedFileName = options.SuggestedFileName,
            DefaultExtension = ConsoloniaFilePickerMapper.NormalizeExtension(options.DefaultExtension)
                ?? ConsoloniaFilePickerMapper.TryGetDefaultExtension(fileTypes),
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
                   ?? desktop.Windows.OfType<Window>()
                   .FirstOrDefault(static w => w.IsActive)
                   ?? desktop.Windows.OfType<Window>()
                   .FirstOrDefault();
        }

        return null;
    }
}
