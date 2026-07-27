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
