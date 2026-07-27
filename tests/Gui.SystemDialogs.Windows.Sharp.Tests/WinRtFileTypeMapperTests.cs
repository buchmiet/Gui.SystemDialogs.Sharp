using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Windows.Sharp;

namespace Gui.SystemDialogs.Windows.Sharp.Tests;

public sealed class WinRtFileTypeMapperTests
{
    [Theory]
    [InlineData("*.png", ".png")]
    [InlineData(".png", ".png")]
    [InlineData("png", ".png")]
    [InlineData("*.*", "*")]
    [InlineData("*", "*")]
    [InlineData("  *.jpg  ", ".jpg")]
    public void NormalizeExtension_ReturnsExpectedValue(string input, string expected)
    {
        Assert.Equal(expected, WinRtFileTypeMapper.NormalizeExtension(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizeExtension_Empty_ReturnsNull(string? input)
    {
        Assert.Null(WinRtFileTypeMapper.NormalizeExtension(input));
    }

    [Fact]
    public void ToOpenExtensions_RemovesDuplicatesIgnoringCase()
    {
        FileDialogFilter[] filters =
        [
            new()
            {
                DisplayName = "Images",
                Patterns = ["*.png", "*.PNG", "*.jpg"]
            }
        ];

        var result = WinRtFileTypeMapper.ToOpenExtensions(filters);
        Assert.Equal([".png", ".jpg"], result);
    }

    [Fact]
    public void ToOpenExtensions_NoFilters_ReturnsWildcard()
    {
        Assert.Equal(["*"], WinRtFileTypeMapper.ToOpenExtensions([]));
    }

    [Fact]
    public void ToSaveExtensions_SkipsWildcards_UsesFallbackWhenEmpty()
    {
        var filter = new FileDialogFilter
        {
            DisplayName = "All",
            Patterns = ["*.*"]
        };

        Assert.Equal(["."], WinRtFileTypeMapper.ToSaveExtensions(filter));
    }
}
