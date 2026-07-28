using Gui.SystemDialogs.Sharp;
using Xunit;

namespace Gui.SystemDialogs.TestSupport;

/// <summary>
/// Cross-adapter conformance for neutral filter mapping.
/// <para>
/// Every adapter targets a different native shape — Avalonia <c>FilePickerFileType</c>, a
/// pipe-delimited WPF filter string, a flat WinRT extension list, a ProGPU extension list. The
/// assertions below are therefore expressed in the only terms a caller of the neutral contract
/// cares about: which file types the dialog ends up accepting. Each adapter supplies a probe, and
/// every guarantee must hold for all of them, so a behavioural divergence between GUI stacks fails
/// in CI instead of surfacing in a consumer.
/// </para>
/// <para>
/// The adapters are deliberately independent implementations (see <c>README.md</c>); this suite
/// constrains their observable behaviour, not their code.
/// </para>
/// </summary>
public static class FilterMappingConformance
{
    /// <summary>
    /// Maps neutral filters through one adapter and returns the raw pattern or extension tokens
    /// handed to the underlying dialog — for example <c>*.txt</c>, <c>.txt</c>, or <c>*</c>.
    /// </summary>
    public delegate IReadOnlyList<string> FilterProbe(IReadOnlyList<FileDialogFilter> filters);

    /// <summary>
    /// No filters means no constraint, so the dialog must accept any file.
    /// </summary>
    public static void NoFilters_AcceptAnyFile(FilterProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);

        var tokens = probe([]);

        Assert.True(
            AcceptsAnyFile(tokens),
            $"An empty filter list must leave the dialog unconstrained, but it produced: {Describe(tokens)}");
    }

    /// <summary>
    /// The caller asked for a specific type, so the adapter must not widen the dialog by adding an
    /// "all files" escape hatch of its own. A caller that wants one adds it explicitly; a caller
    /// that does not has no way to remove an injected entry through the neutral contract.
    /// </summary>
    public static void CallerFilters_DoNotWidenToAnyFile(FilterProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);

        var tokens = probe([Filter("Documents", "*.json")]);

        Assert.False(
            AcceptsAnyFile(tokens),
            $"A caller-supplied filter must stay a hard filter, but the adapter widened it to any file: {Describe(tokens)}");
    }

    /// <summary>
    /// Every pattern the caller supplied has to reach the dialog, across all filters.
    /// </summary>
    public static void CallerFilters_ExposeEveryRequestedExtension(FilterProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);

        var tokens = probe([Filter("Documents", "*.json"), Filter("Logs", "*.txt", "*.log")]);
        var extensions = ToExtensions(tokens);

        foreach (var expected in (string[])["json", "txt", "log"])
        {
            Assert.True(
                extensions.Contains(expected),
                $"Extension '{expected}' was requested by the caller but never reached the dialog: {Describe(tokens)}");
        }
    }

    /// <summary>
    /// A caller that explicitly opts into an "all files" entry must get one.
    /// </summary>
    public static void ExplicitWildcard_IsPreserved(FilterProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);

        var tokens = probe([Filter("All files", "*.*")]);

        Assert.True(
            AcceptsAnyFile(tokens),
            $"An explicit wildcard filter must survive mapping, but it produced: {Describe(tokens)}");
    }

    /// <summary>
    /// Filters that carry no usable pattern express no constraint, so they degrade to "any file"
    /// rather than to a dialog that accepts nothing.
    /// </summary>
    public static void FiltersWithoutPatterns_AcceptAnyFile(FilterProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);

        var tokens = probe([new FileDialogFilter { DisplayName = "Empty", Patterns = [] }]);

        Assert.True(
            AcceptsAnyFile(tokens),
            $"A filter list with no usable pattern must leave the dialog unconstrained, but it produced: {Describe(tokens)}");
    }

    private static FileDialogFilter Filter(string displayName, params string[] patterns) =>
        new() { DisplayName = displayName, Patterns = patterns };

    private static bool AcceptsAnyFile(IReadOnlyList<string> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        return tokens.Count == 0 || tokens.Any(IsWildcard);
    }

    /// <summary>
    /// Wildcards reach the adapters in several spellings: <c>*</c> and <c>*.*</c> on the open path,
    /// <c>.</c> (WinRT) and <c>.*</c> (ProGPU) as save-path fallbacks.
    /// </summary>
    private static bool IsWildcard(string? token) =>
        token?.Trim() is "*" or "*.*" or "." or ".*" or "";

    private static IReadOnlySet<string> ToExtensions(IReadOnlyList<string> tokens) =>
        tokens
            .Where(static token => !IsWildcard(token))
            .Select(static token => token.Trim().TrimStart('*').TrimStart('.').ToLowerInvariant())
            .Where(static extension => extension.Length > 0)
            .ToHashSet(StringComparer.Ordinal);

    private static string Describe(IReadOnlyList<string> tokens) =>
        tokens.Count == 0 ? "<no tokens>" : "[" + string.Join(", ", tokens) + "]";
}
