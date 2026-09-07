# Coding guidelines

These guidelines supplement `.editorconfig`, `.gitattributes`, and `AGENTS.md` with SourceGrid-specific engineering practices.

## Preserve compatibility by default

SourceGrid has a mature public API. Prefer internal refactoring or additive APIs over breaking changes.

Before changing a public type/member:

- Search repository usage and examples.
- Check tests and migration documents.
- Consider both target frameworks.
- Decide whether the change alters source compatibility, binary expectations, runtime behavior, serialized/file output, or exposed third-party types.

Document deliberate breaking changes under `docs/migration/`.

## Put behavior in the correct abstraction

- Shared grid behavior → `GridVirtual` when it applies to virtual and concrete grids.
- Real-cell storage/span behavior → `Grid`.
- Value/data behavior → cell model.
- Rendering/style → cell view or decorator.
- Editing → editor.
- Input/event behavior → controller.
- Selection state/regions → selection subsystem.
- Optional storage backend → backend project, not core.

Avoid fixing a symptom at an example/form layer when the defect is in the reusable library.

## Respect shared cell components

Views and editors can be shared by many cells. Do not store per-cell transient state in a shared component unless it is keyed by `CellContext` or otherwise designed for sharing.

Use `CellContext` to access location-specific state from reusable cell components.

## UI lifecycle and event ordering

WinForms behavior depends on focus, handles, messages, painting, and control disposal. When changing interaction logic:

- Inspect keyboard and mouse paths together.
- Preserve controller/event dispatch ordering unless deliberately changing semantics.
- Consider focus transitions to/from child editor controls.
- Dispose owned controls/resources predictably.
- Avoid introducing modal UI into reusable library failure paths.

## Target-framework compatibility

Code shared by `net48` and `net8.0-windows` must compile and behave appropriately on both.

Prefer APIs available to both targets. If modern .NET requires a different implementation, keep conditional code narrow and explain the compatibility reason.

Do not use a newer C#/.NET runtime API merely because `LangVersion` permits the syntax in a consuming project.

## Dependencies

- Add/update NuGet versions only in `Directory.Packages.props`.
- Do not introduce a dependency from core SourceGrid to examples/tests/backends.
- Treat `libs/` as legacy compatibility infrastructure, not a default source for new dependencies.
- Be cautious when changing dependencies whose concrete types appear in public APIs; this can require a major-version migration note.

## Tests

Regression tests should reproduce the observable bug before the fix when practical.

Prefer tests that:

- exercise the smallest public/internal behavior that demonstrates the issue;
- clean up WinForms controls and resources;
- do not depend on screen position, timing, or human input unless the behavior fundamentally requires it;
- cover both supported targets for shared compatibility changes;
- verify spans/selection/focus when changes touch structural or interaction code.

## Formatting and generated files

Let `.editorconfig` and `.gitattributes` govern formatting/line endings. Avoid broad reformatting of historical files in behavior-focused changes.

Do not hand-edit generated designer/resource/typed-DataSet output unless necessary and understood. Keep analyzer suppressions scoped to generated artifacts rather than disabling diagnostics globally.

## Documentation

Update canonical documentation in the same change when introducing:

- new architecture boundaries;
- new build/runtime requirements;
- dependency policy changes;
- new public concepts;
- breaking/public compatibility changes.

Do not modernize prose inside `docs/legacy/`; archive integrity is intentional.
