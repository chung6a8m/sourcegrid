# Codebase map

This document maps repository paths to responsibilities. Use it to locate the smallest relevant area before editing.

## Root build files

| Path | Responsibility |
| --- | --- |
| `SourceGrid_All.sln` | Full solution containing the six repository projects. |
| `Directory.Build.props` | Shared build defaults, platform configuration, assembly-info policy, warnings-as-errors. |
| `Directory.Packages.props` | Central NuGet package versions. |
| `global.json` | .NET SDK selection and roll-forward policy. |
| `.editorconfig` | Editor formatting/encoding policy. |
| `.gitattributes` | Git text/binary and line-ending policy. |
| `NuGet.config` | NuGet source configuration. |

## Main library: `SourceGrid/`

`SourceGrid/SourceGrid.csproj` is the primary distributable project and targets `net48;net8.0-windows`.

### `SourceGrid/SourceGrid/`

- `Grids/` — `Grid`, `GridVirtual`, rows, columns, layout, grid-level behavior, and accessibility-related partials.
- `Cells/` — real/virtual cell types and cell composition subsystems.
  - `Editors/` — editor base classes/factories and WinForms editing controls.
  - `Controllers/` — cell-level interaction controllers.
  - `Models/` — value/display/tooltip/image and related model behavior.
  - `Views/` — rendering and visual-property classes.
- `Selection/` — selected regions, active positions, and selection logic.
- `Controllers/` — grid-level controllers and interaction infrastructure.
- `Decorators/` — visual adornment/decorator behavior.
- `Exporter/` — printing and export support.
- `QuadTree/` — spatial/range indexing.
- `Common/` — shared control/grid infrastructure.
- `Utils/` — utility helpers.

### Other folders compiled into the main project

- `SourceGrid/DevAge.Windows.Forms/` — WinForms helpers and controls historically shared with SourceGrid.
- `SourceGrid/DevAgeSourcePack4/` — DevAge support library code.
- `SourceGrid/SourceGrid.Extensions/` — higher-level controls and adapters such as `DataGrid`, `ArrayGrid`, `PlanningGrid`, and PingGrid-related extensions.

Do not infer compilation solely from file presence. `SourceGrid.csproj` intentionally excludes a set of obsolete/duplicate controller/editor source files with `<Compile Remove=...>`.

## Tests and executable projects

### `SourceGrid.Tests/`

NUnit test project targeting both supported frameworks. Areas include cell behavior, clipboard, extensions, performance, quad trees, rows, selection, modernization tests, resource tests, and regression tests.

### `DevAge.TestApp/`

Supporting WinForms sample/test application. Some tests reference this project, so changes here can affect the automated test graph.

### `SourceGrid.Examples/`

Interactive WinForms examples. Use this project for exploratory/manual verification and for understanding how APIs are consumed. It references the core library and both PingGrid backends.

## Optional PingGrid backends

### `SourceGrid.PingGrid.Backends.DSet/`

DataSet-based backend. Depends on `SourceGrid`.

### `SourceGrid.PingGrid.Backend.Essent/`

ESENT-based backend. Depends on `SourceGrid` and `libs/Esent.Collections.dll`.

## `libs/`

Contains legacy binary dependencies used by optional/sample infrastructure. Treat these as compatibility baggage rather than preferred dependencies for new core code.

## Documentation

Current technical documentation is under `docs/`. The archived SourceGrid 4.x material under `docs/legacy/sourcegrid-4.x/` is intentionally separated because its runtime/tooling information is outdated.
