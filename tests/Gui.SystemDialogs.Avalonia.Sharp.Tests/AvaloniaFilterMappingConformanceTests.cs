using Gui.SystemDialogs.Avalonia.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.Avalonia.Sharp.Tests;

public sealed class AvaloniaFilterMappingConformanceTests
{
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        AvaloniaFilePickerMapper.ToFilePickerTypes(filters)
            .SelectMany(static type => type.Patterns ?? [])
            .ToArray();

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
