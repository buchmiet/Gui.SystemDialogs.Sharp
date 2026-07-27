namespace Gui.SystemDialogs.Windows.Sharp;

/// <summary>
/// Supplies the native owner-window handle for Windows pickers that require an HWND.
/// </summary>
public interface IWindowHandleProvider
{
    /// <summary>
    /// Returns the native owner-window handle.
    /// </summary>
    nint GetWindowHandle();
}
