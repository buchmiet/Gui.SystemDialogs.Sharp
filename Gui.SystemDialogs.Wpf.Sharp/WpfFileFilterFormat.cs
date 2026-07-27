using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp;

/// <summary>
/// Converts between framework-neutral filters and the legacy WPF/WinForms
/// filter string format: <c>Name|*.ext;*.ext2|Name2|*.foo</c>.
/// </summary>
public static class WpfFileFilterFormat
{
    /// <summary>
    /// Parses a WPF-style filter string into framework-neutral filters.
    /// </summary>
    public static IReadOnlyList<FileDialogFilter> Parse(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return [];
        }

        var result = new List<FileDialogFilter>();
        var parts = filter.Split('|', StringSplitOptions.TrimEntries);

        for (var i = 0; i + 1 < parts.Length; i += 2)
        {
            var patterns = parts[i + 1]
                .Split([';', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(static pattern => !string.IsNullOrWhiteSpace(pattern))
                .ToArray();

            if (patterns.Length == 0)
            {
                continue;
            }

            result.Add(new FileDialogFilter
            {
                DisplayName = parts[i],
                Patterns = patterns
            });
        }

        return result;
    }

    /// <summary>
    /// Formats framework-neutral filters as a WPF/WinForms filter string.
    /// </summary>
    public static string Format(IReadOnlyList<FileDialogFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);

        if (filters.Count == 0)
        {
            return "All Files|*.*";
        }

        var segments = new List<string>(filters.Count * 2);
        foreach (var filter in filters)
        {
            if (filter.Patterns.Count == 0)
            {
                continue;
            }

            segments.Add(filter.DisplayName);
            segments.Add(string.Join(';', filter.Patterns));
        }

        return segments.Count == 0
            ? "All Files|*.*"
            : string.Join('|', segments);
    }
}
