using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Windows.Sharp;

/// <summary>
/// Maps framework-neutral file filters to WinRT picker extension formats.
/// </summary>
internal static class WinRtFileTypeMapper
{
    /// <summary>
    /// Flattens filters into the extension list required by WinRT open pickers.
    /// </summary>
    public static IReadOnlyList<string> ToOpenExtensions(IReadOnlyList<FileDialogFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);

        if (filters.Count == 0)
        {
            return ["*"];
        }

        var extensions = new List<string>();
        foreach (var filter in filters)
        {
            foreach (var pattern in filter.Patterns)
            {
                var extension = NormalizeExtension(pattern);
                if (extension is not null
                    && !extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    extensions.Add(extension);
                }
            }
        }

        return extensions.Count == 0 ? ["*"] : extensions;
    }

    /// <summary>
    /// Converts a pattern such as <c>*.png</c>, <c>.png</c>, or <c>png</c>
    /// to a WinRT extension such as <c>.png</c>. Wildcards become <c>*</c>.
    /// </summary>
    public static string? NormalizeExtension(string? pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            return null;
        }

        var trimmed = pattern.Trim();
        if (trimmed is "*.*" or "*")
        {
            return "*";
        }

        if (trimmed.StartsWith("*.", StringComparison.Ordinal))
        {
            return "." + trimmed[2..];
        }

        if (trimmed.StartsWith('.'))
        {
            return trimmed;
        }

        return "." + trimmed.TrimStart('*', '.');
    }

    /// <summary>
    /// Converts one neutral filter into a distinct WinRT save-picker extension list.
    /// </summary>
    public static IReadOnlyList<string> ToSaveExtensions(
        FileDialogFilter filter,
        string allFilesFallback = ".")
    {
        ArgumentNullException.ThrowIfNull(filter);

        var extensions = filter.Patterns
            .Select(NormalizeExtension)
            .Where(static extension => extension is not null && extension != "*")
            .Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return extensions.Length == 0 ? [allFilesFallback] : extensions;
    }
}
