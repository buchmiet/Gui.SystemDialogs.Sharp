# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.1] - 2026-07-28

### Changed

- `Gui.SystemDialogs.ProGpu.Sharp` now depends on `ProGPU.WinUI` `0.1.0-preview.28` (was `0.1.0-preview.25`)
  and is itself published as a prerelease from now on: a release tag `vX.Y.Z` produces `X.Y.Z-preview` for
  this package while the other seven get `X.Y.Z`. `ProGPU.WinUI` has no stable release, and an adapter
  cannot be more stable than what it binds to, so `NU5104` is resolved at its cause instead of suppressed.
  Releases remain in lockstep; the exception is retired by deleting one target from the project file once
  `ProGPU.WinUI` stabilises.
- The package version is now derived from the nearest git tag by [MinVer](https://github.com/adamralph/minver)
  and the `<Version>` property has been removed. Previously the version lived in two places — the property
  and the tag that `publish.yml` passed as `-p:Version=` — with nothing checking that they agreed, so a
  mistyped tag published a version no one intended, irreversibly. There is now one source of truth.
  `AssemblyVersion` stays pinned at `0.0.0.0`; `FileVersion` and `AssemblyInformationalVersion` follow the
  tag, and the informational version carries the commit hash.
- `publish.yml` now requires a matching CHANGELOG heading, builds and runs the full test suite before
  packing, asserts that the packed version matches the tag, and no longer passes `--skip-duplicate` when
  pushing packages, so republishing an existing version fails loudly instead of succeeding silently. Both
  workflows check out with `fetch-depth: 0`, which MinVer requires.

### Added

- `FilterMappingConformance` in `Gui.SystemDialogs.TestSupport`: a cross-adapter conformance suite stating
  the filter-mapping guarantees every adapter owes a caller of the neutral contract, expressed in terms of
  which file types the dialog ends up accepting rather than in any one framework's native shape. All six
  picker adapters supply a probe and run the full set. The suite was verified to fail on the pre-fix
  behaviour described below.
- README now records that the Avalonia and Consolonia adapters are deliberately independent despite their
  near-identical code, because they track different Avalonia majors and must be free to diverge.
- XML documentation for every public member of the option records, including the semantics of an empty
  `Filters` list and the non-obvious `ConfirmOverwrite = true` default. `CS1591` is no longer suppressed for
  the contracts assembly, so an undocumented public member on the specification now fails the build;
  adapters keep the suppression because they implement already-documented interfaces.

### Fixed

- The Avalonia and Consolonia adapters appended an "all files" entry to every dialog whose caller-supplied
  filters did not already contain one, while the WPF, WinUI, MAUI, and ProGPU adapters passed caller filters
  through unchanged. The same neutral `Filters` value therefore produced a soft filter on two stacks and a
  hard filter on four. Both adapters now pass caller filters through verbatim; the "all files" fallback is
  applied only when `Filters` is empty, i.e. when the caller expressed no constraint at all. A caller that
  wants the escape hatch adds an explicit `*.*` filter — a caller that wants a hard filter previously had no
  way to get one.
- README quick start used a `FileDialogFilter(string, string)` constructor that does not exist; the type is a
  record with `required` init-only members. The sample now uses an object initializer, matching the test suite.
- README quick start used a bare extension (`".txt"`) as a filter pattern. Patterns are globs (`"*.txt"`) and are
  passed to the underlying dialog unchanged, so the documented form matched nothing.

## [0.1.0] - 2026-07-27

### Added

- First public NuGet release of all eight packages (`Gui.SystemDialogs.Sharp` contracts plus WPF, WinUI, MAUI, Avalonia, Consolonia, ProGPU, and Windows adapters).
- GitHub Actions CI and NuGet publish workflows.

## [0.0.2] - 2026-07-27

### Added

- Successful end-to-end NuGet publish validation for all adapter packages.

## [0.0.1] - 2026-07-27

### Added

- Framework-neutral contracts in `Gui.SystemDialogs.Sharp`.
- Adapters for WPF, WinUI, MAUI (Windows), Avalonia 12, Consolonia, ProGPU, and shared Windows helpers.
- xUnit v3 test suite and contract-boundary verification script.

[Unreleased]: https://github.com/buchmiet/Gui.SystemDialogs.Sharp/compare/v0.1.1...HEAD
[0.1.1]: https://github.com/buchmiet/Gui.SystemDialogs.Sharp/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/buchmiet/Gui.SystemDialogs.Sharp/releases/tag/v0.1.0
[0.0.2]: https://github.com/buchmiet/Gui.SystemDialogs.Sharp/compare/v0.0.1...v0.0.2
[0.0.1]: https://github.com/buchmiet/Gui.SystemDialogs.Sharp/releases/tag/v0.0.1
