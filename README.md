# SourceGrid

SourceGrid is a Windows Forms grid library written in C#. The repository preserves the original SourceGrid programming model while maintaining the codebase for current .NET Windows applications.

SourceGrid 5.0 targets both **.NET Framework 4.8** and **.NET 8 for Windows**.

## Highlights

- Two primary grid models: `Grid` for real cells and `GridVirtual` for virtualized/custom data access.
- Extensible cells composed from models, views, editors, and controllers.
- Selection, row/column sizing, sorting, spanning, exporting/printing, tooltips, and custom rendering support.
- Example applications and optional PingGrid backends.
- Multi-target support for `net48` and `net8.0-windows`.
- NUnit test suite covering both target frameworks.

## Repository layout

| Path | Purpose |
| --- | --- |
| `SourceGrid/` | Main SourceGrid library, including DevAge support code and SourceGrid extensions. |
| `SourceGrid.Tests/` | NUnit tests for the library and modernization compatibility. |
| `SourceGrid.Examples/` | WinForms sample application. |
| `DevAge.TestApp/` | Supporting sample/test application referenced by the test project. |
| `SourceGrid.PingGrid.Backends.DSet/` | DataSet-based PingGrid backend. |
| `SourceGrid.PingGrid.Backend.Essent/` | ESENT-based PingGrid backend. |
| `libs/` | Legacy binary dependencies used by samples or optional backends. |
| `docs/` | Current technical, architecture, development, migration, and legacy documentation. |

See [docs/README.md](docs/README.md) for the documentation index and [AI_CONTEXT.md](AI_CONTEXT.md) for a concise system model.

## Prerequisites

Development is Windows-oriented because the projects use Windows Forms.

Recommended setup:

- Visual Studio 2022 with the **.NET desktop development** workload, or an equivalent MSBuild environment.
- .NET 8 SDK. `global.json` currently selects SDK `8.0.100` with feature-band roll-forward enabled.
- .NET Framework 4.8 targeting/developer pack when building the `net48` target.

## Build and test

From the repository root:

```powershell
dotnet restore SourceGrid_All.sln
dotnet build SourceGrid_All.sln -c Release
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Release --no-build
```

Warnings are treated as errors repository-wide. Package versions are centrally managed in `Directory.Packages.props`.

For target-specific commands and troubleshooting, see [docs/development/build-and-test.md](docs/development/build-and-test.md).

## Getting started with the library

The easiest way to explore SourceGrid behavior is to run `SourceGrid.Examples`. For the programming model and key abstractions, read:

1. [Architecture overview](docs/architecture/overview.md)
2. [Grid and cell model](docs/architecture/grid-model.md)
3. [Getting started for contributors](docs/development/getting-started.md)

The historical SourceGrid 4.x guide is preserved under `docs/legacy/sourcegrid-4.x/` for reference only. It describes older runtimes, tooling, and hosting locations and is **not** the source of truth for SourceGrid 5.0.

## SourceGrid 5.0 migration

SourceGrid 5.0 includes compatibility-impacting modernization work, including the move from `System.Data.SqlClient` to `Microsoft.Data.SqlClient` in `DevAge.Data.SqlClient.SqlCommandBuilder` and removal of formatter-based exception serialization APIs.

See [docs/migration/5.0.md](docs/migration/5.0.md).

## Contributing and AI-assisted work

Human contributors and coding agents should read [AGENTS.md](AGENTS.md) before making changes. It defines repository invariants, validation expectations, documentation policy, and the boundary between current and legacy material.

## License

SourceGrid uses the license included at [SourceGrid/SourceGrid.License.txt](SourceGrid/SourceGrid.License.txt).
