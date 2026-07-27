using Microsoft.Win32;

namespace Gui.SystemDialogs.Wpf.Sharp;

internal sealed class NativeWpfDialogBackend : IWpfDialogBackend
{
    public string? ShowOpenFile(WpfOpenFileRequest request)
    {
        var dialog = CreateOpenFileDialog(request, multiselect: false);
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public IReadOnlyList<string> ShowOpenFiles(WpfOpenFileRequest request)
    {
        var dialog = CreateOpenFileDialog(request, multiselect: true);
        return dialog.ShowDialog() == true
            ? dialog.FileNames
            : [];
    }

    public string? ShowSaveFile(WpfSaveFileRequest request)
    {
        var dialog = new SaveFileDialog
        {
            Title = request.Title,
            Filter = request.Filter,
            OverwritePrompt = request.OverwritePrompt,
            AddExtension = true
        };

        if (!string.IsNullOrWhiteSpace(request.InitialDirectory))
        {
            dialog.InitialDirectory = request.InitialDirectory;
        }

        if (!string.IsNullOrWhiteSpace(request.FileName))
        {
            dialog.FileName = request.FileName;
        }

        if (!string.IsNullOrWhiteSpace(request.DefaultExt))
        {
            dialog.DefaultExt = request.DefaultExt;
        }

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? ShowSelectFolder(WpfSelectFolderRequest request)
    {
        var dialog = new OpenFolderDialog
        {
            Title = request.Title,
            Multiselect = false
        };

        if (!string.IsNullOrWhiteSpace(request.InitialDirectory))
        {
            dialog.InitialDirectory = request.InitialDirectory;
            dialog.FolderName = request.InitialDirectory;
        }

        return dialog.ShowDialog() == true ? dialog.FolderName : null;
    }

    private static OpenFileDialog CreateOpenFileDialog(
        WpfOpenFileRequest request,
        bool multiselect)
    {
        var dialog = new OpenFileDialog
        {
            Title = request.Title,
            Filter = request.Filter,
            Multiselect = multiselect,
            CheckFileExists = true
        };

        if (!string.IsNullOrWhiteSpace(request.InitialDirectory))
        {
            dialog.InitialDirectory = request.InitialDirectory;
        }

        if (!string.IsNullOrWhiteSpace(request.InitialFileName))
        {
            dialog.FileName = request.InitialFileName;
        }

        return dialog;
    }
}
