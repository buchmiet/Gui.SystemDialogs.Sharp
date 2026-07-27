using Gui.SystemDialogs.Sharp;

namespace Gui.SystemDialogs.Sharp.Tests;

public sealed class OptionsDefaultsTests
{
    [Fact]
    public void OpenFileDialogOptions_Defaults()
    {
        var options = new OpenFileDialogOptions();
        Assert.Null(options.Title);
        Assert.Empty(options.Filters);
        Assert.Null(options.InitialDirectory);
        Assert.Null(options.InitialFileName);
    }

    [Fact]
    public void SaveFileDialogOptions_ConfirmOverwriteDefaultsTrue()
    {
        var options = new SaveFileDialogOptions();
        Assert.True(options.ConfirmOverwrite);
        Assert.Empty(options.Filters);
    }

    [Fact]
    public void SelectFolderDialogOptions_Defaults()
    {
        var options = new SelectFolderDialogOptions();
        Assert.Null(options.Title);
        Assert.Null(options.InitialDirectory);
    }
}
