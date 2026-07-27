using Gui.SystemDialogs.Sharp;
using Microsoft.UI.Xaml;

namespace Gui.SystemDialogs.ProGpu.Sharp;

/// <summary>
/// ProGPU implementation of <see cref="IFilePickerService"/> using ProGPU's WinUI-shaped storage pickers.
/// </summary>
public sealed class ProGpuFilePickerService : IFilePickerService
{
    public async Task<string?> OpenFileAsync(
        OpenFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var picker = new FileOpenPicker();
        ApplyOpenFilters(picker.FileTypeFilter, options.Filters);
        if (!string.IsNullOrWhiteSpace(options.InitialDirectory))
        {
            picker.SuggestedStartLocation = options.InitialDirectory;
        }

        var file = await picker.PickSingleFileAsync().ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();
        return file?.Path;
    }

    public async Task<IReadOnlyList<string>> OpenFilesAsync(
        OpenFilesDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        // ProGPU FileOpenPicker currently exposes single-file picking only.
        var picker = new FileOpenPicker();
        ApplyOpenFilters(picker.FileTypeFilter, options.Filters);
        if (!string.IsNullOrWhiteSpace(options.InitialDirectory))
        {
            picker.SuggestedStartLocation = options.InitialDirectory;
        }

        var file = await picker.PickSingleFileAsync().ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();

        return file is null
            ? Array.Empty<string>()
            : new[] { file.Path };
    }

    public async Task<string?> SaveFileAsync(
        SaveFileDialogOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();

        var picker = new FileSavePicker
        {
            SuggestedFileName = options.SuggestedFileName ?? "untitled"
        };

        if (!string.IsNullOrWhiteSpace(options.InitialDirectory))
        {
            picker.SuggestedStartLocation = options.InitialDirectory;
        }

        ApplySaveFilters(picker.FileTypeChoices, options.Filters);

        var file = await picker.PickSaveFileAsync().ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();
        return file?.Path;
    }

    private static void ApplyOpenFilters(List<string> target, IReadOnlyList<FileDialogFilter> filters)
    {
        foreach (var extension in ProGpuFileTypeMapper.ToOpenExtensions(filters))
        {
            target.Add(extension);
        }
    }

    private static void ApplySaveFilters(
        Dictionary<string, IList<string>> target,
        IReadOnlyList<FileDialogFilter> filters)
    {
        if (filters.Count == 0)
        {
            target["All files"] = new List<string> { ".*" };
            return;
        }

        foreach (var filter in filters)
        {
            target[filter.DisplayName] =
                ProGpuFileTypeMapper.ToSaveExtensions(filter).ToList();
        }
    }
}
