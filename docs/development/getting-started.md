# Getting started for contributors

## Environment

SourceGrid is a Windows Forms repository and should be developed/tested on Windows for full fidelity.

Recommended tools:

- Windows 10/11 or a supported Windows build environment.
- Visual Studio 2022 with the .NET desktop development workload, or equivalent MSBuild tooling.
- .NET 8 SDK compatible with `global.json`.
- .NET Framework 4.8 targeting/developer pack.
- Git configured to honor the repository `.gitattributes` rules.

## First checkout

From the repository root:

```powershell
dotnet --info
dotnet restore SourceGrid_All.sln
dotnet build SourceGrid_All.sln -c Debug
```

If `net48` reference assemblies are missing, install the .NET Framework 4.8 Developer Pack/targeting pack rather than weakening the project target.

## Understand the repository before editing

Read:

1. `AGENTS.md`
2. `AI_CONTEXT.md`
3. `docs/architecture/overview.md`
4. The architecture/development document relevant to the task
5. The nearest implementation and tests

For historical API usage examples, the archived 4.x document can be useful, but verify every runtime/dependency statement against current SourceGrid 5.0 code before relying on it.

## Explore the examples

`SourceGrid.Examples` is the primary interactive showcase. Running it is useful for visual behavior, editing, selection, tooltips, custom cells, and optional PingGrid scenarios.

Build the examples for the modern target with:

```powershell
dotnet build SourceGrid.Examples/SourceGrid.Examples.csproj -c Debug -f net8.0-windows
```

Then run the produced executable from the target output directory or launch the project from Visual Studio.

## Make a focused change

A good SourceGrid change usually follows this sequence:

1. Identify whether the behavior belongs to `GridVirtual`, `Grid`, a cell composition component, selection, an extension, or optional/sample infrastructure.
2. Locate existing tests for the subsystem.
3. Add a regression test when changing observable behavior.
4. Implement the smallest compatible fix.
5. Run target-specific tests while iterating.
6. Run full build/test validation before completion.
7. Update canonical docs if the change modifies public behavior, architecture, dependencies, supported environments, or migration requirements.

## Common pitfalls

- Fixing behavior only in `Grid` when the same bug exists in `GridVirtual` descendants.
- Treating a cell position inside a span as an independent real cell.
- Mutating a shared view/editor as though it belongs to one cell only.
- Adding package versions directly to a `.csproj` despite central package management.
- Removing legacy code because it looks old without checking `net48` or sample compatibility.
- Running WinForms tests that can display a modal dialog and hang automation.
- Copying instructions from `docs/legacy/` into current docs without verification.
