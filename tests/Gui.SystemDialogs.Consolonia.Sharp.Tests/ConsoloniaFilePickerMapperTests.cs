using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Consolonia.Sharp;
using Gui.SystemDialogs.Sharp;
using FileDialogFilter = Gui.SystemDialogs.Sharp.FileDialogFilter;

namespace Gui.SystemDialogs.Consolonia.Sharp.Tests;

public sealed class ConsoloniaFilePickerMapperTests
{
    [Fact]
    public void ToFilePickerTypes_NoFilters_AddsAll()
    {
        var result = ConsoloniaFilePickerMapper.ToFilePickerTypes([]);
        Assert.Single(result);
        Assert.True(ConsoloniaFilePickerMapper.IsAllFiles(result[0]));
    }

    [Fact]
    public void ToFilePickerTypes_SingleFilter_DoesNotAddAll()
    {
        FileDialogFilter[] filters =
        [
            new() { DisplayName = "Themepack", Patterns = ["*.nctheme"] }
        ];

        var result = ConsoloniaFilePickerMapper.ToFilePickerTypes(filters);
        Assert.Single(result);
        Assert.Equal("Themepack", result[0].Name);
        Assert.False(ConsoloniaFilePickerMapper.IsAllFiles(result[0]));
    }

    [Fact]
    public void ToFilePickerTypes_PassesCallerFiltersThroughVerbatim()
    {
        FileDialogFilter[] filters =
        [
            new() { DisplayName = "Json", Patterns = ["*.json"] },
            new() { DisplayName = "Text", Patterns = ["*.txt", "*.log"] }
        ];

        var result = ConsoloniaFilePickerMapper.ToFilePickerTypes(filters);

        Assert.Equal(2, result.Count);
        Assert.Equal(["Json", "Text"], result.Select(static t => t.Name));
        Assert.Equal(["*.txt", "*.log"], result[1].Patterns!);
    }

    [Fact]
    public void TryGetDefaultExtension_FromJsonPattern()
    {
        var types = new[]
        {
            new FilePickerFileType("Json") { Patterns = ["*.json"] }
        };
        Assert.Equal("json", ConsoloniaFilePickerMapper.TryGetDefaultExtension(types));
    }

    [Fact]
    public void NormalizeExtension_Empty_ReturnsNull()
    {
        Assert.Null(ConsoloniaFilePickerMapper.NormalizeExtension("  "));
    }
}
