using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.ProGpu.Sharp;

internal static class ProGpuFileTypeMapper
{
    public static IReadOnlyList<string> ToOpenExtensions(IReadOnlyList<FileDialogFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);

        if (filters.Count == 0)
        {
            return ["*"];
        }

        var extensions = filters
            .SelectMany(static filter => filter.Patterns)
            .Select(NormalizeExtension)
            .Where(static extension => extension is not null)
            .Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return extensions.Length == 0 ? ["*"] : extensions;
    }

    public static IReadOnlyList<string> ToSaveExtensions(FileDialogFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var extensions = filter.Patterns
            .Select(NormalizeExtension)
            .Where(static extension => extension is not null)
            .Cast<string>()
            .Select(static extension => extension == "*" ? ".*" : extension)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return extensions.Length == 0 ? [".*"] : extensions;
    }

    private static string? NormalizeExtension(string? pattern)
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
}
