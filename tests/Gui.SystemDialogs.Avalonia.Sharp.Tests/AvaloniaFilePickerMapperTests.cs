using Avalonia.Platform.Storage;
using Gui.SystemDialogs.Avalonia.Sharp;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Avalonia.Sharp.Tests;

public sealed class AvaloniaFilePickerMapperTests
{
    [Fact]
    public void ToFilePickerTypes_NoFilters_AddsAll()
    {
        var result = AvaloniaFilePickerMapper.ToFilePickerTypes([]);
        Assert.Single(result);
        Assert.True(AvaloniaFilePickerMapper.IsAllFiles(result[0]));
    }

    [Fact]
    public void ToFilePickerTypes_SkipsEmptyPatterns()
    {
        FileDialogFilter[] filters =
        [
            new() { DisplayName = "Empty", Patterns = [] },
            new() { DisplayName = "Json", Patterns = ["*.json"] }
        ];

        var result = AvaloniaFilePickerMapper.ToFilePickerTypes(filters);
        Assert.Equal(2, result.Count); // Json + All
        Assert.Equal("Json", result[0].Name);
        Assert.True(AvaloniaFilePickerMapper.IsAllFiles(result[1]));
    }

    [Fact]
    public void ToFilePickerTypes_ExistingAll_DoesNotDuplicate()
    {
        FileDialogFilter[] filters =
        [
            new() { DisplayName = "All", Patterns = ["*.*"] }
        ];

        var result = AvaloniaFilePickerMapper.ToFilePickerTypes(filters);
        Assert.Single(result);
    }

    [Theory]
    [InlineData("*.json", "json")]
    [InlineData(".json", "json")]
    public void TryGetDefaultExtension_FromPatterns(string pattern, string expected)
    {
        var types = new[]
        {
            new FilePickerFileType("Json") { Patterns = [pattern] }
        };

        Assert.Equal(expected, AvaloniaFilePickerMapper.TryGetDefaultExtension(types));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizeExtension_Empty_ReturnsNull(string? input)
    {
        Assert.Null(AvaloniaFilePickerMapper.NormalizeExtension(input));
    }

    [Fact]
    public void NormalizeExtension_StripsLeadingDot()
    {
        Assert.Equal("json", AvaloniaFilePickerMapper.NormalizeExtension(".json"));
    }
}
