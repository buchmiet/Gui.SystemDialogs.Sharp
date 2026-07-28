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

> **Versioning:** `0.1.0` is the first public release. Earlier `0.0.x` builds were pre-release validation publishes.

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
        new FileDialogFilter { DisplayName = "Text files", Patterns = ["*.txt"] },
        new FileDialogFilter { DisplayName = "All files", Patterns = ["*.*"] },
    ],
});
```

`FileDialogFilter` is a record with `required` init-only members, so both `DisplayName` and
`Patterns` must be supplied through an object initializer. Patterns are globs (`*.txt`) and are
handed to the underlying dialog unchanged — a bare extension such as `.txt` will not match.

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

### Adapters are deliberately independent

`Gui.SystemDialogs.Avalonia.Sharp` and `Gui.SystemDialogs.Consolonia.Sharp` contain almost identical
code, because Consolonia is an Avalonia backend and both therefore map onto `IStorageProvider`. **This
duplication is intentional — do not merge the projects or share source files between them.** They track
different Avalonia majors (Avalonia 12.x vs Avalonia 11.x by way of Consolonia) and must be free to
diverge without coordination: a shared file would have to compile against both API versions at once the
moment either side moves.

The same rule applies to every adapter pair. Adapters are independent implementations; what keeps them
honest is the conformance suite below, not shared code.

### Cross-adapter conformance

`FilterMappingConformance` in `tests/Gui.SystemDialogs.TestSupport` states the guarantees that every
adapter owes a caller of the neutral contract, expressed in terms of which file types the dialog ends up
accepting:

- an empty filter list leaves the dialog unconstrained;
- caller-supplied filters are never widened with an "all files" entry the caller did not ask for;
- every pattern the caller supplied reaches the dialog;
- an explicit wildcard filter survives mapping;
- filters carrying no usable pattern degrade to "any file".

Each adapter test project supplies a probe and runs the whole set. Add a new adapter, add its probe.

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

There is no `<Version>` property anywhere in the repository. [MinVer](https://github.com/adamralph/minver)
derives the version from the nearest reachable git tag (tags are prefixed with `v`), so the tag is the
single source of truth and the repository can never disagree with what was published.

- on a tagged commit — `v0.1.1` produces `0.1.1`;
- on any later commit — a prerelease such as `0.1.2-alpha.0.3`.

A prerelease from a local pack is therefore expected, not a misconfiguration. `AssemblyVersion` stays
pinned at `0.0.0.0` to keep assembly identity stable; `FileVersion` and `AssemblyInformationalVersion`
follow the tag, and the informational version carries the commit hash.

Override the derived version for a dry run:

```text
dotnet pack Gui.SystemDialogs.Sharp.slnx -c Release -o artifacts/packages -p:MinVerVersionOverride=0.1.2-dev
```

> Anything that computes the version needs the full history: `actions/checkout` must run with
> `fetch-depth: 0`, otherwise MinVer silently reports `0.0.0-alpha.0`.

### ProGPU ships as a prerelease

All packages share one version, with one exception: `Gui.SystemDialogs.ProGpu.Sharp` always carries a
`-preview` suffix. Tag `v0.1.2` publishes seven packages as `0.1.2` and that one as `0.1.2-preview`.

`ProGPU.WinUI` has no stable release yet, and an adapter cannot honestly be more stable than what it
binds to — this is exactly what `NU5104` reports, so the package is marked prerelease rather than the
warning being suppressed. Releases stay in lockstep: a ProGPU dependency bump is published as an
ordinary repository release, which re-publishes the other packages unchanged at the new version.

To retire the exception once `ProGPU.WinUI` is stable, delete the `ProGpuAlwaysPreRelease` target from
`Gui.SystemDialogs.ProGpu.Sharp.csproj`; nothing else changes.

### Native WPF smoke tests (manual / nightly)

FlaUI-driven common dialogs in `tests/Gui.SystemDialogs.Wpf.Sharp.SmokeTests`. Opt-in via env var (skipped otherwise):

```text
set GUI_SYSTEMDIALOGS_SMOKE=1
dotnet test tests/Gui.SystemDialogs.Wpf.Sharp.SmokeTests -c Release
```

## Publishing

CI runs on every push and pull request to `main`. Packages are published to NuGet.org when a version tag is pushed:

```text
git tag v0.1.1
git push origin v0.1.1
```

The tag alone determines the published version. Before packing, the workflow requires a matching
`## [0.1.1]` heading in [CHANGELOG.md](CHANGELOG.md), runs the contract-boundary check and the full test
suite, and then asserts that the produced `.nupkg` really carries the tagged version. Write the release
notes first, or the publish fails.

Set the `NUGET_API_KEY` secret in the GitHub `nuget` environment before the first publish.

## License

MIT — see [LICENSE](LICENSE).
