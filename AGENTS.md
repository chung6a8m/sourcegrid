# AGENTS.md

This file is the repository-level operating contract for Codex and other coding agents. Human contributors should follow the same constraints unless a task explicitly requires a deliberate exception.

## 1. Read order

Before changing code, read the following in order:

1. `AGENTS.md` — repository rules and validation requirements.
2. `AI_CONTEXT.md` — stable system model and architectural vocabulary.
3. `docs/README.md` — documentation map.
4. The relevant files under `docs/architecture/`, `docs/development/`, `docs/maintenance/`, or `docs/migration/`.
5. The implementation and nearby tests for the area being changed.

Do not use `docs/legacy/` as the source of truth for current runtime, build, dependency, or API guidance.

## 2. Repository invariants

Preserve these defaults unless the task explicitly changes them:

- The primary library targets `net48` and `net8.0-windows`.
- SourceGrid is a Windows Forms library; do not introduce cross-platform assumptions into runtime code or test instructions.
- Repository platform is `AnyCPU` with `Prefer32Bit=false`.
- Warnings are treated as errors through `Directory.Build.props`.
- NuGet package versions are centrally managed in `Directory.Packages.props`; do not add package versions directly to project files.
- Public API compatibility matters. Avoid renaming, removing, or behaviorally redefining public members unless a change is explicitly intended and documented.
- Legacy compatibility code is not automatically dead code. Confirm usage, target-framework conditions, tests, and sample dependencies before deleting it.
- Files present in the source tree are not necessarily compiled. Respect explicit `<Compile Remove=...>` and other project-file inclusion/exclusion rules.

## 3. Architectural boundaries

Treat `SourceGrid/SourceGrid/` as the core grid implementation. Important areas include:

- `Grids/` — `Grid`, `GridVirtual`, rows, columns, scrolling, layout, and grid behavior.
- `Cells/` — real/virtual cells, models, views, editors, and cell controllers.
- `Selection/` — selection state and selection behavior.
- `Controllers/` — grid-level controllers and interaction plumbing.
- `Decorators/` — visual decorators.
- `Exporter/` — printing/export support.
- `QuadTree/` — spatial/range indexing used by grid features such as spans.

`SourceGrid/DevAge.Windows.Forms/` and `SourceGrid/DevAgeSourcePack4/` contain supporting legacy infrastructure that is part of the shipped source project. `SourceGrid/SourceGrid.Extensions/` contains higher-level extensions.

Keep optional/sample infrastructure out of the core unless the dependency is genuinely required:

- `SourceGrid.Examples/` is a sample application.
- `SourceGrid.PingGrid.Backends.DSet/` and `SourceGrid.PingGrid.Backend.Essent/` are optional PingGrid backends.
- `libs/` contains legacy binaries used by samples/backends; do not make new core features depend on them.

## 4. Change workflow

For non-trivial work:

1. Inspect the relevant implementation, project files, and tests before proposing changes.
2. Prefer the smallest change that preserves existing public behavior.
3. Add or update tests for observable behavior changes.
4. Build all affected target frameworks when the environment supports them.
5. Update current documentation when architecture, build requirements, compatibility, or public behavior changes.
6. Review the final diff for accidental generated-file, formatting, or legacy-document edits.

Plans intended for later implementation belong under `docs/plans/`.

## 5. Build and test expectations

The normal full validation sequence on Windows is:

```powershell
dotnet restore SourceGrid_All.sln
dotnet build SourceGrid_All.sln -c Release
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Release --no-build
```

For quick iteration, a target-specific test command is acceptable, but final validation should cover both `net48` and `net8.0-windows` when the change can affect both.

Examples:

```powershell
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Release -f net8.0-windows
dotnet test SourceGrid.Tests/SourceGrid.Tests.csproj -c Release -f net48
```

WinForms tests must be automation-safe. A test must not intentionally leave modal dialogs, message boxes, or interactive windows waiting for input. If a failure path can trigger WinForms default UI, install test-side handling so the test process fails deterministically instead of hanging.

## 6. Compatibility and modernization

SourceGrid is being modernized without treating the codebase as a greenfield rewrite.

When modernizing APIs:

- Preserve semantics and wire/file formats unless the task explicitly requires a breaking change.
- Prefer supported BCL/.NET APIs over obsolete compatibility APIs.
- Keep `net48` compilation in mind when selecting language/runtime APIs.
- Use target-framework conditions only when a shared implementation is not practical.
- Document deliberate breaking changes under `docs/migration/`.
- Add regression tests that capture compatibility-sensitive behavior.

Read `docs/maintenance/modernization.md` before broad cleanup or dependency changes.

## 7. Generated and designer files

Treat generated/designer files carefully, including `*.Designer.cs`, generated resources, typed DataSet output, and WinForms designer artifacts.

- Do not hand-edit generated files unless the repository already treats the file as hand-maintained or the task explicitly requires it.
- Prefer changing the generator input or project metadata.
- Preserve the scoped `SYSLIB0051` suppression for the generated typed DataSet artifact unless its generator behavior has changed.

## 8. Formatting and repository hygiene

`.editorconfig` and `.gitattributes` are authoritative for encoding, indentation, and line endings. Do not duplicate or override their rules in ad hoc scripts.

Important examples:

- Markdown: UTF-8, CRLF working-tree EOL.
- C# and Visual Studio project files: Windows-oriented EOL rules.
- YAML, shell, JavaScript/TypeScript, CSS, and HTML: LF where configured.
- Binary assets must remain binary and must not be line-ending normalized.

Avoid unrelated formatting churn in legacy source files.

## 9. Documentation policy

Current technical documentation belongs under `docs/`.

- `docs/architecture/` — current architecture and conceptual model.
- `docs/development/` — setup, build/test, and contributor guidance.
- `docs/maintenance/` — maintenance and modernization strategy.
- `docs/migration/` — version migration guides and breaking-change notes.
- `docs/plans/` — implementation plans.
- `docs/legacy/` — archived historical documents and assets only.

When documentation conflicts with current project configuration or tested code, verify the implementation first and update the current documentation. Do not silently rewrite archived legacy documents to make them look current.

## 10. Completion checklist

Before reporting a task complete, verify all applicable items:

- The intended files changed and unrelated files did not.
- Affected projects build without warnings/errors.
- Relevant tests pass and do not require interactive UI.
- Both supported target frameworks were considered.
- Public API compatibility was reviewed.
- Package versions remain centralized.
- Current docs were updated when necessary.
- No current document points to `Documents/` or treats `docs/legacy/` as canonical.
