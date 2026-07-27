using Gui.SystemDialogs.Sharp;
using Xunit;

namespace Gui.SystemDialogs.TestSupport;

/// <summary>
/// Shared assertion helpers for null-options and pre-cancelled-token guards.
/// </summary>
public static class FilePickerGuardAssertions
{
    public static async Task OpenFileAsync_NullOptions_Throws(IFilePickerService service)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.OpenFileAsync(null!));
    }

    public static async Task OpenFilesAsync_NullOptions_Throws(IFilePickerService service)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.OpenFilesAsync(null!));
    }

    public static async Task SaveFileAsync_NullOptions_Throws(IFilePickerService service)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.SaveFileAsync(null!));
    }

    public static async Task OpenFileAsync_PreCancelledToken_Throws(IFilePickerService service)
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.OpenFileAsync(new OpenFileDialogOptions(), cts.Token));
    }

    public static async Task OpenFilesAsync_PreCancelledToken_Throws(IFilePickerService service)
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.OpenFilesAsync(new OpenFilesDialogOptions(), cts.Token));
    }

    public static async Task SaveFileAsync_PreCancelledToken_Throws(IFilePickerService service)
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.SaveFileAsync(new SaveFileDialogOptions(), cts.Token));
    }
}
