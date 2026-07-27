using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Wpf.Sharp;

namespace Gui.SystemDialogs.Wpf.Sharp.Tests;

public sealed class WpfFileFilterFormatTests
{
    [Fact]
    public void Format_NoFilters_ReturnsAllFiles()
    {
        var result = WpfFileFilterFormat.Format([]);
        Assert.Equal("All Files|*.*", result);
    }

    [Fact]
    public void Format_MultipleFilters_ReturnsWpfFormat()
    {
        FileDialogFilter[] filters =
        [
            new()
            {
                DisplayName = "Images",
                Patterns = ["*.png", "*.jpg"]
            },
            new()
            {
                DisplayName = "Text",
                Patterns = ["*.txt"]
            }
        ];

        var result = WpfFileFilterFormat.Format(filters);
        Assert.Equal("Images|*.png;*.jpg|Text|*.txt", result);
    }

    [Fact]
    public void Format_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => WpfFileFilterFormat.Format(null!));
    }

    [Fact]
    public void Parse_MultipleFilters_ReturnsNeutralModels()
    {
        var result = WpfFileFilterFormat.Parse("Images|*.png;*.jpg|Text|*.txt");

        Assert.Collection(
            result,
            images =>
            {
                Assert.Equal("Images", images.DisplayName);
                Assert.Equal(["*.png", "*.jpg"], images.Patterns);
            },
            text =>
            {
                Assert.Equal("Text", text.DisplayName);
                Assert.Equal(["*.txt"], text.Patterns);
            });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_EmptyInput_ReturnsEmptyCollection(string? input)
    {
        Assert.Empty(WpfFileFilterFormat.Parse(input));
    }
}
