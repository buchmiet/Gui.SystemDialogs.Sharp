# Gui.SystemDialogs.Sharp

Framework-neutral system dialog contracts with separate GUI-framework adapters.

Install only the adapter package for your UI stack. Each adapter depends on the core contracts package automatically.

## NuGet packages

| Package | UI stack | Install |
|---------|----------|---------|
| [Gui.SystemDialogs.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Sharp) | Contracts only | `dotnet add package Gui.SystemDialogs.Sharp` |
| [Gui.SystemDialogs.Wpf.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Wpf.Sharp) | WPF | `dotnet add package Gui.SystemDialogs.Wpf.Sharp` |
| [Gui.SystemDialogs.WinUI.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.WinUI.Sharp) | WinUI 3 | `dotnet add package Gui.SystemDialogs.WinUI.Sharp` |
| [Gui.SystemDialogs.Maui.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Maui.Sharp) | .NET MAUI (Windows) | `dotnet add package Gui.SystemDialogs.Maui.Sharp` |
| [Gui.SystemDialogs.Avalonia.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Avalonia.Sharp) | Avalonia 12 | `dotnet add package Gui.SystemDialogs.Avalonia.Sharp` |
| [Gui.SystemDialogs.Consolonia.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Consolonia.Sharp) | Consolonia / Avalonia 11 | `dotnet add package Gui.SystemDialogs.Consolonia.Sharp` |
| [Gui.SystemDialogs.ProGpu.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.ProGpu.Sharp) | ProGPU | `dotnet add package Gui.SystemDialogs.ProGpu.Sharp` |
| [Gui.SystemDialogs.Windows.Sharp](https://www.nuget.org/packages/Gui.SystemDialogs.Windows.Sharp) | Shared Windows helpers | Usually pulled in transitively by WinUI / MAUI |

> **Versioning:** pre-release packages are published as `0.0.x`. The first public stable release is planned as `0.1.0`.

## Quick start

```csharp
using Gui.SystemDialogs.Sharp;
using Gui.SystemDialogs.Wpf.Sharp;

IFilePickerService filePicker = new WpfFilePickerService();

var path = await filePicker.OpenFileAsync(new OpenFileDialogOptions
{
    Title = "Open document",
    Filters =
    [
        new FileDialogFilter("Text files", ".txt"),
        new FileDialogFilter("All files", "*.*"),
    ],
});
```

Avalonia resolves the owner window internally or via a `Func<TopLevel?>` passed to the constructor:

```csharp
using Gui.SystemDialogs.Avalonia.Sharp;

IFilePickerService filePicker = new AvaloniaFilePickerService();
```

## Contracts (`Gui.SystemDialogs.Sharp`)

- `IFilePickerService` — open / open-many / save (local paths only)
- `IFolderPickerService` — select folder
- `IApplicationExitService` — request application/process exit
- Option/filter models (`FileDialogFilter`, `OpenFileDialogOptions`, …)

The contracts assembly must not reference GUI, Windows, WinRT, HWND, or framework types.

## Adapters

| Project | Role |
|---------|------|
| `Gui.SystemDialogs.Windows.Sharp` | HWND ownership + WinRT filter mapping |
| `Gui.SystemDialogs.Wpf.Sharp` | WPF pickers + exit + `WpfFileFilterFormat` |
| `Gui.SystemDialogs.WinUI.Sharp` | Windows App SDK pickers + exit |
| `Gui.SystemDialogs.Maui.Sharp` | Windows-targeted MAUI pickers + exit |
| `Gui.SystemDialogs.Avalonia.Sharp` | Avalonia 12 storage pickers + exit |
| `Gui.SystemDialogs.Consolonia.Sharp` | Consolonia / Avalonia 11 pickers + exit |
| `Gui.SystemDialogs.ProGpu.Sharp` | ProGPU pickers + exit |

## Dependency direction

```text
Gui.SystemDialogs.Sharp
    ↑
    ├── Gui.SystemDialogs.Avalonia.Sharp
    ├── Gui.SystemDialogs.Consolonia.Sharp
    ├── Gui.SystemDialogs.ProGpu.Sharp
    ├── Gui.SystemDialogs.Wpf.Sharp
    └── Gui.SystemDialogs.Windows.Sharp
            ↑
            ├── Gui.SystemDialogs.WinUI.Sharp
            └── Gui.SystemDialogs.Maui.Sharp
```

## Development

Requirements: .NET 10 SDK, Python 3 (for the boundary check).

```text
python eng/verify-contract-boundary.py
dotnet test Gui.SystemDialogs.Sharp.slnx -c Release --filter Category!=Smoke
```

### Local package build

```text
dotnet pack Gui.SystemDialogs.Sharp.slnx -c Release -o artifacts/packages
```

The default local version is `0.0.1` (see `Directory.Build.props`). Override for a dry run:

```text
dotnet pack Gui.SystemDialogs.Sharp.slnx -c Release -o artifacts/packages -p:Version=0.0.2
```

### Native WPF smoke tests (manual / nightly)

FlaUI-driven common dialogs in `tests/Gui.SystemDialogs.Wpf.Sharp.SmokeTests`. Opt-in via env var (skipped otherwise):

```text
set GUI_SYSTEMDIALOGS_SMOKE=1
dotnet test tests/Gui.SystemDialogs.Wpf.Sharp.SmokeTests -c Release
```

## Publishing

CI runs on every push and pull request to `main`. Packages are published to NuGet.org when a version tag is pushed:

```text
git tag v0.0.2
git push origin v0.0.2
```

Set the `NUGET_API_KEY` secret in the GitHub `nuget` environment before the first publish.

## License

MIT — see [LICENSE](LICENSE).
