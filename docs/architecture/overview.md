# Architecture overview

## Purpose

SourceGrid is a reusable Windows Forms grid library. The design supports both concrete grids that own real cell objects and virtual/custom grids where cell behavior or values can be supplied on demand.

The repository combines a long-lived public API with ongoing runtime/toolchain modernization. Architecture work should therefore optimize for correctness, extensibility, and compatibility rather than wholesale replacement of historical patterns.

## Top-level dependency direction

```text
SourceGrid.Examples
  ├─> SourceGrid
  ├─> SourceGrid.PingGrid.Backends.DSet ─> SourceGrid
  └─> SourceGrid.PingGrid.Backend.Essent ─> SourceGrid
                                           └─ uses legacy ESENT binary in libs/

SourceGrid.Tests
  ├─> SourceGrid
  └─> DevAge.TestApp

SourceGrid
  ├─ SourceGrid/SourceGrid            core grid implementation
  ├─ SourceGrid/DevAge.Windows.Forms supporting WinForms infrastructure
  ├─ SourceGrid/DevAgeSourcePack4    legacy DevAge support code
  └─ SourceGrid/SourceGrid.Extensions higher-level extensions
```

The core library must not gain dependencies on example applications, test projects, PingGrid backend projects, or legacy sample binaries unless an explicit architecture change requires it.

## Main runtime layers

### WinForms control infrastructure

`GridVirtual` ultimately behaves as a WinForms control through SourceGrid's scrolling/control infrastructure. This layer owns focus, input, painting, scrolling, client geometry, and integration with WinForms.

### Grid structure

Rows, columns, positions, and ranges define the logical grid. Layout methods map these structures to visible rectangles and scrolling coordinates.

`GridVirtual` supplies shared behavior and defines abstract row/column creation plus cell retrieval contracts. `Grid` specializes this for stored `ICell` instances.

### Cell composition

Cells separate data and behavior into reusable pieces:

- Models determine values and other model-backed properties.
- Views determine rendering and visual properties.
- Editors provide edit behavior and editing controls.
- Controllers receive and implement interaction events.

This composition is central to SourceGrid extensibility and memory reuse.

### Interaction and selection

Controllers, focus/navigation logic, and the selection subsystem cooperate to process keyboard and mouse input. Event/controller ordering can be behaviorally significant.

### Spans and spatial indexing

Concrete `Grid` supports row/column spanning. Span ownership is tracked so covered positions resolve to the logical cell that owns the span. Quad-tree/range infrastructure supports efficient range lookup.

### Exporting and extensions

Printing/exporting and higher-level controls live outside the smallest grid core but remain in the main library project. Optional PingGrid storage backends are separate projects.

## Architectural principles

1. **Shared behavior belongs as low as practical.** A fix in `GridVirtual` can serve concrete and virtual grids; a `Grid`-only change should be used when it depends on real-cell storage or spans.
2. **Cell concerns remain composable.** Prefer extending models/views/editors/controllers over growing a monolithic cell type.
3. **Dependency direction stays inward.** Examples and optional backends depend on core, never the reverse by accident.
4. **Compatibility is a design constraint.** Public APIs and long-standing observable behaviors should change deliberately, not as collateral cleanup.
5. **WinForms lifecycle matters.** Focus, handles, painting, message dispatch, and modal UI can affect both production behavior and tests.

See [grid-model.md](grid-model.md) for the detailed object model and [../maintenance/modernization.md](../maintenance/modernization.md) for modernization constraints.
