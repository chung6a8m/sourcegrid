# Build and test

## Supported target frameworks

The main library and test project target:

- `net48`
- `net8.0-windows`

The repository is Windows-oriented because it uses Windows Forms.

## SDK selection

`global.json` selects .NET SDK `8.0.100` and allows roll-forward to the latest installed feature band. Use `dotnet --info` when diagnosing SDK resolution.

## Full validation

Run from the repository root on Windows:

```powershell
dotnet restore SourceGrid_All.sln
dotnet build SourceGrid_All.sln -c Release
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Release --no-build
```

`Directory.Build.props` sets `TreatWarningsAsErrors=true`, so a successful build must be warning-clean.

## Target-specific iteration

For faster work on modern .NET:

```powershell
dotnet build SourceGrid/SourceGrid.csproj -c Debug -f net8.0-windows
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Debug -f net8.0-windows
```

For .NET Framework compatibility:

```powershell
dotnet build SourceGrid/SourceGrid.csproj -c Debug -f net48
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Debug -f net48
```

Do not treat a `net8.0-windows`-only pass as proof that a shared library change is compatible with `net48`.

## Central package management

Package versions live in `Directory.Packages.props` with central transitive pinning enabled. Project files normally contain package names only.

When updating a dependency:

1. Change the version centrally.
2. Build all projects that consume it.
3. Run the relevant tests on both target frameworks.
4. Review migration/public API impact for packages that expose types through SourceGrid public APIs.

## Example application

Build the examples with:

```powershell
dotnet build SourceGrid.Examples/SourceGrid.Examples.csproj -c Debug -f net8.0-windows
```

Use the examples for visual/manual verification after automated tests, not instead of automated tests.

## WinForms test safety

Automated test runs must not depend on user interaction.

Potential sources of hangs include:

- `MessageBox.Show` or framework-created error dialogs.
- `DataGridView`/WinForms `DataError` flows that surface UI.
- unhandled exceptions on a UI thread that trigger default application UI.
- modeless or modal forms left open by a failed test.

Tests that intentionally exercise UI failure paths should intercept these conditions and convert them into deterministic test failures. A CI/agent process cannot reliably diagnose a hidden modal dialog.

## Troubleshooting

### .NET Framework reference assemblies missing

Install the .NET Framework 4.8 Developer Pack/targeting pack. Do not change the project to a newer-only target just to make a local environment pass.

### Warning becomes build failure

This is expected because warnings are errors. Fix the warning or, for generated/known compatibility artifacts, scope a suppression as narrowly as possible.

### Resource/designer differences between targets

Check `SourceGrid.csproj`, `Directory.Build.props`, and generated resource settings before editing generated files. The `net48` target has resource-specific build properties.

### Example-only dependency failure

Some optional/sample projects reference binaries under `libs/`. Keep this separate from failures in the core `SourceGrid` project when diagnosing the dependency graph.
