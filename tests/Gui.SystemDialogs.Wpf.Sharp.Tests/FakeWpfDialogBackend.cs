namespace Gui.SystemDialogs.Wpf.Sharp.Tests;

internal sealed class FakeWpfDialogBackend : IWpfDialogBackend
{
    public WpfOpenFileRequest? LastOpenFileRequest { get; private set; }
    public WpfOpenFileRequest? LastOpenFilesRequest { get; private set; }
    public WpfSaveFileRequest? LastSaveFileRequest { get; private set; }
    public WpfSelectFolderRequest? LastSelectFolderRequest { get; private set; }

    public string? OpenFileResult { get; init; }
    public IReadOnlyList<string> OpenFilesResult { get; init; } = [];
    public string? SaveFileResult { get; init; }
    public string? SelectFolderResult { get; init; }

    public string? ShowOpenFile(WpfOpenFileRequest request)
    {
        LastOpenFileRequest = request;
        return OpenFileResult;
    }

    public IReadOnlyList<string> ShowOpenFiles(WpfOpenFileRequest request)
    {
        LastOpenFilesRequest = request;
        return OpenFilesResult;
    }

    public string? ShowSaveFile(WpfSaveFileRequest request)
    {
        LastSaveFileRequest = request;
        return SaveFileResult;
    }

    public string? ShowSelectFolder(WpfSelectFolderRequest request)
    {
        LastSelectFolderRequest = request;
        return SelectFolderResult;
    }
}
