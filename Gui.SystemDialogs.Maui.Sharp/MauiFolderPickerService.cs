using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.Maui.Sharp;

/// <summary>
/// MAUI (Windows) implementation of <see cref="IFolderPickerService"/>.
/// </summary>
public sealed class MauiFolderPickerService : IFolderPickerService
{
    private readonly IMauiPickerBackend _backend;

    public MauiFolderPickerService(IWindowHandleProvider windowHandleProvider)
        : this(new NativeMauiPickerBackend(windowHandleProvider))
    {
    }

    internal MauiFolderPickerService(IMauiPickerBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickFolderAsync(new MauiFolderRequest(), cancellationToken);
    }
}
