using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Consolonia.Sharp;

/// <summary>
/// Consolonia implementation of <see cref="IFolderPickerService"/>.
/// Uses Avalonia <see cref="IStorageProvider"/> (terminal UI under Consolonia).
/// </summary>
public sealed class ConsoloniaFolderPickerService : IFolderPickerService
{
    private readonly Func<TopLevel?> _topLevelProvider;

    public ConsoloniaFolderPickerService(Func<TopLevel?> topLevelProvider)
    {
        _topLevelProvider = topLevelProvider ?? throw new ArgumentNullException(nameof(topLevelProvider));
    }

    public ConsoloniaFolderPickerService()
        : this(ResolveDefaultTopLevel)
    {
    }

    public async Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var storage = _topLevelProvider()?.StorageProvider;
        if (storage is null || !storage.CanPickFolder)
        {
            return null;
        }

        IStorageFolder? start = null;
        if (!string.IsNullOrWhiteSpace(options.InitialDirectory))
        {
            try
            {
                start = await storage.TryGetFolderFromPathAsync(Path.GetFullPath(options.InitialDirectory))
                    .ConfigureAwait(true);
            }
            catch (Exception)
            {
                start = null;
            }
        }

        var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = false,
            SuggestedStartLocation = start
        }).ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();
        return folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
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
