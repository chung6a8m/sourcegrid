# Grid and cell model

## GridVirtual vs Grid

SourceGrid has two related grid concepts.

### `GridVirtual`

`GridVirtual` is the abstract shared grid control. Its design supports large or externally backed data sets because a derived grid can compute or reuse virtual cells instead of allocating a concrete cell object for every coordinate.

A derived implementation must supply row/column objects and cell retrieval behavior. `GridVirtual` also owns common behavior such as sizing, scrolling, selection, focus/navigation, tooltips, event dispatch, and coordinate/range conversion.

### `Grid`

`Grid` derives from `GridVirtual` and provides the concrete/static-cell implementation.

It stores `ICell` instances in row- or column-optimized structures, exposes cell indexers, and supports row/column spans. A position covered by a span can resolve to the span's owning cell rather than a distinct cell object.

Use `Grid` when concrete per-position cell objects and maximum flexibility are useful. Prefer a virtual/custom grid design when data volume makes per-cell allocation undesirable or the data already lives in another model.

## Positions and ranges

`Position` identifies a row/column coordinate. `Range` identifies a rectangular region. These types appear throughout selection, painting, sizing, scrolling, spans, and hit testing.

When writing algorithms:

- Validate positions against the current complete range.
- Account for fixed rows/columns and scrolling when converting to display geometry.
- Do not assume the physical position is the logical start position of a spanned cell.
- Re-check selection/active positions after structural row/column changes.

## Cell interfaces

### `ICellVirtual`

Represents reusable cell behavior independent of concrete value storage at a unique grid coordinate.

`Cells.Virtual.CellVirtual` is the common implementation.

### `ICell`

Represents a concrete cell linked to a `Grid`, row, and column. `Cells.Cell` implements `ICell`, exposes a concrete value through its value model, and supports row/column span information.

## Cell composition

### Models

A cell's `ModelContainer` groups model behavior. Models can provide values and secondary properties such as tooltips or images.

For real `Cell` objects, the value model backs the `Value` property. For virtual cells, a custom value model can resolve values from external data using the current cell context.

### Views

A cell view controls rendering and visual properties. Views are intentionally shareable between many cells to reduce resource usage and enable consistent styling.

Changing a shared view can therefore affect multiple cells.

### Editors

An `EditorBase` controls editing. A `null` editor means the cell does not support editing. `Editors.Factory` creates editors for common .NET value types.

Editors can be shared, so editing logic must account for the active cell context rather than assuming one editor instance belongs permanently to one cell.

### Controllers

Cell controllers implement interaction/event behavior. A cell can attach multiple controllers through a controller container.

The order of controller dispatch can matter. `GridVirtual` installs default controllers for standard behavior, mouse selection, and cell event dispatch during construction.

### CellContext

`CellContext` ties together a grid, position, and cell. It is the contextual object through which reusable models/views/editors/controllers operate on a specific location.

When a shared component needs a cell's current value, display text, geometry, or grid services, look for a `CellContext`-based path before introducing direct coupling.

## Selection and active position

Selection is managed separately from cell storage. The selection subsystem tracks selected regions and an active position used by keyboard/focus behavior.

Grid structural changes can invalidate saved positions. `GridVirtual.CheckPositions()` demonstrates the expected pattern: validate mouse/drag positions and reset selection if the active position or selected region no longer fits the grid.

Selection regressions commonly involve more than the `Selection/` folder. Inspect keyboard handlers, focus transitions, controllers, row/column mutation, and scrolling together.

## Spanning

Concrete `Grid` cells can span multiple rows/columns. `Grid` maintains references to spanned ranges and uses range lookup infrastructure so covered positions can resolve back to the owning cell.

When changing cell insertion/removal, row/column movement, or range indexing, test both normal cells and spans, including overlapping-span rejection behavior.

## Extension strategy

Prefer the existing composition points:

- Data/value concern → model.
- Rendering/style concern → view or decorator.
- Editing concern → editor.
- Input/event concern → controller.
- Whole-grid storage/virtualization concern → `GridVirtual` subclass or extension.

This keeps features reusable across cells and avoids coupling unrelated responsibilities.
