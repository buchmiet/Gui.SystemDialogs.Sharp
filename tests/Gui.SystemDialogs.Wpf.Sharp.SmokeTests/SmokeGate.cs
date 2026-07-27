using System.Runtime.CompilerServices;

namespace Gui.SystemDialogs.Wpf.Sharp.SmokeTests;

/// <summary>
/// Native dialog smoke tests are opt-in: set GUI_SYSTEMDIALOGS_SMOKE=1.
/// They require an interactive Windows desktop session.
/// </summary>
internal static class SmokeGate
{
    public const string EnvironmentVariableName = "GUI_SYSTEMDIALOGS_SMOKE";

    public static bool IsEnabled =>
        string.Equals(
            Environment.GetEnvironmentVariable(EnvironmentVariableName),
            "1",
            StringComparison.Ordinal);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class SmokeFactAttribute : FactAttribute
{
    public SmokeFactAttribute(
        [CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1)
        : base(sourceFilePath, sourceLineNumber)
    {
        if (!SmokeGate.IsEnabled)
        {
            Skip =
                $"Set {SmokeGate.EnvironmentVariableName}=1 to run native dialog smoke tests.";
        }
        else if (!Environment.UserInteractive)
        {
            Skip = "Native dialog smoke tests require an interactive Windows session.";
        }
    }
}
