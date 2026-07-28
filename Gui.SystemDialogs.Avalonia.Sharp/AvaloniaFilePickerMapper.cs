using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Avalonia.Sharp;

/// <summary>
/// Maps neutral filters onto Avalonia storage types.
/// <para>
/// Near-identical to <c>ConsoloniaFilePickerMapper</c> on purpose — both target
/// <c>IStorageProvider</c>, but this project tracks Avalonia 12.x while Consolonia tracks 11.x. The
/// two must be free to diverge when either major moves, so the duplication stays. Behavioural
/// agreement is enforced by <c>FilterMappingConformance</c>, not by sharing code.
/// </para>
/// </summary>
internal static class AvaloniaFilePickerMapper
{
    public static IReadOnlyList<FilePickerFileType> ToFilePickerTypes(IReadOnlyList<FileDialogFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);

        var result = new List<FilePickerFileType>();

        foreach (var filter in filters)
        {
            if (filter.Patterns.Count == 0)
            {
                continue;
            }

            result.Add(new FilePickerFileType(filter.DisplayName)
            {
                Patterns = filter.Patterns.ToArray()
            });
        }

        // Caller-supplied filters are passed through verbatim: an "all files" entry the caller
        // did not ask for cannot be removed through the neutral contract. The fallback applies
        // only to the degenerate case where no constraint was expressed at all.
        if (result.Count == 0)
        {
            result.Add(FilePickerFileTypes.All);
        }

        return result;
    }

    public static string? TryGetDefaultExtension(IReadOnlyList<FilePickerFileType> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        foreach (var type in types)
        {
            if (IsAllFiles(type) || type.Patterns is null)
            {
                continue;
            }

            foreach (var pattern in type.Patterns)
            {
                var ext = Path.GetExtension(pattern.Replace('*', 'x'));
                if (!string.IsNullOrEmpty(ext) && ext != ".")
                {
                    return ext.TrimStart('.');
                }
            }
        }

        return null;
    }

    public static string? NormalizeExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return null;
        }

        return extension.TrimStart('.');
    }

    public static bool IsAllFiles(FilePickerFileType type) =>
        type.Patterns is { Count: > 0 }
        && type.Patterns.Any(static p => p is "*.*" or "*");
}
