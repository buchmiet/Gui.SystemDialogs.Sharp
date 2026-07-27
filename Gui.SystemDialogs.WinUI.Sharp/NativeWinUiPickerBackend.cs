using Gui.SystemDialogs.Windows.Sharp;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Gui.SystemDialogs.WinUI.Sharp;

internal sealed class NativeWinUiPickerBackend(IWindowHandleProvider windowHandleProvider)
    : IWinUiPickerBackend
{
    private readonly IWindowHandleProvider _windowHandleProvider = windowHandleProvider
        ?? throw new ArgumentNullException(nameof(windowHandleProvider));

    public async Task<string?> PickOpenFileAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken)
    {
        var picker = CreateOpenPicker(request);
        var file = await picker.PickSingleFileAsync();
        cancellationToken.ThrowIfCancellationRequested();
        return NullIfWhiteSpace(file?.Path);
    }

    public async Task<IReadOnlyList<string>> PickOpenFilesAsync(
        WinUiOpenFileRequest request,
        CancellationToken cancellationToken)
    {
        var picker = CreateOpenPicker(request);
        var files = await picker.PickMultipleFilesAsync();
        cancellationToken.ThrowIfCancellationRequested();

        if (files is null || files.Count == 0)
        {
            return [];
        }

        return files
            .Select(static f => f.Path)
            .Where(static p => !string.IsNullOrWhiteSpace(p))
            .ToArray();
    }

    public async Task<string?> PickSaveFileAsync(
        WinUiSaveFileRequest request,
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
        WinUiFolderRequest request,
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

    private FileOpenPicker CreateOpenPicker(WinUiOpenFileRequest request)
    {
        var picker = new FileOpenPicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            ViewMode = PickerViewMode.List
        };
        Initialize(picker);

        foreach (var extension in request.FileTypeFilter)
        {
            picker.FileTypeFilter.Add(extension);
        }

        return picker;
    }

    private void Initialize(object picker) =>
        InitializeWithWindow.Initialize(picker, _windowHandleProvider.GetWindowHandle());

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
