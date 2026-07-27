using Gui.SystemDialogs.Sharp;
using Microsoft.UI.Xaml;

namespace Gui.SystemDialogs.ProGpu.Sharp;

/// <summary>
/// ProGPU implementation of <see cref="IFolderPickerService"/>.
/// </summary>
public sealed class ProGpuFolderPickerService : IFolderPickerService
{
    public async Task<string?> SelectFolderAsync(
        SelectFolderDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var picker = new FolderPicker();
        if (!string.IsNullOrWhiteSpace(options.InitialDirectory))
        {
            picker.SuggestedStartLocation = options.InitialDirectory;
        }

        var folder = await picker.PickSingleFolderAsync().ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();
        return folder?.Path;
    }
}
