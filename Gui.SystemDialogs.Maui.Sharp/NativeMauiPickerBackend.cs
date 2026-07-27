using Gui.SystemDialogs.Windows.Sharp;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Gui.SystemDialogs.Maui.Sharp;

internal sealed class NativeMauiPickerBackend(IWindowHandleProvider windowHandleProvider)
    : IMauiPickerBackend
{
    private readonly IWindowHandleProvider _windowHandleProvider = windowHandleProvider
        ?? throw new ArgumentNullException(nameof(windowHandleProvider));

    public async Task<string?> PickOpenFileAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken)
    {
        var result = await FilePicker.Default.PickAsync(ToPickOptions(request))
            .ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();
        return NullIfWhiteSpace(result?.FullPath);
    }

    public async Task<IReadOnlyList<string>> PickOpenFilesAsync(
        MauiOpenPickRequest request,
        CancellationToken cancellationToken)
    {
        var results = await FilePicker.Default.PickMultipleAsync(ToPickOptions(request))
            .ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();

        if (results is null)
        {
            return [];
        }

        return results
            .Select(static r => r.FullPath)
            .Where(static p => !string.IsNullOrWhiteSpace(p))
            .ToArray();
    }

    public async Task<string?> PickSaveFileAsync(
        MauiSavePickRequest request,
        CancellationToken cancellationToken)
    {
        var picker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            SuggestedFileName = request.SuggestedFileName
        };
        Initialize(picker);

        if (!string.IsNullOrWhiteSpace(request.DefaultFileExtension))
        {
            picker.DefaultFileExtension = request.DefaultFileExtension;
        }

        foreach (var choice in request.FileTypeChoices)
        {
            picker.FileTypeChoices[choice.Key] = choice.Value.ToList();
        }

        var file = await picker.PickSaveFileAsync();
        cancellationToken.ThrowIfCancellationRequested();
        return NullIfWhiteSpace(file?.Path);
    }

    public async Task<string?> PickFolderAsync(
        MauiFolderRequest request,
        CancellationToken cancellationToken)
    {
        _ = request;
        var picker = new FolderPicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        };
        Initialize(picker);
        picker.FileTypeFilter.Add("*");

        var folder = await picker.PickSingleFolderAsync();
        cancellationToken.ThrowIfCancellationRequested();
        return NullIfWhiteSpace(folder?.Path);
    }

    private void Initialize(object picker) =>
        InitializeWithWindow.Initialize(picker, _windowHandleProvider.GetWindowHandle());

    private static PickOptions ToPickOptions(MauiOpenPickRequest request)
    {
        return new PickOptions
        {
            PickerTitle = request.Title,
            FileTypes = ToMauiFileTypes(request.Extensions)
        };
    }

    private static FilePickerFileType? ToMauiFileTypes(IReadOnlyList<string> extensions)
    {
        if (extensions.Count == 0 || extensions is ["*"])
        {
            return null;
        }

        return new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, extensions },
            { DevicePlatform.Android, extensions },
            { DevicePlatform.iOS, extensions },
            { DevicePlatform.MacCatalyst, extensions },
            { DevicePlatform.macOS, extensions }
        });
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
