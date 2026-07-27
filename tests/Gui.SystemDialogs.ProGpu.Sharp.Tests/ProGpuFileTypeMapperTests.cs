using Gui.SystemDialogs.ProGpu.Sharp;
using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.ProGpu.Sharp.Tests;

public sealed class ProGpuFileTypeMapperTests
{
    [Fact]
    public void ToOpenExtensions_NoFilters_ReturnsWildcard()
    {
        Assert.Equal(["*"], ProGpuFileTypeMapper.ToOpenExtensions([]));
    }

    [Fact]
    public void ToOpenExtensions_NormalizesAndDedupes()
    {
        FileDialogFilter[] filters =
        [
            new()
            {
                DisplayName = "Images",
                Patterns = ["*.png", "*.PNG", "jpg"]
            }
        ];

        Assert.Equal([".png", ".jpg"], ProGpuFileTypeMapper.ToOpenExtensions(filters));
    }

    [Fact]
    public void ToSaveExtensions_MapsStarToDotStar()
    {
        var filter = new FileDialogFilter
        {
            DisplayName = "All",
            Patterns = ["*"]
        };

        Assert.Equal([".*"], ProGpuFileTypeMapper.ToSaveExtensions(filter));
    }

    [Fact]
    public void ToSaveExtensions_EmptyPatterns_ReturnsDotStar()
    {
        var filter = new FileDialogFilter
        {
            DisplayName = "Empty",
            Patterns = []
        };

        Assert.Equal([".*"], ProGpuFileTypeMapper.ToSaveExtensions(filter));
    }
}
