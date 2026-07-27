using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp;

/// <summary>
/// WPF implementation of <see cref="IFolderPickerService"/> using <see cref="Microsoft.Win32.OpenFolderDialog"/>.
/// </summary>
public sealed class WpfFolderPickerService : IFolderPickerService
{
    private readonly IWpfDialogBackend _backend;

    public WpfFolderPickerService()
        : this(new NativeWpfDialogBackend())
    {
    }

    internal WpfFolderPickerService(IWpfDialogBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var path = _backend.ShowSelectFolder(
            new WpfSelectFolderRequest(
                Title: options.Title ?? string.Empty,
                InitialDirectory: string.IsNullOrWhiteSpace(options.InitialDirectory)
                    ? null
                    : options.InitialDirectory));

        return Task.FromResult(path);
    }
}
