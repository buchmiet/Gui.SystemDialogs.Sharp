using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.Maui.Sharp.Tests;

public sealed class MauiFilterMappingConformanceTests
{
    // The Windows-targeted MAUI adapter shares the WinRT extension mapping with WinUI today. The
    // suite is duplicated deliberately: if MAUI ever grows its own mapping, the guarantees move
    // with it instead of silently dropping out of coverage.
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        WinRtFileTypeMapper.ToOpenExtensions(filters);

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
