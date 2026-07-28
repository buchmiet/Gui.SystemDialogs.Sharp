using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;
using Gui.SystemDialogs.Wpf.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp.Tests;

public sealed class WpfFilterMappingConformanceTests
{
    // WPF carries filters as a pipe-delimited string, so the probe round-trips through the format
    // the dialog actually receives.
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        WpfFileFilterFormat.Parse(WpfFileFilterFormat.Format(filters))
            .SelectMany(static filter => filter.Patterns)
            .ToArray();

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
