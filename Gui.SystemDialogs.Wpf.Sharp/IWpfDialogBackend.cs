namespace Gui.SystemDialogs.Wpf.Sharp;

internal sealed record WpfOpenFileRequest(
    string Title,
    string Filter,
    string? InitialDirectory,
    string? InitialFileName,
    bool Multiselect);

internal sealed record WpfSaveFileRequest(
    string Title,
    string Filter,
    string? InitialDirectory,
    string? FileName,
    string? DefaultExt,
    bool OverwritePrompt);

internal sealed record WpfSelectFolderRequest(
    string Title,
    string? InitialDirectory);

internal interface IWpfDialogBackend
{
    string? ShowOpenFile(WpfOpenFileRequest request);

    IReadOnlyList<string> ShowOpenFiles(WpfOpenFileRequest request);

    string? ShowSaveFile(WpfSaveFileRequest request);

    string? ShowSelectFolder(WpfSelectFolderRequest request);
}
