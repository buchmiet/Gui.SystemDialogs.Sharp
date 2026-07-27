using Gui.SystemDialogs.Sharp;
using Xunit;

namespace Gui.SystemDialogs.TestSupport;

public static class FolderPickerGuardAssertions
{
    public static async Task SelectFolderAsync_NullOptions_Throws(IFolderPickerService service)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.SelectFolderAsync(null!));
    }

    public static async Task SelectFolderAsync_PreCancelledToken_Throws(IFolderPickerService service)
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.SelectFolderAsync(new SelectFolderDialogOptions(), cts.Token));
    }
}
