using Gui.SystemDialogs.ProGpu.Sharp;
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.TestSupport;

namespace Gui.SystemDialogs.ProGpu.Sharp.Tests;

public sealed class ProGpuFilterMappingConformanceTests
{
    private static IReadOnlyList<string> Probe(IReadOnlyList<FileDialogFilter> filters) =>
        ProGpuFileTypeMapper.ToOpenExtensions(filters);

    [Fact] public void NoFilters_AcceptAnyFile() => FilterMappingConformance.NoFilters_AcceptAnyFile(Probe);
    [Fact] public void CallerFilters_DoNotWidenToAnyFile() => FilterMappingConformance.CallerFilters_DoNotWidenToAnyFile(Probe);
    [Fact] public void CallerFilters_ExposeEveryRequestedExtension() => FilterMappingConformance.CallerFilters_ExposeEveryRequestedExtension(Probe);
    [Fact] public void ExplicitWildcard_IsPreserved() => FilterMappingConformance.ExplicitWildcard_IsPreserved(Probe);
    [Fact] public void FiltersWithoutPatterns_AcceptAnyFile() => FilterMappingConformance.FiltersWithoutPatterns_AcceptAnyFile(Probe);
}
