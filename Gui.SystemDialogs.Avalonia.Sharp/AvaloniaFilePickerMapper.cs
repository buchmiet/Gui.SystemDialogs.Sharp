using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Avalonia.Sharp;

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

        if (result.Count == 0 || result.All(static t => !IsAllFiles(t)))
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
