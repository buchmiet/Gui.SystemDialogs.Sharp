using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.WinUI.Sharp.Tests;

public sealed class WinUiFilterMappingConformanceTests
{
    // WinRT open pickers take a flat extension list rather than named filter groups; this is the
    // mapping the WinUI service feeds into FileOpenPicker.FileTypeFilter.
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        WinRtFileTypeMapper.ToOpenExtensions(filters);

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
