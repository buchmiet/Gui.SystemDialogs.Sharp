using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Wpf.Sharp;
using IOPath = System.IO.Path;

namespace Gui.SystemDialogs.Wpf.Sharp.SmokeTests;

[Trait("Category", "Smoke")]
public sealed class WpfFilePickerSmokeTests
{
    private readonly IFilePickerService _files = new WpfFilePickerService();
    private readonly IFolderPickerService _folders = new WpfFolderPickerService();

    [SmokeFact]
    public void OpenFile_Cancel_ReturnsNull()
    {
        var title = UniqueTitle("OpenCancel");

        var result = CommonFileDialogAutomation.RunWithDialogAutomation(
            title,
            () => _files.OpenFileAsync(new OpenFileDialogOptions { Title = title })
                .GetAwaiter()
                .GetResult(),
            CommonFileDialogAutomation.Cancel);

        Assert.Null(result);
    }

    [SmokeFact]
    public void OpenFile_SelectTempFile_ReturnsPath()
    {
        var title = UniqueTitle("OpenSelect");
        var tempFile = CreateTempFile("smoke-open-", ".txt");

        try
        {
            var result = CommonFileDialogAutomation.RunWithDialogAutomation(
                title,
                () => _files.OpenFileAsync(
                        new OpenFileDialogOptions
                        {
                            Title = title,
                            InitialDirectory = IOPath.GetDirectoryName(tempFile),
                            InitialFileName = IOPath.GetFileName(tempFile)
                        })
                    .GetAwaiter()
                    .GetResult(),
                dialog =>
                {
                    CommonFileDialogAutomation.SetFileName(dialog, tempFile);
                    CommonFileDialogAutomation.ConfirmOpenOrSave(dialog);
                });

            Assert.Equal(tempFile, result);
        }
        finally
        {
            TryDelete(tempFile);
        }
    }

    [SmokeFact]
    public void OpenFiles_Cancel_ReturnsEmptyCollection()
    {
        var title = UniqueTitle("OpenManyCancel");

        var result = CommonFileDialogAutomation.RunWithDialogAutomation(
            title,
            () => _files.OpenFilesAsync(new OpenFilesDialogOptions { Title = title })
                .GetAwaiter()
                .GetResult(),
            CommonFileDialogAutomation.Cancel);

        Assert.Empty(result);
    }

    [SmokeFact]
    public void OpenFiles_SelectTempFile_ReturnsSinglePath()
    {
        var title = UniqueTitle("OpenManySelect");
        var tempFile = CreateTempFile("smoke-open-many-", ".txt");

        try
        {
            var result = CommonFileDialogAutomation.RunWithDialogAutomation(
                title,
                () => _files.OpenFilesAsync(
                        new OpenFilesDialogOptions
                        {
                            Title = title,
                            InitialDirectory = IOPath.GetDirectoryName(tempFile)
                        })
                    .GetAwaiter()
                    .GetResult(),
                dialog =>
                {
                    CommonFileDialogAutomation.SetFileName(dialog, tempFile);
                    CommonFileDialogAutomation.ConfirmOpenOrSave(dialog);
                });

            Assert.Equal([tempFile], result);
        }
        finally
        {
            TryDelete(tempFile);
        }
    }

    [SmokeFact]
    public void SaveFile_Cancel_ReturnsNull()
    {
        var title = UniqueTitle("SaveCancel");

        var result = CommonFileDialogAutomation.RunWithDialogAutomation(
            title,
            () => _files.SaveFileAsync(new SaveFileDialogOptions { Title = title })
                .GetAwaiter()
                .GetResult(),
            CommonFileDialogAutomation.Cancel);

        Assert.Null(result);
    }

    [SmokeFact]
    public void SaveFile_SelectTempPath_ReturnsPath()
    {
        var title = UniqueTitle("SaveSelect");
        var directory = IOPath.Combine(IOPath.GetTempPath(), "Gui.SystemDialogs.Smoke");
        System.IO.Directory.CreateDirectory(directory);
        var target = IOPath.Combine(directory, $"smoke-save-{Guid.NewGuid():N}.txt");

        try
        {
            var result = CommonFileDialogAutomation.RunWithDialogAutomation(
                title,
                () => _files.SaveFileAsync(
                        new SaveFileDialogOptions
                        {
                            Title = title,
                            InitialDirectory = directory,
                            SuggestedFileName = IOPath.GetFileName(target),
                            DefaultExtension = "txt",
                            ConfirmOverwrite = false
                        })
                    .GetAwaiter()
                    .GetResult(),
                dialog =>
                {
                    CommonFileDialogAutomation.SetFileName(dialog, target);
                    CommonFileDialogAutomation.ConfirmOpenOrSave(dialog);
                });

            Assert.Equal(target, result);
        }
        finally
        {
            TryDelete(target);
        }
    }

    [SmokeFact]
    public void SelectFolder_Cancel_ReturnsNull()
    {
        var title = UniqueTitle("FolderCancel");

        var result = CommonFileDialogAutomation.RunWithDialogAutomation(
            title,
            () => _folders.SelectFolderAsync(new SelectFolderDialogOptions { Title = title })
                .GetAwaiter()
                .GetResult(),
            CommonFileDialogAutomation.Cancel);

        Assert.Null(result);
    }

    private static string UniqueTitle(string prefix) =>
        $"GSD Smoke {prefix} {Guid.NewGuid():N}";

    private static string CreateTempFile(string prefix, string extension)
    {
        var path = IOPath.Combine(
            IOPath.GetTempPath(),
            prefix + Guid.NewGuid().ToString("N") + extension);
        System.IO.File.WriteAllText(path, "smoke");
        return path;
    }

    private static void TryDelete(string path)
    {
        try
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        catch
        {
            // best-effort cleanup
        }
    }
}
