using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp;

/// <summary>
/// WinUI implementation of <see cref="IFolderPickerService"/>.
/// </summary>
public sealed class WinUiFolderPickerService : IFolderPickerService
{
    private readonly IWinUiPickerBackend _backend;

    public WinUiFolderPickerService(IWindowHandleProvider windowHandleProvider)
        : this(new NativeWinUiPickerBackend(windowHandleProvider))
    {
    }

    internal WinUiFolderPickerService(IWinUiPickerBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        return _backend.PickFolderAsync(new WinUiFolderRequest(), cancellationToken);
    }
}
