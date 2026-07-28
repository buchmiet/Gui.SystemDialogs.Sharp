using Gui.SystemDialogs.Consolonia.Sharp;
using Gui.SystemDialogs.TestSupport;
using FileDialogFilter = Gui.SystemDialogs.Sharp.FileDialogFilter;

namespace Gui.SystemDialogs.Consolonia.Sharp.Tests;

public sealed class ConsoloniaFilterMappingConformanceTests
{
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        ConsoloniaFilePickerMapper.ToFilePickerTypes(filters)
            .SelectMany(static type => type.Patterns ?? [])
            .ToArray();

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
