# AI Context — SourceGrid

This document gives coding agents and new contributors a compact, stable model of the repository. It explains what the system is, how its major pieces relate, and which assumptions should remain true across tasks.

For operational rules, read `AGENTS.md`. For detailed documentation, start at `docs/README.md`.

## Product identity

SourceGrid is a reusable Windows Forms grid/control library written in C#. It supports table-like visualization and editing with both concrete cells and virtualized/custom data access.

The current repository is a modernization of a long-lived SourceGrid codebase, not a clean-room rewrite. Historical design patterns and public APIs remain important compatibility constraints.

Current primary library version: **5.0.0**.

Supported targets:

- `.NET Framework 4.8` (`net48`)
- `.NET 8 for Windows` (`net8.0-windows`)

## Solution topology

`SourceGrid_All.sln` contains six projects:

- `SourceGrid` — main library.
- `DevAge.TestApp` — supporting WinForms test/sample application.
- `SourceGrid.Tests` — NUnit test suite.
- `SourceGrid.Examples` — interactive example application.
- `SourceGrid.PingGrid.Backends.DSet` — DataSet-based PingGrid backend.
- `SourceGrid.PingGrid.Backend.Essent` — ESENT-based PingGrid backend.

The core `SourceGrid` project also compiles supporting code under:

- `SourceGrid/SourceGrid/` — main grid implementation.
- `SourceGrid/DevAge.Windows.Forms/` — supporting WinForms controls/helpers.
- `SourceGrid/DevAgeSourcePack4/` — legacy DevAge support code.
- `SourceGrid/SourceGrid.Extensions/` — higher-level SourceGrid extensions.

## Core mental model

### GridVirtual

`GridVirtual` is the abstract base for grid implementations that can provide cell behavior/value access without storing a concrete `ICell` instance for every position. Derived implementations provide row/column collections and implement cell retrieval.

It owns much of the shared UI machinery: layout, scrolling, selection, focus/navigation, mouse state, cell event dispatch, tooltips, sizing, and painting support.

### Grid

`Grid` derives from `GridVirtual` and represents the concrete/static-cell model. It stores real `ICell` instances and supports row/column spans. Its indexers return real cells, including the logical owner of a spanned area.

Use this distinction when fixing bugs: behavior implemented in `GridVirtual` can affect both concrete and virtual grids, while storage/span behavior in `Grid` is specific to the concrete grid.

### Cells

SourceGrid separates cell concerns instead of putting all behavior into one class.

A virtual cell (`ICellVirtual` / `Cells.Virtual.CellVirtual`) is composed from:

- **Model** — data/value-related behavior exposed through model objects.
- **View** — rendering and visual properties. View instances may be shared across cells.
- **Editor** — editing control/logic; `null` means no editing. Editors may be shared.
- **Controller** — input/event behavior attached to a cell.

A real cell (`ICell` / `Cells.Cell`) adds concrete value storage/binding to a grid position plus row/column span support.

`CellContext` is an important bridge that combines grid, position, and cell so models, views, editors, and controllers can operate against the current location.

### Controllers and interaction

Controller ordering matters. `GridVirtual` installs default cell controllers in a defined sequence for standard behavior, mouse selection, and event dispatch. When adding interaction behavior, inspect existing dispatch order before inserting another controller.

### Selection

Selection is a first-class subsystem under `SourceGrid/SourceGrid/Selection/`. Grid navigation, active position, selected regions, keyboard input, and structural mutations can interact with selection state. Changes to row/column management should consider whether saved positions remain valid.

### Rows, columns, ranges, and spans

`Position`, `Range`, row/column collections, and span bookkeeping are foundational concepts. `Grid` uses span tracking and a range index (`QuadTree` infrastructure) to resolve spanned cells efficiently.

Do not treat an arbitrary physical position as necessarily owning a unique cell; positions covered by a span can resolve to the span's start cell.

## Build and dependency model

The repository uses SDK-style projects and central package management.

- `Directory.Build.props` defines repository-wide build behavior and treats warnings as errors.
- `Directory.Packages.props` owns NuGet versions.
- `global.json` selects the .NET 8 SDK line.
- `.editorconfig` and `.gitattributes` define formatting, encoding, and EOL policy.

The main library currently references `Microsoft.Data.SqlClient`, `Microsoft.Windows.Compatibility`, `System.Text.Json`, and a `net48`-specific `System.Resources.Extensions` dependency.

Optional/sample projects still use legacy DLLs from `libs/`. These dependencies should not leak into the core library without a deliberate architectural decision.

## SourceGrid 5.0 modernization state

Important completed compatibility changes include:

- Multi-targeting `net48` and `net8.0-windows`.
- SDK-style project/build metadata.
- Central package management.
- `DevAge.Data.SqlClient.SqlCommandBuilder` now exposes `Microsoft.Data.SqlClient` command/parameter types.
- Formatter-based exception serialization constructors/attributes were removed from SourceGrid exception hierarchies.
- Legacy DES/SHA-1 helper signatures and output behavior were retained while implementations were modernized.
- Platform metadata for the .NET 8 assembly explicitly identifies Windows support.

See `docs/migration/5.0.md` and `docs/maintenance/modernization.md` before changing these areas.

## Compatibility posture

SourceGrid has a large historical API surface. Default maintenance posture:

1. Fix correctness and platform issues without gratuitous API churn.
2. Preserve behavior relied on by existing WinForms applications.
3. Modernize unsafe/obsolete internals behind stable public contracts when practical.
4. Use tests to pin compatibility-sensitive behavior before broad refactors.
5. Treat a deliberate breaking change as migration work: document it and update versioning expectations.

## Test model

`SourceGrid.Tests` uses NUnit and targets both supported frameworks. Tests reference both `SourceGrid` and `DevAge.TestApp`, so some tests exercise sample/support behavior as well as the core library.

Because this is WinForms code, tests may touch UI primitives. Automated tests must fail deterministically rather than opening modal UI that waits forever for human input.

## Documentation model

Current/canonical documentation lives under `docs/`. Historical SourceGrid 4.x material is archived under `docs/legacy/sourcegrid-4.x/` and may contain obsolete runtime requirements, dead hosting URLs, and old version numbers.

When historical prose conflicts with current source, project files, or tests, use the current implementation as the primary evidence and update canonical documentation if needed.
